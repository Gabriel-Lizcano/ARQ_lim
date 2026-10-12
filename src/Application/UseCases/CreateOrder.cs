using System.Threading;
using System;
using Domain.Entities;
using Domain.Services;

namespace Application.UseCases;

public class CreateOrderUseCase
{
    public Order Execute(string customer, string product, int qty, decimal price)
    {
        var order = OrderService.CreateOrder(customer, product, qty, price);

        Thread.Sleep(1500);

        return order;
    }
}
