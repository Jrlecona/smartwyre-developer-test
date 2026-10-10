using System.Collections.Generic;
using System.Linq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly IEnumerable<IRebateCalculator> _calculators;

    public RebateService(
        IRebateDataStore rebateDataStore,
        IProductDataStore productDataStore,
        IEnumerable<IRebateCalculator> calculators)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _calculators = calculators;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        if (request is null)
        {
            return new CalculateRebateResult();
        }

        Rebate rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        Product product = _productDataStore.GetProduct(request.ProductIdentifier);

        if (rebate is null || product is null)
        {
            return new CalculateRebateResult();
        }

        IRebateCalculator calculator = _calculators.FirstOrDefault(
            candidate => candidate.IncentiveType == rebate.Incentive);

        if (calculator is null ||
            !calculator.TryCalculate(rebate, product, request, out decimal rebateAmount))
        {
            return new CalculateRebateResult();
        }

        _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);

        return new CalculateRebateResult
        {
            Success = true
        };
    }
}