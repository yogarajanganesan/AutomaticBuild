using AutomaticBuild.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;



namespace AutomaticBuild.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 1200 },
                new Product { Id = 2, Name = "Mouse", Price = 25 },
                new Product { Id = 3, Name = "Keyboard", Price = 45 }
            };

            return Ok(products);
        }
    }

  
}
