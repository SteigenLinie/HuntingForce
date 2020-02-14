using Engine.ViewModels;
using HuntingForce.Character;
using HuntingForce.Factory;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
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
        private readonly Dictionary<Key, Action> _userInputActions =
            new Dictionary<Key, Action>();
        public MainWindowViewModel()
        {

            Attack = new DelegateCommand(OnAttack, () => true);
            SecondSkillUp = new DelegateCommand(OnSecondSkillUp, () => true);
            Left = new DelegateCommand(OnLeft, () => true);
            Up = new DelegateCommand(OnUp, () => true);
            Right = new DelegateCommand(OnRight, () => true);
            Down = new DelegateCommand(OnDown, () => true);
        }

        public DelegateCommand Attack { get; set; }
        public DelegateCommand SecondSkillUp { get; set; }

        public DelegateCommand Left { get; set; }
        public DelegateCommand Up { get; set; }
        public DelegateCommand Right { get; set; }
        public DelegateCommand Down { get; set; }



        private Visibility _secondSkill = Visibility.Hidden;
        public Visibility SecondSkill
        {
            get => _secondSkill;
            set => SetProperty(ref _secondSkill, value);
        }
        
        private string _hPbar = "100/100";
        public string HPbar
        {
            get => _hPbar;
            set => SetProperty(ref _hPbar, value);
        }

        private string _mPbar = "100/100";
        public string MPbar
        {
            get => _mPbar;
            set => SetProperty(ref _mPbar, value);
        }

        private string _sPbar = "100/100";
        public string SPbar
        {
            get => _sPbar;
            set => SetProperty(ref _sPbar, value);
        }

        private double _xPbarValue = 0;
        public double XPbarValue
        {
            get => _xPbarValue;
            set => SetProperty(ref _xPbarValue, value);
        }

        private double _xPbarMax = 20;
        public double XPbarMax
        {
            get => _xPbarMax;
            set => SetProperty(ref _xPbarMax, value);
        }

        private string _countOfXPbar = "0/20";
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
        private string _enemy = "Resources/Sprites/RatEnemy.png";
        public string Enemy
        {
            get => _enemy;
            set => SetProperty(ref _enemy, value);
        }
        public void OnAttack()
        {
            mainStats.CurrentHP -= 10;
            if (mainStats.CurrentHP <= 0)
            {
                MessageBox.Show("You died");
                return;
            }
            
            HPbar = $"{mainStats.CurrentHP}/{mainStats.MaxHP}";
            XPPlus();
        }
        public void XPPlus()
        {
            Random r = new Random();
            var rn = r.Next(10, 15);
            mainStats.CurrentXP += rn;
            XPbarValue = mainStats.CurrentXP;
            if (mainStats.CurrentXP < mainStats.MaxXP)
                CountOfXPbar = $"{mainStats.CurrentXP}/{mainStats.MaxXP}";
            else
            {
                mainStats.CurrentXP -= mainStats.MaxXP;
                mainStats.MaxXP *= 2;
                XPbarMax = mainStats.MaxXP;
                XPbarValue = mainStats.CurrentXP;
                CountOfXPbar = $"{mainStats.CurrentXP}/{mainStats.MaxXP}";
                mainStats.SkillPoint += 1;
            }
        }
        public void OnSecondSkillUp()
        {
            if (mainStats.SkillPoint > 0)
            {
                SecondSkillPoints = "1/1";
                SecondSkill = Visibility.Visible;
                mainStats.SkillPoint -= 1;
            }
        }

        public void OnLeft()
        {

        }
        public void OnUp()
        {

        }
        public void OnRight()
        {

        }
        public void OnDown()
        {

        }
    }
}
