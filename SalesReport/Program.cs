
using System.Globalization;
using System.Text;

namespace SalesReport;

internal class Program
{
    private static void Main()
    {
        string projectDirectory = AppContext.BaseDirectory;
        string salesDirectory = Path.Combine(projectDirectory, "sales-data");

        
        string sourceDirectory = Path.Combine(
            Directory.GetCurrentDirectory(), "sales-data");

        if (!Directory.Exists(sourceDirectory))
        {
            Directory.CreateDirectory(sourceDirectory);
        }

        string[] salesFiles = Directory.GetFiles(
            sourceDirectory, "*.csv", SearchOption.TopDirectoryOnly);

        if (salesFiles.Length == 0)
        {
            Console.WriteLine(
                $"No sales CSV files were found in: {sourceDirectory}");
            Console.WriteLine(
                "Add at least one CSV file containing sales amounts, then run again.");
            return;
        }

        decimal grandTotal = 0m;
        var report = new StringBuilder();

        report.AppendLine("Sales Summary");
        report.AppendLine("----------------------------");

        var fileTotals = new List<(string FileName, decimal Total)>();

        foreach (string filePath in salesFiles)
        {
            decimal fileTotal = 0m;
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string value = line.Trim();

                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                if (decimal.TryParse(
                    value,
                    NumberStyles.Currency,
                    CultureInfo.InvariantCulture,
                    out decimal sale))
                {
                    fileTotal += sale;
                }
                else
                {
                    Console.WriteLine(
                        $"Warning: Skipping invalid sales value '{value}' in {Path.GetFileName(filePath)}.");
                }
            }

            fileTotals.Add((Path.GetFileName(filePath), fileTotal));
            grandTotal += fileTotal;
        }

        report.AppendLine(
            $"Total Sales: {grandTotal.ToString("C", CultureInfo.GetCultureInfo("en-US"))}");
        report.AppendLine();
        report.AppendLine("Details:");

        foreach (var item in fileTotals)
        {
            report.AppendLine(
                $" {item.FileName}: {item.Total.ToString("C", CultureInfo.GetCultureInfo("en-US"))}");
        }

        string reportPath = Path.Combine(
            Directory.GetCurrentDirectory(), "SalesSummary.txt");

        File.WriteAllText(reportPath, report.ToString());

        Console.WriteLine(report.ToString());
        Console.WriteLine($"Report saved to: {reportPath}");
    }
}
