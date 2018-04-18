using Publishing.EntityData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publishing        
{
    public class ComboBoxViewModel
    {
        public ObservableCollection<string> SearchTypesCollection { get; set; }
        public ObservableCollection<string> GenreTypesCollection { get; set; }
        public ObservableCollection<string> LanguageTypesCollection { get; set; }
        public ObservableCollection<string> PublisherTypesCollection { get; set; }
        
        public void PublisherTypesComboboxRefresh()
        {
            using (PublishingContext db = new PublishingContext())
            {
                var publishers = db.Publishers.ToList();
                foreach (Publisher p in publishers)
                {
                    if(!PublisherTypesCollection.Contains(p.PublisherName))
                    {
                        PublisherTypesCollection.Add(p.PublisherName.ToString());
                    }                    
                }                    
            }
        }

        public ComboBoxViewModel()
        {
            SearchTypesCollection = new ObservableCollection<string>()
            {
                "Publication Name",                
                "INNS",
                "Genre",
                "Publisher Name",
                "Publication Language",
                "Publication Date",
            };

            GenreTypesCollection = new ObservableCollection<string>()
            {
                "Science",
                "Sci-Fi",
                "Research Work",
                "Technical Report",
                "Dissertation"
            };

            LanguageTypesCollection = new ObservableCollection<string>()
            {                
                "Russian",
                "English",
                "French",
                "German",
                "Chinese",
                "Japanese",
                "Korean"
            };

            PublisherTypesCollection = new ObservableCollection<string>() { "Create New Publisher" };
            PublisherTypesComboboxRefresh();
        }        
    }
}
