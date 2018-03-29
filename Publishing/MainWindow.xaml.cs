﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Publishing
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ComboBoxViewModel();         // Test!!!     // Класс со списком, задающимся в конструкторе. // Далее - биндим в ComboBox ItemSource
            Grid_HomePage.Visibility = Visibility.Visible;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InvisibleGrids();
            Grid_HomePage.Visibility = Visibility.Visible;
        }

        private void InvisibleGrids()
        {
            Grid_HomePage.Visibility = Visibility.Hidden;
            Grid_AddPublication.Visibility = Visibility.Hidden;
            Grid_DeletePublication.Visibility = Visibility.Hidden;
            Grid_SearchInDB.Visibility = Visibility.Hidden;
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
    }
}
