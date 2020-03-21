using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HuntingForce.DialogWindows.Views
{
    /// <summary>
    /// Логика взаимодействия для Options.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        private MainWindow _mainWindow;
        private EscMenu _escMenu;
    
        public SettingsView(MainWindow mainWindow, EscMenu escMenu)
        {
            _escMenu = escMenu;
            _mainWindow = mainWindow;
            InitializeComponent();
            _mainWindow.IsEnabled = false;
            fullScreen.IsChecked = Settings.IsFullScreen;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)fullScreen.IsChecked)
            {
                Settings.IsFullScreen = true; 
                _mainWindow.WindowStyle = WindowStyle.None;
                _mainWindow.ResizeMode = ResizeMode.NoResize;
                _mainWindow.WindowState = WindowState.Maximized;
                _mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                WindowState = WindowState.Normal;
            }
            else
            {
                Settings.IsFullScreen = false;
                _mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_escMenu.Visibility == Visibility.Collapsed)
                _escMenu.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _escMenu.Visibility = Visibility.Visible;
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Close();
                    break;
            }
        }
    }
}
