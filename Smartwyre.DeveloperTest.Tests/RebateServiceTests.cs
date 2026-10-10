using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTests
{
    [Fact]
    public void Calculate_StoresCalculatedAmount_WhenRequestIsValid()
    {
        var rebateDataStore = new FakeRebateDataStore
        {
            Rebate = new Rebate
            {
                Incentive = IncentiveType.FixedRateRebate,
                Percentage = 0.10m
            }
        };
        var productDataStore = new FakeProductDataStore
        {
            Product = new Product
            {
                Price = 100m,
                SupportedIncentives = SupportedIncentiveType.FixedRateRebate
            }
        };
        var service = new RebateService(
            rebateDataStore,
            productDataStore,
            CreateCalculators());
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "rebate-1",
            ProductIdentifier = "product-1",
            Volume = 5m
        };

        CalculateRebateResult result = service.Calculate(request);

        Assert.True(result.Success);
        Assert.Equal(1, rebateDataStore.StoreCallCount);
        Assert.Equal(50m, rebateDataStore.StoredAmount);
    }

    [Fact]
    public void Calculate_DoesNotStoreResult_WhenCalculationIsInvalid()
    {
        var rebateDataStore = new FakeRebateDataStore
        {
            Rebate = new Rebate
            {
                Incentive = IncentiveType.FixedRateRebate,
                Percentage = 0.10m
            }
        };
        var productDataStore = new FakeProductDataStore
        {
            Product = new Product
            {
                Price = 100m,
                SupportedIncentives = SupportedIncentiveType.FixedRateRebate
            }
        };
        var service = new RebateService(
            rebateDataStore,
            productDataStore,
            CreateCalculators());
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "rebate-1",
            ProductIdentifier = "product-1",
            Volume = 0m
        };

        CalculateRebateResult result = service.Calculate(request);

        Assert.False(result.Success);
        Assert.Equal(0, rebateDataStore.StoreCallCount);
    }

    [Fact]
    public void Calculate_DoesNotStoreResult_WhenCalculatorIsNotRegistered()
    {
        var rebateDataStore = new FakeRebateDataStore
        {
            Rebate = new Rebate
            {
                Incentive = (IncentiveType)999
            }
        };
        var productDataStore = new FakeProductDataStore
        {
            Product = new Product()
        };
        var service = new RebateService(
            rebateDataStore,
            productDataStore,
            CreateCalculators());
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "rebate-1",
            ProductIdentifier = "product-1",
            Volume = 5m
        };

        CalculateRebateResult result = service.Calculate(request);

        Assert.False(result.Success);
        Assert.Equal(0, rebateDataStore.StoreCallCount);
    }

    private static IRebateCalculator[] CreateCalculators()
    {
        return
        [
            new FixedCashAmountCalculator(),
            new FixedRateRebateCalculator(),
            new AmountPerUomCalculator()
        ];
    }

    private sealed class FakeRebateDataStore : IRebateDataStore
    {
        public Rebate Rebate { get; set; }

        public int StoreCallCount { get; private set; }

        public decimal StoredAmount { get; private set; }

        public Rebate GetRebate(string rebateIdentifier)
        {
            return Rebate;
        }

        public void StoreCalculationResult(Rebate rebate, decimal rebateAmount)
        {
            StoreCallCount++;
            StoredAmount = rebateAmount;
        }
    }

    private sealed class FakeProductDataStore : IProductDataStore
    {
        public Product Product { get; set; }

        public Product GetProduct(string productIdentifier)
        {
            return Product;
        }
    }
}