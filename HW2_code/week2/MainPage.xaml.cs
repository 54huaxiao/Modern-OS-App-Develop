﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace week2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
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
        }
        private void Add_AppBar_Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewPage), "");
        }
        private void check_Checked(object sender, RoutedEventArgs e)
        {
            if (check.IsChecked == true)
            {
                this.line.Visibility = Visibility.Visible;
            }
            else
            {
                this.line.Visibility = Visibility.Collapsed;
            }
        }
        private void check_Checked1(object sender, RoutedEventArgs e)
        {
            if (check1.IsChecked == true)
            {
                this.line1.Visibility = Visibility.Visible;
            }
            else
            {
                this.line1.Visibility = Visibility.Collapsed;
            }
        }
        private void check_Checked2(object sender, RoutedEventArgs e)
        {
            if (check2.IsChecked == true)
            {
                this.line2.Visibility = Visibility.Visible;
            }
            else
            {
                this.line2.Visibility = Visibility.Collapsed;
            }
        }
        private void check_Checked3(object sender, RoutedEventArgs e)
        {
            if (check3.IsChecked == true)
            {
                this.line3.Visibility = Visibility.Visible;
            }
            else
            {
                this.line3.Visibility = Visibility.Collapsed;
            }
        }
    }
}
