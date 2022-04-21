using Payment.Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace Payment.Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        
        public PaymentController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Models.Merchant>))]
        //public async Task<IEnumerable<Models.Merchant>> Get()
        //{
        //    return await _merchantService.GetMerchants();
        //}

        //[HttpGet("{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.Merchant))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<Models.Merchant>> Get(string id)
        //{
        //    var merchant = await _merchantService.GetMerchant(id);
        //    if (merchant == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(merchant);
        //}


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(Models.CreateTransaction transaction)
        {
            // var merchantId = await _merchantService.AddMerchant(merchant);
            
            return Ok();

            //return CreatedAtAction(nameof(Get), new { id = merchantId }, merchantId);
        }
    }
}