using System.Collections.Generic;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

public sealed class SampleProductDataStore : IProductDataStore
{
    private readonly Dictionary<string, Product> _products = new()
    {
        ["standard"] = new Product
        {
            Identifier = "standard",
            Price = 50m,
            SupportedIncentives =
                SupportedIncentiveType.FixedCashAmount |
                SupportedIncentiveType.FixedRateRebate |
                SupportedIncentiveType.AmountPerUom
        }
    };

    public Product GetProduct(string productIdentifier)
    {
        _products.TryGetValue(productIdentifier, out Product product);
        return product;
    }
}