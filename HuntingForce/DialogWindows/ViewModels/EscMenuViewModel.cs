using HuntingForce.DialogWindows.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingForce.DialogWindows
{
    public class EscMenuViewModel: BindableBase
    {
        private EscMenu _escMenu;
        private MainWindow _mainWindow;
        public EscMenuViewModel(MainWindow mainWindow, EscMenu escMenu)
        {
            _escMenu = escMenu;
            _mainWindow = mainWindow;
            SettingsViewCommand = new DelegateCommand(OnSettingsViewCommand, () => true);
        }
        public DelegateCommand SettingsViewCommand { get; set; }
        public void OnSettingsViewCommand()
        {
            SettingsView options = new SettingsView(_mainWindow, _escMenu);
            options.Show();
            _escMenu.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
