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

namespace HuntingForce.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для TypeOfLoggingSelectionWindow.xaml
    /// </summary>
    public partial class TypeOfLoggingSelectionWindow : Window
    {
        public TypeOfLoggingSelectionWindow(List<string> _currentTypesOfLogs)
        {
            InitializeComponent();
            foreach (CheckBox elm in MainGrid.Children)
                if (_currentTypesOfLogs.Contains(elm.Name))
                    elm.IsChecked = true;
        }
        List<string> result = new List<string>();
        public List<string> Result
        {
            get
            { 
                foreach (CheckBox elm in MainGrid.Children)
                    if ((bool)elm.IsChecked)
                        result.Add(elm.Name);
                return result;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
