using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaluationPartyProduct.Models;
using EvaluationPartyProduct.DTO;
using EvaluationPartyProduct.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace EvaluationPartyProduct.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PartyController : ControllerBase
    {
        private EvaluationDbContext context { get; set; }
        private ControllerHelperFunctions helpers;
        private readonly IMapper mapper;

        public PartyController(EvaluationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            helpers = new ControllerHelperFunctions(this.context);
        }
        [HttpGet]
        public async Task<List<PartyDTO>> GetParty()
        {
            var parties = await context.TblParties.ToListAsync();
            var partiesDTO = mapper.Map<List<PartyDTO>>(parties);
            return partiesDTO;
        }
        [HttpGet("{id:int}", Name = "getParty")]
        public async Task<ActionResult<PartyDTO>> GetSpecificParty(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var party = await context.TblParties.FirstOrDefaultAsync(x => x.Id == id);
            if (party == null) return NoContent();
            var partyDTO = mapper.Map<PartyDTO>(party);
            return Ok(partyDTO);
        }
        [HttpGet("assigned")]
        public async Task<List<PartyDTO>> GetPartyAssigned()
        {
            var commonParties = await context.TblAssignParties.Include(i => i.Party).GroupBy(i => new{ i.PartyId,i.Party.PartyName }).Select(
                i=> new PartyDTO
                {
                    PartyName = i.Key.PartyName,
                    Id = i.Key.PartyId
                }).ToListAsync();
            //var commonParties = await (from party in context.TblParties
            //                           join assign in context.TblAssignParties on party.Id equals assign.PartyId
            //                           group party by new { party.PartyName, party.Id } into partyGroup
            //                           select new PartyDTO
            //                           {
            //                               PartyName = partyGroup.Key.PartyName,
            //                               Id = partyGroup.Key.Id
            //                           }).ToListAsync();
            var partiesDTO = mapper.Map<List<PartyDTO>>(commonParties);
            return partiesDTO;
        }
        [HttpPost]
        public async Task<ActionResult> PostParty([FromBody] PartyCreationDTO partyCreation)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await helpers.CheckPartyExists(partyCreation.PartyName)) return Conflict("Duplicate data is not allowed.");
            var party = mapper.Map<TblParty>(partyCreation);
            context.Add(party);
            await context.SaveChangesAsync();
            var partyDTO = mapper.Map<PartyDTO>(party);
            return new CreatedAtRouteResult("getParty", new { Id = partyDTO.Id }, partyDTO);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteParty(int id)
        {
            var party = await context.TblParties.FirstOrDefaultAsync(i => i.Id == id);
            if (party == null) return NotFound();
            context.TblParties.Remove(party);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutParty(int id, [FromBody] PartyCreationDTO partyCreation)
        {
            var party = mapper.Map<TblParty>(partyCreation);
            if (await helpers.CheckPartyExists(partyCreation.PartyName)) return Conflict("Duplicate data is not allowed.");
            party.Id = id;
            context.Entry(party).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
