namespace ApiApplication.Domain.Exceptions
{
    public class EntityNotFoundException: DomainException
    {
        public EntityNotFoundException(string entityName, string id)
            : base("Domain.EntityNotFound", 404, $"Entity ({entityName}-{id}) Not found")
        {

        }
    }
}
