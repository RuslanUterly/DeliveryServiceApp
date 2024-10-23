using DeliveryServiceApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DeliveryServiceApp.Extensions;
using DeliveryServiceApp.Repositories.Interfaces;
using DeliveryServiceApp.Services.Interfaces;
using DeliveryServiceApp.Validates.Interfaces;
using DeliveryServiceApp.Validates;
using DeliveryServiceApp.Repositories;
using Serilog.Core;

namespace DeliveryServiceApp;

public class Program
{
    const string ORDER_PATH = @"C:\Users\najmi\Desktop\Project\DeliveryService\DeliveryServiceApp\DeliveryServiceApp\Files\Orders.csv";

    static async Task Main(string[] args)
    {

        var validationServiceCollection = new ServiceCollection()
            .AddSingleton<IInputValidator, InputValidator>();

        var inputValidator = validationServiceCollection.BuildServiceProvider().GetService<IInputValidator>();
        var config = await inputValidator.ValidateAsync(args);

        var serviceProvider = new ServiceCollection()
            .AddLogging(config.DeliveryLogPath)
            .AddSingleton<IInputValidator, InputValidator>()
            .AddSingleton<IOrderValidator, OrderValidator>()
            .AddSingleton<IOrderRepository, OrderRepository>()
            .AddSingleton<IOrderService, OrderService>()
            .BuildServiceProvider();

        var logger = serviceProvider.GetService<Microsoft.Extensions.Logging.ILogger<Program>>();

        try
        {
            var orderRepository = serviceProvider.GetService<IOrderRepository>();
            var orderService = serviceProvider.GetService<IOrderService>();

            var orders = await orderRepository.GetOrdersAsync(ORDER_PATH);
            var filteredOrders = await orderService.FilterOrdersAsync(orders, config.District, config.FirstDeliveryTime);

            if (filteredOrders.Any())
                Console.WriteLine(filteredOrders.PrintOrders());
            else
                Console.WriteLine("заказы не найдены");
            await orderRepository.SaveFilteredOrdersAsync(filteredOrders, config.DeliveryOrderPath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Произошла ошибка во время выполнения приложения.");
        }
    }
}
