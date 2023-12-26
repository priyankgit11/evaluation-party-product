using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaluationPartyProduct.DTO;
using EvaluationPartyProduct.Models;
using EvaluationPartyProduct.Helpers;

namespace EvaluationPartyProduct.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductRateController : ControllerBase
    {
        private readonly IMapper mapper;
        public EvaluationDbContext context { get; }

        public ProductRateController(EvaluationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
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
            var productRate = mapper.Map<TblProduct>(productRateCreation);
            productRate.Id = id;
            context.Entry(productRate).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ProductRateCreationDTO> patchDocument)
        {
            if (patchDocument == null) return BadRequest();
            var entityFromDB = await context.TblProductRates.FirstOrDefaultAsync(x => x.Id == id);
            if (entityFromDB == null) return NotFound();
            var entityDTO = mapper.Map<ProductRateCreationDTO>(entityFromDB);
            patchDocument.ApplyTo(entityDTO, ModelState);
            var isValid = TryValidateModel(entityDTO);
            if (!isValid) return BadRequest(ModelState);
            mapper.Map(entityDTO, entityFromDB);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
