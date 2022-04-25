using Payment.Gateway.Services;
using Microsoft.AspNetCore.Mvc;
using Payment.Gateway.Models;
using Payment.Gateway.MongoDb.Models;

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

        [HttpPost]
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(Models.CreateTransaction transaction)
        {
            var response = await _transactionService.MakeTransaction(transaction);
            return Ok(response);
        }
    }
}