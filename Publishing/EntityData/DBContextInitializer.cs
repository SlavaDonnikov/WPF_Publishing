using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            db.Publishers.Add(publisher1);
            db.Publishers.Add(publisher2);
            db.SaveChanges();

            Publication publication1 = new Publication
            {
                PublicationName = "Brave new world: Unknow space.",
                ISSN = "1794-0857",
                Genre = "Science",
                Language = "English",
                NumberOfCopies = 100,
                NumberOfPages = 189,
                Format = "B5 26 x 18 cm",
                DownloadLink = "www.dropbox.com\\djkwodx",
                Cover = new byte[1],
                PublicationDate = "16.04.2018",
                Publisher = publisher1
            };

            Publication publication2 = new Publication
            {
                PublicationName = "Blackholes: Deep space mistery.",
                ISSN = "9987-1534",
                Genre = "Science",
                Language = "English",
                NumberOfCopies = 50,
                NumberOfPages = 172,
                Format = "B5 26 x 18 cm",
                DownloadLink = "www.dropbox.com\\uoyjpior",
                Cover = new byte[1],
                PublicationDate = "16.04.2018",
                Publisher = publisher1                
            };

            Publication publication3 = new Publication
            {
                PublicationName = "Back in past: Jurasic period",
                ISSN = "1167-1913",
                Genre = "Sci-Fi",
                Language = "English",
                NumberOfCopies = 200,
                NumberOfPages = 58,
                Format = "B5 26 x 18 cm",
                DownloadLink = "www.dropbox.com\\pyoiyu",
                Cover = new byte[1],
                PublicationDate = "16.04.2018",
                Publisher = publisher2
            };

            Publication publication4 = new Publication
            {
                PublicationName = "If we can travel into another worlds",
                ISSN = "0143-1798",
                Genre = "Sci-Fi",
                Language = "English",
                NumberOfCopies = 200,
                NumberOfPages = 58,
                Format = "B5 26 x 18 cm",
                DownloadLink = "www.dropbox.com\\gbnvsa",
                Cover = new byte[1],
                PublicationDate = "16.04.2018",
                Publisher = publisher2
            };

            db.Publications.Add(publication1);
            db.Publications.Add(publication2);
            db.Publications.Add(publication3);
            db.Publications.Add(publication4);
            db.SaveChanges();            
        }
    }
}
