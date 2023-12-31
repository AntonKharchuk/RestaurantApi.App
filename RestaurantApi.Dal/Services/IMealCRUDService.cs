﻿
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal.Services
{
    public interface IMealCRUDService: ICRUDService<Meal>
    {
        Task<IEnumerable<Meal>> GetItemsInRangeAsync(int startId, int endId, string include="");
    }
}
