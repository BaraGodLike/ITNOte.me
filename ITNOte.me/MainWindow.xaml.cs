﻿using System.Windows;
using System.Windows.Controls;

namespace ITNOte.me;

public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        InitializeComponent();
        MainFrame.NavigationService.Navigate(new Uri("View/LoginPage.xaml", UriKind.Relative));
        
    }
}