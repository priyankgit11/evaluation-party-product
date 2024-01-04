using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaluationPartyProduct.Models;
using EvaluationPartyProduct.DTO;
using EvaluationPartyProduct.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace EvaluationPartyProduct.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        public EvaluationDbContext context { get; set; }
        private readonly IMapper mapper;

        public ProductController(EvaluationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<bool> checkProductExists(string name)
        {
            var party = await context.TblProducts.FirstOrDefaultAsync(i => i.ProductName == name);
            if (party == null) return false;
            return true;
        }
        [HttpGet]
        public async Task<List<ProductDTO>> Get()
        {
            var products = await context.TblProducts.ToListAsync();
            var productsDTO = mapper.Map<List<ProductDTO>>(products);
            return productsDTO;
        }
        [HttpGet("assigned/{id:int}")]
        public async Task<List<ProductDTO>> GetOnlyAssigned(int id)
        {
            var partyId = id;
            var commonProducts = from product in context.TblProducts
                                 join assign in context.TblAssignParties on product.Id equals assign.ProductId
                                 join rate in context.TblProductRates on product.Id equals rate.ProductId
                                 where assign.PartyId == partyId
                                 select product;
            var products = await commonProducts.ToListAsync();
            var productsDTO = mapper.Map<List<ProductDTO>>(products);
            return productsDTO;
        }
        [HttpGet("getDistinct")]
        public async Task<List<ProductDTO>> GetDistinct()
        {
            var products = await context.TblInvoiceDetails.Include(i=>i.Product).Select(i=>i.Product).Distinct().ToListAsync();
            var productsDTO = mapper.Map<List<ProductDTO>>(products);
            return productsDTO;
        }
        [HttpGet("{id:int}", Name = "getProduct")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var products = await context.TblProducts.FirstOrDefaultAsync(x => x.Id == id);
            if (products == null) return NoContent();
            var productsDTO = mapper.Map<ProductDTO>(products);
            return Ok(productsDTO);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductCreationDTO productCreation)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await checkProductExists(productCreation.ProductName)) return Conflict("Duplicate data is not allowed.");
            var product = mapper.Map<TblProduct>(productCreation);
            context.Add(product);
            await context.SaveChangesAsync();
            var productDTO = mapper.Map<ProductDTO>(product);
            return new CreatedAtRouteResult("getProduct", new { Id = productDTO.Id }, productDTO);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var products = await context.TblProducts.FirstOrDefaultAsync(i => i.Id == id);
            if (products == null) return NotFound();
            context.TblProducts.Remove(products);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductCreationDTO productCreation)
        {
            var product = mapper.Map<TblProduct>(productCreation);
            if (await checkProductExists(productCreation.ProductName)) return Conflict("Duplicate data is not allowed.");
            product.Id = id;
            context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
