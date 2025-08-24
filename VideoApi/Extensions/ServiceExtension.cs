using VideoApi.Repositories;

namespace VideoApi.Extensions
{
    public static class ServiceExtension
    {
        public static void RegisterRepositories(this IServiceCollection collection)
        {
            collection.AddTransient<AuthRepository>();
            collection.AddTransient<UserRepository>();
        }
    }
}