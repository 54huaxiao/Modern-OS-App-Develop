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

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Animal
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private delegate string AnimalSaying(object sender, myEventArgs e);//声明一个委托
        private event AnimalSaying Say;//委托声明一个事件
        private int times = 0;
        private int ttimes = 0;

        public MainPage()
        {
            this.InitializeComponent();
        }

        interface Animal
        {
            //方法
            string saying(object sender, myEventArgs e);
            //属性
            int A { get; set; }
        }
        class pig : Animal
        {
            TextBlock word;
            private int a;

            public pig(TextBlock w)
            {
                this.word = w;
            }
            public string saying(object sender, myEventArgs e)
            {
                this.word.Text += "pig: I am a Pig." + "\n";
                return "";
            }
            public int A
            {
                get { return a; }
                set { this.a = value; }
            }
        }
        class cat : Animal
        {
            TextBlock word;
            private int a;

            public cat(TextBlock w)
            {
                this.word = w;
            }
            public string saying(object sender, myEventArgs e)
            {
                this.word.Text += "cat: I am a Cat." + "\n";
                return "";
            }
            public int A
            {
                get { return a; }
                set { this.a = value; }
            }
        }

        class dog : Animal
        {
            TextBlock word;
            private int a;

            public dog(TextBlock w)
            {
                this.word = w;
            }
            public string saying(object sender, myEventArgs e)
            {
                this.word.Text += "dog: I am a Dog.\n";
                return "";
            }
            public int A
            {
                get { return a; }
                set { this.a = value; }
            }
        }

        private cat c;
        private dog d;
        private pig p;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.textBlock.Text = "";
            if (times == 0)
            {
                c = new cat(this.textBlock);
                d = new dog(this.textBlock);
                p = new pig(this.textBlock);
            }
            Random i = new Random();
            int x = i.Next(3);
            switch (x)
            {
                case 0:
                    Say += new AnimalSaying(c.saying);
                    Say(this, new myEventArgs(times++));
                    break;
                case 1:
                    Say += new AnimalSaying(d.saying);
                    Say(this, new myEventArgs(times++));
                   // Say -= new AnimalSaying(d.saying);
                    break;
                case 2:
                    Say += new AnimalSaying(p.saying);
                    Say(this, new myEventArgs(times++));
                   // Say -= new AnimalSaying(p.saying);
                    break;
            }
        }

        //自定义一个Eventargs传递事件参数
        class myEventArgs : EventArgs
        {
            public int t = 0;
            public myEventArgs(int tt)
            {
                this.t = tt;
            }
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            this.textBlock.Text = "";
            if (ttimes == 0)
            {
                c = new cat(this.textBlock);
                d = new dog(this.textBlock);
                p = new pig(this.textBlock);
                ttimes++;
            }
            switch (this.textBox.Text)
            {
                case "cat":
                    Say += new AnimalSaying(c.saying);
                    Say(this, new myEventArgs(times++));
                   // Say -= new AnimalSaying(c.saying);
                    break;
                case "dog":
                    Say += new AnimalSaying(d.saying);
                    Say(this, new myEventArgs(times++));
                   // Say -= new AnimalSaying(d.saying);
                    break;
                case "pig":
                    Say += new AnimalSaying(p.saying);
                    Say(this, new myEventArgs(times++));
                   // Say -= new AnimalSaying(p.saying);
                    break;
                default:
                    break;
            }
            this.textBox.Text = "";
        }
        private void Mouse_In(object sender, RoutedEventArgs e)
        {
            textBox.Text = "";
        }
    }
}
