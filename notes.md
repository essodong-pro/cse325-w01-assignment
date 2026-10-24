# W01 Assignment: Build .NET Applications with C#

## 1. Pizza API Evidence

**Screenshot:** `evidence/pizzas.png`

The screenshot shows the pizzas returned by the GET endpoint, including the additional `BBQ Chicken Pizza` added to the seed data in `PizzaService`.

## 2. API Requests, Responses, and Status Codes

### GET — Retrieve all pizzas

**Request:**
`GET http://localhost:5102/api/Pizzas`

**Response:** The API returned a JSON array containing four pizzas:

- Classic Margherita
- Pepperoni
- Vegetarian
- BBQ Chicken Pizza

**Status code:** `200 OK`

### POST — Create a pizza

**Request:**
`POST http://localhost:5102/api/Pizzas`

**Request body:**

```json
{
  "name": "Hawaiian",
  "description": "Ham and pineapple",
  "price": 12.50,
  "isGlutenFree": false
}
```

**Response body:**

```json
{
  "id": 5,
  "name": "Hawaiian",
  "description": "Ham and pineapple",
  "price": 12.5,
  "isGlutenFree": false
}
```

**Status code:** `201 Created`

### PUT — Update a pizza

**Request:**
`PUT http://localhost:5102/api/Pizzas/2`

**Request body:**

```json
{
  "id": 2,
  "name": "Extra Cheese Pepperoni",
  "description": "Pepperoni with extra mozzarella",
  "price": 14.50,
  "isGlutenFree": false
}
```

**Response:** No response body was returned.

**Status code:** `204 No Content`

### DELETE — Delete a pizza

**Request:**
`DELETE http://localhost:5102/api/Pizzas/2`

**Response:** No response body was returned.

**Status code:** `204 No Content`

## 3. Sales Summary Function

The `SalesReport` application reads sales amounts from CSV files in the `sales-data` directory. It calculates the total for each file, adds the file totals to calculate the grand total, and saves the summary to `SalesSummary.txt`.

**Test results:**

Sales Summary
----------------------------
Total Sales: $8,767.25

Details:
 February.csv: $2,536.25
 January.csv: $2,480.75
 March.csv: $3,750.25


**Complete working function:**
```csharp
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
```

**Generated report:** `SalesSummary.txt`

The application was run successfully, and I verified that the generated report contains the correct grand total and individual file totals.

The complete working sales summary function is implemented in `SalesReport/Program.cs`.
