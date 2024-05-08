using AutoMapper;

namespace Reservico.Mapping
{
    public interface IHaveCustomMapping
    {
        void CreateMappings(IProfileExpression configuration);
    }
}