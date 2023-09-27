namespace ApiApplication.Database.Entities
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherEntity = (Entity) obj;
            return Id == otherEntity.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
