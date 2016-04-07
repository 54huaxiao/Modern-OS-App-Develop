using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todos.Models;
using Newtonsoft.Json;
using Windows.Storage;

namespace Todos.ViewModels
{
    class TheViewModel : ViewModelBase
    {
        private string title;
        public string Title { get { return title; } set { Set(ref title, value); } }

        private string details;
        public string Details { get { return details; } set { Set(ref details, value); } }

        #region Methods for handling the apps permanent data
        public void LoadData()
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("TheData"))
            {
                MyDataItem data = JsonConvert.DeserializeObject<MyDataItem>(
                    (string)ApplicationData.Current.RoamingSettings.Values["TheData"]);
                Title = data.Title;
                Details = data.Details;
            }
            else
            {
                // New start, initialize the data
                Title = string.Empty;
                Details = string.Empty;
            }
        }

        public void SaveData()
        {
            MyDataItem data = new MyDataItem { Title = this.Title, Details = this.Details };
            ApplicationData.Current.RoamingSettings.Values["TheData"] =
                JsonConvert.SerializeObject(data);
        }
        #endregion

    }
}
