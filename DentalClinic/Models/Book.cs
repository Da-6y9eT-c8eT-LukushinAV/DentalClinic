using System;

namespace DentalClinic.Models
{
    public enum BookStatus
    {
        Available,
        Borrowed,
        Maintenance
    }

    public class Book
    {
        public int Id { get; set; }
        public string ArticleNumber { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public BookStatus Status { get; set; }
        public int? ReaderId { get; set; }
        public User Reader { get; set; }
    }
} 