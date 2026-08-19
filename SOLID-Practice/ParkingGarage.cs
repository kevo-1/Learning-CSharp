using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Channels;
using System.IO.Pipes;

[Flags]
public enum NotificationChannels
{
    EMAIL = 1 << 0,  
    SMS   = 1 << 1,  
    PUSH  = 1 << 2,  
}

public enum VehicleType
{
    Car,
    Truck,
    Bus,
    MotorCycle
}
public class ParkingGarage
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IReceiptPrinter _receiptPrinter;
    private readonly INotifier _notifier;
    private readonly IFeesCalculatorFactory _feesCalculatorFactory;

    public ParkingGarage(IReceiptRepository receiptRepository, IReceiptPrinter receiptPrinter, INotifier notifier, IFeesCalculatorFactory feesCalculatorFactory)
    {
        _receiptRepository = receiptRepository;
        _receiptPrinter = receiptPrinter;
        _notifier = notifier;
        _feesCalculatorFactory = feesCalculatorFactory;
    }

    public void ProcessCar(VehicleType vehicleType, NotificationChannels channels)
    {
        IFeesCalculator feesCalculator = _feesCalculatorFactory.GenerateFeesCalculator(vehicleType);
        double fee = feesCalculator.CalculateFees();
        string receiptPath = _receiptRepository.GenerateReceipt(vehicleType, fee);
        _receiptPrinter.PrintReceipt(receiptPath);
        _notifier.SendNotification(channels);
    }
}

public interface IFeesCalculator
{
    public double CalculateFees();
}

public interface IFeesCalculatorFactory
{
    public IFeesCalculator GenerateFeesCalculator(VehicleType vehicleType);
}
public class FeesCalculatorFactory: IFeesCalculatorFactory
{
    private readonly Dictionary<VehicleType, IFeesCalculator> _calculators = new()
    {
        [VehicleType.Car] = new CarFeeCalculator(),
        [VehicleType.Truck] = new TruckFeeCalculator(),
        [VehicleType.Bus] = new BusFeeCalculator(),
        [VehicleType.MotorCycle] = new MotorCycleFeeCalculator(),
    };
    public IFeesCalculator GenerateFeesCalculator(VehicleType vehicleType)
    {
        return _calculators[vehicleType];
    }
}

public class CarFeeCalculator : IFeesCalculator
{
    public double CalculateFees()
    {
        return 10.0;
    }
}
public class TruckFeeCalculator : IFeesCalculator
{
    public double CalculateFees()
    {
        return 20.0;
    }
}
public class BusFeeCalculator : IFeesCalculator
{
    public double CalculateFees()
    {
        return 30.0;
    }
}
public class MotorCycleFeeCalculator : IFeesCalculator
{
    public double CalculateFees()
    {
        return 5.0;
    }
}

public class ReceiptRepository: IReceiptRepository
{
    public string GenerateReceipt(VehicleType vehicleType, double fees)
    {
        string path = "example.txt";
        string content = $"Vehicle Type: {vehicleType.ToString()}\nFees:{fees}";
        File.WriteAllText(path, content);
        return path;
    }
}

public interface IReceiptRepository
{
    string GenerateReceipt(VehicleType vehicleType, double fees);
}

public interface IReceiptPrinter
{
    void PrintReceipt(string path);
}

public class ReceiptPrinter: IReceiptPrinter
{
    public void PrintReceipt(string path)
    {
        foreach (string line in File.ReadLines(path)) {
            Console.WriteLine(line);
        }
    }
}

public class Vehicle
{
    public int NumberOfWheels { get; }

    protected Vehicle(int numberOfWheels)
    {
        NumberOfWheels = numberOfWheels;
    }
}

public class Truck: Vehicle{
    public Truck(int numberOfWheels): base(numberOfWheels){}
}

public class Car : Vehicle
{
    public Car() : base(4) {}
}



public class MotorCycle : Vehicle
{
    public MotorCycle() : base(2) {}
}

public interface INotifier
{
    void SendNotification(NotificationChannels Channels);
}


public class Notifier : INotifier
{
    private readonly IEmailNotifier _emailNotifier;
    private readonly ISmsNotifier _smsNotifier;
    private readonly IPushNotifier _pushNotifier;

    public Notifier(IEmailNotifier emailNotifier, ISmsNotifier smsNotifier, IPushNotifier pushNotifier)
    {
        _emailNotifier = emailNotifier;
        _smsNotifier = smsNotifier;
        _pushNotifier = pushNotifier;
    }

    public void SendNotification(NotificationChannels channels)
    {
        if (channels.HasFlag(NotificationChannels.EMAIL))
            _emailNotifier.SendEmail("You have received a receipt");

        if (channels.HasFlag(NotificationChannels.SMS))
            _smsNotifier.SendSms("You have received a receipt");

        if (channels.HasFlag(NotificationChannels.PUSH))
            _pushNotifier.SendPush("You have received a receipt");
    }
}

public interface IEmailNotifier { void SendEmail(string message); }
public interface ISmsNotifier { void SendSms(string message); }
public interface IPushNotifier { void SendPush(string message); }

public class EmailNotifier: IEmailNotifier
{
    public void SendEmail(string message)
    {
        Console.WriteLine($"Email sent:\n{message}");
    }
}

public class SMSNotifier: ISmsNotifier
{
    public void SendSms(string message)
    {
        Console.WriteLine($"SMS sent:\n{message}");
    }
}

public class PushNotifier: IPushNotifier
{
    public void SendPush(string message)
    {
        Console.WriteLine($"Push sent:\n{message}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        ParkingGarage garage = new(new ReceiptRepository(), new ReceiptPrinter(), new Notifier(new EmailNotifier(), new SMSNotifier(), new PushNotifier()), new FeesCalculatorFactory());
        garage.ProcessCar(VehicleType.Bus, NotificationChannels.EMAIL | NotificationChannels.SMS);
    }
}