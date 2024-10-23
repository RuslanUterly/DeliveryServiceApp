using DeliveryServiceApp.Models;
using DeliveryServiceApp.Validates.Interfaces;
using System.Globalization;

namespace DeliveryServiceApp.Validates;

public class InputValidator : IInputValidator
{
    public Task<AppConfig> ValidateAsync(string[] args)
    {
        if (args.Length != 4)
        {
            throw new ArgumentException(@"Неверное количество параметров. 
                Введите следующие параметры:
                1 - Район доставки
                2 - Начальное время доставки (формат yyyy-MM-dd HH:mm:ss)
                3 - Путь к файлу логов
                4 - Путь к файлу отфильтрованных заказов");
        }

        if (!DateTime.TryParseExact(args[1], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime firstDeliveryTime))
            throw new ArgumentException("Неверный формат даты. Ожидается формат yyyy-MM-dd HH:mm:ss");

        if (args[2].IndexOfAny(Path.GetInvalidPathChars()) != -1)
            throw new ArgumentException("Неверный путь к файлу логов.");

        if (args[3].IndexOfAny(Path.GetInvalidPathChars()) != -1)
            throw new ArgumentException("Неверный путь к файлу отфильтрованных заказов.");

        string district = args[0];
        string deliveryLogPath = args[2];
        string deliveryOrderPath = args[3];

        return Task.FromResult(new AppConfig(district, firstDeliveryTime, deliveryLogPath, deliveryOrderPath));
    }

    private bool HasInvalidPathChars(string path)
    {
        return path.IndexOfAny(Path.GetInvalidPathChars()) != -1;
    }
}
