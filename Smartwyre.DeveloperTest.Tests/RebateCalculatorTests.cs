using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateCalculatorTests
{
    [Fact]
    public void FixedCashAmount_ReturnsRebateAmount_WhenValid()
    {
        var calculator = new FixedCashAmountCalculator();
        var rebate = new Rebate
        {
            Amount = 25m
        };
        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };
        var request = new CalculateRebateRequest();

        bool success = calculator.TryCalculate(
            rebate,
            product,
            request,
            out decimal rebateAmount);

        Assert.True(success);
        Assert.Equal(25m, rebateAmount);
    }

    [Fact]
    public void FixedRateRebate_ReturnsPriceTimesPercentageTimesVolume_WhenValid()
    {
        var calculator = new FixedRateRebateCalculator();
        var rebate = new Rebate
        {
            Percentage = 0.10m
        };
        var product = new Product
        {
            Price = 100m,
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
        };
        var request = new CalculateRebateRequest
        {
            Volume = 5m
        };

        bool success = calculator.TryCalculate(
            rebate,
            product,
            request,
            out decimal rebateAmount);

        Assert.True(success);
        Assert.Equal(50m, rebateAmount);
    }

    [Fact]
    public void AmountPerUom_ReturnsAmountTimesVolume_WhenValid()
    {
        var calculator = new AmountPerUomCalculator();
        var rebate = new Rebate
        {
            Amount = 3m
        };
        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.AmountPerUom
        };
        var request = new CalculateRebateRequest
        {
            Volume = 4m
        };

        bool success = calculator.TryCalculate(
            rebate,
            product,
            request,
            out decimal rebateAmount);

        Assert.True(success);
        Assert.Equal(12m, rebateAmount);
    }

    [Fact]
    public void FixedRateRebate_ReturnsFalse_WhenVolumeIsZero()
    {
        var calculator = new FixedRateRebateCalculator();
        var rebate = new Rebate
        {
            Percentage = 0.10m
        };
        var product = new Product
        {
            Price = 100m,
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
        };
        var request = new CalculateRebateRequest
        {
            Volume = 0m
        };

        bool success = calculator.TryCalculate(
            rebate,
            product,
            request,
            out decimal rebateAmount);

        Assert.False(success);
        Assert.Equal(0m, rebateAmount);
    }
}