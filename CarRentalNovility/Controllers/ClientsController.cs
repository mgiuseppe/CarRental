using AutoMapper;
using BusinessLayer.Contracts;
using CarRentalNovility.Web.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CarRentalNovility.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IBLClient blClient;
        private readonly ILogger<ClientsController> logger;
        private readonly IMapper mapper;

        public ClientsController(IBLClient blClient, ILogger<ClientsController> logger, IMapper mapper)
        {
            this.blClient = blClient;
            this.logger = logger;
            this.mapper = mapper;
        }

        /// <summary>
        /// Returns total fees (Rental rate fee, Cancellation fee,  Deposit fee) from all reservations for specified client.
        /// </summary>
        /// <param name="id">client id</param>
        /// <returns></returns>
        [HttpGet("{id}/Balance")]
        public async Task<ActionResult<ClientAccountDto>> GetBalance(long id)
        {
            var balance = await blClient.GetBalanceAsync(id);
            return Ok(mapper.Map<ClientAccountDto>(balance));
        }
    }
}
