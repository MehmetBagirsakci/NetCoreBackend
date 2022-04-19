using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _productService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(Product product)
        {
            var result = _productService.Add(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        //[HttpGet]
        //public List<Product> Get()
        //{
        //    IProductService productService = new ProductManager(new EfProductDal());
        //    var result = productService.GetAll();
        //    return result.Data;
        //}



        //[HttpGet]
        //public List<Product> Get()
        //{
        //    return new List<Product>
        //    {
        //        new Product{ProductId=1, ProductName="Elma"},
        //        new Product{ProductId=2, ProductName="Armut"}
        //    };
        //}



        //[HttpGet]
        //public string Get()
        //{
        //    return "Merhaba";
        //}
    }
}
