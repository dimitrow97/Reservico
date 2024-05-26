using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservico.Common.EmailSender;
using Reservico.Common.Models;
using Reservico.Data.Entities;
using Reservico.Data.Interfaces;
using Reservico.Identity.UserManager.Models;
using Reservico.Identity.UserPasswordManager;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;

namespace Reservico.Identity.UserManager
{
    public class UserManager : IUserManager
    {
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;
        private readonly IPasswordHasher passwordHasher;
        private readonly IPasswordGenerator passwordGenerator;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Role> roleRepository;
        private readonly IRepository<UserClient> userClientRepository;

        public UserManager(
            IMapper mapper,
            IEmailSender emailSender,
            IPasswordHasher passwordHasher,
            IPasswordGenerator passwordGenerator,
            IRepository<User> userRepository,
            IRepository<Role> roleRepository,
            IRepository<UserClient> userClientRepository)
        {
            this.mapper = mapper;
            this.emailSender = emailSender;
            this.passwordHasher = passwordHasher;
            this.passwordGenerator = passwordGenerator;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.userClientRepository = userClientRepository;
        }

        public async Task<ServiceResponse<UserRegistrationResponseModel>> RegisterAsync(
            UserRegistrationRequestModel model)
        {
            var userExists = await GetUserByEmailAsync<User>(model.Email);

            if (userExists.IsSuccess && userExists.Data.IsDeleted)
            {
                var reactivation = await this.Activate(userExists.Data);

                if (!reactivation.IsSuccess)
                {
                    return ServiceResponse<UserRegistrationResponseModel>.
                        Error($"Reactivating user with id: {userExists.Data.Id} failed.");
                }

                return ServiceResponse<UserRegistrationResponseModel>.Success(
                    new UserRegistrationResponseModel(UserRegisterStatus.Reactivated));
            }

            if (userExists.IsSuccess)
            {
                return ServiceResponse<UserRegistrationResponseModel>.Error(
                    "User with the provided email already exists!");
            }

            var user = new User()
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                IsUsingDefaultPassword = true,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString("N").ToUpper(),
                CreatedOn = DateTime.UtcNow
            };

            var password = passwordGenerator.GeneratePassword(8);
            var hashedPassword = this.passwordHasher.Hash(
                password, user.SecurityStamp);

            user.PasswordHash = hashedPassword;

            await this.userRepository.AddAsync(user);

            var addingToRolesResult = await this.AddUserToRoles(user, model.Roles);

            if (!addingToRolesResult.IsSuccess)
            {
                return ServiceResponse<UserRegistrationResponseModel>
                    .Error(addingToRolesResult.ErrorMessage);
            }

            if (model.ClientId.HasValue)
            {
                var userClient = new UserClient
                {
                    UserId = user.Id,
                    ClientId = model.ClientId.Value,
                    IsSelected = true,
                    CreatedOn = DateTime.UtcNow,
                };

                user.UserClients.Add(userClient);
            }

            await this.userRepository.SaveChangesAsync();

            await emailSender.SendRegistrationEmail(user.Email, password);

            return ServiceResponse<UserRegistrationResponseModel>.Success(
                new UserRegistrationResponseModel(UserRegisterStatus.Registered));
        }

        public async Task<ServiceResponse> UpdateAsync(
            UserDetailsViewModel model)
        {
            var user = this.userRepository
                .Query()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.Id.ToString().Equals(model.UserId));

            if (user == null)
            {
                return ServiceResponse.Error("User not found.");
            }

            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.IsActive = model.IsActive;
            user.UpdatedOn = DateTime.UtcNow;

            if (user.PhoneNumber != model.PhoneNumber)
            {
                user.PhoneNumber = model.PhoneNumber;
                user.PhoneNumberConfirmed = false;
            }

            await this.HandleRolesOnUpdate(
                user,
                model.Roles.ToList());

            await this.userRepository.UpdateAsync(user);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse> UpdateAsync(
            User user)
        {
            if (user == null)
            {
                return ServiceResponse.Error("User should not be null.");
            }

            user.UpdatedOn = DateTime.UtcNow;

            await this.userRepository.UpdateAsync(user);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse> DeleteAsync(
            Guid userId)
        {
            var user = await this.userRepository
                .FindByConditionAsync(x => x.Id.Equals(userId));

            if (user == null)
            {
                return ServiceResponse.Error("User not found.");
            }

            user.IsDeleted = true;
            user.UpdatedOn = DateTime.UtcNow;

            await this.userRepository.UpdateAsync(user);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse<TModel>> GetUserByIdAsync<TModel>(Guid userId)
            where TModel : class, new()
        {
            var user = await this.userRepository
                .Query()
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                .Include(x => x.UserClients)
                    .ThenInclude(x => x.Client)
                .FirstOrDefaultAsync(x => x.Id.Equals(userId));

            if (user == null)
            {
                return ServiceResponse<TModel>.Error("User not found.");
            }

            var result = mapper.Map(user, new TModel());

            return ServiceResponse<TModel>.Success(result);
        }

        public async Task<ServiceResponse<TModel>> GetUserByEmailAsync<TModel>(string email)
            where TModel : class, new()
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return ServiceResponse<TModel>
                    .Error("Invalid email provided.");
            }

            var user = await this.userRepository
                .FindByConditionAsync(x => x.Email.Equals(email));

            if (user is null || user.IsDeleted)
            {
                return ServiceResponse<TModel>
                    .Error("No user matches the provided email.");
            }

            var result = mapper.Map(user, new TModel());

            return ServiceResponse<TModel>.Success(result);
        }

        public async Task<ServiceResponse<IEnumerable<TModel>>> GetUsersByRoleAsync<TModel>(string role)
            where TModel : class, new()
        {
            var users = await this.userRepository
                .Query()
                .Include(x => x.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(x => x.UserRoles.Any(x => x.Role.NormalizedName.Equals(role)))
                .ToListAsync();

            if (users is null || !users.Any())
            {
                return ServiceResponse<IEnumerable<TModel>>
                    .Error($"No users found in role: {role}");
            }

            var result = users.Where(x => x.IsDeleted.Equals(false))
                .Select(user => mapper.Map(user, new TModel()));

            return ServiceResponse<IEnumerable<TModel>>.Success(
                result);
        }

        public async Task<ServiceResponse> VerifyUserPhoneNumberAsync(Guid userId)
        {
            var user = await this.GetUserByIdAsync<User>(userId);

            if (!user.IsSuccess)
            {
                return ServiceResponse.Error(user.ErrorMessage);
            }

            user.Data.PhoneNumberConfirmed = true;

            var result = await this.UpdateAsync(user.Data);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse<IEnumerable<string>>> GetUserRoles(Guid userId)
        {
            var user = await this.userRepository
                .Query()
                .Include(x => x.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(x => x.Id.Equals(userId) && !x.IsDeleted);

            if (user is null)
            {
                return ServiceResponse<IEnumerable<string>>
                    .Error("User not found.");
            }

            if (user.UserRoles is null || !user.UserRoles.Any())
            {
                return ServiceResponse<IEnumerable<string>>
                    .Error($"User has no roles.");
            }

            return ServiceResponse<IEnumerable<string>>
                .Success(user.UserRoles.Select(x => x.Role.Name));
        }

        public async Task<ServiceResponse<Client>> GetUserSelectedClient(Guid userId)
        {
            var selectedUserClient = await userClientRepository
                .Query()
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.IsSelected);

            if (selectedUserClient is null || selectedUserClient.Client is null)
            {
                return ServiceResponse<Client>.Error("User has no Client selected.");
            }

            return ServiceResponse<Client>.Success(selectedUserClient.Client);
        }

        public async Task<ServiceResponse<ListViewModel<UserDetailsViewModel>>> Get(
            string filter = null,
            int skip = 0,
            int take = 10)
        {
            var queryable = this.userRepository.Query()
                .Include(x => x.UserClients)
                    .ThenInclude(x => x.Client)
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                .Where(x => !x.UserRoles.Any(x => x.Role.NormalizedName.Equals(IdentityRoles.AdminRole)))
                .Where(x => x.IsDeleted.Equals(false));

            var userVms = await this.GetInner(queryable, filter, skip, take);

            if (!userVms.IsSuccess)
            {
                return ServiceResponse<ListViewModel<UserDetailsViewModel>>
                    .Error(userVms.ErrorMessage);
            }

            return userVms;
        }

        private async Task<ServiceResponse<ListViewModel<UserDetailsViewModel>>> GetInner(
            IQueryable<User> queryable,
            string filter = null,
            int skip = 0,
            int take = 10)
        {

            //filtering
            if (!string.IsNullOrWhiteSpace(filter))
            {
                filter = RenameQueryFields(filter);

                queryable = queryable.Where(filter);
            }

            var totalCount = await queryable.CountAsync();

            //paging
            queryable = queryable.Skip(skip).Take(take);

            var users = await queryable
                .OrderBy(x => x.Email)
                .ToListAsync();

            var result = users.Select(user => mapper.Map(user, new UserDetailsViewModel()));

            return ServiceResponse<ListViewModel<UserDetailsViewModel>>.Success(
                new ListViewModel<UserDetailsViewModel>
                {
                    TotalCount = totalCount,
                    Data = result
                });
        }

        private string RenameQueryFields(string query)
        {
            var result = query.ToLower()
               .Replace("id", "Id")
               .Replace("email", "Email")
               .Replace("username", "UserName")
               .Replace("name", "string.Concat(FirstName, LastName)");

            var rgx = new Regex("client.contains\\((.+)\\)");

            if (rgx.IsMatch(result))
            {
                var match = rgx.Match(result);
                match.Groups.TryGetValue("1", out var clientNameFilter);

                result = rgx.Replace(result, $"UserClients.Any(y => y.Client.Name.Contains({clientNameFilter.Value}))");
            }

            return result;
        }

        private async Task HandleRolesOnUpdate(
            User user,
            IList<string> roles)
        {
            var rolesInDb = await this.roleRepository
                .Query()
                .Where(r => roles.Any(x => r.NormalizedName.Equals(x)))
                .ToListAsync();

            if (user.UserRoles is null || !user.UserRoles.Any())
            {
                foreach (var dbRole in rolesInDb)
                {
                    user.UserRoles.Add(new UserRole
                    {
                        RoleId = dbRole.Id,
                        UserId = user.Id
                    });
                }
            }
            else
            {
                foreach (var role in user.UserRoles)
                {
                    if (roles.Any(x => x.Equals(role)))
                    {
                        roles.Remove(role.Role.NormalizedName);
                    }
                    else
                    {
                        user.UserRoles.Remove(role);
                    }
                }

                if (roles.Any())
                {
                    foreach (var role in rolesInDb.Where(x => roles.Any(r => x.NormalizedName.Equals(r))))
                    {
                        user.UserRoles.Add(
                            new UserRole 
                            { 
                                RoleId = role.Id, 
                                UserId = user.Id 
                            });
                    }
                }
            }
        }

        private async Task<ServiceResponse> Activate(User user)
        {
            var userClients = await userClientRepository
                .FindAllByConditionAsync(x => x.UserId.Equals(user.Id));

            user.IsDeleted = false;
            user.UpdatedOn = DateTime.UtcNow;

            await this.userRepository.UpdateAsync(user);

            return ServiceResponse.Success();
        }

        private async Task<ServiceResponse> AddUserToRoles(
            User user,
            IEnumerable<string> roles)
        {
            if (roles is null ||
                !roles.Any())
            {
                return ServiceResponse.Error("Invalid user model.");
            }

            var rolesInDb = await this.roleRepository
                .Query()
                .Where(x => roles.Any(r => x.NormalizedName.Equals(r)))
                .ToListAsync();

            if (!rolesInDb.Any())
            {
                return ServiceResponse
                    .Error("Invalid user roles provided.");
            }

            foreach (var role in rolesInDb)
            {
                user.UserRoles.Add(
                    new UserRole
                    {
                        RoleId = role.Id,
                        UserId = user.Id
                    });
            }

            user.UpdatedOn = DateTime.UtcNow;

            await this.userRepository.UpdateAsync(user, false);

            return ServiceResponse.Success();
        }
    }
}