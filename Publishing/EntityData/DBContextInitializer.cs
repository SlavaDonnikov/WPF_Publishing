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
        private System.Windows.Controls.Image getImageFromResources(string name)
        {
            // Uri uri = new Uri("pack://application:,,,/Resources/no.png", UriKind.RelativeOrAbsolute);
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            Uri uri = new Uri("pack://application:,,,/Resources/" + name, UriKind.RelativeOrAbsolute);
            ImageSource imgSource = new BitmapImage(uri);
            image.Source = imgSource;

            return image;
        }

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

            // Use 'empty' image
            //System.Windows.Controls.Image image = new System.Windows.Controls.Image(); 
            //Uri uri = new Uri("pack://application:,,,/Resources/no.png", UriKind.RelativeOrAbsolute);
            //ImageSource imgSource = new BitmapImage(uri);
            //image.Source = imgSource;            

            Publication publication1 = new Publication
            {
                PublicationName = "В поисках \"Братьев по разуму\"",
                ISSN = "1794-0857",
                Genre = "Science",
                Language = "Russian",
                NumberOfCopies = 100,
                NumberOfPages = 189,
                Format = "B5 26 x 18 cm",
                DownloadLink = "www.dropbox.com\\unknown",
                Cover = MainWindow.ConvertImageToBinary(getImageFromResources("ns1.jpg")),
                PublicationDate = "16.04.2018",
                Publisher = publisher3
            };

            Publication publication2 = new Publication
            {
                PublicationName = "Жизнь после войны",
                ISSN = "9987-1534",
                Genre = "Science",
                Language = "Russian",
                NumberOfCopies = 50,
                NumberOfPages = 172,
                Format = "B5 26 x 18 cm",
                DownloadLink = "www.dropbox.com\\unknown",
                Cover = MainWindow.ConvertImageToBinary(getImageFromResources("ns2.jpg")),
                PublicationDate = "16.04.2018",
                Publisher = publisher3                
            };

            Publication publication3 = new Publication
            {
                PublicationName = "Миры под красным солнцем",
                ISSN = "1167-1913",
                Genre = "Sci-Fi",
                Language = "Russian",
                NumberOfCopies = 200,
                NumberOfPages = 58,
                Format = "B5 26 x 18 cm",
                DownloadLink = "www.dropbox.com\\unknown",
                Cover = MainWindow.ConvertImageToBinary(getImageFromResources("ns3.jpg")),
                PublicationDate = "16.04.2018",
                Publisher = publisher3
            };

            Publication publication4 = new Publication
            {
                PublicationName = "Миссия а Алфе Центавра",
                ISSN = "0143-1798",
                Genre = "Sci-Fi",
                Language = "English",
                NumberOfCopies = 300,
                NumberOfPages = 72,
                Format = "B5 26 x 18 cm",
                DownloadLink = "www.dropbox.com\\unknown",
                Cover = MainWindow.ConvertImageToBinary(getImageFromResources("ns4.jpg")),
                PublicationDate = "16.04.2018",
                Publisher = publisher4
            };

            Publication publication5 = new Publication
            {
                PublicationName = "Смерть цивилизации: Возможные сценарии",
                ISSN = "0143-1798",
                Genre = "Sci-Fi",
                Language = "English",
                NumberOfCopies = 170,
                NumberOfPages = 60,
                Format = "B5 26 x 18 cm",
                DownloadLink = "www.dropbox.com\\unknown",
                Cover = MainWindow.ConvertImageToBinary(getImageFromResources("ns5.jpg")),
                PublicationDate = "16.04.2018",
                Publisher = publisher4
            };

            Publication publication6 = new Publication
            {
                PublicationName = "Рейтинг колонизации: лучшии земли",
                ISSN = "0143-1798",
                Genre = "Sci-Fi",
                Language = "English",
                NumberOfCopies = 140,
                NumberOfPages = 48,
                Format = "B5 26 x 18 cm",
                DownloadLink = "www.dropbox.com\\unknown",
                Cover = MainWindow.ConvertImageToBinary(getImageFromResources("ns6.jpg")),
                PublicationDate = "16.04.2018",
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
