using AutoMapper;
using System.Reflection;

namespace Reservico.Mapping
{
    public static class ReservicoAutoMapperConfig
    {
        private const string ProfileNameTemplate = "ReflectionProfile";

        public static IMapper RegisterMappings(params Assembly[] assemblies)
            => RegisterMappings(ProfileNameTemplate, assemblies);

        public static IMapper RegisterMappings(string profileName = ProfileNameTemplate, params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(a => a.GetExportedTypes()).ToList();

            var config = new MapperConfigurationExpression();
            config.CreateProfile(profileName,
                configuration =>
                {
                    CreateFromMappings(configuration, types);
                    CreateToMappings(configuration, types);
                    CreateCustomMappings(configuration, types);
                    CreateGenericCustomMappings(configuration, types);
                });

            return new Mapper(new MapperConfiguration(config));
        }

        private static void CreateFromMappings(IProfileExpression configuration, List<Type> types)
            => GetFromMaps(types).ToList().ForEach(map => configuration.CreateMap(map.Source, map.Destination));


        private static void CreateToMappings(IProfileExpression configuration, List<Type> types)
            => GetToMaps(types).ToList().ForEach(map => configuration.CreateMap(map.Source, map.Destination));

        private static void CreateCustomMappings(IProfileExpression configuration, List<Type> types)
            => GetCustomMappings(types).ToList().ForEach(map => map.CreateMappings(configuration));

        private static void CreateGenericCustomMappings(IProfileExpression configuration, List<Type> types)
        => GetGenericCustomMappings(types).ToList().ForEach(map => map.CreateMappings(configuration));

        private static IEnumerable<TypesMap> GetFromMaps(IEnumerable<Type> types)
            => types.Where(t =>
                    t.IsAbstract &&
                    t.IsInterface)
               .Where(t => t.GetInterfaces()
                    .Any(i => i.GetTypeInfo().IsGenericType &&
                             i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
               .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.GetTypeInfo().IsGenericType &&
                             i.GetGenericTypeDefinition() == typeof(IMapFrom<>))
                    .Select(i => new TypesMap
                    {
                        Source = i.GetTypeInfo().GetGenericArguments()[0],
                        Destination = t
                    }));

        private static IEnumerable<TypesMap> GetToMaps(IEnumerable<Type> types)
            => types.Where(t =>
                    t.IsAbstract &&
                    t.IsInterface)
               .Where(t => t.GetInterfaces()
                    .Any(i => i.GetTypeInfo().IsGenericType &&
                             i.GetGenericTypeDefinition() == typeof(IMapTo<>)))
               .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.GetTypeInfo().IsGenericType &&
                             i.GetGenericTypeDefinition() == typeof(IMapTo<>))
                    .Select(i => new TypesMap
                    {
                        Source = t,
                        Destination = i.GetTypeInfo().GetGenericArguments()[0]
                    }));

        private static IEnumerable<IHaveCustomMapping> GetCustomMappings(IEnumerable<Type> types)
            => types.Where(t =>
                    !t.IsAbstract &&
                    !t.IsInterface &&
                    !t.GetTypeInfo().ContainsGenericParameters)
                .Where(t => t.GetInterfaces().Any(i => typeof(IHaveCustomMapping).IsAssignableFrom(t)))
                .Select(t => (IHaveCustomMapping)Activator.CreateInstance(t));

        private static IEnumerable<IHaveCustomMapping> GetGenericCustomMappings(IEnumerable<Type> types)
            => types.Where(t =>
                    !t.IsAbstract &&
                    !t.IsInterface &&
                    t.GetTypeInfo().ContainsGenericParameters)
                .Where(t => t.GetInterfaces().Any(i => typeof(IHaveCustomMapping).IsAssignableFrom(t)))
                .Select(t => (IHaveCustomMapping)Activator.CreateInstance(t.MakeGenericType(typeof(object))));
    }
}