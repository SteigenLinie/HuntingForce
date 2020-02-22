using EngineHF;
using EngineHF.Model;
using Prism.Commands;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HuntingForce
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int GridColumnForInventory = -1;
        private int GridRowForInventory = -1;
        private bool skillSelected;
        private bool isSelectedItem;
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

        private Dictionary<string, TextBlock> inventoryItem = new Dictionary<string, TextBlock>();
        private List<Image> images = new List<Image>();
        

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(_gameSession, this);
            AddingNewSkills();
            AddingFirstItem();
            VoidInventory();
        }
        private void VoidInventory()
        {
            for(int i = 0; i < 2; i++)
                for(int j = 0; j < 9; j++)
                {
                    Border border = new Border
                    {
                        BorderBrush = Brushes.White,
                        BorderThickness = new Thickness(0),
                        Padding = new Thickness(0)
                    };
                    TextBlock textBlock = new TextBlock
                    {
                        VerticalAlignment = VerticalAlignment.Bottom,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Foreground = Brushes.White
                    };
                    Grid.SetRow(border, i);
                    Grid.SetRow(textBlock, i);
                    Grid.SetColumn(border, j);
                    Grid.SetColumn(textBlock, j);
                    Image image = new Image
                    {
                        Source = new BitmapImage(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\Resources\Black.png")),
                        Margin = new Thickness(2)
                    };

                    image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
                    border.Child = image;
                    inventoryItem.Add($"{i}.{j}", textBlock);
                    Inventory.Children.Add(border);
                    Inventory.Children.Add(textBlock);
                }
        }
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
        public void AddingFirstItem()
        {
            var currentWeapon = _gameSession.mainStats.CurrentWeapon;
            var border = (Border)Weapon.Children[1];
            var image = (Image)border.Child;
            image.Source = new BitmapImage(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\" + currentWeapon.ImageName.Remove(0, 1)));
            image.Name = currentWeapon.Name + "_" + currentWeapon.Category.ToString();
        }
        public void AddNewItemInInventory(Drop drop)
        {
            var newItemInInventory = _gameSession._standardGameItems.First(x => x.ItemID == drop.DropID);
           
            Border border = new Border
            {
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(0)
            };
            TextBlock textBlock = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right,
                Foreground = Brushes.White
            };
            
            Image image = new Image
            {
                Source = new BitmapImage(new Uri(@"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\" + newItemInInventory.ImageName.Remove(0, 1))),
                Name = newItemInInventory.Name + "_" + newItemInInventory.Category.ToString(),
                Margin = new Thickness(2)
            };

            if (!inventoryItem.ContainsKey(image.Name))
            {
                if (GridColumnForInventory >= 9)
                {

                    Grid.SetRow(border, ++GridRowForInventory);
                    Grid.SetRow(textBlock, GridRowForInventory);
                    GridColumnForInventory = 0;
                }
                Grid.SetColumn(border, ++GridColumnForInventory);
                Grid.SetColumn(textBlock, GridColumnForInventory);
                var ss = Grid.GetRow(border);
                var sss = Grid.GetColumn(textBlock);
            }

            image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            border.Child = image;
            if (inventoryItem.ContainsKey(image.Name) && image.Name != "")
            {
                var a = inventoryItem.First(x => x.Key == image.Name);
                if (a.Value.Text == "")
                    a.Value.Text = "2";
                else
                    a.Value.Text = $"{Convert.ToInt32(a.Value.Text) + 1}";
            }
            else
            {
                inventoryItem.Add(image.Name, textBlock);
                Inventory.Children.Add(border);
                Inventory.Children.Add(textBlock);
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            if (isSelectedItem)
                SwapImage(image);      
            else
                SelectItemInInventory(image);
        }

        private void TabItemQuestBook_Selected(object sender, RoutedEventArgs e) => QuestBookTabItem.Foreground = Brushes.White;
        private void TabItemQuestBook_Unselected(object sender, RoutedEventArgs e) => QuestBookTabItem.Foreground = Brushes.LightGray;
        private void TabItemInventory_Selected(object sender, RoutedEventArgs e) => InventoryTabItem.Foreground = Brushes.White;
        private void TabItemInventory_Unselected(object sender, RoutedEventArgs e) => InventoryTabItem.Foreground = Brushes.LightGray;
        private void TabItemMap_Selected(object sender, RoutedEventArgs e) => MapTabItem.Foreground = Brushes.White;
        private void TabItemMap_Unselected(object sender, RoutedEventArgs e) => MapTabItem.Foreground = Brushes.LightGray;
        private void TabItemSkills_Selected(object sender, RoutedEventArgs e) => SkillsTabItem.Foreground = Brushes.White;
        private void TabItemSkills_Unselected(object sender, RoutedEventArgs e) => SkillsTabItem.Foreground = Brushes.LightGray;

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindowViewModel main = (MainWindowViewModel)DataContext;
            main.DialogAddAll();
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
            switch(skillUse.Type)
            {
                case EngineHF.Model.Skills.TypeOfSkill.Attack:
                    if (_gameSession.currentPos.monster != null)
                        _gm.Logging("useSkill", new string[] {_gameSession.mainStats.Name, skillUse.Name, _gameSession.currentPos.monster.Name });
                    for(int i = 0; i < skillUse.Attack.CountOfSlash; i++)
                        _gm.Attacking(_gameSession.mainStats.CurrentWeapon.Weapon.MinDamage + skillUse.Attack.MinBonusDamage, _gameSession.mainStats.CurrentWeapon.Weapon.MaxDamage + skillUse.Attack.MaxBonusDamage);
                    break;
                case EngineHF.Model.Skills.TypeOfSkill.Heal:
                    break;
            }          
        }

        private void SwapImage(Image newImage)
        {
            (newImage.Name, lastImage.Name) = (lastImage.Name, newImage.Name);
            (newImage.Source, lastImage.Source) = (lastImage.Source, newImage.Source);
            var lastBorder = (Border)lastImage.Parent;
            lastBorder.BorderThickness = new Thickness(1);

            GridColumnForInventory -= Grid.GetColumn(lastBorder);
            GridRowForInventory -= Grid.GetRow(lastBorder);
            inventoryItem.Remove(newImage.Name);
            isSelectedItem = false;
        }
        private void SelectItemInInventory(Image image)
        {
            if (image.Name == "")
                return;
            var border = (Border)image.Parent;
            border.BorderThickness = new Thickness(3);

            if (lastImage != null && isSelectedItem)
                UnselectItemInInventory();
            lastImage = image;
            isSelectedItem = true;
        }
        private void UnselectItemInInventory()
        {
            var clear = (Border)lastImage.Parent;
            clear.BorderThickness = new Thickness(1);
            isSelectedItem = false;
            return;
        }
        private void Weapon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Image newimage = (Image)sender;
                if (isSelectedItem)
                {
                    if (lastImage.Name != newimage.Name && lastImage.Name.Split('_')[1] == GameItem.ItemCategory.Weapon.ToString())
                    {
                        SwapImage(newimage);
                        _gameSession.mainStats.CurrentWeapon = _gameSession._standardGameItems.First(x => x.Name == newimage.Name.Split('_')[0]);
                    }
                    else
                        UnselectItemInInventory();
                }
                else
                    SelectItemInInventory(newimage);
            }
            catch
            {
                MessageBox.Show("gay");
            }
        }
        private void Armor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Image newimage = (Image)sender;
                if (isSelectedItem)
                {
                    if (lastImage.Name.Split('_')[1] == GameItem.ItemCategory.Armor.ToString())
                    {
                        SwapImage(newimage);
                        _gameSession.mainStats.CurrentArmor = _gameSession._standardGameItems.First(x => x.Name == newimage.Name.Split('_')[0]);
                    }
                    else
                        UnselectItemInInventory();
                }
                else
                    SelectItemInInventory(newimage);
            }
            catch
            {
                MessageBox.Show("gay");
            }
        }
    }
}
