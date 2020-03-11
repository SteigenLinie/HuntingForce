using EngineHF;
using EngineHF.Model;
using HuntingForce.DialogWindows;
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
        private bool _animationIsPlaying;
        public readonly GameSession _gameSession;
        readonly MainWindow _mainWindow;
        private bool inFight;
        private List<Logging> _logs = new List<Logging>();
        public List<string> _currentTypesOfLogs = new List<string>()
        {
            "takeDamage",
            "dead",
            "giveDamage",
            "addXP",
            "lvlUp",
            "useSkill"
        };
        string[] str = new string[22];
        public MainWindowViewModel(GameSession gameSession, MainWindow mainWindow)
        {
            _gameSession = gameSession;
            _mainWindow = mainWindow;
            Attack = new DelegateCommand(OnAttack, () => true);
            West = new DelegateCommand(OnWest, () => true);
            North = new DelegateCommand(OnNorth, () => true);
            East = new DelegateCommand(OnEast, () => true);
            South = new DelegateCommand(OnSouth, () => true);
            TeleportToHome = new DelegateCommand(OnTeleportToHome, () => true);
            SkipDialog = new DelegateCommand(OnSkipDialog, () => true);
            ClearTheLog = new DelegateCommand(OnClearTheLog, () => true);
            TypeOfLogsMenu = new DelegateCommand(OnTypeOfLogsMenu, () => true);

            SetVisibilityForMovement();
            HPbar = $"{_gameSession.mainStats.CurrentHP}/{_gameSession.mainStats.MaxHP}";
            MPbar = $"{_gameSession.mainStats.CurrentMP}/{_gameSession.mainStats.MaxMP}";
            SPbar = $"{_gameSession.mainStats.CurrentSP}/{_gameSession.mainStats.MaxSP}";
            NickName = _gameSession.mainStats.Name;
            XPbarMax = _gameSession.mainStats.MaxXP;
            XPbarValue = _gameSession.mainStats.CurrentXP;
            CountOfXPbar = $"{XPbarValue}/{XPbarMax}";

            CurrentAttack = $"{_gameSession.mainStats.CurrentWeapon.Weapon.MinDamage}-{_gameSession.mainStats.CurrentWeapon.Weapon.MaxDamage}";
            CurrentArmor = Convert.ToString(Convert.ToInt32(CurrentArmor) + _gameSession.mainStats.CurrentArmor.Armor.PlusArmor);
            CurrentLvl = $"{_gameSession.mainStats.CurrentLevel}";

            Enemy = _gameSession.CurrentWorld.LocationAt(0, 0).ImageName.Remove(0, 1);
            ToCommonLocation();
            DialogAdd(_gameSession.currentPos.Description);
            NameOfLocation = _gameSession.currentPos.Name;
            for (int i = 1; i <= 22; i++)
            {
                if (i < 10)
                    str[i - 1] = $"Resources/Sprites/Enemy/Wolf/darksaber_attack000{i}.png";
                else
                    str[i - 1] = $"Resources/Sprites/Enemy/Wolf/darksaber_attack00{i}.png";
            }
        }
        public async void DialogAdd(string text)
        {
            MoveButton = false;
            Dialog = "";
            await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
            {
                foreach (var _char in text)
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
        public async Task PlayAnimation(string[] arrOfPng)
        {
            //await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
            //{
            while (_animationIsPlaying)
                await Task.Delay(100);
            
            if (!_animationIsPlaying)
                {
                    _animationIsPlaying = true;
                    foreach (var Png in arrOfPng)
                    {
                        MainPlayerPng = Png;
                        await Task.Delay(50);
                    }
                    _animationIsPlaying = false;
                }
            //}
            //));
        }
        public async Task PlayAnimationForEnemy(string[] arrOfPng)
        {
            //await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
            //{
                while (_animationIsPlaying)
                    await Task.Delay(100);

                if (!_animationIsPlaying)
                {
                    _animationIsPlaying = true;
                    foreach (var Png in arrOfPng)
                    {
                        Enemy = Png;
                        await Task.Delay(50);
                    }
                    _animationIsPlaying = false;
                }
            //}
            //));
        }
        public void DialogAddAll(string text)
        {
            if (!inFight)
            {
                MoveButton = true;
                Dialog = text;
            }
        }
        public DelegateCommand ClearTheLog { get; set; }
        public DelegateCommand TypeOfLogsMenu { get; set; }

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

        #region - Stats -
        private string _currentGoldTextBlock = "0";
        public string CurrentGoldTextBlock
        {
            get => _currentGoldTextBlock;
            set => SetProperty(ref _currentGoldTextBlock, value);
        }
        private string _currentAttack;
        public string CurrentAttack
        {
            get => _currentAttack;
            set
            { 
                SetProperty(ref _currentAttack, value);

                _mainWindow.popup1.IsOpen = true;

            }
        }
        private string _currentArmor = "2";
        public string CurrentArmor
        {
            get => _currentArmor;
            set => SetProperty(ref _currentArmor, value);
        }
        private string _currentLvl;
        public string CurrentLvl
        {
            get => _currentLvl;
            set => SetProperty(ref _currentLvl, value);
        }
        #endregion
        private string _mainPlayerPng = $"Resources/Sprites/Player/attack (1).png";
        public string MainPlayerPng
        {
            get => _mainPlayerPng;
            set => SetProperty(ref _mainPlayerPng, value);
        }
        #region Attack Methods
        public void OnAttack()
        {
            if(MoveButton && !_animationIsPlaying)
                Attacking(_gameSession.mainStats.CurrentWeapon.Weapon.MinDamage, _gameSession.mainStats.CurrentWeapon.Weapon.MaxDamage);
        }
        public async void Attacking(int minDamage, int maxDamage, int countOfSlash = 1)
        {
            if (_gameSession.currentPos.Monster == null)
            {
                MessageBox.Show("В городе нельзя драться!");
                return;
            }
            for (int i = 0; i < countOfSlash; i++)
                await HeroAttacks(minDamage, maxDamage);
            MonsterAttacks();
        }
        private async Task HeroAttacks(int minDamage, int maxDamage)
        {
            string[] arrOfPng = new string[10];
            for (int i = 1; i <= 10; i++)
                arrOfPng[i-1] = $"Resources/Sprites/Player/attack ({i}).png";
            await PlayAnimation(arrOfPng);
            var damage = new Random().Next(minDamage, maxDamage);
            _gameSession.currentPos.Monster.CurrentHP -= damage;
            inFight = true;
            Logging("takeDamage", new string[] {_gameSession.currentPos.Monster.Name, Convert.ToString(damage) ,_gameSession.mainStats.Name});

            if (_gameSession.currentPos.Monster.CurrentHP <= 0)
                MonsterDead();
            HPBarOfMonster = $"{_gameSession.currentPos.Monster.CurrentHP}";

        }
        private void MonsterDead()
        {
            Logging("dead", new string[] { _gameSession.currentPos.Monster.Name });
            foreach (Drop drop in _gameSession.currentPos.Monster.DropList)
                if (new Random().Next(0, 100) <= drop.Chance)
                    _mainWindow.AddNewItemInInventory(drop);

            var giveGold = _gameSession.currentPos.Monster.Gold + 5 * (_gameSession.currentPos.Monster.Level - _gameSession.mainStats.CurrentLevel);
            if (giveGold > 0)
                CurrentGoldTextBlock = Convert.ToString(giveGold + Convert.ToInt32(CurrentGoldTextBlock));

            _gameSession.currentPos.Monster.CurrentHP = _gameSession.currentPos.Monster.MaxHP;

            XPPlus();

            foreach (var quest in _gameSession.mainStats.QuestOnPlayer)
                if (_gameSession.currentPos.Monster.QuestProgress.Contains(quest.ID) && quest.Progress != "done")
                    quest.Progress = $"{Convert.ToInt32(quest.Progress.Split('/')[0]) + 1}/{quest.Progress.Split('/')[1]}";

            inFight = false;
        }
        private async void MonsterAttacks()
        {
            if (inFight)
            {
                await PlayAnimationForEnemy(str);

                var damage = new Random().Next(_gameSession.currentPos.Monster.AttackMin, _gameSession.currentPos.Monster.AttackMax);
                var a = (0.06 * _gameSession.mainStats.CurrentArmor.Armor.PlusArmor) / (1 + 0.06 * _gameSession.mainStats.CurrentArmor.Armor.PlusArmor);
                if (a == 0) a = 1;
                _gameSession.mainStats.CurrentHP -= Convert.ToInt32(damage * a);
                Logging("takeDamage", new string[] { _gameSession.mainStats.Name, Convert.ToString(damage), _gameSession.currentPos.Monster.Name });

                DialogAddAll($"\"{_gameSession.mainStats.Name}\" take {Convert.ToString(damage)} damage from \"{_gameSession.currentPos.Monster.Name}\"");

                if (_gameSession.mainStats.CurrentHP <= 0)
                {
                    PlayerDead();
                    return;
                }
                HPbar = $"{_gameSession.mainStats.CurrentHP}/{_gameSession.mainStats.MaxHP}";
            }
        }
        private void PlayerDead()
        {
            Move(0, 0);

            _gameSession.mainStats.CurrentHP = _gameSession.mainStats.MaxHP;
            HPbar = $"{_gameSession.mainStats.CurrentHP}/{_gameSession.mainStats.MaxHP}";

            SetVisibilityForMovement();
            Logging("dead", new string[] { _gameSession.mainStats.Name });
            inFight = false;

            DialogAddAll(_gameSession.currentPos.Description);
        }
        #endregion
        public void XPPlus()
        {
            var plusXP = 25 * (10 + _gameSession.currentPos.Monster.Level - _gameSession.mainStats.CurrentLevel) / (10 + _gameSession.mainStats.CurrentLevel);
            _gameSession.mainStats.CurrentXP += plusXP;
            Logging("addXP", new string[] { _gameSession.mainStats.Name, Convert.ToString(plusXP) });
            XPbarValue = _gameSession.mainStats.CurrentXP;
            if (_gameSession.mainStats.CurrentXP < _gameSession.mainStats.MaxXP)
                CountOfXPbar = $"{_gameSession.mainStats.CurrentXP}/{_gameSession.mainStats.MaxXP}";
            else
            {
                _gameSession.mainStats.CurrentXP -= _gameSession.mainStats.MaxXP;
                _gameSession.mainStats.MaxXP = Convert.ToInt32(50 * Math.Pow(1.337, _gameSession.mainStats.CurrentLevel + 2));
                XPbarMax = _gameSession.mainStats.MaxXP;
                XPbarValue = _gameSession.mainStats.CurrentXP;
                CountOfXPbar = $"{_gameSession.mainStats.CurrentXP}/{_gameSession.mainStats.MaxXP}";
                _gameSession.mainStats.CurrentLevel++;
                CurrentLvl = $"{_gameSession.mainStats.CurrentLevel}";
                _gameSession.mainStats.SkillPoint++;
                _gameSession.mainStats.TempSkillPoint++;
                Logging("lvlUp", new string[] { _gameSession.mainStats.Name, Convert.ToString(_gameSession.mainStats.CurrentLevel)});
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
        }
        private void Move(int X, int Y)
        {
            _gameSession.currentPos = _gameSession.CurrentWorld.LocationAt(X, Y);
                
            SetVisibilityForMovement();
            if (_gameSession.currentPos.Monster != null)
                ToMonster();
            else
                ToCommonLocation();
            DialogAdd(_gameSession.currentPos.Description);
            inFight = false;
            _mainWindow.AddNewBorderForMap(_gameSession.currentPos);
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
            Enemy = _gameSession.currentPos.Monster.ImageName.Remove(0, 1);
            NameOfLocation = _gameSession.currentPos.Monster.Name;
            HPBarOfMonster = $"{_gameSession.currentPos.Monster.CurrentHP}";
            NameOfLocationVerticalAlignment = "Bottom";
            NameOfLocationRowSpan = 1;
        }
        public void ToCommonLocation()
        {
            if (_gameSession.currentPos.Quest != null && !_gameSession.currentPos.Quest.IsDone)
                _mainWindow.AddNewQuest(_gameSession.currentPos.Quest);
            Enemy = _gameSession.currentPos.ImageName.Remove(0, 1);
            NameOfLocation = _gameSession.currentPos.Name;
            HPBarOfMonster = "";
            NameOfLocationVerticalAlignment = "Center";
            NameOfLocationRowSpan = 2;
        }
        #endregion

        public void OnTeleportToHome()
        {
            Move(0, 0);
        }
        public void OnSkipDialog()
        {
            DialogAddAll(_gameSession.currentPos.Description);
        }
        public void Logging(string type, string[] _desc)
        {
            switch (type)
            {
                case "takeDamage":
                    if(_currentTypesOfLogs.Contains(type))
                        Log += $"\"{_desc[0]}\" take {_desc[1]} damage from \"{_desc[2]}\"\r\n";
                    _logs.Add(new Logging("takeDamage", $"\"{_desc[0]}\" take {_desc[1]} damage from \"{_desc[2]}\"\r\n"));
                    break;
                case "dead":
                    if (_currentTypesOfLogs.Contains(type))
                        Log += $"\"{_desc[0]}\" is deadinside\r\n";
                    _logs.Add(new Logging("dead", $"\"{_desc[0]}\" is deadinside\r\n"));
                    break;
                case "giveDamage":
                    if (_currentTypesOfLogs.Contains(type))
                        Log += $"\"{_desc[0]}\" hit the \"{_desc[1]}\" with {_desc[2]} damage\r\n";
                    _logs.Add(new Logging("giveDamage", $"\"{_desc[0]}\" hit the \"{_desc[1]}\" with {_desc[2]} damage\r\n"));
                    break;
                case "addXP":
                    if (_currentTypesOfLogs.Contains(type))
                        Log += $"\"{_desc[0]}\" got {_desc[1]} experience point\r\n";
                    _logs.Add(new Logging("addXP", $"\"{_desc[0]}\" got {_desc[1]} experience point\r\n"));
                    break;
                case "lvlUp":
                    if (_currentTypesOfLogs.Contains(type))
                        Log += $"\"{_desc[0]}\" raised the level to {_desc[1]}\r\n";
                    _logs.Add(new Logging("lvlUp", $"\"{_desc[0]}\" raised the level to {_desc[1]}\r\n"));
                    break;
                case "useSkill":
                    if (_currentTypesOfLogs.Contains(type))
                        Log += $"\"{_desc[0]}\" use \"{_desc[1]}\" on \"{_desc[2]}\"\r\n";
                    _logs.Add(new Logging("useSkill", $"\"{_desc[0]}\" use \"{_desc[1]}\" on \"{_desc[2]}\"\r\n"));
                    break;
            }
        }
        public void OnClearTheLog() => Log = "";
        public void OnTypeOfLogsMenu()
        {
            TypeOfLoggingSelectionWindow typeOfLoggingSelectionWindow = new TypeOfLoggingSelectionWindow(_currentTypesOfLogs);
            typeOfLoggingSelectionWindow.ShowDialog();
            _currentTypesOfLogs.Clear();

            foreach (var elm in typeOfLoggingSelectionWindow.Result)
                _currentTypesOfLogs.Add(elm);

            Log = "";
            foreach(var elm in _logs.Where(x => _currentTypesOfLogs.Contains(x.Type)))
                Log += elm.Message;
        }
    }
}
