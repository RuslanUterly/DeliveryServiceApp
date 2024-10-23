namespace DeliveryServiceApp.Models;

public record Order(int Id, double Weight, string Destrict, DateTime DeliveryTime)
{
    public override string ToString()
    {
        return $"{Id};{Weight};{Destrict};{DeliveryTime:yyyy-MM-dd HH:mm:ss}";
    }
}
