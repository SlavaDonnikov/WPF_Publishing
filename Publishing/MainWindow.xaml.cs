﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;
using WPFPdfViewer;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.IO;
using System.Data.Entity;
using Publishing.EntityData;
using System.Data;
using System.Windows.Automation.Peers;

namespace Publishing
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Global Variables
        private MediaPlayer ApplicationMusicTheme; // add application music theme, which plays all time  
        public delegate void DragSliderTimerTick();
        //public string LoadedItemName { get; set; }   
        #endregion

        #region MainWindow()
        public MainWindow()
        {
            InitializeComponent();

            InvisibleGrids();

            ApplicationBackgroundMusic();

            Grid_HomePage.Visibility = Visibility.Visible;

            LoadDB();           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetFullDate();
            GetRealTime();

            VideoPlayerButtonsOnOff(0);
            BlockUnblock_AddPublisherInfoFields(0);

            DataContext = new ComboBoxViewModel();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Облегчить как нибудь закрытие приложения!
            // Разобраться с пдф ошибкой
            // Сменить фоновую музыку
            // Додумать список воспроизведения для плеера
            //db.Dispose();
        }
        #endregion

        #region DB        
        public BitmapImage[] Home_page_images { get; set; } //= new BitmapImage[6];


		/// <summary>
		/// Load data from BD to DataGrid. 
		/// Also can be used as a DataGrid Refresh().
		/// </summary>
		private void LoadDB()
        {
            using (PublishingContext db = new PublishingContext())
            {
                //var publications = db.Publications.ToList();  // lazy
                //var publications = db.Publications.Include(p => p.Publisher).ToList();
                // Add_Page_DataGrid.ItemsSource = db.Publications.Local.ToBindingList();
                Add_Page_DataGrid.ItemsSource = Delete_Page_DataGrid.ItemsSource = null;                
                try
                {
                    Add_Page_DataGrid.ItemsSource = Delete_Page_DataGrid.ItemsSource = db.Publications.Include(p => p.Publisher).ToList();

                    // вывод первых 6ти обложек из БД на главную страницу.
                    List<Publication> publications = db.Publications.Include(p => p.Publisher).ToList();
					Home_page_images = new BitmapImage[6];
					
					for (int i = 0; i < 6; i++)
                    {
                        if(publications.Count > i)
                        {
                            Home_page_images[i] = BitmapImageFromBytes(publications[i].Cover);
                        } 
                        else
                        {
                            Home_page_images[i] = BitmapImageFromBytes(ConvertImageToBinary(GetImageFromResources("no.png")));
                        }
                    }					

					UpdateHomePageImageSource();
				}
				catch (Exception ex)
                {
                    MessageBox.Show("DB loading error : " + ex.Message + ", " + ex.Source);
                }

                this.Closing += MainWindow_Closing;
            }           
        }

		public void UpdateHomePageImageSource()
		{   // получили все Image Controll, расположенные на гриде Images_Grid_HomePage (6 штук)		
			Image[] images = FindVisualChildren<Image>(Images_Grid_HomePage).ToArray();
			for (int i = 0; i < 6; i++)
			{   // помещаем в них изображения записей из БД			
				images.ElementAt(i).Source = Home_page_images[i];
			}
		}

		/// <summary>
		/// Refresh PublisherTypesComboBox after new item was added to DB.
		/// </summary>
		public void OverridePublisherTypesComboboxRefresh()
        {
            ComboBoxViewModel cbmv = new ComboBoxViewModel();
            cbmv.PublisherTypesComboboxRefresh();
            PublisherTypesComboBox.ItemsSource = cbmv.PublisherTypesCollection;
        }

        /// <summary>
        /// Check is all fields filled before save data in DB.
        /// </summary>
        /// <returns></returns>
        public bool IsDataFieldsAreNotEmpty()
        {			
			List<bool> txt = new List<bool>(10);
			List<bool> chk = new List<bool>(3);
			List<bool> img = new List<bool>(1);
			//txt.ForEach(i => i = false);
			
			foreach (TextBox child in FindVisualChildren<TextBox>(Grid_AddPublication))
			{
				if (!string.IsNullOrEmpty(child.Text) & !string.IsNullOrWhiteSpace(child.Text)) txt.Add(true);
			}
			foreach (ComboBox child in FindVisualChildren<ComboBox>(Grid_AddPublication))
			{
				if (child.SelectedItem != null & child.SelectedIndex != -1) chk.Add(true);
			}
			foreach (Image child in FindVisualChildren<Image>(Grid_AddPublication))
			{
				if (child.Source != null) img.Add(true);
			}
			
			if ( (txt.Any(a => a == true) & txt.Count == 10) & (chk.Any(a => a == true) & chk.Count == 3) & (img.Any(a => a == true) & img.Count == 1)) return true;
            else return false;            
        }       

        /// <summary>
        /// Bolck & Unblock textboxes with information about publisher.
        /// </summary>
        /// <param name="x"></param>
        public void BlockUnblock_AddPublisherInfoFields(int x)
        {
            switch(x)
            {
                case 0:
                    {
                        Add_Publisher_Name_TextBox.IsEnabled =
                        Add_Publisher_Address_TextBox.IsEnabled =
                        Add_Publisher_Email_TextBox.IsEnabled = false;
                    }
                    break;
                case 1:
                    {
                        Add_Publisher_Name_TextBox.IsEnabled = 
                        Add_Publisher_Address_TextBox.IsEnabled = 
                        Add_Publisher_Email_TextBox.IsEnabled = true;
                    }
                    break;
            }
        }

        #region Image Converting
        /// <summary>
        /// Convert Image into byte[] for saving in DB.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ConvertImageToBinary(Image image)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        /// <summary>
        /// Convert byte[] into Image when retreaving from DB.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static BitmapImage BitmapImageFromBytes(byte[] bytes)
        {            
            BitmapImage image = null;
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(bytes);
                stream.Seek(0, SeekOrigin.Begin);
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                image = new BitmapImage();
                image.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.StreamSource.Seek(0, SeekOrigin.Begin);
                image.EndInit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("'BitmapImageFromBytes(byte[] bytes)' method error: " + ex.Message + " or byte[] == null, " + ex.Source);
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }            
            return image;
        }

        /// <summary>
        /// Convert image from resources into System.Windows.Controls.Image
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static System.Windows.Controls.Image GetImageFromResources(string name)
        {
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            Uri uri = new Uri("pack://application:,,,/Resources/" + name, UriKind.RelativeOrAbsolute);
            ImageSource imgSource = new BitmapImage(uri);
            image.Source = imgSource;

            return image;
        }
        #endregion

        #region PreviewTextInput Event TextBoxes
        /// <summary>
        /// Data Validation for textboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Publication_ISSN_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != "-" && IsNumber(e.Text) == false) e.Handled = true;
        }
        private void Add_Publication_NumberOfCopies_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsNumber(e.Text) == false) e.Handled = true;
        }
        private void Add_Publication_NumberOfPages_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsNumber(e.Text) == false) e.Handled = true;
        }
        private void Add_Publication_PublicationDate_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != "." && e.Text != "/" && IsNumber(e.Text) == false) e.Handled = true;
        }

        /// <summary>
        /// Check is string represents a number
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool IsNumber(string text)
        {
            return int.TryParse(text, out int output);
        }
        #endregion

        #region Save & Clear Buttons _ AddPublicationGrid | Save, Clear, Delete, Modify Buttons _ DeletePublicationGrid
        // Проверку на существование в БД издателей. Почему одинаковые издатели заносятся в БД??        
       
        private void Add_Publication_Save_Button_Click(object sender, RoutedEventArgs e)
        {
            if(!IsDataFieldsAreNotEmpty())
            {
                MessageBox.Show("All fields must be filled!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {     
                using (PublishingContext db = new PublishingContext())
                {
                    Publisher publisher;
                    if (PublisherTypesComboBox.SelectedItem.ToString() == "Create New Publisher")
                    {
                        publisher = new Publisher
                        {
                            PublisherName = Add_Publisher_Name_TextBox.Text,
                            Addres = Add_Publisher_Address_TextBox.Text,
                            Email = Add_Publisher_Email_TextBox.Text
                        };
                        db.Publishers.Add(publisher);
                        db.SaveChanges();
                    }
                    else
                    {
                        publisher = db.Publishers.FirstOrDefault(p => p.PublisherName == PublisherTypesComboBox.SelectedItem.ToString());
                        db.Entry(publisher).State = EntityState.Modified;
                    }

                    Publication publication = new Publication
                    {
                        PublicationName = Add_Publication_Name_TextBox.Text,
                        ISSN = Add_Publication_ISSN_TextBox.Text,
                        Genre = Add_Publication_Genre_ComboBox.SelectedValue.ToString(),
                        Language = Add_Publication_Language_ComboBox.SelectedValue.ToString(),
                        NumberOfCopies = Convert.ToInt32(Add_Publication_NumberOfCopies_TextBox.Text),
                        NumberOfPages = Convert.ToInt32(Add_Publication_NumberOfPages_TextBox.Text),
                        Format = Add_Publication_Format_TextBox.Text,
                        DownloadLink = Add_Publication_DownloadLink_TextBox.Text,
                        Cover = ConvertImageToBinary(NewPublication_OpenImage),
                        PublicationDate = Add_Publication_PublicationDate_TextBox.Text,
                        Publisher = publisher
                    };
                    db.Publications.Add(publication);
                    publisher.Publications.Add(publication);
                    db.SaveChanges();

                    LoadDB();
                    OverridePublisherTypesComboboxRefresh();
                    Add_Publication_Clear_Button_Click(new object(), new RoutedEventArgs());
                    MessageBox.Show("You have been successfully saved new DB Item.", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Add_Publication_Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox child in FindVisualChildren<TextBox>(Grid_AddPublication))
            {
                child.Text = string.Empty;
            }
            foreach (ComboBox child in FindVisualChildren<ComboBox>(Grid_AddPublication))
            {
                child.SelectedIndex = -1;
            }
            foreach (Image child in FindVisualChildren<Image>(Grid_AddPublication))
            {
                child.Source = null;
            }
        }

        private void Delete_Publication_Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox child in FindVisualChildren<TextBox>(Grid_Modify_Publication))
            {
                child.Text = string.Empty;
            }
            foreach (ComboBox child in FindVisualChildren<ComboBox>(Grid_Modify_Publication))
            {
                child.SelectedIndex = -1;
            }
            foreach (Image child in FindVisualChildren<Image>(Grid_Modify_Publication))
            {
                child.Source = null;
            }
        }

        private void Delete_Publication_Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            TglBtnDel.IsChecked = false;
            TglBtnDel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Delete_Publication_Modify_Button.Content = "Modify";
            Delete_Publication_Delete_Button.IsEnabled = true;

            Delete_Publication_Cancel_Button.Visibility = Visibility.Collapsed;
            Delete_Publication_Cancel_Button.IsEnabled = false;
        }

        // удалять ли запись об удаленной publication в соответствующем ей publisher?
        private void Delete_Publication_Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Delete_Page_DataGrid.SelectedItem != null)
            {
                //Publisher publisher;
                Publication publication;
                using (PublishingContext db = new PublishingContext())
                {
                    int id = (Delete_Page_DataGrid.SelectedItem as Publication).PublicationId;
                    //publication = (Publication)Delete_Page_DataGrid.SelectedItem;
                    publication = db.Publications.Find(id);
                    db.Publications.Remove(publication);
                    db.SaveChanges();

                    // System.NotSupportedException: 'Unable to create a constant value of type 'Publishing.EntityData.Publication'. Only primitive types or enumeration types are supported in this context.'
                    //publisher = db.Publishers.FirstOrDefault(p => p.Publications == publication);    //publication.Publisher
                    //db.Entry(publisher).State = EntityState.Modified;
                    //publisher.Publications.Remove(publication);

                    LoadDB();
                }
            }
            else MessageBox.Show("No selected items!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }
                
        public Publication Publication { get; set; }
        private void Delete_Publication_Modify_Button_Click(object sender, RoutedEventArgs e)
        {           
           if (Delete_Page_DataGrid.SelectedItem != null)
           {
                if (TglBtnDel.IsChecked == false)
                {
                    TglBtnDel.IsChecked = true;
                    TglBtnDel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                else
                {
                    TglBtnDel.IsChecked = false;
                    TglBtnDel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                                                             
                if (Delete_Publication_Modify_Button.Content.ToString() == "Modify")                                            
                {
                    Delete_Publication_Modify_Button.Content = "Save";
                    Delete_Publication_Delete_Button.IsEnabled = false;

                    Delete_Publication_Cancel_Button.Visibility = Visibility.Visible;
                    Delete_Publication_Cancel_Button.IsEnabled = true;

                    using (PublishingContext db = new PublishingContext())
                    {
                        int id = (Delete_Page_DataGrid.SelectedItem as Publication).PublicationId;
                        Publication = db.Publications.Find(id);
                                                
                        Delete_Publication_Name_TextBox.Text = Publication.PublicationName;
                        Delete_Publication_ISSN_TextBox.Text = Publication.ISSN;
                        Delete_Publication_Genre_ComboBox.SelectedValue = Publication.Genre;
                        Delete_Publication_Language_ComboBox.SelectedValue = Publication.Language;
                        Delete_Publication_NumberOfCopies_TextBox.Text = Publication.NumberOfCopies.ToString();
                        Delete_Publication_NumberOfPages_TextBox.Text = Publication.NumberOfPages.ToString();
                        Delete_Publication_Format_TextBox.Text = Publication.Format;
                        Delete_Publication_DownloadLink_TextBox.Text = Publication.DownloadLink;
                        Delete_PublisherTypesComboBox.SelectedValue = Publication.Publisher.PublisherName;
                        DeletePublication_Image.Source = BitmapImageFromBytes(Publication.Cover);
                        Delete_Publication_PublicationDate_TextBox.Text = Publication.PublicationDate;                        
                    }
                }                
                else if (Delete_Publication_Modify_Button.Content.ToString() == "Save")                
                {
                    Delete_Publication_Modify_Button.Content = "Modify";
                    Delete_Publication_Delete_Button.IsEnabled = true;

                    Delete_Publication_Cancel_Button.Visibility = Visibility.Collapsed;
                    Delete_Publication_Cancel_Button.IsEnabled = false;

                    Publisher publisher;   

                    using (PublishingContext db = new PublishingContext())
                    {
                        publisher = db.Publishers.FirstOrDefault(p => p.PublisherName == Delete_PublisherTypesComboBox.SelectedItem.ToString());
                        db.Entry(publisher).State = EntityState.Modified;

                        int id = (Delete_Page_DataGrid.SelectedItem as Publication).PublicationId;
                        Publication = db.Publications.Find(id);
                        db.Entry(Publication).State = EntityState.Modified;
                                                
                        Publication.PublicationName = Delete_Publication_Name_TextBox.Text;
                        Publication.ISSN = Delete_Publication_ISSN_TextBox.Text;
                        Publication.Genre = Delete_Publication_Genre_ComboBox.SelectedValue.ToString();
                        Publication.Language = Delete_Publication_Language_ComboBox.SelectedValue.ToString();
                        Publication.NumberOfCopies = Convert.ToInt32(Delete_Publication_NumberOfCopies_TextBox.Text);
                        Publication.NumberOfPages = Convert.ToInt32(Delete_Publication_NumberOfPages_TextBox.Text);
                        Publication.Format = Delete_Publication_Format_TextBox.Text;
                        Publication.DownloadLink = Delete_Publication_DownloadLink_TextBox.Text;
                        Publication.Publisher = publisher;
                        Publication.Cover = ConvertImageToBinary(DeletePublication_Image);
                        Publication.PublicationDate = Delete_Publication_PublicationDate_TextBox.Text;
                                                
                        db.SaveChanges();
                        
                        LoadDB();
                    }

                    Publication = new Publication();
                    publisher = new Publisher();
                }
           }
           else MessageBox.Show("No selected items!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Find Control
        /// <summary>
        /// Helper function for searching all controls of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of control.</typeparam>
        /// <param name="depObj">Where to look for controls.</param>
        /// <returns>Enumerable list of controls.</returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        #endregion

        #region Home page grid, Retrieve images from DB records

        // Очистка информации о публикации на главной странице. Пока не используется(данные заданы абсолютно)
        private void HomePage_Clear_Publication_Description()
        {
            foreach (TextBlock child in FindVisualChildren<TextBlock>(Grid_Journal_Information))
            {
                child.Text = string.Empty;
            }            
        }

        #endregion


        // --->
        #endregion

        #region MainWindow ClearFocus();
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
        #endregion

        #region MainWindow DragMove();
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        #endregion

        #region Application Full Format Date & Time
        public void GetFullDate()
        {
            ApplicationFullDate.Content = DateTime.Now.ToLongDateString();
            // In ru-RU culture : ApplicationFullDate.Content = DateTime.Now.DayOfWeek.ToString() + ", " + DateTime.Now.ToLongDateString();
        }

        public void GetRealTime()
        {
            System.Timers.Timer Timer = new System.Timers.Timer
            {
                Interval = 1000
            };
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() => ApplicationRealTime.Content = DateTime.Now.ToLongTimeString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message + ", " + ex.Source);
            }

            // In ru-RU culture : ApplicationRealTime.Content = DateTime.Now.ToLongTimeString() + " " + DateTime.Now.AddHours(12).ToString("tt", CultureInfo.InvariantCulture)
        }
        #endregion

        #region BackgroundMusic & Button_Sounds        
        public void ButtonClickSound(string soundName)      // Buttons click sounds
        {
            try
            {
                var path = Application.GetResourceStream(new Uri("pack://application:,,,/Resources/" + soundName + ".wav"));
                if (path != null)
                {
                    using (path.Stream)
                    {
                        SoundPlayer player = new SoundPlayer(path.Stream);
                        player.Load();
                        player.Play();
                        player.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in sound generating method - " + ex.Message + ", " + ex.Source);
            }
        }

        public void ApplicationBackgroundMusic()        // Background music theme
        {
            try
            {
                ApplicationMusicTheme = new MediaPlayer();
                ApplicationMusicTheme.MediaFailed += (o, args) => { MessageBox.Show("Failed an ApplicationMusicTheme() method."); };
                ApplicationMusicTheme.MediaEnded += (o, args) => { ApplicationMusicTheme.Position = new TimeSpan(0, 0, 1); ApplicationMusicTheme.Play(); }; // Repeat
                ApplicationMusicTheme.Open(new Uri("space_ambient_music.mp3", UriKind.Relative));
                ApplicationMusicTheme.Volume = 0.4;
                ApplicationMusicTheme.Position = TimeSpan.Zero;
                ApplicationMusicTheme.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Background music error  - " + ex.Message + ", " + ex.Source);
            }
        }
        #endregion

        #region VideoPlayer
        
        private void VideoPlayerButtonsOnOff(int x)       // Main Set Method
        {
            switch (x)
            {
                case 0:
                    {
                        VideoPlayerMenu_StackPanel.Visibility = VideoPlayer_Slider_StackPanel.Visibility = Visibility.Collapsed;
                        VideoPlayerMenu_StackPanel.IsEnabled = VideoPlayer_Slider_StackPanel.IsEnabled = false;
                    }
                    break;
                case 1:
                    {
                        VideoPlayerMenu_StackPanel.Visibility = VideoPlayer_Slider_StackPanel.Visibility = Visibility.Visible;
                        VideoPlayerMenu_StackPanel.IsEnabled = VideoPlayer_Slider_StackPanel.IsEnabled = true;
                    }
                    break;
            }
        }

        private void VideoPlayer_Initiate_Button_Click(object sender, RoutedEventArgs e)        // Initial Button
        {
            VideoPlayerButtonsOnOff(1);

            VideoPlayer_Initiate_Button.Visibility = Visibility.Collapsed;

            VideoPlayer.Play();
            VideoPlayer_Play_Button.Content = FindResource("Pause");

            ApplicationMusicTheme.Pause();
            ApplicationVolumeOffOnButton.Content = FindResource("ApplicationVolumeOff");

            VideoPlayerBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5F5F5"));            
        }

        private void VideoPlayer_MediaEnded(object sender, RoutedEventArgs e) { VideoPlayer_Stop_Button_Click(new object(), new RoutedEventArgs()); }

        private void VideoPlayer_MediaFailed(object sender, ExceptionRoutedEventArgs e) { MessageBox.Show("Media filed! Smth was wrong and occured an error."); }

        #region Slider & Timers. Movie_Content_Slider. Movie_Volume_Slider. Movie_Time_Timer.
        // Добавить: Возможность перемотать видео не перетягивая слайдер, а щелкнув в любубю его часть        
        // Style https://github.com/ButchersBoy/MaterialDesignInXamlToolkit/blob/master/MaterialDesignThemes.Wpf/Themes/MaterialDesignTheme.Slider.xaml
        // How to do http://www.cyberforum.ru/wpf-silverlight/thread421374.html, https://stackoverflow.com/questions/10208959/binding-mediaelement-to-slider-position-in-wpf
        private void VideoPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            VideoPlayer_RewindValue_TextBox.Text = "5";

            // Volume Slider Initialization
            VideoPlayer.Volume = (double)VideoPlayer_VolumeSlider.Value;

            // Content Slider Initialization
            VideoPlayer_ContentSlider.Minimum = 0;
            VideoPlayer_ContentSlider.Maximum = VideoPlayer.NaturalDuration.TimeSpan.TotalSeconds;

            // Movie Time Timer
            DispatcherTimer VidepPlayerTimeTimer = new DispatcherTimer();
            VidepPlayerTimeTimer.Interval = TimeSpan.FromSeconds(0);
            VidepPlayerTimeTimer.Tick += VideoPlayerTimeTimer_Tick;
            VidepPlayerTimeTimer.Start();

            // Movie Content Slider Timer
            DispatcherTimer MoveSliderTimerTicks = new DispatcherTimer();
            MoveSliderTimerTicks.Interval = TimeSpan.FromSeconds(1);
            MoveSliderTimerTicks.Tick += Ticks_Tick;
            MoveSliderTimerTicks.Start();
        }

        // Movie Time Timer Tick Event
        private void VideoPlayerTimeTimer_Tick(object sender, EventArgs e)
        {
            if (VideoPlayer.Source != null)
            {
                if (VideoPlayer.NaturalDuration.HasTimeSpan)
                    VideoPlayer_Time_Label.Content =
                        String.Format($"{VideoPlayer.Position.ToString(@"mm\:ss")} / {VideoPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss")}");
            }
        }

        // Movie Slider Timer Events & Methods
        void ChangeStatus()
        {
            VideoPlayer_ContentSlider.Value = VideoPlayer.Position.TotalSeconds;
        }

        private void Ticks_Tick(object sender, EventArgs e)
        {
            Dispatcher.Invoke(new DragSliderTimerTick(ChangeStatus));
        }

        private void VideoPlayer_Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0, (int)VideoPlayer_ContentSlider.Value);
            VideoPlayer.Position = ts;
        }

        // Volume Slider. Change movie volume.
        private void VideoPlayer_VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VideoPlayer.Volume = (double)VideoPlayer_VolumeSlider.Value;

            if (VideoPlayer_VolumeSlider.Value == 0) VideoPlayerVolumeOffOnButton.Content = FindResource("PlayerVolumeOff");
            else VideoPlayerVolumeOffOnButton.Content = FindResource("PlayerVolumeOn");
        }
        #endregion

        #region Menu Buttons Clicks
        private void VideoPlayer_LoadNewVideo_Button_Click(object sender, RoutedEventArgs e)
        {
            string formats = "All Videos Files |*.dat; *.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf; *.ts; *.tts; *.vob; *.vro; *.webm; " +
                                                " *.avi; *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso; *.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; " +
                                                " *.mkv; *.mov; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4; " +
                                                " *.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm; *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; ";

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Open new video",
                Filter = formats,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            if (openFileDialog.ShowDialog() == true)
            {  
                VideoPlayer_Stop_Button_Click(new object(), new RoutedEventArgs());
                ApplicationMusicTheme.Pause();
                ApplicationVolumeOffOnButton.Content = FindResource("ApplicationVolumeOff");

                try
                {
                    VideoPlayer.Source = new Uri(openFileDialog.FileName, UriKind.Absolute);
                    VideoPlayer_Play_Button.Content = FindResource("Pause");
                    VideoPlayer.Play();

                    //------------------------------------------------------------  For ListView
                    //FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                    //LoadedItemName = fileInfo.Name;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in loading new MediaElement : " + ex.Message + ", " + ex.Source);
                }                
            }
            //------------------------- For ListView
            //AddNewVideoInListView();
        }

        private void VideoPlayer_Play_Button_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayerBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5F5F5"));

            if (VideoPlayer.Source == null)
            {
                try
                {
                    VideoPlayer.Source = new Uri(@"E:\ТРПО\Publishing\Publishing\bin\Debug\A_dream_within_a_dream.mp4", UriKind.Relative);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in MediaElement load Uri : " + ex.Message + ", " + ex.Source);
                }
            }

            if (VideoPlayer_Play_Button.Content == FindResource("Play"))
            {
                ApplicationMusicTheme.Pause();
                ApplicationVolumeOffOnButton.Content = FindResource("ApplicationVolumeOff");

                VideoPlayer.Play();
                VideoPlayer_Play_Button.Content = FindResource("Pause");
            }
            else
            {
                VideoPlayer.Pause();
                VideoPlayer_Play_Button.Content = FindResource("Play");
            }
        }

        private void VideoPlayer_Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            ApplicationMusicTheme.Play();
            ApplicationVolumeOffOnButton.Content = FindResource("ApplicationVolumeOn");

            VideoPlayer.Stop();
            VideoPlayer.ClearValue(MediaElement.SourceProperty);

            VideoPlayerBorder.BorderBrush = Brushes.White;

            VideoPlayer_Play_Button.Content = FindResource("Play");

            VideoPlayerVolumeOffOnButton.Content = FindResource("PlayerVolumeOn");
            VideoPlayer_VolumeSlider.Value = 0.5;
            
            VideoPlayer_ContentSlider.Value = VideoPlayer_ContentSlider.Minimum; // сброс позиции слайдера
        }

        private void VideoPlayer_Forward_Button_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Position += TimeSpan.FromSeconds(Convert.ToInt32(VideoPlayer_RewindValue_TextBox.Text));
            VideoPlayer_ContentSlider.Value = VideoPlayer.Position.TotalSeconds; // вроде работает
        }

        private void VideoPlayer_Rewind_Button_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Position -= TimeSpan.FromSeconds(Convert.ToInt32(VideoPlayer_RewindValue_TextBox.Text));
            VideoPlayer_ContentSlider.Value = VideoPlayer.Position.TotalSeconds; // вроде работает
        }

        private void VideoPlayerVolumeOffOnButton_Click(object sender, RoutedEventArgs e)
        {
            if (VideoPlayerVolumeOffOnButton.Content == FindResource("PlayerVolumeOn"))
            {
                VideoPlayer_VolumeSlider.Value = 0;
                VideoPlayerVolumeOffOnButton.Content = FindResource("PlayerVolumeOff");                
            }
            else
            {
                VideoPlayer_VolumeSlider.Value = 1;
                VideoPlayerVolumeOffOnButton.Content = FindResource("PlayerVolumeOn");
            }
        }

        private void VideoPlayer_RewindUp_Button_Click(object sender, RoutedEventArgs e)
        {
            int value = Convert.ToInt32(VideoPlayer_RewindValue_TextBox.Text);
            if (value < 5)
            {
                value += 1;
            }
            if (value >= 5 & value < 60)
            {
                value += 5;
            }
            VideoPlayer_RewindValue_TextBox.Text = value.ToString();
        }

        private void VideoPlayer_RewindDown_Button_Click(object sender, RoutedEventArgs e)
        {
            int value = Convert.ToInt32(VideoPlayer_RewindValue_TextBox.Text);
            if (value != 0)
            {
                if (value == 1) return;
                if (value <= 5)
                {
                    value -= 1;
                }
                if (value > 5 & value <= 60)
                {
                    value -= 5;
                }
            }
            VideoPlayer_RewindValue_TextBox.Text = value.ToString();
        }
        #endregion

        // Is i need this? 
        #region Video ListView 
        //public void AddNewVideoInListView()
        //{
        //    string duration = "";

        //    if (VideoPlayer.Source != null)
        //    {
        //        if (VideoPlayer.NaturalDuration.HasTimeSpan)
        //            duration = String.Format($"{VideoPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss")}");                
        //    }
        //    //Videos V = new Videos();
        //    //V.NewVideoName = LoadedItemName;
        //    //V.NewVideoDuration = duration;
        //    //VideoList.Items.Add(V);
        //    //MessageBox.Show(V.NewVideoDuration);

        //    VideoList.Items.Add(new Videos(LoadedItemName, duration));
        //}  
        #endregion

        
        #endregion

        #region "Power" button (close application) click event.
        /// <summary>
        /// Upper grid with "Close" & "Maximaze" buttons, "Close" button click logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region Application Volume Off/On Buttons Click
        private void ApplicationVolumeOffOnButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationVolumeOffOnButton.Content == FindResource("ApplicationVolumeOn"))
            {
                ApplicationMusicTheme.Pause();
                ApplicationVolumeOffOnButton.Content = FindResource("ApplicationVolumeOff");
            }
            else
            {
                ApplicationMusicTheme.Play();
                ApplicationVolumeOffOnButton.Content = FindResource("ApplicationVolumeOn");
            }
        }        
        #endregion

        #region Hide all grids ();
        /// <summary>
        /// All grids Visibility = Visibility.Hidden;
        /// </summary>
        private void InvisibleGrids()
        {
            Grid_HomePage.Visibility = Visibility.Collapsed;
            Grid_PDFView.Visibility = Visibility.Collapsed;
            Grid_MediaPlayer.Visibility = Visibility.Collapsed;
            Grid_AddPublication.Visibility = Visibility.Collapsed;
            Grid_DeletePublication.Visibility = Visibility.Collapsed;
            Grid_SearchInDB.Visibility = Visibility.Collapsed;            
            Grid_Settings.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Sliding programm main menu labels, click events.
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InvisibleGrids();
            Grid_HomePage.Visibility = Visibility.Visible;
            ButtonClickSound("btn_click_sound_1");
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            InvisibleGrids();
            Grid_AddPublication.Visibility = Visibility.Visible;
            ButtonClickSound("btn_click_sound_1");
        }

        private void Label_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            InvisibleGrids();
            Grid_DeletePublication.Visibility = Visibility.Visible;
            ButtonClickSound("btn_click_sound_1");
        }

        private void Label_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            InvisibleGrids();
            Grid_SearchInDB.Visibility = Visibility.Visible;
            ButtonClickSound("btn_click_sound_1");
        }

        private void Label_MouseLeftButtonDown_4(object sender, MouseButtonEventArgs e)
        {
            InvisibleGrids();
            Grid_PDFView.Visibility = Visibility.Visible;
            ButtonClickSound("btn_click_sound_1");
        }

        private void Label_MouseLeftButtonDown_5(object sender, MouseButtonEventArgs e)
        {
            InvisibleGrids();
            Grid_Settings.Visibility = Visibility.Visible;
            ButtonClickSound("btn_click_sound_1");
        }

        private void Label_MouseLeftButtonDown_6(object sender, MouseButtonEventArgs e)
        {
            InvisibleGrids();
            Grid_MediaPlayer.Visibility = Visibility.Visible;
            ButtonClickSound("btn_click_sound_1");
        }
        #endregion

        #region Open cover image button click events: 'Add Publication' menu page 'Grid_AddPublication' grid _ & _ 'Modify_publication'menu page 'Grid_Modify_Publication' grid .
        private void OpenCoverImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg; *.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
            };
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    if(((Button)sender).Name == "OpenCoverImageButton")
                    {
                        NewPublication_OpenImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    }
                    else if(((Button)sender).Name == "Modify_OpenCoverImageButton")
                    {
                        DeletePublication_Image.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    }
                }                    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Opening cover image error  - " + ex.Message + ", " + ex.Source);
            }            
        }
        #endregion

        #region Change publisher combobox, "Add Publication" menu page, "Grid_AddPublication" grid.
        /// <summary>
        /// At "Add Publication" menu page, "Grid_AddPublication" grid, publisher changing combobox logic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PublisherTypesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var PublisherTypesComboBox = sender as ComboBox;
            if(PublisherTypesComboBox.SelectedIndex == -1)
            {   
                PublisherTypesComboBox.SelectedIndex = -1;  // need for clear combobox
            }
            else if (PublisherTypesComboBox.SelectedItem.ToString() == "Create New Publisher")
            {
                BlockUnblock_AddPublisherInfoFields(1);

                Add_Publisher_Name_TextBox.Clear();
                Add_Publisher_Address_TextBox.Clear();
                Add_Publisher_Email_TextBox.Clear();
            }
            else
            {
                using (PublishingContext db = new PublishingContext())
                {                    
                    Publisher publisher = db.Publishers.FirstOrDefault(p => p.PublisherName == PublisherTypesComboBox.SelectedItem.ToString());
                    Add_Publisher_Name_TextBox.Text = publisher.PublisherName.ToString();
                    Add_Publisher_Address_TextBox.Text = publisher.Addres.ToString();
                    Add_Publisher_Email_TextBox.Text = publisher.Email.ToString();

                    BlockUnblock_AddPublisherInfoFields(0);                    
                }
            }            
        }
        #endregion

        #region "Open" & "Clear" button_Click Event, "View PDF File" menu page, "Grid_PDFView" grid.
        /// <summary>
        /// At "View PDF File" menu page, "Grid_PDFView" grid, "Open" button event.
        /// Open pdf-file using OpenFileDialog. Filter - Pdf Files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowserContentOpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "View PDF File",
                Filter = "Pdf Files | *.pdf"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    //WebBrowser.Navigate(openFileDialog.FileName);
                    pdfViewer.LoadFile(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source);
                }
            }
        }

        /// <summary>       
        /// Clear WebBrowser.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowserContentClearButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //WebBrowser.Navigate((Uri)null);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }




















        #endregion

        
    }
}
