using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Popups;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.ApplicationModel.DataTransfer;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Week3
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static string title1;
        public static string details1;
        public static ImageSource image1;
        public MainPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
            this.ViewModel = new ViewModels.MainpageItemViewModel(image);
            DataTransferManager.GetForCurrentView().DataRequested += MainPage_DataRequest;
        }

        ViewModels.MainpageItemViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType() == typeof(ViewModels.MainpageItemViewModel))
                this.ViewModel = (ViewModels.MainpageItemViewModel)(e.Parameter);
        }
        private void TodoItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            if (InlineToDoItemViewGrid.Visibility != Visibility.Visible)
            {
                ViewModel.SelectedItem = (Models.MainpageItem)(e.ClickedItem);
                Frame.Navigate(typeof(NewPage), ViewModel);
            }
            else
            {
                UpdateButton.Visibility = Visibility.Visible;
                createButton.Visibility = Visibility.Collapsed;
                ViewModel.SelectedItem = (Models.MainpageItem)(e.ClickedItem);
                title.Text = ViewModel.SelectedItem.title;
                image.Source = ViewModel.SelectedItem.src;
                details.Text = ViewModel.SelectedItem.details;
                date.Date = ViewModel.SelectedItem.datetime;
                // ViewModel.SelectItem = null;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem == null)
            {
                title.Text = "";
                details.Text = "";
                this.date.Date = DateTime.Now;
                Windows.UI.Xaml.Media.Imaging.BitmapImage bit = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                bit.UriSource = new Uri(image.BaseUri, "Assets/qianyang.jpeg");
                image.Source = bit;
            }
            else
            {
                title.Text = ViewModel.SelectedItem.title;
                details.Text = ViewModel.SelectedItem.details;
                date.Date = ViewModel.SelectedItem.datetime;
                image.Source = ViewModel.SelectedItem.src;
            }
        }

        private void Create_Clicked(object sender, RoutedEventArgs e)
        {
            bool judge = this.date.Date < DateTime.Today;
            if (title.Text != "" && details.Text != "" && !judge)
            {
                    var n = new MessageDialog("Congratulations! You have created a new item successfully!").ShowAsync();
                    ViewModel.AddTodoItem(title.Text, image.Source, details.Text, date.Date.DateTime);
                    Frame.Navigate(typeof(MainPage), ViewModel);
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

        private void Update_Clicked(object sender, RoutedEventArgs e)
        {
            bool judge = this.date.Date < DateTime.Today;
            if (title.Text != "" && details.Text != "" && !judge)
            {
                var n = new MessageDialog("Congratulations! You have updated a new item successfully!").ShowAsync();
                ViewModel.UpdateTodoItem(ViewModel.SelectedItem.GetId(), title.Text, details.Text, image.Source, date.Date.DateTime);
                Frame.Navigate(typeof(MainPage), ViewModel);
                ViewModel.SelectedItem = null;
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

        private async void MainPage_DataRequest(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var defl = args.Request.GetDeferral();
            var dp = args.Request.Data;
            var photofile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(image.BaseUri, ((BitmapImage)image1).UriSource.ToString()));
            dp.Properties.Title = title1;
            dp.Properties.Description = details1;
            dp.SetText(details1);
            dp.SetStorageItems(new List<StorageFile> { photofile });
            defl.Complete();
        }

        private void share_click(object sender, RoutedEventArgs e)
        {
            //To Do
            MenuFlyoutItem item = sender as MenuFlyoutItem;
            if (item != null)
            {
                Models.MainpageItem models = item.DataContext as Models.MainpageItem;
                title1 = models.title;
                details1 = models.details;
                image1 = models.src;
            }
            DataTransferManager.ShowShareUI();
        }

        private void Add_AppBar_Button_Click(object sender, RoutedEventArgs e)
        {
            if (InlineToDoItemViewGrid.Visibility != Visibility.Visible)
            {
                Frame.Navigate(typeof(NewPage), ViewModel);
            }
            else
            {
                title.Text = "";
                details.Text = "";
                this.date.Date = DateTime.Now;
                Windows.UI.Xaml.Media.Imaging.BitmapImage bit = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                bit.UriSource = new Uri(image.BaseUri, "Assets/qianyang.jpeg");
                image.Source = bit;
                UpdateButton.Content = "Create";
            }
        }

        private void updatetile_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument tile = new XmlDocument();
            tile.LoadXml(File.ReadAllText("tile.xml"));

            XmlNodeList tileText = tile.GetElementsByTagName("text");
            for (int i = 0; i < tileText.Count; i++)
            {
                ((XmlElement)tileText[i]).InnerText = ViewModel.AllItems[ViewModel.AllItems.Count - 1].title;
                i++;
                ((XmlElement)tileText[i]).InnerText = ViewModel.AllItems[ViewModel.AllItems.Count - 1].details;

            }

            TileNotification notification = new TileNotification(tile);
            var updator = TileUpdateManager.CreateTileUpdaterForApplication();
            updator.Update(notification);
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
    public class boolToint : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool? ischeched = value as bool?;
            if (ischeched == null || ischeched == false)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
