using AutoMapper;
using Reservico.Data.Entities;
using Reservico.Mapping;
using Reservico.Services.Reservations.Models;

namespace Reservico.Services.Locations.Models
{
    public class TableViewModel : IHaveCustomMapping
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public int WorkingHoursFrom { get; set; }
        public int WorkingHoursTo { get; set; }
        public bool CanTableTurn { get; set; }
        public int TableTurnOffset { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Table, TableViewModel>()
                .ForMember(x => x.Id, cfg => cfg.MapFrom(y => y.Id))
                .ForMember(x => x.Name, cfg => cfg.MapFrom(y => y.Name))
                .ForMember(x => x.Capacity, cfg => cfg.MapFrom(y => y.Capacity))
                .ForMember(x => x.Description, cfg => cfg.MapFrom(y => y.Description))
                .ForMember(x => x.IsDeleted, cfg => cfg.MapFrom(y => y.IsDeleted))
                .ForMember(x => x.WorkingHoursFrom, cfg => cfg.MapFrom(y => y.WorkingHoursFrom))
                .ForMember(x => x.WorkingHoursTo, cfg => cfg.MapFrom(y => y.WorkingHoursTo))
                .ForMember(x => x.CanTableTurn, cfg => cfg.MapFrom(y => y.CanTableTurn))
                .ForMember(x => x.TableTurnOffset, cfg => cfg.MapFrom(y => y.TableTurnOffset))
                .ForMember(x => x.LocationId, cfg => cfg.MapFrom(y => y.LocationId))
                .ForMember(x => x.LocationName, cfg => cfg.MapFrom(y => y.Location.Name));
        }
    }
}
