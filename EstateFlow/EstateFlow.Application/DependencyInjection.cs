using EstateFlow.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
namespace EstateFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
// Az összes validátor felderítése és regisztrálása a DI konténerbe (scopedéletciklussal)
services.AddValidatorsFromAssemblyContaining<IApplicationDbContext>();
        services.AddScoped<IMeterReadingService, MeterReadingService>();
        return services;
    }
}