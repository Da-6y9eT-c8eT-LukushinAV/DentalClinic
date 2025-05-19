namespace DentalClinic.Models
{
    public class Applic
    {
        public int Id { get; set; }
        public string ApplicationNumber { get; set; }
        public string ShortDescription { get; set; }
        public string Type { get; set; }
        public string FullDescription { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public Status Status { get; set; }
        public int? UserId { get; set; } // Внешний ключ для исполнителя
        public User User { get; set; } // Навигационное свойство
    }

    public enum Status
    {
        UnderConsideration,
        InProgress,
        Completed
    }
}