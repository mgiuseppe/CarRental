using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Contracts;
using CarRentalNovility.Entities;
using CarRentalNovility.Web.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalNovility.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IBLReservation blReservation;
        private readonly IMapper mapper;

        public ReservationsController(IBLReservation blReservation, IMapper mapper)
        {
            this.blReservation = blReservation;
            this.mapper = mapper;
        }

        /// <summary>
        /// Browse Reservations
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> Get([FromQuery] BrowseReservationsParametersDto parameters)
        {
            var reservations = await blReservation.BrowseReservationAsync(mapper.Map<BrowseReservationsParameters>(parameters));
            return Ok(mapper.Map<List<ReservationDto>>(reservations));
        }

        /// <summary>
        /// Book reservation
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>created reservation</returns>
        [HttpPost]
        public async Task<ActionResult<ReservationDto>> Post([FromBody] BookCarReservationParametersDto parameters)
        {
            var reservation = await blReservation.BookCarReservationAsync(mapper.Map<BookCarReservationParameters>(parameters));
            return Ok(mapper.Map<ReservationDto>(reservation));   
        }

        /// <summary>
        /// Pick up car
        /// </summary>
        /// <param name="id">reservation id</param>
        [HttpPut("{id}/PickUpCar")]
        public async Task<IActionResult> PickUpCar(long id)
        {
            await blReservation.PickUpCarAsync(id);
            return Ok();
        }

        /// <summary>
        /// Return car
        /// </summary>
        /// <param name="id">reservation id</param>
        [HttpPut("{id}/ReturnCar")]
        public async Task<IActionResult> ReturnCar(long id)
        {
            await blReservation.ReturnCarAsync(id);
            return Ok();
        }

        /// <summary>
        /// Cancel reservation
        /// </summary>
        /// <param name="id">reservation id</param>
        // DELETE api/values/5
        [HttpDelete("{id}/CancelReservation")]
        public async Task<IActionResult> Delete(long id)
        {
            await blReservation.CancelCarReservationAsync(id);
            return Ok();
        }
    }
}
