﻿using DeRoso.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DeRoso.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
        {
            CurrentContent = ContentManager.Instance.GetContent("HOME");
        }

        private UserControl _currentContent;

        public UserControl CurrentContent
        {
            get { return _currentContent; }
            set
            {
                if (_currentContent == value)
                    return;

                _currentContent = value;
                OnPropertyChanged();
            }
        } 


        public static RoutedUICommand HomePage
        {
            get;
        } = new RoutedUICommand("Go to home page", "Home", typeof(MainWindowViewModel), null);

        public static RoutedUICommand EditDBPage
        {
            get;
        } = new RoutedUICommand("Edit database", "EditDB", typeof(MainWindowViewModel), null);

        public static RoutedUICommand ArchivePage
        {
            get;
        } = new RoutedUICommand("Go to archive page", "Archive", typeof(MainWindowViewModel), null);

        public static RoutedUICommand HelpPage
        {
            get;
        } = new RoutedUICommand("Go to help page", "Home", typeof(MainWindowViewModel), null);

        public static RoutedUICommand TestingPage
        {
            get;
        } = new RoutedUICommand("Go to testing page", "Testing", typeof(MainWindowViewModel), null);

        public static RoutedUICommand StartTestDevice
        {
            get;
        } = new RoutedUICommand("Test device", "Testing device", typeof(MainWindowViewModel), null);


        public static RoutedUICommand SelectTestsPage
        {
            get;
        } = new RoutedUICommand("Select tests", "Select test for process", typeof(MainWindowViewModel), null);

        public static RoutedUICommand StartTest
        {
            get;
        } = new RoutedUICommand("Begin testing", "Begin testing process", typeof(MainWindowViewModel), null);

        public static RoutedUICommand StopTest
        {
            get;
        } = new RoutedUICommand("Stop testing", "Break testing process", typeof(MainWindowViewModel), null);

        public static RoutedUICommand PauseTest
        {
            get;
        } = new RoutedUICommand("Pause testing", "Pause testing process", typeof(MainWindowViewModel), null);
    }
}
