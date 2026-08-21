using System;
using System.Collections.Generic;

[Flags]
public enum NotificationChannels
{
    EMAIL = 1 << 0,
    SMS = 1 << 1
}

public enum MemberType
{
    Standard,
    Premium,
    Student
}

public class CheckableLibraryItem: LibraryItem
{

    public int DaysCheckedOut { get; set; }

    public virtual double GetLateFee()
    {
        return DaysCheckedOut * 0.25;
    }
}

public class LibraryItem
{
    public string Title { get; set; } = string.Empty;
}

public class Book : CheckableLibraryItem{}

public class ReferenceBook : LibraryItem{}

public class AudioBook : LibraryItem
{
    public int HoursLength { get; set; }
}

public interface ILibraryService
{
    void CheckOut(CheckableLibraryItem item, string memberName);
    void Return(CheckableLibraryItem item);
    void PrintMonthlyReport();
    double CalculateLateFee(CheckableLibraryItem item, MemberType memberType);
    void SendOverdueMessage(NotificationChannels channels, string context);
}

public interface IReportGenereator
{
    string GenerateMonthlyReport();
}

public class ReportGenerator: IReportGenereator
{
    private readonly IReportRepository _reportRepository;
    public ReportGenerator(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }
    public string GenerateMonthlyReport()
    {
        string report = "---- Monthly Report ----\n";
        report += _reportRepository.GetCheckoutLogs();
        return report;
    }
}

public interface IReportManager
{
    public void PrintReport();
    public void AddCheckoutLog(string entry);
}

public class ReportManager: IReportManager
{
    private readonly IReportRepository _reportRepository;
    private readonly IReportGenereator _reportGenereator;

    public ReportManager( IReportRepository reportRepository, IReportGenereator reportGenereator)
    {
        _reportRepository = reportRepository;
        _reportGenereator = reportGenereator;
    }

    public void AddCheckoutLog(string entry)
    {
        _reportRepository.AddCheckoutLog(entry);
    }
    public void PrintReport()
    {
        Console.WriteLine(_reportGenereator.GenerateMonthlyReport());
    }
}

public interface IReportRepository
{
    public void AddCheckoutLog(string entry);
    public string GetCheckoutLogs();
}

public class ReportRepository: IReportRepository
{
    private readonly List<string> _checkedoutLogs = [];

    public void AddCheckoutLog(string entry)
    {
        _checkedoutLogs.Add(entry);
    }

    public string GetCheckoutLogs()
    {
        string logs = "";
        foreach( string log in _checkedoutLogs)
        {
            logs += $"{log}\n";
        }
        return logs;
    }
}

public interface ILateFeeCalculator
{
    public double CalculateLateFee(MemberType memberType, CheckableLibraryItem item);
}

public class LateFeeCalculator: ILateFeeCalculator
{
    private readonly Dictionary<MemberType, ILateFeeCalculator> _calculators = new()
    {
        [MemberType.Standard] = new StandardLateFeeCalculator(),
        [MemberType.Student] = new StudentLateFeeCalculator(),
        [MemberType.Premium] = new PremiumLateFeeCalculator()
    };
    public double CalculateLateFee(MemberType memberType, CheckableLibraryItem item)
    {
        return _calculators[memberType].CalculateLateFee(memberType, item);
    }
}

public class StudentLateFeeCalculator : ILateFeeCalculator
{

    public double CalculateLateFee(MemberType memberType, CheckableLibraryItem item)
    {
        return item.GetLateFee()*0.75;
    }
}

public class PremiumLateFeeCalculator : ILateFeeCalculator
{

    public double CalculateLateFee(MemberType memberType, CheckableLibraryItem item)
    {
        return item.GetLateFee()*0.5;
    }
}

public class StandardLateFeeCalculator : ILateFeeCalculator
{

    public double CalculateLateFee(MemberType memberType, CheckableLibraryItem item)
    {
        return item.GetLateFee();
    }
}



public interface IOverdueNotifier
{
    public void SendNotification(NotificationChannels channels, string context);
}


public class OverdueNotifier: IOverdueNotifier
{
    private readonly IEmailNotifier _emailNotifier;
    private readonly ISmsNotifier _smsNotifier;

    public OverdueNotifier( IEmailNotifier emailNotifier, ISmsNotifier smsNotifier)
    {
        _emailNotifier = emailNotifier;
        _smsNotifier = smsNotifier;
    }

    public void SendNotification(NotificationChannels channels, string context)
    {
        if (channels.HasFlag(NotificationChannels.SMS))
        {
            _smsNotifier.SendSms(context);
        }
        if(channels.HasFlag(NotificationChannels.EMAIL))
        {
            _emailNotifier.SendEmail(context);
        }
    }
}

public interface IEmailNotifier
{
    public void SendEmail(string email);
}

public interface ISmsNotifier
{
    public void SendSms(string phone);
}

public class EmailNotifier : IEmailNotifier
{
    public void SendEmail(string email)
    {
        Console.WriteLine($"Emailing {email}: your item is overdue.");
    }
}

public class SmsNotifier : ISmsNotifier
{
    public void SendSms(string phone)
    {
        Console.WriteLine($"Texting {phone}: your item is overdue.");
    }
}

public class LibraryManager : ILibraryService
{
    private readonly ILateFeeCalculator _lateFeeCalculator;
    private readonly IOverdueNotifier _overdueNotifier;
    private readonly IReportManager _reportManager;

    public LibraryManager(ILateFeeCalculator lateFeeCalculator, IOverdueNotifier overdueNotifier, IReportManager reportManager)
    {
        _lateFeeCalculator = lateFeeCalculator;
        _overdueNotifier = overdueNotifier;
        _reportManager = reportManager;
    }

    public void CheckOut(CheckableLibraryItem item, string memberName)
    {
        string entry = $"{item.Title} checked out by {memberName}";
        _reportManager.AddCheckoutLog(entry);
        Console.WriteLine(entry);
    }

    public void Return(CheckableLibraryItem item)
    {
        Console.WriteLine($"{item.Title} returned.");
    }

    public double CalculateLateFee(CheckableLibraryItem item, MemberType memberType)
    {
        return _lateFeeCalculator.CalculateLateFee(memberType, item);
    }

    public void SendOverdueMessage(NotificationChannels channels, string context)
    {
        _overdueNotifier.SendNotification(channels, context);
    }

    public void PrintMonthlyReport()
    {
        _reportManager.PrintReport();
    }
}

class Program
{
    static void Main(string[] args)
    {
        var reportRepository = new ReportRepository();
        var reportGenerator = new ReportGenerator(reportRepository);
        var reportManager = new ReportManager(reportRepository, reportGenerator);

        var overdueNotifier = new OverdueNotifier(
            new EmailNotifier(),
            new SmsNotifier());

        var lateFeeCalculator = new LateFeeCalculator();

        var manager = new LibraryManager(
            lateFeeCalculator,
            overdueNotifier,
            reportManager);


        Book book = new Book { Title = "Clean Code", DaysCheckedOut = 5 };
        manager.CheckOut(book, "Alice");

        double fee = manager.CalculateLateFee(book, MemberType.Student);
        Console.WriteLine($"Late fee: {fee}");

        manager.SendOverdueMessage(NotificationChannels.EMAIL,"alice@example.com");
        manager.PrintMonthlyReport();
    }
}