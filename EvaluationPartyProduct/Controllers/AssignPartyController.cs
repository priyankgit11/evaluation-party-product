using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using EvaluationPartyProduct.DTO;
using EvaluationPartyProduct.Models;
using EvaluationPartyProduct.Helpers;

namespace EvaluationPartyProduct.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignPartyController : ControllerBase
    {
        public EvaluationDbContext context { get; }
        private readonly IMapper mapper;
        public AssignPartyController(EvaluationDbContext Context, IMapper mapper)
        {
            context = Context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<List<AssignPartyRelationDTO>> Get()
        {
            var assignParties = await context.TblAssignParties.Include(i=>i.Party).Include(i=>i.Product).ToListAsync();
            var assignPartiesDTO = mapper.Map<List<AssignPartyRelationDTO>>(assignParties);
            return assignPartiesDTO;
        }
        [HttpGet("{id:int}", Name = "getAssignParty")]
        public async Task<ActionResult<AssignPartyRelationDTO>> Get(int id)
        {
            var assignParty = await context.TblAssignParties.FindAsync(id);
            if (assignParty == null) return NoContent();
            var assignPartyDTO = mapper.Map<AssignPartyRelationDTO>(assignParty);
            return Ok(assignPartyDTO);
        }
        [HttpGet("byParty/{id}", Name = "ByParty")]
        public async Task<ActionResult<List<AssignPartyRelationDTO>>> GetByParty(int id)
        {
            var assignParties = await context.TblAssignParties.Where(i => i.PartyId == id).ToListAsync();
            if (assignParties == null) return NoContent();
            var assignPartiesDTO = mapper.Map<AssignPartyRelationDTO>(assignParties);
            return Ok(assignPartiesDTO);
        }
        [HttpGet("byProduct/{id}", Name = "ByProduct")]
        public async Task<ActionResult<List<AssignPartyRelationDTO>>> GetByProduct(int id)
        {
            var assignParties = await context.TblAssignParties.Where(i => i.ProductId == id).ToListAsync();
            if (assignParties == null) return NoContent();
            var assignPartiesDTO = mapper.Map<AssignPartyRelationDTO>(assignParties);
            return Ok(assignPartiesDTO);
        }
        [HttpPost]
        public async Task<ActionResult<AssignPartyRelationDTO>> Post([FromBody] AssignPartyCreationDTO assignPartyCreation)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var assignParty = mapper.Map<TblAssignParty>(assignPartyCreation);
            context.TblAssignParties.Add(assignParty);
            await context.SaveChangesAsync();
            var productRateDTO = mapper.Map<AssignPartyRelationDTO>(assignParty);
            return new CreatedAtRouteResult("getAssignParty", new { Id = productRateDTO.Id }, productRateDTO);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var assignParty = await context.TblAssignParties.FirstOrDefaultAsync(i => i.Id == id);
            if (assignParty == null) return NotFound();
            context.TblAssignParties.Remove(assignParty);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] AssignPartyCreationDTO assignPartyCreation)
        {
            var assignParty = mapper.Map<TblAssignParty>(assignPartyCreation);
            assignParty.Id = id;
            context.Entry(assignParty).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<AssignPartyCreationDTO> patchDocument)
        {
            if (patchDocument == null) return BadRequest();
            var entityFromDB = await context.TblAssignParties.FirstOrDefaultAsync(x => x.Id == id);
            if (entityFromDB == null) return NotFound();
            var entityDTO = mapper.Map<AssignPartyCreationDTO>(entityFromDB);
            patchDocument.ApplyTo(entityDTO, ModelState);
            var isValid = TryValidateModel(entityDTO);
            if (!isValid) return BadRequest(ModelState);
            mapper.Map(entityDTO, entityFromDB);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
