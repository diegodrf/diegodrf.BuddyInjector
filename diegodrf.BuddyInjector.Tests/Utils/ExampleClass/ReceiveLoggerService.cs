namespace diegodrf.BuddyInjector.Tests.Utils.ExampleClass;

public class ReceiveLoggerService
{
    public LoggerService<ReceiveLoggerService, bool> LoggerService { get; set; }

    public ReceiveLoggerService(LoggerService<ReceiveLoggerService, bool> loggerService)
    {
        LoggerService = loggerService;
    }
}