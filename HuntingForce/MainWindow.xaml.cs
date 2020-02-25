using EngineHF;
using EngineHF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HuntingForce
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isItemSelected;
        private int? _indexOfLastSelectedBorder = null;
        private Border lastBorder;
        int indnt = 0;




        int gay = 0;
        private bool skillSelected;
        private Button lastButton;
        private Image lastImage;
        private static readonly GameSession _gameSession = new GameSession();
        private List<TextBlock> arrayOfTextBlock = new List<TextBlock>();
        private List<Border> arrayOfBorder = new List<Border>();

        private List<TextBlock> _arrayOfTextBlock = new List<TextBlock>();
        private List<TextBlock> _arrayOfSkillTextBlock = new List<TextBlock>();
        private List<Border> _arrayOfBorder = new List<Border>();

        private TextBlock[] _arrayOfTextBlock1 = new TextBlock[_gameSession._standartSkills.Count];
        private TextBlock[] _arrayOfSkillTextBlock1 = new TextBlock[_gameSession._standartSkills.Count];
        private Border[] _arrayOfBorder1 = new Border[_gameSession._standartSkills.Count];


        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(_gameSession, this);
            AddingNewSkills();
            AddingInventory();
            AddFirstWeapon();
        }
        #region - Inventory -
        private List<ItemInInventory> _inventoryItems = new List<ItemInInventory>();
        private Border GenerateInventoryItem(string imageName, string text, int gridRow, int gridColumn)
        {
            Border border = new Border()
            {
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(1)
            };

            Grid grid = new Grid();

            Image image = new Image()
            {
                Margin = new Thickness(3),
                Source = new BitmapImage(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\" + imageName.Remove(0,1)))
            };
            image.MouseLeftButtonDown += Image_MouseLeftButtonDown;

            TextBlock textBlock = new TextBlock()
            {
                Text = text,
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

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isItemSelected)
                SwapItems((Image)sender);
            else
                OnItemInInventorySelecting((Image)sender);
        }

        private void AddingInventory()
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 9; j++)
                    _inventoryItems.Add(new ItemInInventory(GenerateInventoryItem(@".\Resources\Black.png", null, i, j), null, i, j));

            foreach (var item in _inventoryItems)
                Inventory.Children.Add(item.Item);
        }
        private void AddFirstWeapon()
        {
            var firstWeapon = _gameSession.mainStats.CurrentWeapon;
            var border = (Border)Weapon.Children[1];
            var image = (Image)border.Child;
            image.Source = new BitmapImage(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\" + firstWeapon.ImageName.Remove(0, 1)));
        }
        public void AddNewItemInInventory(Drop drop)
        {
            var newAddItem = _gameSession._standardGameItems.First(x => x.ItemID == drop.DropID);
            if(newAddItem.Category == GameItem.ItemCategory.Miscellaneous && _inventoryItems.(x => x.Name == ""))
            {
                var item = _inventoryItems.First(x => x.Name.Split('_')[0] + "_" + x.Name.Split('_')[1] == $"{newAddItem.Name}_{newAddItem.Category}");
                var grid = (Grid)item.Item.Child;
                var textBlock = (TextBlock)grid.Children[1];
                textBlock.Text = "2";
            }
            foreach(var item in _inventoryItems)
            {
                if(item.IsEmpty)
                {
                    var grid = (Grid)item.Item.Child;
                    var image = (Image)grid.Children[0];
                    image.MouseLeftButtonDown -= Image_MouseLeftButtonDown;

                    var border = GenerateInventoryItem(newAddItem.ImageName, null, item.GridRow, item.GridColumn);
                    Inventory.Children.Remove(item.Item);
                    item.Item = border;
                    item.Name = $"{newAddItem.Name}_{newAddItem.Category}_{indnt++}";
                    item.IsEmpty = false;
                    Inventory.Children.Add(item.Item);
                    break;
                }
            }
        }

        private void OnItemInInventorySelecting(Image image)
        {
            var grid = (Grid)image.Parent;
            var border = (Border)grid.Parent;
            border.BorderThickness = new Thickness(3);
            _indexOfLastSelectedBorder = Grid.GetRow(border)*9 + Grid.GetColumn(border);
            lastBorder = border;
            _isItemSelected = true;
        }

        private void SwapItems(Image image)
        {
            lastBorder.BorderThickness = new Thickness(1);
            _isItemSelected = false;

            var grid = (Grid)image.Parent;
            var border = (Border)grid.Parent;
            var _indexOfNewSelectedBorder = Grid.GetRow(border) * 9 + Grid.GetColumn(border);



            var lastItem = _inventoryItems[_indexOfLastSelectedBorder.Value];
            var newItem = _inventoryItems[_indexOfNewSelectedBorder];

            Grid.SetRow(lastItem.Item, newItem.GridRow);
            Grid.SetColumn(lastItem.Item, newItem.GridColumn);
            Grid.SetRow(newItem.Item, lastItem.GridRow);
            Grid.SetColumn(newItem.Item, lastItem.GridColumn);

            (lastItem.GridRow, newItem.GridRow) = (newItem.GridRow, lastItem.GridRow);
            (lastItem.GridColumn, newItem.GridColumn) = (newItem.GridColumn, lastItem.GridColumn);

            _inventoryItems[_indexOfNewSelectedBorder] = lastItem;
            _inventoryItems[_indexOfLastSelectedBorder.Value] = newItem;

            _inventoryItems = _inventoryItems.OrderBy(x => x.GridRow).ThenBy(y => y.GridColumn).ToList();

            lastBorder = null;
            _indexOfLastSelectedBorder = null;
        }
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
        #endregion

        #region - Skills -
        public void AddingNewSkills()
        {
            foreach (var skill in _gameSession._standartSkills)
            {
                Border border = new Border
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    BorderBrush = Brushes.White,
                    BorderThickness = new Thickness(1),
                    Padding = new Thickness(0),
                    Width = 60
                };
                Grid.SetRow(border, skill.GridRow);
                Grid.SetColumn(border, skill.GridColumn);

                Button button = new Button
                {
                    Name = "Skill" + "_" + skill.ID,
                    Background = Brushes.Black,
                    VerticalAlignment = VerticalAlignment.Top,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(0),
                    Margin = new Thickness(1),
                    Height = 60
                };
                button.Click += SkillButtonOnClick;

                Image image = new Image
                {
                    Source = new BitmapImage(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\" + skill.ImageName.Remove(0, 1))),
                    Margin = new Thickness(2)
                };


                button.Content = image;
                border.Child = button;

                TextBlock textBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Text = "0/1",
                    FontSize = 20,
                    Foreground = Brushes.White
                };
                textBlock.FontFamily = new FontFamily(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Fonts\#Determination2");
                Grid.SetRow(textBlock, skill.GridRow);
                Grid.SetColumn(textBlock, skill.GridColumn);
                if (skill.ID != 1)
                {
                    button.IsEnabled = false;
                    border.BorderBrush = Brushes.Gray;
                    image.Opacity = 0.7;
                    textBlock.Opacity = 0.5;
                }
                arrayOfTextBlock.Add(textBlock);
                arrayOfBorder.Add(border);
                SkillsGrid.Children.Add(border);
                SkillsGrid.Children.Add(textBlock);
            }
        }
        private void DesignOfSkillUpdate(int i)
        {
            arrayOfBorder[i].BorderBrush = Brushes.White;
            arrayOfBorder[i].Child.IsEnabled = true;
            Button buttonSkill = (Button)arrayOfBorder[i].Child;
            Image image = (Image)buttonSkill.Content;
            image.Opacity = 1;
            arrayOfTextBlock[i].Opacity = 1;
        }
        private void DesignOfSkillDegrade(int i)
        {
            _arrayOfBorder[i].BorderBrush = Brushes.Gray;
            _arrayOfBorder[i].Child.IsEnabled = false;
            Button buttonSkill = (Button)_arrayOfBorder[i].Child;
            Image image = (Image)buttonSkill.Content;
            image.Opacity = 0.7;
            _arrayOfSkillTextBlock[i].Opacity = 0.5;
        }
        public void SkillButtonOnClick(object sender, EventArgs eventArgs)
        {
            var button = (Button)sender;

            var sr = Convert.ToInt32(button.Name.Split('_')[1]);
            if (arrayOfTextBlock[sr - 1].Text != "1/1" && _gameSession.mainStats.TempSkillPoint > 0)
            {
                Cancel.IsEnabled = true;
                arrayOfTextBlock[sr - 1].Text = "1/1";
                _gameSession.mainStats.TempSkillPoint--;
                if (sr == 1)
                    for (int i = sr; i < sr + 2; i++)
                    {
                        DesignOfSkillUpdate(i);
                        _arrayOfBorder.Add(arrayOfBorder[i]);
                        _arrayOfSkillTextBlock.Add(arrayOfTextBlock[i]);
                    }
                else if (sr + 1 < arrayOfBorder.Count())
                {
                    DesignOfSkillUpdate(sr + 1);
                    _arrayOfBorder.Add(arrayOfBorder[sr + 1]);
                    _arrayOfSkillTextBlock.Add(arrayOfTextBlock[sr + 1]);
                }
                _arrayOfTextBlock.Add(arrayOfTextBlock[sr - 1]);
            }
            else
            {
                if (Keyboard.IsKeyDown(Key.LeftShift) && !Cancel.IsEnabled)
                {
                    var a = (Border)button.Parent;
                    a.BorderThickness = new Thickness(3);
                    skillSelected = true;
                    lastButton = button;
                }
            }
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            foreach (var a in _arrayOfBorder1)
                _arrayOfBorder.Remove(a);
            foreach (var a in _arrayOfSkillTextBlock1)
                _arrayOfSkillTextBlock.Remove(a);
            foreach (var a in _arrayOfTextBlock1)
                _arrayOfTextBlock.Remove(a);

            for (int i = 0; i < _arrayOfTextBlock.Count; i++)
                _arrayOfTextBlock[i].Text = "0/1";
            for (int a = 0; a < _arrayOfBorder.Count; a++)
                DesignOfSkillDegrade(a);

            _arrayOfBorder.Clear();
            _arrayOfSkillTextBlock.Clear();
            _arrayOfTextBlock.Clear();
            Cancel.IsEnabled = false;
            _gameSession.mainStats.TempSkillPoint = _gameSession.mainStats.SkillPoint;
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            _gameSession.mainStats.SkillPoint = _gameSession.mainStats.TempSkillPoint;
            _arrayOfBorder.CopyTo(_arrayOfBorder1);
            _arrayOfSkillTextBlock.CopyTo(_arrayOfSkillTextBlock1);
            _arrayOfTextBlock.CopyTo(_arrayOfTextBlock1);
            Cancel.IsEnabled = false;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (skillSelected)
            {
                Image image = (Image)sender;
                Button button = (Button)image.Parent;
                button.PreviewMouseDown += SkillDetected;
                button.Name = _gameSession._standartSkills.First().Name;
                Image bimage = (Image)lastButton.Content;
                image.Source = bimage.Source;
                Border border = (Border)lastButton.Parent;
                border.BorderThickness = new Thickness(1);
                skillSelected = false;
            }
        }

        public void SkillDetected(object sender, MouseButtonEventArgs e)
        {
            Button button = (Button)sender;
            var _gm = (MainWindowViewModel)DataContext;
            var skillUse = _gameSession._standartSkills.First(x => x.Name == button.Name);
            switch (skillUse.Type)
            {
                case EngineHF.Model.Skills.TypeOfSkill.Attack:
                    if (_gameSession.currentPos.monster != null)
                        _gm.Logging("useSkill", new string[] { _gameSession.mainStats.Name, skillUse.Name, _gameSession.currentPos.monster.Name });
                    for (int i = 0; i < skillUse.Attack.CountOfSlash; i++)
                        _gm.Attacking(_gameSession.mainStats.CurrentWeapon.Weapon.MinDamage + skillUse.Attack.MinBonusDamage, _gameSession.mainStats.CurrentWeapon.Weapon.MaxDamage + skillUse.Attack.MaxBonusDamage);
                    break;
                case EngineHF.Model.Skills.TypeOfSkill.Heal:
                    break;
            }
        }
        #endregion


        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindowViewModel main = (MainWindowViewModel)DataContext;
            main.DialogAddAll();
        }

        private void Weapon_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        private void Armor_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
