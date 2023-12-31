﻿
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal.Services
{
    public interface IOrderItemCRUDService: ICRUDService<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetItemByOrderIdAsync(int id, string includes = "");
    }
}
