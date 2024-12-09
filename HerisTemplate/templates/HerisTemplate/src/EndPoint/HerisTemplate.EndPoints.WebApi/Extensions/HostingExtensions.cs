namespace HerisTemplate.EndPoints.WebApi.Extensions;
public static class HostingExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBaseApiCore("Heris", "HerisTemplate");
        services.AddEndpointsApiExplorer();
        services.AddHerisNewtonSoftSerializer();
        services.AddHerisAutoMapperProfiles(option =>
        {
            option.AssemblyNamesForLoadProfiles = configuration["AutoMapper:AssemblyNamesForLoadProfiles"] ?? throw new NotFoundException();
        });

        services.AddDbContext<BaseDbContext, HerisTemplateDbContext>(
            c => c.UseNpgsql(configuration.GetConnectionString("HerisTemplateConnectionString"), options =>
            {
                options.MigrationsAssembly(typeof(HerisTemplateDbContext).Assembly.GetName().Name);
            }));

        services.AddHerisSwagger(configuration, "Swagger");
        return services;
    }
    public static async Task<WebApplication> ConfigurePipeline(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BaseDbContext>();
        if (!app.Environment.IsDevelopment())
        {
            if (!await context.Database.CanConnectAsync())
            {
                await context.Database.MigrateAsync();
            }
        }
        app.UseCustomExceptionHandler();
        app.UseHerisSwagger();
        app.UseStatusCodePages();
        app.UseStaticFiles();
        app.UseCors(corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin();
            corsPolicyBuilder.AllowAnyHeader();
            corsPolicyBuilder.AllowAnyMethod();
        });
        app.UseHttpsRedirection();
        app.MapControllers();
        return app;
    }
}