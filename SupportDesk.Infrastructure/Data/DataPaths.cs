namespace SupportDesk.Infrastructure.Data;

public static class DataPaths
{
    public static string DataDirectory => Path.Combine(AppContext.BaseDirectory, "Data");

    public static string CustomerFilePath => Path.Combine(DataDirectory, "customers.json");

    public static string TicketsFilePath => Path.Combine(DataDirectory, "tickets.json");

}
