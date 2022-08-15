using AutoMapper;
using ClinicService.Mapping;

namespace ClinicService.Extensions
{
    public static class MapperEx
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            var mapper = new MapperConfiguration(config => config.AddProfile(new MapperProfile())).CreateMapper();
            var serviceDescriptor = new ServiceDescriptor(typeof(IMapper), mapper);
            services.Add(serviceDescriptor);
            return services;
        }
    }
}
