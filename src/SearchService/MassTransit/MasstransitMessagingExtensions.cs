using MassTransit;

namespace SearchService.MassTransit;

public static class MasstransitMessagingExtensions
{
    public static IServiceCollection AddMassTransitWithConfigurations(this IServiceCollection services)
    {
        services.AddMassTransit(opt =>
        {
            opt.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

            opt.AddConsumer<CatalogItemCreatedConsumer>();
            opt.AddConsumer<CatalogItemUpdatedConsumer>();
            opt.AddConsumer<CatalogItemDeletedConsumer>();

            opt.UsingRabbitMq((context, config) =>
            {
                config.ReceiveEndpoint("search-catalog-item-created",
                    eConfig =>
                    {
                        eConfig.UseMessageRetry(r => r.Intervals(4000, 6000, 8000, 10000, 12000));
                        eConfig.ConfigureConsumer<CatalogItemCreatedConsumer>(context);
                    });
                config.ReceiveEndpoint("search-catalog-item-updated",
                    eConfig =>
                    {
                        eConfig.UseMessageRetry(r => r.Intervals(4000, 6000, 8000, 10000, 12000));
                        eConfig.ConfigureConsumer<CatalogItemUpdatedConsumer>(context);
                    });
                config.ReceiveEndpoint("search-catalog-item-deleted",
                    eConfig =>
                    {
                        eConfig.UseMessageRetry(r => r.Intervals(4000, 6000, 8000, 10000, 12000));
                        eConfig.ConfigureConsumer<CatalogItemDeletedConsumer>(context);
                    });
                config.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}