using AutoMapper;

namespace EventPlanning.Server.Configurations
{
    public static class AutomapperConfiguration
    {
        public static void ConfigureAutomapper(this IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                var config = new MapperConfiguration(autoMapperConfig =>
                {

                });

                return config.CreateMapper();
            });
        }
    }
}