using EngineHF;
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
        private GameSession _gameSession;
        public EscMenuViewModel(MainWindow mainWindow, EscMenu escMenu, GameSession gameSession)
        {
            _escMenu = escMenu;
            _mainWindow = mainWindow;
            _gameSession = gameSession;
            SettingsViewCommand = new DelegateCommand(OnSettingsViewCommand, () => true);
            SaveViewCommand = new DelegateCommand(OnSaveViewCommand, () => true);
        }
        public DelegateCommand SettingsViewCommand { get; set; }
        public DelegateCommand SaveViewCommand { get; set; }
        public void OnSettingsViewCommand()
        {
            SettingsView options = new SettingsView(_mainWindow, _escMenu);
            options.Show();
            _escMenu.Visibility = System.Windows.Visibility.Collapsed;
        }
        public void OnSaveViewCommand()
        {
            SaveView save = new SaveView(_gameSession);
            save.Owner = _mainWindow;
            save.Show();
            _escMenu.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
