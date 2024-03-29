﻿namespace RestaurantAPI.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        private const string defaultMessage = "Entity not found.";

        public EntityNotFoundException(int id) : base($"Entity with ID '{id}' was not found.") { }

        public EntityNotFoundException(string message) : base(message) { }

        public EntityNotFoundException() : base(defaultMessage) { }
    }
}