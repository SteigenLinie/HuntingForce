using EngineHF;
using EngineHF.Model;
using EngineHF.Model.Inventory;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace HuntingForce
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

        private bool skillSelected;
        private Button lastButton;
        private static readonly GameSession _gameSession = new GameSession();
        private List<TextBlock> arrayOfTextBlock = new List<TextBlock>();
        private List<Border> arrayOfBorder = new List<Border>();

        private List<TextBlock> _arrayOfTextBlock = new List<TextBlock>();
        private List<TextBlock> _arrayOfSkillTextBlock = new List<TextBlock>();
        private List<Border> _arrayOfBorder = new List<Border>();

        private TextBlock[] _arrayOfTextBlock1 = new TextBlock[_gameSession._standartSkills.Count];
        private TextBlock[] _arrayOfSkillTextBlock1 = new TextBlock[_gameSession._standartSkills.Count];
        private Border[] _arrayOfBorder1 = new Border[_gameSession._standartSkills.Count];

        MainWindowViewModel _mainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(_gameSession, this);
            AddingNewSkills();
            AddFirstWeapon();
            AddingInventory();
        }
        #region - Inventory -
        #region DopMethod
        private Border GenerateInventoryItem(string name, string imageName, string text, int gridRow, int gridColumn)
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
                Source = new BitmapImage(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\" + imageName.Remove(0,1)))
            };
            image.MouseLeftButtonDown += Image_MouseLeftButtonDown;

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
            Weaponimage.Name = null;
            Weaponimage.Source = new BitmapImage(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Black.png"));


            var firstArmor = _gameSession.mainStats.CurrentArmor;
            var Armorborder = (Border)Armor.Children[0];
            var Armorgrid = (Grid)Armorborder.Child;
            var Armorimage = (Image)Armorgrid.Children[0];
            Armorimage.Name = null;
            Armorimage.Source = new BitmapImage(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Black.png"));

        }
        public void AddNewItemInInventory(Drop drop)
        {
            var newAddItem = _gameSession._standardGameItems.First(x => x.ItemID == drop.DropID);
            if(newAddItem.Category == GameItem.ItemCategory.Miscellaneous && dict.ContainsValue(newAddItem.ItemID))
            {
                var item = _inventoryItems.First(x => x.ID == dict.First(x => x.Value == newAddItem.ItemID).Key);
                var grid = (Grid)item.ItemInInventory.Item.Child;
                var textBlock = (TextBlock)grid.Children[1];
                textBlock.Text = Convert.ToString(++item.Count);
                return;
            }
            foreach(var item in _inventoryItems)
            {
                if(item.ItemInInventory.IsEmpty)
                {
                    var grid = (Grid)item.ItemInInventory.Item.Child;
                    var image = (Image)grid.Children[0];
                    image.MouseLeftButtonDown -= Image_MouseLeftButtonDown;

                    var border = GenerateInventoryItem(newAddItem.Name ,newAddItem.ImageName, null, item.ItemInInventory.GridRow, item.ItemInInventory.GridColumn);
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
        private void OnItemInAmmoUnselecting(Border border)
        {
            border.BorderThickness = new Thickness(1);
            _isSelectedInAmmo = false;
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
                        _gameSession.mainStats.CurrentWeapon = _gameSession._standardGameItems.First(x => x.ItemID == dict.First(x => x.Key == temp.ID).Value);
                    else
                        _gameSession.mainStats.CurrentWeapon = _gameSession._standardGameItems[0];
                    if (image.Name == null)
                        temp.ID = _gameSession._standardGameItems[0].ItemID;
                    _mainWindowViewModel.CurrentAttack = $"{_gameSession.mainStats.CurrentWeapon.Weapon.MinDamage}-{_gameSession.mainStats.CurrentWeapon.Weapon.MaxDamage}";
                    break;
                case "Armor":
                    if (temp.Name != null)
                        _gameSession.mainStats.CurrentArmor = _gameSession._standardGameItems.First(x => x.ItemID == dict.First(x => x.Key == temp.ID).Value);
                    else
                        _gameSession.mainStats.CurrentArmor = _gameSession._standardGameItems[1];
                    if (image.Name == null)
                        temp.ID = _gameSession._standardGameItems[1].ItemID;
                    _mainWindowViewModel.CurrentArmor = Convert.ToString(_gameSession.mainStats.CurrentArmor.Armor.PlusArmor);
                    break;
            }
            

            if (image.Name == null)
            {
                temp.Category = null;
                temp.ItemInInventory.IsEmpty = true;
            }
            else
                temp.ID = dict.First(x => x.Value == _gameSession._standardGameItems.First(y => y.Name == image.Name).ItemID).Key;            

            temp.Name = image.Name;
            (image.Name, imagelast.Name) = (imagelast.Name, image.Name);
            (image.Source, imagelast.Source) = (imagelast.Source, image.Source);

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
                    Name = skill.Name,
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
                Image bimage = (Image)lastButton.Content;
                button.Name = _gameSession._standartSkills.First(x => x.Name == bimage.Name).Name;
                image.Source = bimage.Source;
                Border border = (Border)lastButton.Parent;
                border.BorderThickness = new Thickness(1);
                skillSelected = false;
            }
        }

        public void SkillDetected(object sender, MouseButtonEventArgs e)
        {
            var button = (Button)sender;
            if (Keyboard.IsKeyDown(Key.LeftShift) && button.Name != "")
            {
                var imagee = (Image)button.Content;
                button.PreviewMouseDown -= SkillDetected;
                button.Name = "";
                imagee.Source = new BitmapImage(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Black.png"));
                return;
            }
            var _gm = (MainWindowViewModel)DataContext;
            if (_gm.MoveButton)
            {
                var skillUse = _gameSession._standartSkills.First(x => x.Name == button.Name);
                switch (skillUse.Type)
                {
                    case EngineHF.Model.Skills.TypeOfSkill.Attack:
                        if (_gameSession.currentPos.Monster != null)
                            _gm.Logging("useSkill", new string[] { _gameSession.mainStats.Name, skillUse.Name, _gameSession.currentPos.Monster.Name });
                        for (int i = 0; i < skillUse.Attack.CountOfSlash; i++)
                            _gm.Attacking(_gameSession.mainStats.CurrentWeapon.Weapon.MinDamage + skillUse.Attack.MinBonusDamage, _gameSession.mainStats.CurrentWeapon.Weapon.MaxDamage + skillUse.Attack.MaxBonusDamage);
                        break;
                    case EngineHF.Model.Skills.TypeOfSkill.Heal:
                        break;
                }
            }
        }
        #endregion

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindowViewModel main = (MainWindowViewModel)DataContext;
            main.DialogAddAll();
        }

        private void popup1_Opened(object sender, EventArgs e)
        {
            DispatcherTimer time = new DispatcherTimer();
            time.Interval = TimeSpan.FromSeconds(10);
            time.Start();
            time.Tick += delegate
            {
                popup1.IsOpen = false;
                time.Stop();
            };
        }

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

                image.Source = new BitmapImage(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Black.png"));
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
        public void AddNewQuest(Quest questItem)
        {
            questItem.IsDone = true;
            questsGrid.Items.Add(questItem);
            _gameSession.mainStats.QuestOnPlayer.Add(_gameSession.currentPos.Quest);
        }

        private void bToCorrect_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            Quest quest = (Quest)button.DataContext;
            if (quest.Progress == "done")
            {
                questsGrid.Items.Remove(quest);
                _gameSession.mainStats.QuestOnPlayer.Remove(quest);
            }
        }
    }
}
