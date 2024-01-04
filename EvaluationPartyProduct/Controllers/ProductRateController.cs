using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaluationPartyProduct.DTO;
using EvaluationPartyProduct.Models;
using EvaluationPartyProduct.Helpers;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;

namespace EvaluationPartyProduct.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductRateController : ControllerBase
    {
        private readonly IMapper mapper;
        public EvaluationDbContext context { get; set; }

        public ProductRateController(EvaluationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<bool> checkRateExists(ProductRateCreationDTO rateDTO)
        {
            var productRate = await context.TblProductRates.FirstOrDefaultAsync(i=>i.ProductId == rateDTO.ProductId && i.Rate == rateDTO.Rate);
            if(productRate == null) { return false; }
            return true;
        }
        [HttpGet]
        public async Task<List<ProductRateRelationDTO>> Get()
        {
            var productRates = await context.TblProductRates.Include(i=>i.Product).ToListAsync();
            var productRatesDTO = mapper.Map<List<ProductRateRelationDTO>>(productRates);
            return productRatesDTO;
        }
        [HttpGet("{id:int}", Name = "getRate")]
        public async Task<ActionResult<ProductRateRelationDTO>> GetRate(int id)
        {
            var rate = await context.TblProductRates.FirstOrDefaultAsync(i => i.Id == id);
            if (rate == null) return NoContent();
            var productRateDTO = mapper.Map<ProductRateRelationDTO>(rate);
            return Ok(productRateDTO);
        }
        [HttpGet("getProductRate/{id:int}",Name = "getProductRate")]
        public async Task<ActionResult<ProductRateRelationDTO>> GetProductRate(int id)
        {
            var productRate = await context.TblProductRates.OrderByDescending(i => i.Id).FirstOrDefaultAsync(i => i.ProductId == id);
            if (productRate == null) return NoContent();
            var productRateDTO = mapper.Map<ProductRateRelationDTO>(productRate);
            return Ok(productRateDTO);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductRateCreationDTO productRateCreation)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if(await checkRateExists(productRateCreation)) { return Conflict("Duplicate data is not allowed"); }
            var productRate = mapper.Map<TblProductRate>(productRateCreation);
            context.TblProductRates.Add(productRate);
            await context.SaveChangesAsync();
            var productRateDTO = mapper.Map<ProductRateRelationDTO>(productRate);
            return new CreatedAtRouteResult("getRate", new { Id = productRateDTO.Id }, productRateDTO);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var productRate = await context.TblProductRates.FirstOrDefaultAsync(i => i.Id == id);
            if (productRate == null) return NotFound();
            context.TblProductRates.Remove(productRate);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductRateCreationDTO productRateCreation)
        {
            if (await checkRateExists(productRateCreation)) return Conflict("Duplicate data is not allowed");
            var productRate = mapper.Map<TblProductRate>(productRateCreation);
            productRate.Id = id;
            context.Entry(productRate).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
