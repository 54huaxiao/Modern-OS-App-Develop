using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Imaging;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Week3
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

        private ViewModels.MainpageItemViewModel ViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = ((ViewModels.MainpageItemViewModel)e.Parameter);
            if (ViewModel.SelectedItem == null)
            {
                createButton.Content = "Create";
                var i = new MessageDialog("Welcome to our pirate hotel !").ShowAsync();
            }
            else
            {
                createButton.Content = "Update";
                title.Text = ViewModel.SelectedItem.title;
                details.Text = ViewModel.SelectedItem.details;
                datetime.Date = ViewModel.SelectedItem.datetime;
                //Windows.UI.Xaml.Media.Imaging.BitmapImage bit = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                //bit.UriSource = new Uri(image.BaseUri, ViewModel.SelectedItem.src);
                image.Source = ViewModel.SelectedItem.src;
                var i = new MessageDialog("Do you want to update something ?").ShowAsync();
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem == null)
            {
                title.Text = "";
                details.Text = "";
                this.datetime.Date = DateTime.Now;
                Windows.UI.Xaml.Media.Imaging.BitmapImage bit = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                bit.UriSource = new Uri(image.BaseUri, "Assets/qianyang.jpeg");
                image.Source = bit;
            }
            else
            {
                title.Text = ViewModel.SelectedItem.title;
                details.Text = ViewModel.SelectedItem.details;
                datetime.Date = ViewModel.SelectedItem.datetime;
                image.Source = ViewModel.SelectedItem.src;
            }
        }

        private void CreateButton_Clicked(object sender, RoutedEventArgs e)
        {
            bool judge = this.datetime.Date < DateTime.Today;
            if (title.Text != "" && details.Text != "" && !judge)
            {
                if (createButton.Content.ToString() == "Create")
                {
                    var n = new MessageDialog("Congratulations! You have created a new item successfully!").ShowAsync();
                    ViewModel.AddTodoItem(title.Text, image.Source, details.Text, datetime.Date.DateTime);
                    Frame.Navigate(typeof(MainPage), ViewModel);
                }
                if (createButton.Content.ToString() == "Update")
                {
                    var n = new MessageDialog("Congratulations! You have updated a new item successfully!").ShowAsync();
                    ViewModel.UpdateTodoItem(ViewModel.SelectedItem.GetId(),title.Text, details.Text, image.Source, datetime.Date.DateTime);
                    Frame.Navigate(typeof(MainPage), ViewModel);
                    ViewModel.SelectedItem = null;
                }
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
        private async void DeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            MessageDialog msg = new MessageDialog("Do you want to delete this item?");
            msg.Commands.Add(new UICommand("Yes", command =>
            {
                Frame.Navigate(typeof(MainPage), ViewModel);
            }));
            msg.Commands.Add(new UICommand("No", command =>
            {

            }));
            var msg1info = await msg.ShowAsync();

            if (ViewModel.SelectedItem != null)
            {
                ViewModel.RemoveTodoItem(ViewModel.SelectedItem.GetId());
            }

            ViewModel.SelectedItem = null;
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
    }
}
