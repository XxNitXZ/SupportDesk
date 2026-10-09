using System;
using System.IO;

namespace SupportDesk.Infrastructure.Data;

public static class DataPaths
{
    public static string DataDirectory =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SupportDesk",
            "Data");

    public static string CustomerFilePath =>
        Path.Combine(DataDirectory, "customers.json");

    public static string TicketsFilePath =>
        Path.Combine(DataDirectory, "tickets.json");
}