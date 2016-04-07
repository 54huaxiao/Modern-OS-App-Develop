using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Week3.ViewModels
{
    class MainpageItemViewModel
    {
        private ObservableCollection<Models.MainpageItem> allItems = new ObservableCollection<Models.MainpageItem>();
        public ObservableCollection<Models.MainpageItem> AllItems { get { return this.allItems; } }

        private Models.MainpageItem selectedItem = default(Models.MainpageItem);
        public Models.MainpageItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; } }

        public MainpageItemViewModel(object sender)
        {
            Image image = sender as Image;
            this.allItems.Add(new Models.MainpageItem("现代操作系统", new BitmapImage(new Uri(image.BaseUri, "Assets/xiaochou.jpg")), "GPA:4.8", DateTime.Today));
            this.allItems.Add(new Models.MainpageItem("数据库系统", new BitmapImage(new Uri(image.BaseUri, "Assets/aisi.jpg")), "GPA:4.0", DateTime.Today));
            this.allItems.Add(new Models.MainpageItem("计算机组成原理", new BitmapImage(new Uri(image.BaseUri, "Assets/lufei.jpeg")), "GPA:4.5", DateTime.Today));
            this.allItems.Add(new Models.MainpageItem("信号系统", new BitmapImage(new Uri(image.BaseUri, "Assets/putong.jpg")), "GPA:4.5", DateTime.Today));
        }

        public void AddTodoItem(string title, ImageSource src, string details, DateTime datetime)
        {
            this.allItems.Add(new Models.MainpageItem(title, src, details, datetime));
        }

        public void RemoveTodoItem(string id)
        {
            //DIY
            for (int i = allItems.Count-1; i >= 0; i--)
            {
                var item = allItems[i];
                if (item.GetId() == id)
                {
                    allItems.RemoveAt(i);
                }
            }
            //set selectedItem to null after remove
            this.selectedItem = null;
        }

        public void UpdateTodoItem(string id, string title, string details, ImageSource src, DateTime datetime)
        {
            // DIY
            for (int i = allItems.Count-1; i>= 0; i--)
            {
                var item = allItems[i];
                if (item.GetId() == id)
                {
                    item.title = title;
                    item.details = details;
                    item.datetime = datetime;
                    item.src = src;
                }
            }
            // set selectedItem to null after update
            this.selectedItem = null;
        }
    }
}
