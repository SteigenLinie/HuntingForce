using EngineHF;
using EngineHF.Model;
using EngineHF.Model.Inventory;
using HuntingForce.DialogWindows;
using HuntingForce.DialogWindows.Views;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HuntingForce
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MediaPlayer player = new MediaPlayer();



        #region Inventory fields
        private int? _indexOfLastSelectedBorder = null;
        private int indnt = 0;
        private readonly Dictionary<int, int> dict = new Dictionary<int, int>();
        private bool _isSelectedInInventory;
        private bool _isSelectedInAmmo;
        private List<InfoForItemInInventory> _inventoryItems = new List<InfoForItemInInventory>();
        private Border _lastBorder;
        private Border _lastBorderInAmmo;
        #endregion

        #region Skill fields
        Skills _lastSkill;
        List<Border> ListOfSkillBorder = new List<Border>();
        List<Border> SelectedSkillList = new List<Border>();
        List<Grid> GridsRectangle = new List<Grid>();
        List<Border> CanUpgrade = new List<Border>();
        #endregion
        public static readonly GameSession _gameSession = new GameSession();

        private Border _lastMapBorder;
        public Dictionary<Location, Border> _map = new Dictionary<Location, Border>();

        MainWindowViewModel _mainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
            AddNewBorderForMap(_gameSession.currentPos);
            FillingSkills();
            AddingInventory();
            DataContext = new MainWindowViewModel(_gameSession, this);
            AddFirstWeapon();
            
        }
        #region - Inventory -
        #region Constructors
        private Border GenerateInventoryItem(string name, string imageName, string text, int gridRow, int gridColumn, GameItem gameItem = null)
        {
            Border border = new Border()
            {
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(1)
            };

            Grid grid = new Grid();

            Image image = new Image()
            {
                Name = name,
                Margin = new Thickness(3),
                Source = new BitmapImage(new Uri(imageName.Remove(0,1), UriKind.Relative))
            };
            image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            if (gameItem != null)
            {
                if (gameItem.Category.ToString() == "Miscellaneous")
                {
                    image.ToolTip = new ToolTip
                    {
                        Content = GenerateToolTipMiscellaneous(gameItem.Name, "Miscellaneous", gameItem.ImageName, gameItem.Descrtiption),
                        Background = Brushes.Transparent
                    };
                }
                else
                {
                    image.ToolTip = new ToolTip
                    {
                        Content = GenerateToolTip(gameItem),
                        Background = Brushes.Transparent
                    };
                    image.ToolTipOpening += Image_ToolTipOpening;
                }
            }
            TextBlock textBlock = new TextBlock()
            {
                Text = text,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            grid.Children.Add(image);
            grid.Children.Add(textBlock);

            border.Child = grid;
            Grid.SetRow(border, gridRow);
            Grid.SetColumn(border, gridColumn);
            return border;
        }

        private void Image_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            var image = (Image)sender;
            var newItem = _gameSession._standardGameItems.First(x => x.Name == image.Name);

            var toolTip = (ToolTip)image.ToolTip;
            var grid = (Grid)toolTip.Content;
            var border = (Border)grid.Children[0];
            var oneMoreGrid = (Grid)border.Child;
            var andOneMoreGrid = (Grid)oneMoreGrid.Children[5];
            var bonusHealth = (TextBlock)andOneMoreGrid.Children[3];
            var bonusDamage = (TextBlock)andOneMoreGrid.Children[5];
            var bonusArmor = (TextBlock)andOneMoreGrid.Children[7];
            var category = newItem.Category.ToString();
            switch(category)
            {
                case "Weapon":
                    var averageDamageCurrWeapon = (_gameSession.mainStats.CurrentWeapon.Weapon.MinDamage + _gameSession.mainStats.CurrentWeapon.Weapon.MaxDamage) / 2.0;
                    var averageDamageNewWeapon = (newItem.Weapon.MinDamage + newItem.Weapon.MaxDamage) / 2.0;
                    var blablabla = (averageDamageNewWeapon - averageDamageCurrWeapon) / averageDamageCurrWeapon;
                    var bonusDamegeInProc = Math.Round(blablabla, 2) * 100;

                    if (bonusDamegeInProc > 0)
                    {
                        bonusDamage.Opacity = 1;
                        bonusDamage.Text = "+" + bonusDamegeInProc + "%";
                        bonusDamage.Foreground = new SolidColorBrush(Color.FromRgb(76, 187, 23));

                    }
                    else if (bonusDamegeInProc < 0)
                    {
                        bonusDamage.Opacity = 1;
                        bonusDamage.Text = bonusDamegeInProc + "%";
                        bonusDamage.Foreground = new SolidColorBrush(Color.FromRgb(248, 0, 0));
                    }
                    else
                    {
                        bonusDamage.Opacity = 0.8;
                        bonusDamage.Text = "+" + bonusDamegeInProc + "%";
                        bonusDamage.Foreground = Brushes.White;
                    }
                    break;
                case "Armor":
                    double armor = newItem.Armor.PlusArmor - _gameSession.mainStats.CurrentArmor.Armor.PlusArmor;
                    if (_gameSession.mainStats.CurrentArmor.Armor.PlusArmor != 0)
                        armor /= _gameSession.mainStats.CurrentArmor.Armor.PlusArmor;
                    var bonusArmorIn = Math.Round(armor, 2) * 100;

                    if (bonusArmorIn > 0)
                    {
                        bonusArmor.Opacity = 1;
                        bonusArmor.Text = "+" + bonusArmorIn + "%";
                        bonusArmor.Foreground = new SolidColorBrush(Color.FromRgb(76, 187, 23));

                    }
                    else if (bonusArmorIn < 0)
                    {
                        bonusArmor.Opacity = 1;
                        bonusArmor.Text = bonusArmorIn + "%";
                        bonusArmor.Foreground = new SolidColorBrush(Color.FromRgb(248, 0, 0));
                    }
                    else
                    {
                        bonusArmor.Opacity = 0.8;
                        bonusArmor.Text = "+" + bonusArmorIn + "%";
                        bonusArmor.Foreground = Brushes.White;
                    }
                    break;
            }
        }

        private Grid GenerateToolTip(GameItem gameItem)
        {
            var mainborder = new Border
            {
                Background = Brushes.Black,
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(1)
            };

            var grid2 = new Grid
            {
                Margin = new Thickness(10)
            };

            var row0 = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            var row1 = new RowDefinition { Height = new GridLength(4, GridUnitType.Star) };
            var row2 = new RowDefinition { Height = new GridLength(2.5, GridUnitType.Star) };
            var row3 = new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) };

            var col0 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            var col1 = new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) };

            grid2.RowDefinitions.Add(row0);
            grid2.RowDefinitions.Add(row1);
            grid2.RowDefinitions.Add(row2);
            grid2.RowDefinitions.Add(row3);

            grid2.ColumnDefinitions.Add(col0);
            grid2.ColumnDefinitions.Add(col1);

            var name = new TextBlock
            {
                Text = gameItem.Name,
                Foreground = Brushes.White,
                FontSize = 30,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2")
            };
            Grid.SetColumnSpan(name, 2);

            var border = new Border
            {
                Background = Brushes.White,
                Height = 2,
                VerticalAlignment = VerticalAlignment.Bottom,
            };

            Grid.SetColumnSpan(border, 2);

            var borderForImage = new Border
            {
                Background = Brushes.Black,
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 10, 5, 0)
            };
            Grid.SetRow(borderForImage, 1);

            var Image = new Image
            {
                Source = new BitmapImage(new Uri(gameItem.ImageName.Remove(0, 1), UriKind.Relative)),
                Margin = new Thickness(0, 10, 0, 0)
            };
            borderForImage.Child = Image;

            var gridHar = new Grid
            {
                Margin = new Thickness(5, 0, 0, 0)
            };
            Grid.SetRow(gridHar, 1);
            Grid.SetColumn(gridHar, 1);

            var rowHar0 = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            var rowHar1 = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            var rowHar2 = new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) };
            var rowHar3 = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            
            gridHar.RowDefinitions.Add(rowHar0);
            gridHar.RowDefinitions.Add(rowHar1);
            gridHar.RowDefinitions.Add(rowHar2);
            gridHar.RowDefinitions.Add(rowHar3);




            var myUnderline = new TextDecoration
            {

                // Create a solid color brush pen for the text decoration.
                Pen = new Pen(Brushes.White, 1),
                PenThicknessUnit = TextDecorationUnit.FontRecommended
            };

            // Set the underline decoration to a TextDecorationCollection and add it to the text block.
            var myCollection = new TextDecorationCollection
            {
                myUnderline
            };




            var type = new TextBlock
            {
                Text = gameItem.Category.ToString(),
                Foreground = Brushes.White,
                FontSize = 30,
                Margin = new Thickness(0, 10, 0, 0),
                FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2"),
                TextDecorations = myCollection
            };
            Grid.SetRow(type, 0);
            gridHar.Children.Add(type);
            var averageDamage = new TextBlock
            {
                Foreground = Brushes.White,
                FontSize = 50,
                FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2")
            };
            Grid.SetRow(averageDamage, 1);


            var bonusHealth = new TextBlock
            {
                Text = "+0",
                Foreground = Brushes.White,
                Opacity = 0.8,
                FontSize = 15,
                FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2"),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            Grid.SetColumn(bonusHealth, 1);
            var bonusDamage = new TextBlock
            {
                Text = "+0",
                Foreground = Brushes.White,
                Opacity = 0.8,
                FontSize = 15,
                FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2"),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(bonusDamage, 1);
            Grid.SetColumn(bonusDamage, 1);

            var bonusArmor = new TextBlock
            {
                Text = "+0",
                Foreground = Brushes.White,
                Opacity = 0.8,
                FontSize = 15,
                FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2"),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetRow(bonusArmor, 2);
            Grid.SetColumn(bonusArmor, 1);

            var bonusHealthInfo = new TextBlock
            {
                Text = "к здоровью",
                Foreground = Brushes.White,
                FontSize = 15,
                FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2"),
                Margin = new Thickness(5, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            Grid.SetColumn(bonusHealthInfo, 2);

            var bonusDamageInfo = new TextBlock
            {
                Text = "к урону",
                Foreground = Brushes.White,
                FontSize = 15,
                FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2"),
                Margin = new Thickness(5, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(bonusDamageInfo, 1);
            Grid.SetColumn(bonusDamageInfo, 2);

            var bonusArmorInfo = new TextBlock
            {
                Text = "к защите",
                Foreground = Brushes.White,
                FontSize = 15,
                FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2"),
                Margin = new Thickness(5, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetRow(bonusArmorInfo, 2);
            Grid.SetColumn(bonusArmorInfo, 2);

            if (gameItem.Weapon != null)
            {
                var averageDamageNewWeapon = (gameItem.Weapon.MinDamage + gameItem.Weapon.MaxDamage) / 2.0;
                averageDamage.Text = String.Format("{0:0.0}", averageDamageNewWeapon);
                var text = new TextBlock
                {
                    Text = "ед. урона в сек",
                    Foreground = Brushes.Gray,
                    FontSize = 15,
                    FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2")
                };
                Grid.SetRow(text, 2);

                gridHar.Children.Add(averageDamage);
                gridHar.Children.Add(text);

                var damage = new TextBlock
                {
                    Text = gameItem.Weapon.MinDamage + "-" + gameItem.Weapon.MaxDamage,
                    Foreground = Brushes.White,
                    FontSize = 25,
                    FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2")
                };
                Grid.SetRow(damage, 3);
                gridHar.Children.Add(damage);
               
            }
            else if (gameItem.Armor != null)
            {
                averageDamage.Text = $"{gameItem.Armor.PlusArmor}";
                gridHar.Children.Add(averageDamage);
            }


            var border2 = new Border
            {
                Background = Brushes.White,
                Height = 2,
                VerticalAlignment = VerticalAlignment.Bottom,
            };
            Grid.SetRow(border2, 2);
            Grid.SetColumnSpan(border2, 2);

            var gridBonus = new Grid();
            Grid.SetRow(gridBonus, 2);
            Grid.SetColumnSpan(gridBonus, 2);

            var rowBonus0 = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            var rowBonus1 = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            var rowBonus2 = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };

            var colBonus0 = new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) };
            var colBonus1 = new ColumnDefinition { Width = new GridLength(0.6, GridUnitType.Star) };
            var colBonus2 = new ColumnDefinition { Width = new GridLength(1.4, GridUnitType.Star) };

            gridBonus.RowDefinitions.Add(rowBonus0);
            gridBonus.RowDefinitions.Add(rowBonus1);
            gridBonus.RowDefinitions.Add(rowBonus2);

            gridBonus.ColumnDefinitions.Add(colBonus0);
            gridBonus.ColumnDefinitions.Add(colBonus1);
            gridBonus.ColumnDefinitions.Add(colBonus2);

            var info1 = new TextBlock
            {
                Text = "Если вы экипируетесь",
                Foreground = Brushes.White,
                Opacity = 0.8,
                FontSize = 12,
                FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            var info2 = new TextBlock
            {
                Text = "этим предметом, то",
                Foreground = Brushes.White,
                Opacity = 0.8,
                FontSize = 12,
                FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(info2, 1);
            var info3 = new TextBlock
            {
                Text = "получите:",
                Foreground = Brushes.White,
                Opacity = 0.8,
                FontSize = 12,
                FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetRow(info3, 2);


            gridBonus.Children.Add(info1);
            gridBonus.Children.Add(info2);
            gridBonus.Children.Add(info3);
            gridBonus.Children.Add(bonusHealth);
            gridBonus.Children.Add(bonusHealthInfo);
            gridBonus.Children.Add(bonusDamage);
            gridBonus.Children.Add(bonusDamageInfo);
            gridBonus.Children.Add(bonusArmor);
            gridBonus.Children.Add(bonusArmorInfo);


            grid2.Children.Add(name);
            grid2.Children.Add(border);
            grid2.Children.Add(borderForImage);
            grid2.Children.Add(gridHar);
            grid2.Children.Add(border2);
            grid2.Children.Add(gridBonus);

            mainborder.Child = grid2;

            var grid1 = new Grid();
            grid1.Width = 250;
            grid1.Height = 300;
            grid1.Children.Add(mainborder);

            return grid1;
        }

        private Grid GenerateToolTipMiscellaneous(string _name, string type, string imageName, string description)
        {
            var mainborder = new Border
            {
                Background = Brushes.Black,
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(1)
            };

            var grid2 = new Grid
            {
                Margin = new Thickness(10)
            };

            var row0 = new RowDefinition { Height = new GridLength(1.5, GridUnitType.Star) };
            var row1 = new RowDefinition { Height = new GridLength(5, GridUnitType.Star) };
            var row2 = new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) };

            var col0 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            var col1 = new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) };

            grid2.RowDefinitions.Add(row0);
            grid2.RowDefinitions.Add(row1);
            grid2.RowDefinitions.Add(row2);

            grid2.ColumnDefinitions.Add(col0);
            grid2.ColumnDefinitions.Add(col1);

            var name = new TextBlock
            {
                Text = _name,
                Foreground = Brushes.White,
                FontSize = 30,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2")
            };
            Grid.SetColumnSpan(name, 2);

            var border = new Border
            {
                Background = Brushes.White,
                Height = 2,
                VerticalAlignment = VerticalAlignment.Bottom,
            };

            Grid.SetColumnSpan(border, 2);

            var borderForImage = new Border
            {
                Background = Brushes.Black,
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 10, 5, 10)
            };
            Grid.SetRow(borderForImage, 1);

            var Image = new Image
            {
                Source = new BitmapImage(new Uri(imageName.Remove(0, 1), UriKind.Relative)),
                Margin = new Thickness(0, 5, 0, 0)
            };
            borderForImage.Child = Image;

            var gridHar = new Grid
            {
                Margin = new Thickness(5, 0, 0, 0)
            };
            Grid.SetRow(gridHar, 1);
            Grid.SetColumn(gridHar, 1);

            var rowHar0 = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            var rowHar1 = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            var rowHar2 = new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) };
            var rowHar3 = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };

            gridHar.RowDefinitions.Add(rowHar0);
            gridHar.RowDefinitions.Add(rowHar1);
            gridHar.RowDefinitions.Add(rowHar2);
            gridHar.RowDefinitions.Add(rowHar3);


            var border2 = new Border
            {
                Background = Brushes.White,
                Height = 2,
                VerticalAlignment = VerticalAlignment.Bottom,
            };
            Grid.SetRow(border2, 1);
            Grid.SetColumnSpan(border2, 2);
            TextBlock text;
            switch(type)
            {
                case "Miscellaneous":
                    text = new TextBlock
                    {
                        Text = description,
                        Foreground = Brushes.White,
                        FontSize = 15,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        TextWrapping = TextWrapping.Wrap,
                        FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2"),
                    };
                    Grid.SetRowSpan(text, 4);
                    gridHar.Children.Add(text);
                    break;
                case "Attack":
                    text = new TextBlock
                    {
                        Text = description,
                        Foreground = Brushes.White,
                        FontSize = 15,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        TextWrapping = TextWrapping.Wrap,
                        FontFamily = new FontFamily(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts"), "#Determination2"),
                    };
                    Grid.SetRowSpan(text, 4);
                    gridHar.Children.Add(text);
                    break;
                case "Passive":
                    break;
            }
            

            grid2.Children.Add(name);
            grid2.Children.Add(border);
            grid2.Children.Add(borderForImage);
            grid2.Children.Add(gridHar);
            grid2.Children.Add(border2);

            mainborder.Child = grid2;

            var grid1 = new Grid();
            grid1.Width = 250;
            grid1.Height = 175;
            grid1.Children.Add(mainborder);

            return grid1;
        }
        #endregion

        #region MouseDownMethods
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = (Image)sender;
            var grid = (Grid)image.Parent;
            var border = (Border)grid.Parent;
            if(border == _lastBorder)
            {
                _lastBorder = null;
                _isSelectedInInventory = false;
                border.BorderThickness = new Thickness(1);
                return;
            }
            if (_isSelectedInInventory)
                SwapItems(image);
            else if (_isSelectedInAmmo)
            {
                var grid2 = (Grid)_lastBorderInAmmo.Child;
                var image2 = (Image)grid2.Children[0];
                if (image.Name != null && _gameSession._standardGameItems.First(x => x.Name == image.Name).Category.ToString() != _gameSession._standardGameItems.First(x => x.Name == image2.Name).Category.ToString())
                { }
                else
                    SwapItemsAmmoToInventory(image);
            }
            else if (image.Name != null)
                OnItemInInventorySelecting(image);

        }
        private void Weapon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var imagee = (Image)sender;
            if (!_isSelectedInAmmo)
            {
                if (_lastBorder != null)
                {
                    var grid = (Grid)_lastBorder.Child;
                    var image = (Image)grid.Children[0];

                    if (_isSelectedInInventory && _inventoryItems.First(x => x.Name == image.Name).Category == "Weapon")
                        SwapItemsInventoryToAmmo(imagee);
                }
                else if (imagee.Name != null)
                {
                    var grid = (Grid)imagee.Parent;
                    var border = (Border)grid.Parent;
                    _lastBorderInAmmo = border;
                    OnItemInAmmoSelecting(imagee);
                }
            }
        }
        private void Armor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var imagee = (Image)sender;
            if (!_isSelectedInAmmo)
            {
                if (_lastBorder != null)
                {
                    var grid = (Grid)_lastBorder.Child;
                    var image = (Image)grid.Children[0];
                    if (_isSelectedInInventory && _inventoryItems.First(x => x.Name == image.Name).Category == "Armor")
                        SwapItemsInventoryToAmmo((Image)sender);
                }
                else if (imagee.Name != null)
                {
                    var grid = (Grid)imagee.Parent;
                    var border = (Border)grid.Parent;
                    _lastBorderInAmmo = border;
                    OnItemInAmmoSelecting((Image)sender);
                }
            }
        }
        #endregion

        #region AddingMethods
        private void AddingInventory()
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 9; j++)
                    _inventoryItems.Add(new InfoForItemInInventory(null, null, 1, i*9+j+1,
                            new ItemInInventory(GenerateInventoryItem(null, @".\Resources\Black.png", null, i, j), i, j)));

            foreach (var item in _inventoryItems.Select(x => x.ItemInInventory))
                Inventory.Children.Add(item.Item);
        }
        private void AddFirstWeapon()
        {
            var firstWeapon = _gameSession.mainStats.CurrentWeapon;
            var Weaponborder = (Border)Weapon.Children[0];
            var Weapongrid = (Grid)Weaponborder.Child;
            var Weaponimage = (Image)Weapongrid.Children[0];
           
            Weaponimage.ToolTip = new ToolTip
            {
                Background = Brushes.Transparent
            };
            if (firstWeapon.Name != "")
                Weaponimage.Name = firstWeapon.Name;
            else
                Weaponimage.Name = null;
            Weaponimage.Source = new BitmapImage(new Uri(firstWeapon.ImageName.Remove(0, 1), UriKind.Relative));


            var firstArmor = _gameSession.mainStats.CurrentArmor;
            var Armorborder = (Border)Armor.Children[0];
            var Armorgrid = (Grid)Armorborder.Child;
            var Armorimage = (Image)Armorgrid.Children[0];
            if (firstArmor.Name != "")
                Armorimage.Name = firstArmor.Name;
            else
                Armorimage.Name = null;
            Armorimage.Source = new BitmapImage(new Uri(firstArmor.ImageName.Remove(0, 1), UriKind.Relative));

        }
        public void AddNewItemInInventory(int dropId)
        {
            var newAddItem = _gameSession._standardGameItems.First(x => x.ItemID == dropId);
            if (newAddItem.Category == GameItem.ItemCategory.Miscellaneous && dict.ContainsValue(newAddItem.ItemID))
            {
                var item = _inventoryItems.First(x => x.ID == dict.First(x => x.Value == newAddItem.ItemID).Key);
                var grid = (Grid)item.ItemInInventory.Item.Child;
                var textBlock = (TextBlock)grid.Children[1];
                newAddItem.Count = (int)++item.Count;
                textBlock.Text = Convert.ToString(newAddItem.Count);
                return;
            }
            foreach(var item in _inventoryItems)
            {
                if(item.ItemInInventory.IsEmpty)
                {
                    _gameSession._itemInInventory.Add(newAddItem);

                    var grid = (Grid)item.ItemInInventory.Item.Child;
                    var image = (Image)grid.Children[0];
                    image.MouseLeftButtonDown -= Image_MouseLeftButtonDown;

                    var border = GenerateInventoryItem(newAddItem.Name ,newAddItem.ImageName, null, item.ItemInInventory.GridRow, item.ItemInInventory.GridColumn, newAddItem);
                    Inventory.Children.Remove(item.ItemInInventory.Item);
                    item.ItemInInventory.Item = border;
                    item.Name = $"{newAddItem.Name}";
                    item.Category = $"{newAddItem.Category}";
                    item.ID = indnt + 1;
                    indnt++;
                    item.ItemInInventory.IsEmpty = false;
                    
                    Inventory.Children.Add(item.ItemInInventory.Item);
                    dict.Add(item.ID, newAddItem.ItemID);
                    break;
                }
            }
        }
        #endregion

        #region Selecting Methods
        private void OnItemInInventorySelecting(Image image)
        {
            var grid = (Grid)image.Parent;
            var border = (Border)grid.Parent;
            border.BorderThickness = new Thickness(3);
            _lastBorder = border;
            _indexOfLastSelectedBorder = Grid.GetRow(border)*9 + Grid.GetColumn(border);
            _isSelectedInInventory = true;
        }
        private void OnItemInAmmoSelecting(Image image)
        {
            var grid = (Grid)image.Parent;
            var border = (Border)grid.Parent;
            border.BorderThickness = new Thickness(3);
            _isSelectedInAmmo = true;
        }
        #endregion

        #region Swap Methods
        private void SwapItemsInventoryToAmmo(Image image)
        {
            _lastBorder.BorderThickness = new Thickness(1);
            _isSelectedInInventory = false;
            
            var gridlast = (Grid)_lastBorder.Child;
            var imagelast = (Image)gridlast.Children[0];
            var temp = _inventoryItems.First(x => x.ItemInInventory.Item == _lastBorder);
            _mainWindowViewModel = (MainWindowViewModel)DataContext;

            switch (temp.Category)
            {

                case "Weapon":
                    if (temp.Name != null)
                    {
                        _gameSession.mainStats.CurrentWeapon = _gameSession._standardGameItems.First(x => x.ItemID == dict.First(x => x.Key == temp.ID).Value);
                        _gameSession._itemInInventory.Remove(_gameSession._standardGameItems.First(x => x.ItemID == dict.First(x => x.Key == temp.ID).Value));
                    }
                    else
                        _gameSession.mainStats.CurrentWeapon = _gameSession._standardGameItems[0];
                    if (image.Name == null)
                        temp.ID = _gameSession._standardGameItems[0].ItemID;
                    _mainWindowViewModel.CurrentAttack = $"{_gameSession.mainStats.CurrentWeapon.Weapon.MinDamage}-{_gameSession.mainStats.CurrentWeapon.Weapon.MaxDamage}";
                    break;
                case "Armor":
                    if (temp.Name != null)
                    {
                        _gameSession.mainStats.CurrentArmor = _gameSession._standardGameItems.First(x => x.ItemID == dict.First(x => x.Key == temp.ID).Value);
                        _gameSession._itemInInventory.Remove(_gameSession._standardGameItems.First(x => x.ItemID == dict.First(x => x.Key == temp.ID).Value));
                    }
                    else
                        _gameSession.mainStats.CurrentArmor = _gameSession._standardGameItems[1];
                    if (image.Name == null)
                        temp.ID = _gameSession._standardGameItems[1].ItemID;
                    _mainWindowViewModel.CurrentArmor = Convert.ToString(_gameSession.mainStats.CurrentArmor.Armor.PlusArmor);
                    break;
            }
            

            if (image.Name == null)
            {
                imagelast.ToolTipOpening -= Image_ToolTipOpening;
                temp.Category = null;
                temp.ItemInInventory.IsEmpty = true;
            }
            else
                temp.ID = dict.First(x => x.Value == _gameSession._standardGameItems.First(y => y.Name == image.Name).ItemID).Key;            

            temp.Name = image.Name;
            (image.Name, imagelast.Name) = (imagelast.Name, image.Name);
            (image.Source, imagelast.Source) = (imagelast.Source, image.Source);
            (image.ToolTip, imagelast.ToolTip) = (imagelast.ToolTip, image.ToolTip);
            image.ToolTipOpening += Image_ToolTipOpening;
            
            _lastBorder = null;
            
        }
        private void SwapItemsAmmoToInventory(Image image)
        {
            var grid = (Grid)_lastBorderInAmmo.Child;
            var newImage = (Image)grid.Children[0];

            _lastBorderInAmmo.BorderThickness = new Thickness(1);
            _isSelectedInAmmo = false;

            var newGrid = (Grid)image.Parent;
            var newBorder = (Border)newGrid.Parent;
            var temp = _inventoryItems.First(x => x.ItemInInventory.Item == newBorder);
            var tempID = dict.First(x => x.Value == _gameSession._standardGameItems.First(y => y.Name == newImage.Name).ItemID).Key;

            _mainWindowViewModel = (MainWindowViewModel)DataContext;
            switch (_gameSession._standardGameItems.First(x => x.Name == newImage.Name).Category.ToString())
            {
                case "Weapon":
                    if (temp.Name == null)
                    {
                        _gameSession.mainStats.CurrentWeapon = _gameSession._standardGameItems[0];
                        temp.Category = "Weapon";
                        temp.ItemInInventory.IsEmpty = false;
                    }
                    else
                        _gameSession.mainStats.CurrentWeapon = _gameSession._standardGameItems.First(x => x.ItemID == dict.First(x => x.Key == temp.ID).Value);
                    _mainWindowViewModel.CurrentAttack = $"{_gameSession.mainStats.CurrentWeapon.Weapon.MinDamage}-{_gameSession.mainStats.CurrentWeapon.Weapon.MaxDamage}";
                    break;
                case "Armor":
                    if (temp.Name == null)
                    {
                        _gameSession.mainStats.CurrentArmor = _gameSession._standardGameItems[1];
                        temp.Category = "Armor";
                        temp.ItemInInventory.IsEmpty = false;
                    }
                    else
                        _gameSession.mainStats.CurrentArmor = _gameSession._standardGameItems.First(x => x.ItemID == dict.First(x => x.Key == temp.ID).Value);
                    _mainWindowViewModel.CurrentArmor = Convert.ToString(_gameSession.mainStats.CurrentArmor.Armor.PlusArmor);
                    break;
            }

            temp.ID = tempID;
            temp.Name = newImage.Name;
            (image.Name, newImage.Name) = (newImage.Name, image.Name);
            (image.Source, newImage.Source) = (newImage.Source, image.Source);
            (image.ToolTip, newImage.ToolTip) = (newImage.ToolTip, image.ToolTip);
            if(newImage.Name == null)
                newImage.ToolTipOpening -= Image_ToolTipOpening;
            image.ToolTipOpening += Image_ToolTipOpening;
        }
        private void SwapItems(Image image)
        {
            _inventoryItems[_indexOfLastSelectedBorder.Value].ItemInInventory.Item.BorderThickness = new Thickness(1);
            _isSelectedInInventory = false;
           
            var grid = (Grid)image.Parent;
            var border = (Border)grid.Parent;
            var _indexOfNewSelectedBorder = Grid.GetRow(border) * 9 + Grid.GetColumn(border);

            var lastItem = _inventoryItems.First(x => x.ItemInInventory.Item == _lastBorder);
            var newItem = _inventoryItems[_indexOfNewSelectedBorder];

            Grid.SetRow(lastItem.ItemInInventory.Item, newItem.ItemInInventory.GridRow);
            Grid.SetColumn(lastItem.ItemInInventory.Item, newItem.ItemInInventory.GridColumn);
            Grid.SetRow(newItem.ItemInInventory.Item, lastItem.ItemInInventory.GridRow);
            Grid.SetColumn(newItem.ItemInInventory.Item, lastItem.ItemInInventory.GridColumn);

            (lastItem.ItemInInventory.GridRow, newItem.ItemInInventory.GridRow) = (newItem.ItemInInventory.GridRow, lastItem.ItemInInventory.GridRow);
            (lastItem.ItemInInventory.GridColumn, newItem.ItemInInventory.GridColumn) = (newItem.ItemInInventory.GridColumn, lastItem.ItemInInventory.GridColumn);

            _inventoryItems[_indexOfNewSelectedBorder] = lastItem;
            _inventoryItems[_indexOfLastSelectedBorder.Value]= newItem;

            _inventoryItems = _inventoryItems.OrderBy(x => x.ItemInInventory.GridRow).ThenBy(y => y.ItemInInventory.GridColumn).ToList();
            _lastBorder = null;
            _indexOfLastSelectedBorder = null;
        }
        #endregion

        #region Trash Methods
        private void Trash_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_lastBorder != null)
            {
                _lastBorder.BorderThickness = new Thickness(1);
                _isSelectedInInventory = false;
                var clear = _inventoryItems.First(x => x.ItemInInventory.Item == _lastBorder);
                int index = _inventoryItems.IndexOf(clear);
                var grid = (Grid)_lastBorder.Child;
                var image = (Image)grid.Children[0];
                var textBlock = (TextBlock)grid.Children[1];

                image.Source = new BitmapImage(new Uri(@"Resources\Black.png", UriKind.Relative));
                image.Name = null;
                textBlock.Text = null;

                _inventoryItems.Remove(clear);
                _inventoryItems.Insert(index, new InfoForItemInInventory(null, null, 1, clear.ID,
                        new ItemInInventory(_lastBorder,
                        clear.ItemInInventory.GridRow, clear.ItemInInventory.GridColumn)));

                _lastBorder = null;
                _inventoryItems = _inventoryItems.OrderBy(x => x.ItemInInventory.GridRow).ThenBy(y => y.ItemInInventory.GridColumn).ToList();
                dict.Remove(clear.ID);
            }
        }
        #endregion
        #endregion

        #region - TabItemSelecter -
        private void TabItemQuestBook_Selected(object sender, RoutedEventArgs e) => QuestBookTabItem.Foreground = Brushes.White;
        private void TabItemQuestBook_Unselected(object sender, RoutedEventArgs e) => QuestBookTabItem.Foreground = Brushes.LightGray;
        private void TabItemInventory_Selected(object sender, RoutedEventArgs e) => InventoryTabItem.Foreground = Brushes.White;
        private void TabItemInventory_Unselected(object sender, RoutedEventArgs e) => InventoryTabItem.Foreground = Brushes.LightGray;
        private void TabItemMap_Selected(object sender, RoutedEventArgs e) => MapTabItem.Foreground = Brushes.White;
        private void TabItemMap_Unselected(object sender, RoutedEventArgs e) => MapTabItem.Foreground = Brushes.LightGray;
        private void TabItemSkills_Selected(object sender, RoutedEventArgs e) => SkillsTabItem.Foreground = Brushes.White;
        private void TabItemSkills_Unselected(object sender, RoutedEventArgs e) => SkillsTabItem.Foreground = Brushes.LightGray;

        private void TabItemStats_Selected(object sender, RoutedEventArgs e) => StatsTabItem.Foreground = Brushes.White;
        private void TabItemStats_Unselected(object sender, RoutedEventArgs e) => StatsTabItem.Foreground = Brushes.LightGray;
        private void TabItemLog_Selected(object sender, RoutedEventArgs e) => LogTabItem.Foreground = Brushes.White;
        private void TabItemLog_Unselected(object sender, RoutedEventArgs e) => LogTabItem.Foreground = Brushes.LightGray;
        private void TabItemStory_Selected(object sender, RoutedEventArgs e) => StoryTabItem.Foreground = Brushes.White;
        private void TabItemStory_Unselected(object sender, RoutedEventArgs e) => StoryTabItem.Foreground = Brushes.LightGray;
        #endregion

        #region - Skills -
        public Dictionary<Border, Skills> border_Skills = new Dictionary<Border, Skills>();
        public void FillingSkills()
        {
            foreach(var elm in Skills.Children)
            {
                if(elm is Border)
                {
                    var border = (Border)elm;
                    if (border.Name != "")
                    {
                        if (border.Name == "Skill_1")
                            CanUpgrade.Add(border);
                        var newSkill = _gameSession._standartSkills.FirstOrDefault(x => x.ID == Convert.ToInt32(border.Name.Split('_')[1]));
                        if (newSkill != null)
                        {
                            var image = (Image)border.Child;
                            image.Source = new BitmapImage(new Uri(newSkill.ImageName.Remove(0, 1), UriKind.Relative));
                            border_Skills.Add(border, newSkill);
                            ListOfSkillBorder.Add(border);
                        }
                    }
                }
                else if(elm is Grid)
                {
                    GridsRectangle.Add((Grid)elm);
                }
            }
        }
        #endregion

        #region Quest Methods
        public void AddNewQuest(Quest questItem)
        {
            if (!questItem.IsDone)
            {
                questItem.IsDone = true;
                questsGrid.Items.Add(questItem);
                _gameSession._questOnPlayer.Add(questItem);
            }
        }

        private void bToCorrect_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            Quest quest = (Quest)button.DataContext;
            if (quest.Progress == "done")
            {
                questsGrid.Items.Remove(quest);
                _gameSession._questOnPlayer.Remove(quest);
            }
        }
        #endregion

        #region Map Methods
        public void AddNewBorderForMap(Location location)
        {
            if (_lastMapBorder != null)
                _lastMapBorder.Opacity = 0.2;
            Border border = new Border
            {
                Background = Brushes.Black,
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(1)
            };
            Grid.SetColumn(border, 4 + location.CurrentCoordinate.CurrentX);
            Grid.SetRow(border, 4 - location.CurrentCoordinate.CurrentY);
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(location.ImageName.Remove(0, 1), UriKind.Relative));

            border.Child = image;
            if (!_map.ContainsKey(location))
            {
                _map.Add(location, border);
                Map.Children.Add(border);
                _lastMapBorder = border;
            }
            else
            {
                var currBorder = _map.First(x => x.Key == location).Value;
                currBorder.Opacity = 1;
                _lastMapBorder = currBorder;
            }
           
        }
        #endregion

        #region Window Methods
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    var mainWindow = (MainWindow)sender;
                    EscMenu escMenu = new EscMenu(mainWindow, _gameSession);
                    escMenu.Owner = this;
                    mainWindow.IsEnabled = false;
                    escMenu.Show();
                    break;
                case Key.Space:
                    MainWindowViewModel main = (MainWindowViewModel)DataContext;
                    main.DialogAddAll(_gameSession.currentPos.Description);
                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (Window elm in Application.Current.Windows)
                if (elm != (Window)sender)
                    elm.Close();
        }
        #endregion

        #region ProgressBars
        private void XP_ProgressBar_MouseEnter(object sender, MouseEventArgs e) => XPBar.Visibility = Visibility.Visible;

        private void XP_ProgressBar_MouseLeave(object sender, MouseEventArgs e) => XPBar.Visibility = Visibility.Hidden;

        private void HP_ProgressBar_MouseEnter(object sender, MouseEventArgs e) => HPBar.Visibility = Visibility.Visible;

        private void HP_ProgressBar_MouseLeave(object sender, MouseEventArgs e) => HPBar.Visibility = Visibility.Hidden;

        private void MP_ProgressBar_MouseEnter(object sender, MouseEventArgs e) => MPBar.Visibility = Visibility.Visible;

        private void MP_ProgressBar_MouseLeave(object sender, MouseEventArgs e) => MPBar.Visibility = Visibility.Hidden;

        //private void SP_ProgressBar_MouseEnter(object sender, MouseEventArgs e) => SPBar.Visibility = Visibility.Visible;

        //private void SP_ProgressBar_MouseLeave(object sender, MouseEventArgs e) => SPBar.Visibility = Visibility.Hidden;
        #endregion

        

        private void SkillInTree_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = (Image)sender;
            var border = (Border)image.Parent;
            if (!SelectedSkillList.Contains(border) && CanUpgrade.Contains(border) && _gameSession.mainStats.SkillPoint > 0)
            {
                _gameSession.mainStats.SkillPoint--;
                _mainWindowViewModel = (MainWindowViewModel)DataContext;
                _mainWindowViewModel.CountOfSkillPoint = $"{_gameSession.mainStats.SkillPoint}";
                SelectedSkillList.Add(border);
                CanUpgrade.Clear();
                var a = GridsRectangle.FindAll(x => x.Name.Split('_')[1] == border.Name.Split('_')[1]);
                foreach (var b in a)
                {
                    var c = ListOfSkillBorder.FirstOrDefault(x => x.Name.Split('_')[1] == b.Name.Split('_')[2]);
                    if(c != null)
                        CanUpgrade.Add(c);
                }
                if (_lastSkill != null)
                {
                    foreach (Rectangle rect in GridsRectangle.First(x => (x.Name.Split('_')[1] + x.Name.Split('_')[2]) == (_lastSkill.ID + border.Name.Split('_')[1])).Children)
                    {
                        rect.Fill = new SolidColorBrush(Color.FromRgb(22, 204, 0));
                        rect.Opacity = 1;
                    }
                }
                _lastSkill = border_Skills.First(x => x.Key == border).Value;
                border.Opacity = 1;
            }
        }
        //private async void AnimationForRectangle(Rectangle rectangle)
        //{
        //    await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
        //    {
        //        while (_isActiveLeft)
        //        {
        //            for (short i = 0; i <= 255; i += 3)
        //            {
        //                if (!_isActiveLeft && !_isActiveRight)
        //                    return;

        //                rectangle.Fill = new SolidColorBrush(Color.FromRgb(Convert.ToByte(i), 255, Convert.ToByte(i)));
        //                await Task.Delay(1);
        //            }
        //            for (short i = 255; i >= 0; i -= 3)
        //            {
        //                if (!_isActiveLeft && !_isActiveRight)
        //                    return;
        //                rectangle.Fill = new SolidColorBrush(Color.FromRgb(Convert.ToByte(i), 255, Convert.ToByte(i)));
        //                await Task.Delay(1);
        //            }
        //        }
                
        //    }));
        //}
        //List<Grid> gridsList = new List<Grid>();
        //List<Grid> gridsList2 = new List<Grid>();
        //private async void AnimationForArrow(List<Grid> gridList)
        //{
        //    _isActiveLeft = true;
        //    _isActiveRight = true;
        //    await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
        //    {
        //        foreach (Grid grid in gridsList)
        //            foreach (Rectangle rectangle in grid.Children)
        //                AnimationForRectangle(rectangle);

        //        await Task.Delay(3);
        //    }
        //    ));
        //}


        private void Skill_MouseEnter(object sender, MouseEventArgs e)
        {
            var image = (Image)sender;
            var border = (Border)image.Parent;
            if(CanUpgrade.Contains(border) && !SelectedSkillList.Contains(border))
                border.Opacity = 1;
        }

        private void Skill_MouseLeave(object sender, MouseEventArgs e)
        {
            var image = (Image)sender;
            var border = (Border)image.Parent;
            if (CanUpgrade.Contains(border) && !SelectedSkillList.Contains(border))
                border.Opacity = 0.5;
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel = (MainWindowViewModel)DataContext;
            if (_gameSession.currentPos.Dialogs != null)
            {
                this.IsEnabled = false;
                var HistoryOfDialog = new HistoryOfDialog(this, _gameSession.currentPos, _mainWindowViewModel);
                HistoryOfDialog.Owner = this;
                HistoryOfDialog.Show();
            }
        }

        //#region StartUp

        //private void Image_MouseMove(object sender, MouseEventArgs e)
        //{
        //    Image image;
        //    TextBlock textBlock;
        //    Grid grid;
        //    if (sender is Image)
        //    {
        //        image = (Image)sender;
        //        grid = (Grid)image.Parent;
        //        textBlock = (TextBlock)grid.Children[1];
        //    }
        //    else
        //    {
        //        textBlock = (TextBlock)sender;
        //        grid = (Grid)textBlock.Parent;
        //        image = (Image)grid.Children[0];

        //    }
        //    image.Source = new BitmapImage(new Uri(@"\Resources\StartUp\Buttons\new_button_white.png", UriKind.Relative));
        //    textBlock.Opacity = 1;
        //}

        //private void Image_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    Image image;
        //    TextBlock textBlock;
        //    Grid grid;
        //    if (sender is Image)
        //    {
        //        image = (Image)sender;
        //        grid = (Grid)image.Parent;
        //        textBlock = (TextBlock)grid.Children[1];
        //    }
        //    else
        //    {
        //        textBlock = (TextBlock)sender;
        //        grid = (Grid)textBlock.Parent;
        //        image = (Image)grid.Children[0];

        //    }
        //    image.Source = new BitmapImage(new Uri(@"\Resources\StartUp\Buttons\new_button_black.png", UriKind.Relative));
        //    textBlock.Opacity = 0.8;
        //}
        //private void Image2_MouseMove(object sender, MouseEventArgs e)
        //{
        //    Image image;
        //    TextBlock textBlock;
        //    Grid grid;
        //    if (sender is Image)
        //    {
        //        image = (Image)sender;
        //        grid = (Grid)image.Parent;
        //        textBlock = (TextBlock)grid.Children[1];
        //    }
        //    else
        //    {
        //        textBlock = (TextBlock)sender;
        //        grid = (Grid)textBlock.Parent;
        //        image = (Image)grid.Children[0];

        //    }
        //    image.Source = new BitmapImage(new Uri(@"\Resources\StartUp\Buttons\new_button2_white.png", UriKind.Relative));
        //    textBlock.Opacity = 1;
        //}

        //private void Image2_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    Image image;
        //    TextBlock textBlock;
        //    Grid grid;
        //    if (sender is Image)
        //    {
        //        image = (Image)sender;
        //        grid = (Grid)image.Parent;
        //        textBlock = (TextBlock)grid.Children[1];
        //    }
        //    else
        //    {
        //        textBlock = (TextBlock)sender;
        //        grid = (Grid)textBlock.Parent;
        //        image = (Image)grid.Children[0];

        //    }
        //    image.Source = new BitmapImage(new Uri(@"\Resources\StartUp\Buttons\new_button2_black.png", UriKind.Relative));
        //    textBlock.Opacity = 0.8;
        //}
        //private void Image3_MouseMove(object sender, MouseEventArgs e)
        //{
        //    Image image;
        //    TextBlock textBlock;
        //    Grid grid;
        //    if (sender is Image)
        //    {
        //        image = (Image)sender;
        //        grid = (Grid)image.Parent;
        //        textBlock = (TextBlock)grid.Children[1];
        //    }
        //    else
        //    {
        //        textBlock = (TextBlock)sender;
        //        grid = (Grid)textBlock.Parent;
        //        image = (Image)grid.Children[0];

        //    }
        //    image.Source = new BitmapImage(new Uri(@"\Resources\StartUp\Buttons\new_button3_white.png", UriKind.Relative));
        //    textBlock.Opacity = 1;
        //}

        //private void Image3_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    Image image;
        //    TextBlock textBlock;
        //    Grid grid;
        //    if (sender is Image)
        //    {
        //        image = (Image)sender;
        //        grid = (Grid)image.Parent;
        //        textBlock = (TextBlock)grid.Children[1];
        //    }
        //    else
        //    {
        //        textBlock = (TextBlock)sender;
        //        grid = (Grid)textBlock.Parent;
        //        image = (Image)grid.Children[0];

        //    }
        //    image.Source = new BitmapImage(new Uri(@"\Resources\StartUp\Buttons\new_button3_black.png", UriKind.Relative));
        //    textBlock.Opacity = 0.8;
        //}

        //private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    this.Close();
        //}

        //private async void Play_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    player.MediaFailed += (s, e) => MessageBox.Show("Error");
        //    player.Open(new Uri("../../DokiDoki-SayoNara.wav", UriKind.RelativeOrAbsolute));
        //    player.Volume = 0.05;
        //    player.Play();
        //    //MainWindowGrid.Visibility = Visibility.Visible;
        //    await OpacityForStartUpWindow();
        //    Thread.Sleep(10);
        //    foreach(var elm in Captions.Children)
        //    {
        //        if (elm is TextBlock)
        //            await AsyncUp((TextBlock)elm);
        //    }
        //    await AsyncUp(GifOnStartUp);
        //    GifOnStartUp.Visibility = Visibility.Collapsed;
            
            
        //}

        //private async Task AsyncUp(TextBlock textBlock)
        //{
        //    textBlock.Visibility = Visibility.Visible;
        //    while (textBlock.Opacity < 1)
        //    {
        //        textBlock.Opacity += 0.0055;
        //        await Task.Delay(1);
        //    }
        //    while (textBlock.Opacity > 0)
        //    {
        //        textBlock.Opacity -= 0.0055;
        //        await Task.Delay(1);
        //    }
        //    textBlock.Visibility = Visibility.Collapsed;
        //}
        //private async Task AsyncUp(Image image)
        //{
        //    image.Visibility = Visibility.Visible;
        //    while (image.Opacity < 1)
        //    {
        //        image.Opacity += 0.015;
        //        await Task.Delay(1);
        //    }
        //    await Task.Delay(3000);
        //    MainWindowGrid.Visibility = Visibility.Visible;
        //    await OpacityForMainWindow();
        //    while (image.Opacity > 0)
        //    {
        //        image.Opacity -= 0.025;
        //        await Task.Delay(1);
        //    }
        //}

        //private async Task OpacityForStartUpWindow()
        //{
        //    await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
        //    {
        //        while (StartUp.Opacity != 0)
        //        {
        //            StartUp.Opacity -= 0.015;
        //            await Task.Delay(1);
        //        }
        //    }));
        //}
        //private async Task OpacityForMainWindow()
        //{
        //    await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
        //    {
        //        while (MainWindowGrid.Opacity < 1)
        //        {
        //            MainWindowGrid.Opacity += 0.005;
        //            await Task.Delay(1);
        //        }
        //    }));
        //}
        //#endregion
    }
}
