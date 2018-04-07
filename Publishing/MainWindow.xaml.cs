using Microsoft.Win32;
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
        #endregion

        #region MainWindow()
        public MainWindow()
        {            
            InitializeComponent();

            InvisibleGrids();
                     
            ApplicationBackgroundMusic();

            Grid_HomePage.Visibility = Visibility.Visible;
            
        }
        #endregion

        #region MainWindow Window_Loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            GetFullDate();
            GetRealTime();

            VideoPlayerButtonsOnOff(0);

            DataContext = new ComboBoxViewModel();                                           
        }
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
            switch(x)
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
            TimeSpan ts = new TimeSpan(0, 0, 0,  (int)VideoPlayer_ContentSlider.Value);
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in loading new MediaElement : " + ex.Message + ", " + ex.Source);
                }
            }
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
            VideoPlayer.Volume = 1;

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
                VideoPlayer.Volume = 0;
                VideoPlayerVolumeOffOnButton.Content = FindResource("PlayerVolumeOff");

                VideoPlayer_VolumeSlider.Value = 0;
            }
            else
            {
                VideoPlayer.Volume = 1;
                VideoPlayerVolumeOffOnButton.Content = FindResource("PlayerVolumeOn");

                VideoPlayer_VolumeSlider.Value = 1;
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
            if(value != 0)
            {
                if (value == 1) return;
                if(value <= 5 )
                {
                    value -= 1;
                }
                if(value > 5 & value <= 60)
                {
                    value -= 5;
                }
            }
            VideoPlayer_RewindValue_TextBox.Text = value.ToString();
        }
        #endregion

        #endregion

        #region "Power" button (close application)_Click Event.
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

        #region Volume Off/On Buttons Click
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

        #region Sliding menu labels_Click Event.
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

        #region Open cover image button_Click Event, "Add Publication" menu page, "Grid_AddPublication" grid.
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
                    NewPublication_OpenImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
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
            if (PublisherTypesComboBox.SelectedItem.ToString() == "Create New Publisher")
            {
                Add_Publisher_Name_TextBox.Clear();
                Add_Publisher_Address_TextBox.Clear();
                Add_Publisher_Email_TextBox.Clear();
            }
            else
            {
                Add_Publisher_Name_TextBox.Text = "McGraw - Hill Education";
                Add_Publisher_Address_TextBox.Text = "Unated States of America, DC Washington, 123 6th St.Melbourne, FL 32904";
                Add_Publisher_Email_TextBox.Text = "slavadonnikov@gmail.com";
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



        //-----------------------------------------------------------------------------------------------------
        #region Test functional.
        //private void Grid_MouseMouseLeftButtonDownDown(object sender, MouseButtonEventArgs e)
        //{
        //    byte click_num = 0;

        //    if (e.ClickCount == 1)
        //    {
        //        click_num++;
        //    }
        //    if (click_num > 2) click_num = 2;

        //    switch (click_num)
        //    {
        //        case 1:
        //            pic_stp_1.Visibility = Visibility.Visible;
        //            break;
        //        case 2:
        //            pic_stp_1.Visibility = Visibility.Hidden;
        //            break;
        //    }
        //}       
        #endregion

        
    }
}
