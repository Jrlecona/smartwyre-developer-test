using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

public sealed class FixedRateRebateCalculator : IRebateCalculator
{
    public IncentiveType IncentiveType => IncentiveType.FixedRateRebate;

    public bool TryCalculate(
        Rebate rebate,
        Product product,
        CalculateRebateRequest request,
        out decimal rebateAmount)
    {
        rebateAmount = 0m;

        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate) ||
            rebate.Percentage == 0 ||
            product.Price == 0 ||
            request.Volume == 0)
        {
            return false;
        }

        rebateAmount = product.Price * rebate.Percentage * request.Volume;
        return true;
    }
}