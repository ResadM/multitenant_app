using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using multitenant_app.Context;
using multitenant_app.Models;
using multitenant_app.Models.UserModels;

namespace multitenant_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContextUser _databaseContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(DatabaseContextUser databaseContext, UserManager<ApplicationUser> userManager)
        {
            _databaseContext = databaseContext;
            _userManager = userManager;
        }


        [HttpPost("SeedData")]
        public async Task<IActionResult> SeedData()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                //Seed data
                List<Products> products = new List<Products>();
                Random random = new Random();
                for (int i = 0; i < 10; i++)
                {
                    products.Add(new Products
                    {
                        ProductName = "Product" + i,
                        ProductPrice = random.Next(1, 100),
                        ProductCode = user.FirstName + "_" + i
                    });
                }
                _databaseContext.Products.AddRange(products);
                await _databaseContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("getProducts")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                //Get products
                var products = await _databaseContext.Products.ToListAsync();
                return Ok(products);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
