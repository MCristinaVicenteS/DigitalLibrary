namespace UniversalLibrary.Data.Entities
{
    public class BookingDetailTemp : IEntity
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Book Book { get; set; }

        public int QuantityOfBooks { get; set; }
    }
}
