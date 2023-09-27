using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Exceptions;
using ApiApplication.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.UseCases.Showtimes.CreateShowtime
{
    public class CreateShowtimeCommandHandler: IRequestHandler<CreateShowtimeCommand, int>
    {
        private readonly IMovieProvider movieProvider;
        private readonly IShowtimesRepository showtimeRepository;
        private readonly ILocalMovieRepository movieRepository;
        private readonly IAuditoriumsRepository auditoriumsRepository;

        public CreateShowtimeCommandHandler(IMovieProvider movieProvider,
            IShowtimesRepository showtimeRepository,
            ILocalMovieRepository movieRepository,
            IAuditoriumsRepository auditoriumsRepository)
        {
            this.movieProvider = movieProvider;
            this.showtimeRepository = showtimeRepository;
            this.movieRepository = movieRepository;
            this.auditoriumsRepository = auditoriumsRepository;
        }

        public async Task<int> Handle(CreateShowtimeCommand command, CancellationToken cancel)
        {
            GuardAgainstPastDates(command);
            await GuardUnexistingAuditorium(command, cancel);

            var alreadyExistingMovie = await movieRepository.GetByImdbIdAsync(command.MovieId, cancel);
            var movie = alreadyExistingMovie ?? await movieProvider.GetMovie(command.MovieId);
            var showtime = ShowtimeEntity.CreateShowtime(movie, command.AuditoriumId, command.SessionDate);

            try
            {
                await showtimeRepository.CreateShowtime(showtime, cancel);
                var saved = showtimeRepository.GetWithMoviesByIdAsync(showtime.Id, cancel);
                return showtime.Id;
            }
            catch (Exception e)
            {
                throw new SqlException(e.Message);
            }
        }

        private void GuardAgainstPastDates(CreateShowtimeCommand command)
        {
            if(command.SessionDate < DateTime.Now)
            {
                throw new UserInputValidationException("InvalidOperation.CreateShowtime.WrongDate", "Cannot create showtime in past");
            }
        }

        private async Task GuardUnexistingAuditorium(CreateShowtimeCommand command, CancellationToken cancel)
        {
            var auditorium = await auditoriumsRepository.GetAsync(command.AuditoriumId, cancel);
            if(auditorium is null)
            {
                throw new UserInputValidationException("InvalidOperation.CreateShowtime.UnknowAuditorium", "Cannot create showtime without in a non valid auditorium");
            }
        }
    }
}
