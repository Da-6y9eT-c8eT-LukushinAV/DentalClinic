using System;
using System.Collections.Generic;

namespace DentalClinic.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        // Список книг, которые взял пользователь
        public ICollection<Book> BorrowedBooks { get; set; } = new List<Book>();
    }
}