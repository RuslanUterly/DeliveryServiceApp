namespace DeliveryServiceApp.Models;

public record Order(int Id, double Weight, string Destrict, DateTime DeliveryTime);
