using ApiApplication.Domain.Models;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApiApplication.UseCases.Reservations.ConfirmReservation
{
    public class ConfirmReservation : IRequest<Unit>
    {
        [Required]
        public Guid ReservationId { get; set; }
    }
}
