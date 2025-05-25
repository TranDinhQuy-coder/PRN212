using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystemAs
{
    internal class DVD : LibraryItem, IBorrowable
    {
        public string Director { get; set; }
        public int Runtime { get; set; } // minutes
        public string AgeRating { get; set; }

        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsAvailable { get; set; }

        public DVD(int id, string title, int publicationYear, string director)
            : base(id, title, publicationYear)
        {
            Director = director;
            IsAvailable = true;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"[DVD] ID: {Id}, Title: {Title}, Director: {Director}, Year: {PublicationYear}, Runtime: {Runtime} mins, Rating: {AgeRating}, Available: {IsAvailable}");
        }

        public override decimal CalculateLateReturnFee(int daysLate)
        {
            return daysLate * 1.00m;
        }

        public void Borrow()
        {
            if (!IsAvailable)
            {
                Console.WriteLine($"DVD '{Title}' is already borrowed.");
                return;
            }

            BorrowDate = DateTime.Now;
            IsAvailable = false;
            Console.WriteLine($"DVD '{Title}' borrowed on {BorrowDate.Value:yyyy-MM-dd}.");
        }

        public void Return()
        {
            if (IsAvailable)
            {
                Console.WriteLine($"DVD '{Title}' was not borrowed.");
                return;
            }

            ReturnDate = DateTime.Now;
            IsAvailable = true;
            Console.WriteLine($"DVD '{Title}' returned on {ReturnDate.Value:yyyy-MM-dd}.");
        }
    }
}
