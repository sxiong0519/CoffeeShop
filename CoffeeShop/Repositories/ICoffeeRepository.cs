using CoffeeShop.Models;
using System.Collections.Generic;

namespace CoffeeShop.Repositories
{
    public interface ICoffeeRepository
    {
        void AddCoffee(Coffee coffee);
        void DeleteCoffee(int id);
        void EditCoffee(Coffee coffee);
        List<Coffee> GetAll();
        Coffee GetCoffeeById(int id);
    }
}