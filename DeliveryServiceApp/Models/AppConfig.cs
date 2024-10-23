namespace DeliveryServiceApp.Models;

public record AppConfig(string District, DateTime FirstDeliveryTime, string DeliveryLogPath, string DeliveryOrderPath);
