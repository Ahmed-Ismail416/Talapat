using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Entities.Order;
using TalabatCore.Repositories;
using TalabatCore.Services;
using TalabatCore.Services.IPayment;
using TalabatCore.Services.IUnitOfWork;
using TalabatCore.Specification.OrderSpec;

namespace TalabatService
{
    public class OrderService : IOrderService
    {
        public OrderService(IBasketRepositories BasketRepo, IUnitOfWork UnitOfWork, IPaymentService paymentservice)
        {
            this._BasketRepo = BasketRepo;
            this._UnitOfWork = UnitOfWork;
            _Paymentservice = paymentservice;
        }

        public IBasketRepositories _BasketRepo { get; }
        public IUnitOfWork _UnitOfWork { get; }
        public IPaymentService _Paymentservice { get; }

        public async Task<Order> CreateOrderAsync(string BuyerEmail, string BasketId, int DelvieryMethodId, Address ShippingAddress)
        {
            //Get Selected Item From Basket Repo, Ceating the OrderItem
            var Basket = await _BasketRepo.GetBasketAsync(BasketId);
            var OrderItems = new List<OrderItem>();
            if(Basket?.Items.Count()> 0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product = await _UnitOfWork.Repostiory<Product>().GetbyIdAsync(item.Id);
                    var ProductItemOrdered = new ProductItemOrdered(Product.Id, Product.Name, Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrdered, item.Quantity, Product.Price);
                    OrderItems.Add(OrderItem);
                }
            }
            // Get Subtotal
            var Subtotal = Basket.Items.Sum(b => b.Price * b.Quantity);
            // Get Delivery Method
            var DeliveryMethod =await _UnitOfWork.Repostiory<DeliveryMethod>().GetbyIdAsync(DelvieryMethodId);
            // Create Order
            var spec = new OrderPaymentIdSpec(Basket.PaymentIntentId);
            var ExOrder = await _UnitOfWork.Repostiory<Order>().GetEntityWithSpecAsync(spec);
            if(ExOrder != null)
            {
                // Order Already Exist
                _UnitOfWork.Repostiory<Order>().Delete(ExOrder);
                await _Paymentservice.CreateOrUpdatePaymentIntent(BasketId);
            }
            var Order = new Order(BuyerEmail, ShippingAddress, DeliveryMethod, OrderItems, Subtotal, Basket.PaymentIntentId);
            // Add Order Locally
            await _UnitOfWork.Repostiory<Order>().AddAsync(Order);
            // Add Order to DB 
            var Result = await _UnitOfWork.CompleteAsync();
            if (Result <= 0)
            {
                return null;
            }
            return Order;
        }

        public Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int OrderId)
        {
            var spec = new OrderSpecification(buyerEmail, OrderId);
            var Order = _UnitOfWork.Repostiory<Order>().GetEntityWithSpecAsync(spec);
            return Order;

        }

        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail);
            var Orders = await _UnitOfWork.Repostiory<Order>().GetAllWithSpecAsync(spec);
            return Orders;
        }

    }
}
