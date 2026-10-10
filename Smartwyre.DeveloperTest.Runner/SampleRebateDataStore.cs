using System.Collections.Generic;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

public sealed class SampleRebateDataStore : IRebateDataStore
{
    private readonly Dictionary<string, Rebate> _rebates = new()
    {
        ["fixed-cash"] = new Rebate
        {
            Identifier = "fixed-cash",
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 100m
        },
        ["fixed-rate"] = new Rebate
        {
            Identifier = "fixed-rate",
            Incentive = IncentiveType.FixedRateRebate,
            Percentage = 0.10m
        },
        ["per-uom"] = new Rebate
        {
            Identifier = "per-uom",
            Incentive = IncentiveType.AmountPerUom,
            Amount = 2.50m
        }
    };

    public decimal? LastStoredAmount { get; private set; }

    public Rebate GetRebate(string rebateIdentifier)
    {
        _rebates.TryGetValue(rebateIdentifier, out Rebate rebate);
        return rebate;
    }

    public void StoreCalculationResult(Rebate rebate, decimal rebateAmount)
    {
        LastStoredAmount = rebateAmount;
    }
}