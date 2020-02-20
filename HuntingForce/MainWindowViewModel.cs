using EngineHF;
using EngineHF.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace HuntingForce
{
    public class MainWindowViewModel: BindableBase
    {
        public readonly GameSession _gameSession; 
        private bool inFight;
        //private readonly Dictionary<Key, Action> _userInputActions =
        //    new Dictionary<Key, Action>();
        public MainWindowViewModel(GameSession gameSession)
        {
            _gameSession = gameSession;
            Attack = new DelegateCommand(OnAttack, () => true);
            West = new DelegateCommand(OnWest, () => true);
            North = new DelegateCommand(OnNorth, () => true);
            East = new DelegateCommand(OnEast, () => true);
            South = new DelegateCommand(OnSouth, () => true);
            TeleportToHome = new DelegateCommand(OnTeleportToHome, () => true);
            SkipDialog = new DelegateCommand(OnSkipDialog, () => true);
            ClearTheLog = new DelegateCommand(OnClearTheLog, () => true);

            SetVisibilityForMovement();
            HPbar = $"{_gameSession.mainStats.CurrentHP}/{_gameSession.mainStats.MaxHP}";
            MPbar = $"{_gameSession.mainStats.CurrentMP}/{_gameSession.mainStats.MaxMP}";
            SPbar = $"{_gameSession.mainStats.CurrentSP}/{_gameSession.mainStats.MaxSP}";
            NickName = _gameSession.mainStats.Name;
            XPbarMax = _gameSession.mainStats.MaxXP;
            XPbarValue = _gameSession.mainStats.CurrentXP;
            CountOfXPbar = $"{XPbarValue}/{XPbarMax}";
            _gameSession.mainStats.CurrentWeapon = _gameSession._standardGameItems.First(x => x.ItemID == 1);
            Enemy = _gameSession.CurrentWorld.LocationAt(0, 0).ImageName.Remove(0, 1);
            ToCommonLocation();
            NameOfLocation = _gameSession.currentPos.Name;

        }
        public async void DialogAdd()
        {
            MoveButton = false;
            Dialog = "";
            await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
            {
                foreach (var _char in _gameSession.currentPos.Description)
                {
                    if (MoveButton)
                        return;
                    Dialog += _char;
                    await Task.Delay(50);
                }
                MoveButton = true;
            }
            ));
        }
        public void DialogAddAll()
        {
            MoveButton = true;
            Dialog = _gameSession.currentPos.Description;
        }
        public DelegateCommand ClearTheLog { get; set; }

        public DelegateCommand SkipDialog { get; set; }
        public DelegateCommand Attack { get; set; }

        public DelegateCommand TeleportToHome { get; set; }
        public DelegateCommand West { get; set; }
        public DelegateCommand North { get; set; }
        public DelegateCommand East { get; set; }
        public DelegateCommand South { get; set; }

        private bool _moveButton = true;
        public bool MoveButton
        {
            get => _moveButton;
            set => SetProperty(ref _moveButton, value);
        }
        #region Movement Visibility
        private Visibility _westVis;
        public Visibility WestVis
        {
            get => _westVis;
            set => SetProperty(ref _westVis, value);
        }

        private Visibility _northVis;
        public Visibility NorthVis
        {
            get => _northVis;
            set => SetProperty(ref _northVis, value);
        }

        private Visibility _eastVis;
        public Visibility EastVis
        {
            get => _eastVis;
            set => SetProperty(ref _eastVis, value);
        }

        private Visibility _southVis;
        public Visibility SouthVis
        {
            get => _southVis;
            set => SetProperty(ref _southVis, value);
        }
        #endregion

        #region Dialog
        private string _dialog;
        public string Dialog
        {
            get => _dialog;
            set => SetProperty(ref _dialog, value);
        }
        #endregion

        #region Bars
        private string _nickName;
        public string NickName
        {
            get => _nickName;
            set => SetProperty(ref _nickName, value);
        }
        private string _hPbar;
        public string HPbar
        {
            get => _hPbar;
            set => SetProperty(ref _hPbar, value);
        }

        private string _mPbar;
        public string MPbar
        {
            get => _mPbar;
            set => SetProperty(ref _mPbar, value);
        }

        private string _sPbar;
        public string SPbar
        {
            get => _sPbar;
            set => SetProperty(ref _sPbar, value);
        }

        private double _xPbarValue;
        public double XPbarValue
        {
            get => _xPbarValue;
            set => SetProperty(ref _xPbarValue, value);
        }

        private double _xPbarMax;
        public double XPbarMax
        {
            get => _xPbarMax;
            set => SetProperty(ref _xPbarMax, value);
        }

        private string _countOfXPbar;
        public string CountOfXPbar
        {
            get => _countOfXPbar;
            set => SetProperty(ref _countOfXPbar, value);
        }
        #endregion

        #region Location information
        private string _enemy;
        public string Enemy
        {
            get => _enemy;
            set => SetProperty(ref _enemy, value);
        }
        private string _nameOfLocation;
        public string NameOfLocation
        {
            get => _nameOfLocation;
            set => SetProperty(ref _nameOfLocation, value);
        }
        private string _hPBarOfMonster;
        public string HPBarOfMonster
        {
            get => _hPBarOfMonster;
            set => SetProperty(ref _hPBarOfMonster, value);
        }
        private string _nameOfLocationVerticalAlignment;
        public string NameOfLocationVerticalAlignment
        {
            get => _nameOfLocationVerticalAlignment;
            set => SetProperty(ref _nameOfLocationVerticalAlignment, value);
        }
        private int _nameOfLocationRowSpan;
        public int NameOfLocationRowSpan
        {
            get => _nameOfLocationRowSpan;
            set => SetProperty(ref _nameOfLocationRowSpan, value);
        }
        #endregion

        #region Loging
        private string _log;
        public string Log
        {
            get => _log;
            set => SetProperty(ref _log, value);
        }
        #endregion

        #region Attack Methods
        public void OnAttack()
        {
            Attacking(_gameSession.mainStats.CurrentWeapon.MinDamage, _gameSession.mainStats.CurrentWeapon.MaxDamage);
        }
        public void Attacking(int minDamage, int maxDamage)
        {
            if (_gameSession.currentPos.monster == null)
            {
                MessageBox.Show("В городе нельзя драться!");
                return;
            }
            HeroAttacks(minDamage, maxDamage);
            MonsterAttacks();
        }
        private void HeroAttacks(int minDamage, int maxDamage)
        {
            var damage = new Random().Next(minDamage, maxDamage);
            _gameSession.currentPos.monster.CurrentHP -= damage;
            inFight = true;
            Logging("takeDamage", new string[] {_gameSession.currentPos.monster.Name, Convert.ToString(damage) ,_gameSession.mainStats.Name});
            if (_gameSession.currentPos.monster.CurrentHP <= 0)
            {
                Logging("dead",new string[] {_gameSession.currentPos.monster.Name});
                _gameSession.currentPos.monster.CurrentHP = _gameSession.currentPos.monster.MaxHP;
                XPPlus();
                inFight = false;
            }
            HPBarOfMonster = $"{_gameSession.currentPos.monster.CurrentHP}";

        }
        private void MonsterAttacks()
        {
            if (inFight)
            {
                var damage = new Random().Next(_gameSession.currentPos.monster.AttackMin, _gameSession.currentPos.monster.AttackMax);
                _gameSession.mainStats.CurrentHP -= damage;
                Logging("takeDamage", new string[] { _gameSession.mainStats.Name, Convert.ToString(damage), _gameSession.currentPos.monster.Name });
                if (_gameSession.mainStats.CurrentHP <= 0)
                {
                    Move(0, 0);
                    _gameSession.mainStats.CurrentHP = _gameSession.mainStats.MaxHP;
                    HPbar = $"{_gameSession.mainStats.CurrentHP}/{_gameSession.mainStats.MaxHP}";
                    SetVisibilityForMovement();
                    Logging("dead", new string[] { _gameSession.mainStats.Name });
                    inFight = false;
                    DialogAddAll();
                    return;
                }
                HPbar = $"{_gameSession.mainStats.CurrentHP}/{_gameSession.mainStats.MaxHP}";
            }
        }
        #endregion
        public void XPPlus()
        {
            _gameSession.mainStats.CurrentXP += _gameSession.currentPos.monster.GiveXP;
            Logging("addXP", new string[] { _gameSession.mainStats.Name, Convert.ToString(_gameSession.currentPos.monster.GiveXP)});
            XPbarValue = _gameSession.mainStats.CurrentXP;
            if (_gameSession.mainStats.CurrentXP < _gameSession.mainStats.MaxXP)
                CountOfXPbar = $"{_gameSession.mainStats.CurrentXP}/{_gameSession.mainStats.MaxXP}";
            else
            {
                _gameSession.mainStats.CurrentXP -= _gameSession.mainStats.MaxXP;
                _gameSession.mainStats.MaxXP *= 2;
                XPbarMax = _gameSession.mainStats.MaxXP;
                XPbarValue = _gameSession.mainStats.CurrentXP;
                CountOfXPbar = $"{_gameSession.mainStats.CurrentXP}/{_gameSession.mainStats.MaxXP}";
                _gameSession.mainStats.CurrentLevel++;
                Logging("lvlUp", new string[] { _gameSession.mainStats.Name, Convert.ToString(_gameSession.mainStats.CurrentLevel)});
                _gameSession.mainStats.SkillPoint++;
                _gameSession.mainStats.TempSkillPoint++;
            }
        }

        #region Move Methods
        public void OnWest() //WEST
        {
            if (_gameSession.HasLocationToWest)
            {
                if (!inFight)
                    Move(_gameSession.currentPos.CurrentX - 1, _gameSession.currentPos.CurrentY);
                else
                {
                    if (new Random().Next(0, 100) < 25)
                        Move(_gameSession.currentPos.CurrentX - 1, _gameSession.currentPos.CurrentY);
                    else
                        MonsterAttacks();
                }
            }
        }
        public void OnNorth() //NORTH
        {
            
            if (_gameSession.HasLocationToNorth)
            {
                if (!inFight)
                    Move(_gameSession.currentPos.CurrentX, _gameSession.currentPos.CurrentY + 1);
                else
                {
                    if (new Random().Next(0, 100) < 25)
                        Move(_gameSession.currentPos.CurrentX, _gameSession.currentPos.CurrentY + 1);
                    else
                        MonsterAttacks();
                }
            }
            DialogAdd();
        }
        public void OnEast() //EAST
        {
            if (_gameSession.HasLocationToEast)
            {
                if (!inFight)
                    Move(_gameSession.currentPos.CurrentX + 1, _gameSession.currentPos.CurrentY);
                else
                {
                    if (new Random().Next(0, 100) < 25)
                        Move(_gameSession.currentPos.CurrentX + 1, _gameSession.currentPos.CurrentY);
                    else
                        MonsterAttacks();
                }
            }
        }
        public void OnSouth() //SOUTH
        {
            if (_gameSession.HasLocationToSouth)
            {
                if (!inFight)
                    Move(_gameSession.currentPos.CurrentX, _gameSession.currentPos.CurrentY - 1);
                else
                {
                    if (new Random().Next(0, 100) < 25)
                        Move(_gameSession.currentPos.CurrentX, _gameSession.currentPos.CurrentY - 1);
                    else
                        MonsterAttacks();
                }   
            }
            DialogAdd();
            
        }
        private void Move(int X, int Y)
        {
            _gameSession.currentPos = _gameSession.CurrentWorld.LocationAt(X, Y);
            if (Enemy != _gameSession.currentPos.ImageName.Remove(0, 1))
                Enemy = _gameSession.currentPos.ImageName.Remove(0, 1);
            SetVisibilityForMovement();
            if (_gameSession.currentPos.monster != null)
                ToMonster();
            else
                ToCommonLocation();
            inFight = false;
        }
        private void SetVisibilityForMovement()
        {
            WestVis = _gameSession.HasLocationToWest ? Visibility.Visible : Visibility.Hidden;
            NorthVis = _gameSession.HasLocationToNorth ? Visibility.Visible : Visibility.Hidden;
            EastVis = _gameSession.HasLocationToEast ? Visibility.Visible : Visibility.Hidden;
            SouthVis = _gameSession.HasLocationToSouth ? Visibility.Visible : Visibility.Hidden;
        }
        public void ToMonster()
        {
            NameOfLocation = _gameSession.currentPos.Name;
            HPBarOfMonster = $"{_gameSession.currentPos.monster.CurrentHP}";
            NameOfLocationVerticalAlignment = "Bottom";
            NameOfLocationRowSpan = 1;
        }
        public void ToCommonLocation()
        {
            NameOfLocation = _gameSession.currentPos.Name;
            HPBarOfMonster = "";
            NameOfLocationVerticalAlignment = "Center";
            NameOfLocationRowSpan = 2;
        }
        #endregion

        public void OnTeleportToHome()
        {
           
        }
        public void OnSkipDialog()
        {
            DialogAddAll();
        }
        public void Logging(string type, string[] _desc)
        {
            switch (type)
            {
                case "takeDamage":
                    Log += $"\"{_desc[0]}\" take {_desc[1]} damage from \"{_desc[2]}\"\r\n";
                    break;
                case "dead":
                    Log += $"\"{_desc[0]}\" is deadinside\r\n";
                    break;
                case "giveDamage":
                    Log += $"\"{_desc[0]}\" hit the \"{_desc[1]}\" with {_desc[2]} damage\r\n";
                    break;
                case "addXP":
                    Log += $"\"{_desc[0]}\" got {_desc[1]} experience point\r\n";
                    break;
                case "lvlUp":
                    Log += $"\"{_desc[0]}\" raised the level to {_desc[1]}\r\n";
                    break;
                case "useSkill":
                    Log += $"\"{_desc[0]}\" use \"{_desc[1]}\" on \"{_desc[2]}\"\r\n";
                    break;
            }
        }
        public void OnClearTheLog() => Log = "";
    }
}
