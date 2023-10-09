namespace UniversalLibrary.Data.Entities
{
    public class BookingDetail : IEntity
    {
        public int Id { get; set; }

        public Book Book { get; set; }

        public int QuantityOfBooks { get; set; }
    }
}
