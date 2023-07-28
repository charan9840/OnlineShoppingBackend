using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OnlineShoppingBackend.Data;
using OnlineShoppingBackend.Models;

namespace OnlineShoppingBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ProductController : ControllerBase
    {
        private readonly IMongoCollection<Product> _product;
        private readonly IDatabaseSettings _settings;

        public ProductController(IConfiguration configuration, IDatabaseSettings settings)
        {
            _settings = settings;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _product = database.GetCollection<Product>("Products");
        }

        /// <summary>
        /// ViewAllProducts
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllProducts/user")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllProducts()
        {
            StatusMessage response = new StatusMessage();
            var temp = await _product.Find(f => true).ToListAsync();
            if (temp != null)
            {
                response.Status = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Value = temp;
                response.Message = "Product List";
                return Ok(response);
            }
            else
            {
                response.Status = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "Product not found";
                return BadRequest(response);
            }
        }

        /// <summary>
        /// get by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById/user")]
        [AllowAnonymous]
        public async Task<ActionResult> GetById(string Id)
        {
            StatusMessage response = new StatusMessage();
            var temp = await _product.Find(f => f.Id == Id).FirstOrDefaultAsync();
            if (temp != null)
            {
                response.Status = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Value = temp;
                response.Message = "Product List";
                return Ok(response);
            }
            else
            {
                response.Status = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "Product not found";
                return BadRequest(response);
            }
        }


        /// <summary>
        /// GetProductByName
        /// </summary>
        /// <param name="ProductName"></param>
        /// <returns></returns>
        [HttpGet("GetByName/user")]
        [AllowAnonymous]
        public async Task<ActionResult> GetByName(string ProductName)
        {
            StatusMessage response = new StatusMessage();
            var temp = _product.Find(x => x.ProductName == ProductName).FirstOrDefault();
            if (temp != null)
            {
                response.Status = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Value = temp;
                response.Message = "Product List";
                return Ok(response);
            }
            else
            {
                response.Status = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "No Products to show";
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Add Product By Admin
        /// </summary>
        /// <param name="NewProduct"></param>
        /// <returns></returns>
        [HttpPost("AddProduct/Admin")]
        public async Task<ActionResult> AddProduct(ProductDetailsDto NewProduct)
        {
            bool Temp = await _product.Find(m => m.ProductName == NewProduct.ProductName).AnyAsync();

            if (!Temp)
            {
                Product temp = new Product
                {
                    ProductName = NewProduct.ProductName,
                    Price = NewProduct.Price,
                    Brand = NewProduct.Brand,
                    Features = NewProduct.Features,
                    ProductDescription = NewProduct.ProductDescription,
                    ImageURL = NewProduct.ImageURL
                };
                _product.InsertOne(temp);
                return Ok("Product added Successfully");
            }
            else
            {
                return BadRequest("Product Already exist");
            }
        }

        /// <summary>
        /// Update Product by Admin
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("UpdateProduct/Admin")]
        public async Task<ActionResult> UpdateProduct(string Id, ProductDetailsDto product)
        {
            StatusMessage response = new StatusMessage();

            var Temp = await _product.Find(m => m.Id == Id).FirstOrDefaultAsync();

            if (Temp != null)
            {
                Temp.ProductName = product.ProductName;
                Temp.Price = product.Price;
                Temp.Brand = product.Brand;
                Temp.Features = product.Features;
                Temp.ProductDescription = product.ProductDescription;
                Temp.ImageURL = product.ImageURL;
                await _product.ReplaceOneAsync(x => x.Id == Id, Temp);
                response.Status = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = "Product details Updated successfully";
                return Ok(response);
            }
            else
            {
                response.Status = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "Product not found";
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Delete Product by Admin
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteProduct/Admin")]
        public async Task<ActionResult> UserDelete(string Id)
        {
            StatusMessage response = new StatusMessage();
            var Temp = await _product.Find(x => x.Id == Id).FirstOrDefaultAsync();

            if (Temp != null)
            {
                await _product.DeleteOneAsync(x => x.Id == Id);
                response.Status = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = "Product has been deleted successfully";
                return Ok(response);
            }
            else
            {
                response.Status = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "Product not found";
                return BadRequest(response);
            }
        }
    }
}
