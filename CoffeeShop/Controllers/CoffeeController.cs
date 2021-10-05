using CoffeeShop.Models;
using CoffeeShop.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeController : ControllerBase
    {
        private readonly ICoffeeRepository _coffeeRepository;

        public CoffeeController (ICoffeeRepository coffeeRepository)
        {
            _coffeeRepository = coffeeRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_coffeeRepository.GetAll());
        }

        // https://localhost:5001/api/beanvariety/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var coffee = _coffeeRepository.GetCoffeeById(id);
            if (coffee == null)
            {
                return NotFound();
            }
            return Ok(coffee);
        }

        // https://localhost:5001/api/beanvariety/
        [HttpPost]
        public IActionResult Post(Coffee coffee)
        {
            _coffeeRepository.AddCoffee(coffee);
            return CreatedAtAction("Get", new { id = coffee.Id }, coffee);
        }
    }
}
