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
using WPFPdfViewer;

namespace Publishing
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region MainWindow()
        public MainWindow()
        {
            InitializeComponent();            

            InvisibleGrids();
            DataContext = new ComboBoxViewModel();         
            Grid_HomePage.Visibility = Visibility.Visible;

            GetFullDate();
            GetRealTime();
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
            Dispatcher.Invoke(() => ApplicationRealTime.Content = DateTime.Now.ToLongTimeString());

            // In ru-RU culture : ApplicationRealTime.Content = DateTime.Now.ToLongTimeString() + " " + DateTime.Now.AddHours(12).ToString("tt", CultureInfo.InvariantCulture)
        }
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

        #region Hide all grids method.
        /// <summary>
        /// All grids Visibility = Visibility.Hidden;
        /// </summary>
        private void InvisibleGrids()
        {
            Grid_HomePage.Visibility = Visibility.Hidden;
            Grid_AddPublication.Visibility = Visibility.Hidden;
            Grid_DeletePublication.Visibility = Visibility.Hidden;
            Grid_SearchInDB.Visibility = Visibility.Hidden;
            Grid_PDFView.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Sliding menu items_Click Event.
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InvisibleGrids();
            Grid_HomePage.Visibility = Visibility.Visible;
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            InvisibleGrids();
            Grid_AddPublication.Visibility = Visibility.Visible;
        }

        private void Label_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            InvisibleGrids();
            Grid_DeletePublication.Visibility = Visibility.Visible;
        }

        private void Label_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            InvisibleGrids();
            Grid_SearchInDB.Visibility = Visibility.Visible;
        }

        private void Label_MouseLeftButtonDown_4(object sender, MouseButtonEventArgs e)
        {
            InvisibleGrids();
            Grid_PDFView.Visibility = Visibility.Visible;
        }
        #endregion

        #region "Add Publication" menu page, "Grid_AddPublication" grid, opening cover image button_Click Event.
        private void OpenCoverImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg; *.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
            };

            if (openFileDialog.ShowDialog() == true)
                NewPublication_OpenImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
        }
        #endregion

        #region "Add Publication" menu page, "Grid_AddPublication" grid, publisher changing combobox.
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

        #region "View PDF File" menu page, "Grid_PDFView" grid, "Open" & "Clear" button_Click Event.
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
