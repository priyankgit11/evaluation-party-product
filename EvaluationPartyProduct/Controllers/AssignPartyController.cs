using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using EvaluationPartyProduct.DTO;
using EvaluationPartyProduct.Models;
using EvaluationPartyProduct.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace EvaluationPartyProduct.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssignPartyController : ControllerBase
    {
        private readonly EvaluationDbContext context;
        private readonly ControllerHelperFunctions helpers;
        private readonly IMapper mapper;
        public AssignPartyController(EvaluationDbContext Context, IMapper mapper)
        {
            this.context = Context;
            this.mapper = mapper;
            helpers = new ControllerHelperFunctions(this.context);
        }
        [HttpGet]
        public async Task<List<AssignPartyRelationDTO>> GetAssignParty()
        {
            var assignParties = await context.TblAssignParties.Include(i => i.Party).Include(i => i.Product).ToListAsync();
            var assignPartiesDTO = mapper.Map<List<AssignPartyRelationDTO>>(assignParties);
            return assignPartiesDTO;
        }
        [HttpGet("{id:int}", Name = "getAssignParty")]
        public async Task<ActionResult<AssignPartyRelationDTO>> GetSpecificAssignParty(int id)
        {
            var assignParty = await context.TblAssignParties.Include(i => i.Party).Include(i => i.Product).FirstOrDefaultAsync(i => i.Id == id);
            if (assignParty == null) return NoContent();
            var assignPartyDTO = mapper.Map<AssignPartyRelationDTO>(assignParty);
            return Ok(assignPartyDTO);
        }
        [HttpGet("byParty/{id}", Name = "ByParty")]
        public async Task<ActionResult<List<AssignPartyRelationDTO>>> GetAssignPartyByParty(int id)
        {
            var assignParties = await context.TblAssignParties.Where(i => i.PartyId == id).ToListAsync();
            if (assignParties == null) return NoContent();
            var assignPartiesDTO = mapper.Map<AssignPartyRelationDTO>(assignParties);
            return Ok(assignPartiesDTO);
        }
        [HttpGet("byProduct/{id}", Name = "ByProduct")]
        public async Task<ActionResult<List<AssignPartyRelationDTO>>> GetAssignPartyByProduct(int id)
        {
            var assignParties = await context.TblAssignParties.Where(i => i.ProductId == id).ToListAsync();
            if (assignParties == null) return NoContent();
            var assignPartiesDTO = mapper.Map<AssignPartyRelationDTO>(assignParties);
            return Ok(assignPartiesDTO);
        }
        [HttpPost]
        public async Task<ActionResult<AssignPartyRelationDTO>> PostAssignParty([FromBody] AssignPartyCreationDTO assignPartyCreation)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await helpers.CheckAssignExists(assignPartyCreation)) return Conflict("Duplicate data not allowed");
            var assignParty = mapper.Map<TblAssignParty>(assignPartyCreation);
            context.TblAssignParties.Add(assignParty);
            await context.SaveChangesAsync();
            var productRateDTO = mapper.Map<AssignPartyRelationDTO>(assignParty);
            return new CreatedAtRouteResult("getAssignParty", new { Id = productRateDTO.Id }, productRateDTO);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAssignParty(int id)
        {
            var assignParty = await context.TblAssignParties.FirstOrDefaultAsync(i => i.Id == id);
            if (assignParty == null) return NotFound();
            context.TblAssignParties.Remove(assignParty);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAssignParty(int id, [FromBody] AssignPartyCreationDTO assignPartyCreation)
        {
            if (await helpers.CheckAssignExists(assignPartyCreation)) return Conflict("Duplicate data not allowed");
            var assignParty = mapper.Map<TblAssignParty>(assignPartyCreation);
            assignParty.Id = id;
            context.Entry(assignParty).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
