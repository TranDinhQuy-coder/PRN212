using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystemAs
{
    internal class Book : LibraryItem, IBorrowable
    {
        public string Author { get; set; }
        public int Pages { get; set; }
        public string Genre { get; set; }

        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsAvailable { get; set; }

        public Book(int id, string title, int publicationYear, string author)
            : base(id, title, publicationYear)
        {
            Author = author;
            IsAvailable = true;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"[Book] ID: {Id}, Title: {Title}, Author: {Author}, Year: {PublicationYear}, Pages: {Pages}, Genre: {Genre}, Available: {IsAvailable}");
        }

        public override decimal CalculateLateReturnFee(int daysLate)
        {
            return daysLate * 0.75m;
        }

        public void Borrow()
        {
            if (!IsAvailable)
            {
                Console.WriteLine($"Book '{Title}' is already borrowed.");
                return;
            }

            BorrowDate = DateTime.Now;
            IsAvailable = false;
            Console.WriteLine($"Book '{Title}' borrowed on {BorrowDate.Value:yyyy-MM-dd}.");
        }

        public void Return()
        {
            if (IsAvailable)
            {
                Console.WriteLine($"Book '{Title}' was not borrowed.");
                return;
            }

            ReturnDate = DateTime.Now;
            IsAvailable = true;
            Console.WriteLine($"Book '{Title}' returned on {ReturnDate.Value:yyyy-MM-dd}.");
        }
    }
}
