﻿using System;

namespace ApiApplication.Exceptions
{
    public class MovieNotFoundException: ApplicationException
    {
        public MovieNotFoundException(string movieId)
            :base("ProvidedAPI.MovieNotFound", 404, $"Movie with ImdbId({movieId}) does'nt exist")
        {

        }
    }
}
