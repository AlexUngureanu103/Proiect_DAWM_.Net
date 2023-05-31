namespace RestaurantAPI.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        private const string defaultMessage = "Entity not found!";

        public EntityNotFoundException(string message) : base(message) { }

        public EntityNotFoundException() : base(defaultMessage) { }
    }
}