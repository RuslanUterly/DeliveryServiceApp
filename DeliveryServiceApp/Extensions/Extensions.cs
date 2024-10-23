using DeliveryServiceApp.Extensions;
using DeliveryServiceApp.Models;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Text;

namespace DeliveryServiceApp.Extensions;

public static class Extensions
{
    public static string PrintOrders(this IEnumerable<Order> orders)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var order in orders)
        {
            sb.Append(order.ToString());
        }
        return sb.ToString();
    }

    public static IServiceCollection AddLogging(this IServiceCollection services, string logPath)
    {
        var serilogLogger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(logPath)
            .CreateLogger();

        return services.AddLogging(builder =>
        {
            builder.AddSerilog(serilogLogger, true);
        });
    }
}