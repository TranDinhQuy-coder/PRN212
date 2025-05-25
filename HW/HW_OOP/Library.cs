using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystemAs
{
    internal class Library
    {
        private List<LibraryItem> items = new List<LibraryItem>();

        public void AddItem(LibraryItem item)
        {
            items.Add(item);
        }

        public LibraryItem? SearchByTitle(string title)
        {
            return items.FirstOrDefault(i => i.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        public void DisplayAllItems()
        {
            Console.WriteLine("\n===== Library Items =====");
            foreach (var item in items)
            {
                item.DisplayInfo();
            }
        }
    }
}
