﻿using AutoMapper;
using EvaluationPartyProduct.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EvaluationPartyProduct.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EvaluationPartyProduct.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private EvaluationDbContext context;
        private readonly IMapper mapper;

        public InvoiceController(IMapper mapper, EvaluationDbContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task CalculateGrandTotals(List<InvoiceDTO> invoiceDTO)
        {
            foreach (var item in invoiceDTO)
            {
                var invoiceDetails = await context.TblInvoiceDetails.Where(i => i.InvoiceId == item.Id).ToListAsync();
                decimal gt = 0;
                invoiceDetails.ForEach(i =>
                {
                    gt += i.Rate * i.Quantity;
                });
                item.GrandTotal = gt;
            }
        }

        public void CalculateTotals(List<InvoiceDetailDTO> invoiceDetailDTO)
        {
            invoiceDetailDTO.ForEach(i =>
            {
                i.Total = i.Rate * i.Quantity;
            });
        }

        [HttpGet]
        public async Task<ActionResult<List<InvoiceDTO>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 1, [FromQuery] string filter = "", bool sort = true)
        {
            if (page > pageSize || page <= 0 || pageSize <= 0)
            {
                return BadRequest();
            }
            IQueryable<TblInvoice> query;
            if (!string.IsNullOrEmpty(filter))
            {
                query = sort ? context.TblInvoices.Include(i => i.Party).Where(i => i.Party.PartyName.Contains(filter)).OrderBy(i => i.Party.PartyName).Skip((page - 1) * pageSize).Take(pageSize)
                    :
                    context.TblInvoices.Include(i => i.Party).Where(i => i.Party.PartyName.Contains(filter)).OrderByDescending(i => i.Party.PartyName).Skip((page - 1) * pageSize).Take(pageSize)
;
            }
            else
            {
                query = sort ? context.TblInvoices.Include(i => i.Party).Where(i => i.Party.PartyName.Contains(filter)).OrderBy(i => i.Party.PartyName).Skip((page - 1) * pageSize).Take(pageSize)
                    :
                    context.TblInvoices.Include(i => i.Party).Where(i => i.Party.PartyName.Contains(filter)).OrderByDescending(i => i.Party.PartyName).Skip((page - 1) * pageSize).Take(pageSize)
;
            }

            var invoices = await query.ToListAsync();
            var invoicesDTO = mapper.Map<List<InvoiceDTO>>(invoices);
            await CalculateGrandTotals(invoicesDTO);
            return Ok(invoicesDTO);
        }

        [HttpGet("InvoiceDetails")]
        public async Task<ActionResult<List<InvoiceDetailDTO>>> GetDetails([FromQuery] int page = 1, [FromQuery] int pageSize = 1, [FromQuery] string filter = "", bool sort = true)
        {
            if (page > pageSize || page <= 0 || pageSize <= 0)
            {
                return BadRequest();
            }
            IQueryable<TblInvoiceDetail> query;
            if (!string.IsNullOrEmpty(filter))
            {
                query = sort ? context.TblInvoiceDetails.Include(i => i.Product).Include(i=>i.Invoice).ThenInclude(i=>i.Party).OrderBy(i => i.Product.ProductName).Skip((page - 1) * pageSize).Take(pageSize)
                    :
                    context.TblInvoiceDetails.Include(i => i.Product).Include(i=>i.Invoice).ThenInclude(i=>i.Party).OrderByDescending(i => i.Product.ProductName).Skip((page - 1) * pageSize).Take(pageSize)
;
            }
            else
            {
                query = sort ? context.TblInvoiceDetails.Include(i => i.Product).Include(i=>i.Invoice).ThenInclude(i=>i.Party).Where(i => i.Product.ProductName.Contains(filter)).OrderBy(i => i.Product.ProductName).Skip((page - 1) * pageSize).Take(pageSize)
                    :
                    context.TblInvoiceDetails.Include(i => i.Product).Include(i=>i.Invoice).ThenInclude(i=>i.Party).Where(i => i.Product.ProductName.Contains(filter)).OrderByDescending(i => i.Product.ProductName).Skip((page - 1) * pageSize).Take(pageSize)
;
            }

            var invoices = await query.ToListAsync();
            var invoicesDTO = mapper.Map<List<InvoiceDTO>>(invoices);
            await CalculateGrandTotals(invoicesDTO);
            return Ok(invoicesDTO);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<List<InvoiceDetailDTO>>> Get(int id)
        {
            var invoiceDetails = await context.TblInvoiceDetails.Include(i => i.Invoice).ThenInclude(i => i.Party).Include(i => i.Product).Where(i => i.InvoiceId == id).ToListAsync();
            if (invoiceDetails == null) return NoContent();
            var invoiceDetailDTO = mapper.Map<List<InvoiceDetailDTO>>(invoiceDetails);
            CalculateTotals(invoiceDetailDTO);
            return Ok(invoiceDetailDTO);
        }
        [HttpGet("InvoiceDetails/{id:int}", Name = "getInvoiceDetails")]
        public async Task<ActionResult<InvoiceDetailDTO>> GetDetail(int id)
        {
            var invoiceDetail = await context.TblInvoiceDetails.Include(i => i.Invoice).ThenInclude(i => i.Party).Include(i => i.Product).Where(i => i.Id == id).FirstOrDefaultAsync();
            if (invoiceDetail == null) return NoContent();
            var invoiceDetailDTO = mapper.Map<InvoiceDetailDTO>(invoiceDetail);
            return Ok(invoiceDetailDTO);
        }
        [HttpPost]
        public async Task<ActionResult<InvoiceDTO>> Post([FromBody] InvoiceCreationDTO invoiceCreation)
        {
            if (!ModelState.IsValid) return BadRequest();
            var invoice = mapper.Map<TblInvoice>(invoiceCreation);
            context.Add(invoice);
            await context.SaveChangesAsync();
            var invoiceDTO = mapper.Map<InvoiceDTO>(invoice);
            var invoiceDetails = await context.TblInvoiceDetails.Where(i => i.InvoiceId == invoiceDTO.Id).ToListAsync();
            decimal gt = 0;
            invoiceDetails.ForEach(i =>
            {
                gt += i.Rate * i.Quantity;
            });
            invoiceDTO.GrandTotal = gt;
            return Ok(invoiceDTO);
        }
        [HttpPost("/InvoiceDetails")]
        public async Task<ActionResult> Post([FromBody] InvoiceDetailCreationDTO invoiceDetailCreation)
        {
            if (!ModelState.IsValid) return BadRequest();
            var invoiceDetail = mapper.Map<TblInvoiceDetail>(invoiceDetailCreation);
            context.Add(invoiceDetail);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var invoice = await context.TblInvoices.FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null) return BadRequest();
            context.TblInvoices.Remove(invoice);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("InvoiceDetails/{id:int}")]
        public async Task<ActionResult> DeleteDetails(int id)
        {
            var invoiceDetail = await context.TblInvoiceDetails.FirstOrDefaultAsync(i => i.Id == id);
            if (invoiceDetail == null) return BadRequest();
            context.TblInvoiceDetails.Remove(invoiceDetail);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] InvoiceCreationDTO invoiceCreation)
        {
            var invoice = mapper.Map<TblInvoice>(invoiceCreation);
            invoice.Id = id;
            context.Entry(invoice).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("InvoiceDetails/{id}")]
        public async Task<ActionResult> PutDetail(int id, [FromBody] InvoiceDetailCreationDTO invoiceDetailCreation)
        {
            var invoiceDetail = mapper.Map<TblInvoiceDetail>(invoiceDetailCreation);
            invoiceDetail.Id = id;
            context.Entry(invoiceDetail).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
