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
        private EvaluationDbContext context { get; set; }
        private ControllerHelperFunctions helpers;
        public ProductRateController(EvaluationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            helpers = new ControllerHelperFunctions(this.context);
        }
                [HttpGet]
        public async Task<List<ProductRateRelationDTO>> GetRate()
        {
            var productRates = await context.TblProductRates.Include(i=>i.Product).ToListAsync();
            var productRatesDTO = mapper.Map<List<ProductRateRelationDTO>>(productRates);
            return productRatesDTO;
        }
        [HttpGet("{id:int}", Name = "getRate")]
        public async Task<ActionResult<ProductRateRelationDTO>> GetSpecificRate(int id)
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
        public async Task<ActionResult> PostRate([FromBody] ProductRateCreationDTO productRateCreation)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if(await helpers.CheckRateExists(productRateCreation)) { return Conflict("Duplicate data is not allowed"); }
            var productRate = mapper.Map<TblProductRate>(productRateCreation);
            context.TblProductRates.Add(productRate);
            await context.SaveChangesAsync();
            var productRateDTO = mapper.Map<ProductRateRelationDTO>(productRate);
            return new CreatedAtRouteResult("getRate", new { Id = productRateDTO.Id }, productRateDTO);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRate(int id)
        {
            var productRate = await context.TblProductRates.FirstOrDefaultAsync(i => i.Id == id);
            if (productRate == null) return NotFound();
            context.TblProductRates.Remove(productRate);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutRate(int id, [FromBody] ProductRateCreationDTO productRateCreation)
        {
            if (await helpers.CheckRateExists(productRateCreation)) return Conflict("Duplicate data is not allowed");
            var productRate = mapper.Map<TblProductRate>(productRateCreation);
            productRate.Id = id;
            context.Entry(productRate).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
