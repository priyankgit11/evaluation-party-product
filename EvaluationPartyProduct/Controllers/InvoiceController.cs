using AutoMapper;
using EvaluationPartyProduct.Models;
using Microsoft.AspNetCore.Mvc;
using EvaluationPartyProduct.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EvaluationPartyProduct.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<ActionResult<List<InvoiceDTO>>> Get([FromQuery] string searchProducts = "", [FromQuery(Name = "order[0][column]")] int sortColumn = 0, int start = 0, int length = 10, [FromQuery(Name = "search[value]")] string search = "",
        [FromQuery(Name = "order[0][dir]")] string sortDirection = "asc", [FromQuery] string startDate = "", [FromQuery] string endDate = "")
        {
            var invoiceDetails = context.TblInvoiceDetails.Include(i => i.Invoice).ThenInclude(i => i.Party).Include(p => p.Product).Where(p => p.Invoice.Party.PartyName.Contains(search));

            //Implement search logic for multiple items within a column
            string[] searchTerms = searchProducts.Split(", ");
            if (searchTerms != null && searchTerms.Length > 0)
            {
                // Assuming 'name' is the column to search
                var temp = invoiceDetails.Where(i => searchTerms.Any(p => i.Product.ProductName.Contains(p)));
                invoiceDetails = temp;
            }
            DateTime startingDate;
            DateTime endingDate;

            if (DateTime.TryParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startingDate)
                &&
                DateTime.TryParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endingDate))
            {
                invoiceDetails = invoiceDetails.Where(i => i.Invoice.Date.Date >= startingDate.Date && i.Invoice.Date.Date <= endingDate.Date);
            }

            var invoices = invoiceDetails.Select(p => p.Invoice).Distinct();
            // Apply pagination using 'start' and 'length' parameters
            var invoiceList = await invoices.ToListAsync();
            //var result = await invoices.Skip(start).Take(length).ToListAsync();
            var invoiceDTO = mapper.Map<List<InvoiceDTO>>(invoiceList);
            await CalculateGrandTotals(invoiceDTO);
            switch (sortColumn)
            {
                case 1:
                    if (sortDirection == "asc")
                    {
                        invoiceDTO = invoiceDTO.OrderBy(e => e.PartyName).ToList();
                        // Similar lines for other sorting cases
                    }
                    else
                    {
                        invoiceDTO = invoiceDTO.OrderByDescending(e => e.PartyName).ToList();
                        // Similar lines for other sorting cases
                    }
                    break;
                case 2:
                    if (sortDirection == "asc")
                    {
                        invoiceDTO = invoiceDTO.OrderBy(e => e.Date).ToList();
                        // Similar lines for other sorting cases
                    }
                    else
                    {
                        invoiceDTO = invoiceDTO.OrderByDescending(e => e.Date).ToList();
                        // Similar lines for other sorting cases
                    }
                    break;
                // Add more cases for other columns as needed

                case 3:
                    if (sortDirection == "asc")
                    {
                        invoiceDTO = invoiceDTO.OrderBy(e => e.GrandTotal).ToList();
                        // Similar lines for other sorting cases
                    }
                    else
                    {
                        invoiceDTO = invoiceDTO.OrderByDescending(e => e.GrandTotal).ToList();
                        // Similar lines for other sorting cases
                    }
                    break;
                default:
                    if (sortDirection == "asc")
                    {
                        invoiceDTO = invoiceDTO.OrderBy(e => e.Id).ToList();
                        // Similar lines for other sorting cases
                    }
                    else
                    {
                        invoiceDTO = invoiceDTO.OrderByDescending(e => e.PartyName).ToList();
                        // Similar lines for other sorting cases
                    }
                    break;
            }
            int totalRecords = invoiceDTO.Count();
            var filteredDTO = invoiceDTO.Skip(start).Take(length);

            int filteredRecords = filteredDTO.Count();
            var response = new
            {
                recordsTotal = totalRecords,
                recordsFiltered = filteredRecords,
                data = filteredDTO
            };

            return Ok(response);
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
                query = sort ? context.TblInvoiceDetails.Include(i => i.Product).Include(i => i.Invoice).ThenInclude(i => i.Party).OrderBy(i => i.Product.ProductName).Skip((page - 1) * pageSize).Take(pageSize)
                    :
                    context.TblInvoiceDetails.Include(i => i.Product).Include(i => i.Invoice).ThenInclude(i => i.Party).OrderByDescending(i => i.Product.ProductName).Skip((page - 1) * pageSize).Take(pageSize)
;
            }
            else
            {
                query = sort ? context.TblInvoiceDetails.Include(i => i.Product).Include(i => i.Invoice).ThenInclude(i => i.Party).Where(i => i.Product.ProductName.Contains(filter)).OrderBy(i => i.Product.ProductName).Skip((page - 1) * pageSize).Take(pageSize)
                    :
                    context.TblInvoiceDetails.Include(i => i.Product).Include(i => i.Invoice).ThenInclude(i => i.Party).Where(i => i.Product.ProductName.Contains(filter)).OrderByDescending(i => i.Product.ProductName).Skip((page - 1) * pageSize).Take(pageSize)
;
            }

            var invoiceDetails = await query.ToListAsync();
            var invoicesDetailDTO = mapper.Map<List<InvoiceDetailDTO>>(invoiceDetails);
            CalculateTotals(invoicesDetailDTO);
            return Ok(invoiceDetails);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<List<InvoiceDetailDTO>>> Get(int id)
        {
            var invoices = await context.TblInvoices.Include(i => i.Party).Where(i => i.Id == id).ToListAsync();
            if (invoices == null) return NoContent();
            var invoiceDTO = mapper.Map<List<InvoiceDTO>>(invoices);
            await CalculateGrandTotals(invoiceDTO);
            return Ok(invoiceDTO);
        }
        [HttpGet("details/{id:int}")]
        public async Task<ActionResult<List<InvoiceDetailDTO>>> GetDetails(int id)
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
        [HttpPost("InvoiceDetails")]
        public async Task<ActionResult<InvoiceDetailDTO>> Post([FromBody] InvoiceDetailCreationDTO invoiceDetailCreation)
        {
            if (!ModelState.IsValid) return BadRequest();
            var invoiceDetail = mapper.Map<TblInvoiceDetail>(invoiceDetailCreation);
            context.Add(invoiceDetail);
            await context.SaveChangesAsync();
            var invoiceDetailDTO = mapper.Map<InvoiceDetailDTO>(invoiceDetail);
            invoiceDetailDTO.Total = invoiceDetailDTO.Quantity * invoiceDetailDTO.Rate;
            return Ok(invoiceDetailDTO);
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
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] InvoiceCreationDTO invoiceCreation)
        {
            var invoice = mapper.Map<TblInvoice>(invoiceCreation);
            invoice.Id = id;
            context.Entry(invoice).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("InvoiceDetails/{id:int}")]
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
