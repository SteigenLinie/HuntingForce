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

namespace HuntingForce
{
    public class MainWindowViewModel: BindableBase
    {
        private readonly GameSession _gameSession = new GameSession();
        private bool inFight;
        //private readonly Dictionary<Key, Action> _userInputActions =
        //    new Dictionary<Key, Action>();

        public MainWindowViewModel()
        {

            Attack = new DelegateCommand(OnAttack, () => true);
            SecondSkillUp = new DelegateCommand(OnSecondSkillUp, () => true);
            West = new DelegateCommand(OnWest, () => true);
            North = new DelegateCommand(OnNorth, () => true);
            East = new DelegateCommand(OnEast, () => true);
            South = new DelegateCommand(OnSouth, () => true);
            SetVisibilityForMovement();
            HPbar = $"{_gameSession.mainStats.CurrentHP}/{_gameSession.mainStats.MaxHP}";
            MPbar = $"{_gameSession.mainStats.CurrentMP}/{_gameSession.mainStats.MaxMP}";
            SPbar = $"{_gameSession.mainStats.CurrentSP}/{_gameSession.mainStats.MaxSP}";
            XPbarMax = _gameSession.mainStats.MaxXP;
            XPbarValue = _gameSession.mainStats.CurrentXP;
            CountOfXPbar = $"{XPbarValue}/{XPbarMax}";
            Enemy = _gameSession.CurrentWorld.LocationAt(0, 0).ImageName.Remove(0, 1);
        }

        public DelegateCommand Attack { get; set; }
        public DelegateCommand SecondSkillUp { get; set; }

        public DelegateCommand West { get; set; }
        public DelegateCommand North { get; set; }
        public DelegateCommand East { get; set; }
        public DelegateCommand South { get; set; }

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
        private Visibility _secondSkill = Visibility.Hidden;
        public Visibility SecondSkill
        {
            get => _secondSkill;
            set => SetProperty(ref _secondSkill, value);
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
        private string _secondSkillPoints = "0/1";
        public string SecondSkillPoints
        {
            get => _secondSkillPoints;
            set => SetProperty(ref _secondSkillPoints, value);
        }
        private string _enemy;
        public string Enemy
        {
            get => _enemy;
            set => SetProperty(ref _enemy, value);
        }
        public void OnAttack()
        {
            if (_gameSession.currentPos.monster == null)
            {
                MessageBox.Show("В городе нельзя драться!");
                return;
            }
            _gameSession.mainStats.CurrentHP -= new Random().Next(_gameSession.currentPos.monster.AttackMin, _gameSession.currentPos.monster.AttackMax);
            inFight = true;

            if (_gameSession.mainStats.CurrentHP <= 0)
            {
                _gameSession.currentPos = _gameSession.CurrentWorld.LocationAt(0, 0);
                if (Enemy != _gameSession.currentPos.ImageName.Remove(0, 1))
                    Enemy = _gameSession.currentPos.ImageName.Remove(0, 1);
                _gameSession.mainStats.CurrentHP = _gameSession.mainStats.MaxHP;
                HPbar = $"{_gameSession.mainStats.CurrentHP}/{_gameSession.mainStats.MaxHP}";
                SetVisibilityForMovement();
                inFight = false;
                MessageBox.Show("You died");
                return;
            }
            
            HPbar = $"{_gameSession.mainStats.CurrentHP}/{_gameSession.mainStats.MaxHP}";
            XPPlus();
        }
        public void XPPlus()
        {
            _gameSession.mainStats.CurrentXP += _gameSession.currentPos.monster.GiveXP;
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
                _gameSession.mainStats.SkillPoint += 1;
            }
        }
        public void OnSecondSkillUp()
        {
            if (_gameSession.mainStats.SkillPoint > 0)
            {
                SecondSkillPoints = "1/1";
                SecondSkill = Visibility.Visible;
                _gameSession.mainStats.SkillPoint -= 1;
            }
        }

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
                        OnAttack();
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
                        OnAttack();
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
                        OnAttack();
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
                        OnAttack();
                }   
            }
        }
        private void Move(int X, int Y)
        {
            _gameSession.currentPos = _gameSession.CurrentWorld.LocationAt(X, Y);
            if (Enemy != _gameSession.currentPos.ImageName.Remove(0, 1))
                Enemy = _gameSession.currentPos.ImageName.Remove(0, 1);
            SetVisibilityForMovement();
            inFight = false;
        }
        private void SetVisibilityForMovement()
        {
            WestVis = _gameSession.HasLocationToWest ? Visibility.Visible : Visibility.Hidden;
            NorthVis = _gameSession.HasLocationToNorth ? Visibility.Visible : Visibility.Hidden;
            EastVis = _gameSession.HasLocationToEast ? Visibility.Visible : Visibility.Hidden;
            SouthVis = _gameSession.HasLocationToSouth ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
