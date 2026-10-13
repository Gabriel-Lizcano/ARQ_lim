using System.Collections.Generic;
using System.Security.Cryptography;
using Domain.Entities;

namespace Domain.Services;

public static class OrderService
{
    private static readonly List<Order> _lastOrders = new();
    public static IReadOnlyCollection<Order> LastOrders => _lastOrders.AsReadOnly();

    public static Order CreateOrder(string customer, string product, int qty, decimal price)
    {
        int secureId = RandomNumberGenerator.GetInt32(1, 10000000);

        var o = new Order
        {
            Id = secureId,
            CustomerName = customer,
            ProductName = product,
            Quantity = qty,
            UnitPrice = price
        };

        _lastOrders.Add(o);
        return o;
    }
}