using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Publishing.EntityData
{    
    class DBContextInitializer : DropCreateDatabaseAlways<PublishingContext>
    {   
        protected override void Seed(PublishingContext db)
        {
            Publisher publisher1 = new Publisher
            {
                PublisherName = "HarperCollins",
                Addres = "195 Broadway New York, NY 10007, (212) 207 - 7000",
                Email = "harpercollins.com"
            };

            Publisher publisher2 = new Publisher
            {
                PublisherName = "Macmillan Publishers",
                Addres = "175 Fifth Avenue New York, NY 10010, 646 - 307 - 5151",
                Email = "us.macmillan.com"
            };

            Publisher publisher3 = new Publisher
            {
                PublisherName = "Naked Science RU",
                Addres = "Russian Federation, Moskov",
                Email = "naked-science.ru"
            };

            Publisher publisher4 = new Publisher
            {
                PublisherName = "Naked Science COM",
                Addres = "The Institute of Continuing Education Madingley Hall Cambridge University",
                Email = "www.thenakedscientists.com"
            };

            db.Publishers.Add(publisher1);
            db.Publishers.Add(publisher2);
            db.Publishers.Add(publisher3);
            db.Publishers.Add(publisher4);
            db.SaveChanges();
                  
            Publication publication1 = new Publication
            {
                PublicationName = "In search of \"Brothers in mind \"",
                ISSN = "1794-0857",
                Genre = "Science",
                Language = "Russian",
                NumberOfCopies = 100,
                NumberOfPages = 189,
                Format = "A4 210 × 297",
                DownloadLink = "www.dropbox.com\\unknown",
                Cover = MainWindow.ConvertImageToBinary(MainWindow.GetImageFromResources("ns1.jpg")),
                PublicationDate = "16.04.2018",
                Publisher = publisher3
            };

            Publication publication2 = new Publication
            {
                PublicationName = "Life after the war",
                ISSN = "9987-1534",
                Genre = "Science",
                Language = "Russian",
                NumberOfCopies = 50,
                NumberOfPages = 172,
                Format = "B5 176 × 250",
                DownloadLink = "www.dropbox.com\\unknown",
                Cover = MainWindow.ConvertImageToBinary(MainWindow.GetImageFromResources("ns2.jpg")),
                PublicationDate = "19.05.2018",
                Publisher = publisher3                
            };

            Publication publication3 = new Publication
            {
                PublicationName = "Worlds under the red sun",
                ISSN = "1167-1913",
                Genre = "Sci-Fi",
                Language = "Russian",
                NumberOfCopies = 200,
                NumberOfPages = 58,
                Format = "B6 125 × 176",
                DownloadLink = "www.dropbox.com\\unknown",
                Cover = MainWindow.ConvertImageToBinary(MainWindow.GetImageFromResources("ns3.jpg")),
                PublicationDate = "23.05.2018",
                Publisher = publisher3
            };

            Publication publication4 = new Publication
            {
                PublicationName = "Mission to Alpha Centauri",
                ISSN = "0143-1798",
                Genre = "Sci-Fi",
                Language = "English",
                NumberOfCopies = 300,
                NumberOfPages = 72,
                Format = "A5 148 × 210",
                DownloadLink = "www.dropbox.com\\unknown",
                Cover = MainWindow.ConvertImageToBinary(MainWindow.GetImageFromResources("ns4.jpg")),
                PublicationDate = "17.04.2018",
                Publisher = publisher4
            };

            Publication publication5 = new Publication
            {
                PublicationName = "Death of civilization: Possible scenarios",
                ISSN = "9550-1110",
                Genre = "Sci-Fi",
                Language = "English",
                NumberOfCopies = 170,
                NumberOfPages = 60,
                Format = "C6 114 × 162",
                DownloadLink = "www.dropbox.com\\unknown",
                Cover = MainWindow.ConvertImageToBinary(MainWindow.GetImageFromResources("ns5.jpg")),
                PublicationDate = "28.03.2018",
                Publisher = publisher4
            };

            Publication publication6 = new Publication
            {
                PublicationName = "The colonization rate: the best lands",
                ISSN = "7319-2018",
                Genre = "Sci-Fi",
                Language = "English",
                NumberOfCopies = 140,
                NumberOfPages = 48,
                Format = "B5 176 × 250",
                DownloadLink = "www.dropbox.com\\unknown",
                Cover = MainWindow.ConvertImageToBinary(MainWindow.GetImageFromResources("ns6.jpg")),
                PublicationDate = "30.05.2018",
                Publisher = publisher4
            };

            db.Publications.Add(publication1);
            db.Publications.Add(publication2);
            db.Publications.Add(publication3);
            db.Publications.Add(publication4);
            db.Publications.Add(publication5);
            db.Publications.Add(publication6);
            db.SaveChanges();            
        }
    }
}
