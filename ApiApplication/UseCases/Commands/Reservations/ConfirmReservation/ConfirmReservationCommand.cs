using ApiApplication.Domain.Models;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApiApplication.UseCases.Commands.Reservations.ConfirmReservation
{
    public class ConfirmReservationCommand : IRequest<Unit>
    {
        [Required]
        public Guid ReservationId { get; set; }
    }
}
