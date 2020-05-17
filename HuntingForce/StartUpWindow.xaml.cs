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
using System.Windows.Shapes;

namespace HuntingForce
{
    /// <summary>
    /// Логика взаимодействия для StartUpWindow.xaml
    /// </summary>
    public partial class StartUpWindow : Window
    {
        public StartUpWindow()
        {
            InitializeComponent();
            //myMediaElement.Play();
        }

        private void back_MediaEnded(object sender, RoutedEventArgs e)
        {
            //myMediaElement.Position = TimeSpan.Zero;
            //myMediaElement.Play();
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            Image image;
            TextBlock textBlock;
            Grid grid;
            if (sender is Image)
            {
                image = (Image)sender;
                grid = (Grid)image.Parent;
                textBlock = (TextBlock)grid.Children[1];
            }
            else
            {
                textBlock = (TextBlock)sender;
                grid = (Grid)textBlock.Parent;
                image = (Image)grid.Children[0];

            }
            image.Source = new BitmapImage(new Uri(@"\Resources\StartUp\Buttons\new_button_white.png", UriKind.Relative));
            textBlock.Opacity = 1;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Image image;
            TextBlock textBlock;
            Grid grid;
            if (sender is Image)
            {
                image = (Image)sender;
                grid = (Grid)image.Parent;
                textBlock = (TextBlock)grid.Children[1];
            }
            else
            {
                textBlock = (TextBlock)sender;
                grid = (Grid)textBlock.Parent;
                image = (Image)grid.Children[0];

            }
            image.Source = new BitmapImage(new Uri(@"\Resources\StartUp\Buttons\new_button_black.png", UriKind.Relative));
            textBlock.Opacity = 0.8;
        }
        private void Image2_MouseMove(object sender, MouseEventArgs e)
        {
            Image image;
            TextBlock textBlock;
            Grid grid;
            if (sender is Image)
            {
                image = (Image)sender;
                grid = (Grid)image.Parent;
                textBlock = (TextBlock)grid.Children[1];
            }
            else
            {
                textBlock = (TextBlock)sender;
                grid = (Grid)textBlock.Parent;
                image = (Image)grid.Children[0];

            }
            image.Source = new BitmapImage(new Uri(@"\Resources\StartUp\Buttons\new_button2_white.png", UriKind.Relative));
            textBlock.Opacity = 1;
        }

        private void Image2_MouseLeave(object sender, MouseEventArgs e)
        {
            Image image;
            TextBlock textBlock;
            Grid grid;
            if (sender is Image)
            {
                image = (Image)sender;
                grid = (Grid)image.Parent;
                textBlock = (TextBlock)grid.Children[1];
            }
            else
            {
                textBlock = (TextBlock)sender;
                grid = (Grid)textBlock.Parent;
                image = (Image)grid.Children[0];

            }
            image.Source = new BitmapImage(new Uri(@"\Resources\StartUp\Buttons\new_button2_black.png", UriKind.Relative));
            textBlock.Opacity = 0.8;
        }
        private void Image3_MouseMove(object sender, MouseEventArgs e)
        {
            Image image;
            TextBlock textBlock;
            Grid grid;
            if (sender is Image)
            {
                image = (Image)sender;
                grid = (Grid)image.Parent;
                textBlock = (TextBlock)grid.Children[1];
            }
            else
            {
                textBlock = (TextBlock)sender;
                grid = (Grid)textBlock.Parent;
                image = (Image)grid.Children[0];

            }
            image.Source = new BitmapImage(new Uri(@"\Resources\StartUp\Buttons\new_button3_white.png", UriKind.Relative));
            textBlock.Opacity = 1;
        }

        private void Image3_MouseLeave(object sender, MouseEventArgs e)
        {
            Image image;
            TextBlock textBlock;
            Grid grid;
            if (sender is Image)
            {
                image = (Image)sender;
                grid = (Grid)image.Parent;
                textBlock = (TextBlock)grid.Children[1];
            }
            else
            {
                textBlock = (TextBlock)sender;
                grid = (Grid)textBlock.Parent;
                image = (Image)grid.Children[0];
                
            }
            image.Source = new BitmapImage(new Uri(@"\Resources\StartUp\Buttons\new_button3_black.png", UriKind.Relative));
            textBlock.Opacity = 0.8;
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Play_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
