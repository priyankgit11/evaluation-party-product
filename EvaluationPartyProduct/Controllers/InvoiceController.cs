using AutoMapper;
using EvaluationPartyProduct.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EvaluationPartyProduct.DTO;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet]
        public async Task<List<InvoiceDTO>> Get()
        {
            var invoices = await context.TblInvoices.Include(i=>i.Party).ToListAsync();
            var invoicesDTO = mapper.Map<List<InvoiceDTO>>(invoices);
            foreach (var item in invoicesDTO)
            {
                var invoiceDetails = await context.TblInvoiceDetails.Where(i=>i.InvoiceId == item.Id).ToListAsync();
                decimal gt = 0;
                invoiceDetails.ForEach(i =>
                {
                    gt += i.Rate * i.Quantity;
                });
                item.GrandTotal = gt;
            }
            return invoicesDTO;
        }

    }
}
