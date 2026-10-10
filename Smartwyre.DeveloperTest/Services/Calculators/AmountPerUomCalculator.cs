using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

public sealed class AmountPerUomCalculator : IRebateCalculator
{
    public IncentiveType IncentiveType => IncentiveType.AmountPerUom;

    public bool TryCalculate(
        Rebate rebate,
        Product product,
        CalculateRebateRequest request,
        out decimal rebateAmount)
    {
        rebateAmount = 0m;

        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom) ||
            rebate.Amount == 0 ||
            request.Volume == 0)
        {
            return false;
        }

        rebateAmount = rebate.Amount * request.Volume;
        return true;
    }
}