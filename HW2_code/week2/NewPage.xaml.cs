using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace week2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage : Page
    {
        public NewPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
            var i = new MessageDialog("Welcome to the register page!").ShowAsync();
        }
        private async void photo_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.Desktop;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".bmp");
            picker.FileTypeFilter.Add(".gif");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                Windows.UI.Xaml.Media.Imaging.BitmapImage bmp = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                bmp.SetSource(stream);
                this.image.Source = bmp;
            }
        }
        private void Name_In(object sender, RoutedEventArgs e)
        {
            name.Text = "";
        }
        private void Phone_In(object sender, RoutedEventArgs e)
        {
            phone.Text = "";
        }
        private void Request_In(object sender, RoutedEventArgs e)
        {
            request.Text = "";
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            name.Text = "";
            phone.Text = "";
            request.Text = "";
            this.date.Date = DateTime.Now;
            Windows.UI.Xaml.Media.Imaging.BitmapImage bit = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
            // bit.UriSource = new Uri("http://img5.duitang.com/uploads/item/201207/10/20120710182553_48WU3.jpeg");
            bit.UriSource = new Uri(image.BaseUri, "Assets/qianyang.jpeg");
            image.Source = bit;
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            bool judge = this.date.Date < DateTime.Today;
            string na = "Enter your name...", ph = "Enter your phone number...", re = "Enter your request...";
            if (name.Text != "" && phone.Text != "" && request.Text != "" && name.Text != na && phone.Text != ph && request.Text != re && !judge)
            {
                name.Text = "Succeed";
                phone.Text = "Succeed";
                request.Text = "Succeed";
                var n = new MessageDialog("Congratulations! You have registered successfully!").ShowAsync();
            }
            else if (judge)
            {
                var n = new MessageDialog("You can't enter the date before!").ShowAsync();
            }
            else
            {
                var n = new MessageDialog("Something has not finished above!").ShowAsync();
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
