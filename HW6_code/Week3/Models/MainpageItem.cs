using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Week3.Models
{
    class MainpageItem
    {
        private string id;

        public string title { get; set; }

        public ImageSource src { get; set; }

        public string details { get; set; }

        public bool completed { get; set; }

        public DateTime datetime { get; set; }

        public MainpageItem(string title, ImageSource src, string details, DateTime datetime)
        {
            this.id = Guid.NewGuid().ToString(); //生成id
            this.title = title;
            this.src = src;
            this.details = details;
            this.datetime = datetime;
            this.completed = false; //默认为未完成
        }
        public string GetId()
        {
            return id;
        }
    }
}
