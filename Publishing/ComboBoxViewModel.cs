using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publishing        // Just for test!
{
    public class ComboBoxViewModel
    {
        public List<string> SearchTypesCollection { get; set; }
        public List<string> GenreTypesCollection { get; set; }
        public List<string> PublisherTypesCollection { get; set; }

        public ComboBoxViewModel()
        {
            SearchTypesCollection = new List<string>()
            {
                "Name",                
                "INNS",
                "Genre",
                "Publisher Name",
                "Publication Language",
                "Publication Date",
            };

            GenreTypesCollection = new List<string>()
            {
                "Science",
                "Sci-Fi",
                "Research Work",
                "Technical Report",
                "Dissertation"
            };

            PublisherTypesCollection = new List<string>()
            {
                "Create New Publisher",
                "Choose Existing Publisher"
            };
        }
    }
}
