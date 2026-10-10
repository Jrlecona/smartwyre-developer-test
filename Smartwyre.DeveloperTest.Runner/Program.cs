using System;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

public static class Program
{
    public static int Main(string[] args)
    {
        if (!TryReadRequest(args, out CalculateRebateRequest request))
        {
            return 1;
        }

        var rebateDataStore = new SampleRebateDataStore();
        var productDataStore = new SampleProductDataStore();

        IRebateCalculator[] calculators =
        [
            new FixedCashAmountCalculator(),
            new FixedRateRebateCalculator(),
            new AmountPerUomCalculator()
        ];

        var service = new RebateService(
            rebateDataStore,
            productDataStore,
            calculators);

        CalculateRebateResult result = service.Calculate(request);

        if (!result.Success)
        {
            Console.WriteLine("The rebate could not be calculated.");
            return 1;
        }

        Console.WriteLine(
            $"Calculation succeeded. Rebate amount: {rebateDataStore.LastStoredAmount:0.00}");

        return 0;
    }

    private static bool TryReadRequest(
        string[] args,
        out CalculateRebateRequest request)
    {
        request = null;

        if (args.Length == 0)
        {
            return TryReadInteractiveRequest(out request);
        }

        if (args.Length != 3 || !decimal.TryParse(args[2], out decimal volume))
        {
            PrintUsage();
            return false;
        }

        request = new CalculateRebateRequest
        {
            RebateIdentifier = args[0],
            ProductIdentifier = args[1],
            Volume = volume
        };

        return true;
    }

    private static bool TryReadInteractiveRequest(
        out CalculateRebateRequest request)
    {
        Console.Write("Rebate identifier (fixed-cash, fixed-rate, or per-uom): ");
        string rebateIdentifier = Console.ReadLine();

        Console.Write("Product identifier (standard): ");
        string productIdentifier = Console.ReadLine();

        Console.Write("Volume: ");
        string volumeInput = Console.ReadLine();

        if (!decimal.TryParse(volumeInput, out decimal volume))
        {
            Console.WriteLine("Volume must be a valid number.");
            request = null;
            return false;
        }

        request = new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        };

        return true;
    }

    private static void PrintUsage()
    {
        Console.WriteLine(
            "Usage: dotnet run --project Smartwyre.DeveloperTest.Runner -- " +
            "<rebate> <product> <volume>");
        Console.WriteLine("Rebates: fixed-cash, fixed-rate, per-uom");
        Console.WriteLine("Products: standard");
    }
}