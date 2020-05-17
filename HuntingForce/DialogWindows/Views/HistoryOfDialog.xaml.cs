using EngineHF;
using EngineHF.Model;
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
    /// Логика взаимодействия для HistoryOfDialog.xaml
    /// </summary>
    public partial class HistoryOfDialog : Window
    {
        MainWindowViewModel _mainWindowViewModel;
        MainWindow _mainWindow;
        Location _location;
        string lastName;
        public HistoryOfDialog(MainWindow mainWindow, Location location, MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            _mainWindowViewModel = mainWindowViewModel;
            _location = location;
            _mainWindow = mainWindow;

            var dialog = _location.Dialogs.FindAll(x => x.WasRead == true);
            
            foreach (var elm in dialog)
            {
                var grid = new Grid { Margin = new Thickness(0, 0, 0, 0) };
                var col0 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
                var col1 = new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) };
                grid.ColumnDefinitions.Add(col0);
                grid.ColumnDefinitions.Add(col1);
                TextBlock name;
                if (lastName != elm.Name)
                {
                    name = new TextBlock
                    {
                        Text = elm.Name,
                        Foreground = Brushes.White,
                        FontSize = 25,
                        TextWrapping = TextWrapping.Wrap,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2")
                    };
                    grid.Margin = new Thickness(0, 10, 0, 0);
                    lastName = elm.Name;
                }
                else
                    name = new TextBlock();

                var text = new TextBlock
                {
                    Text = elm.Text,
                    Foreground = Brushes.White,
                    FontSize = 25,
                    TextWrapping = TextWrapping.Wrap,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2")
                };
                Grid.SetColumn(text, 1);
                grid.Children.Add(name);
                grid.Children.Add(text);
                NameAndDialog.Children.Add(grid);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    _mainWindow.IsEnabled = true;
                    _mainWindowViewModel.AddingTextToDialogTextBlock(_location);
                    this.Close();
                    break;
            }
        }

        private void Window_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
