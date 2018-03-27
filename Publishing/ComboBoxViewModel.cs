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

        public ComboBoxViewModel()
        {
            SearchTypesCollection = new List<string>()
            {
                "Search by Name",
                "Search by INNS",
                "Search by publishing house",
                "Search by category"
            };
        }
    }
}
