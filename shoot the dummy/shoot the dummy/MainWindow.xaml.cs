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
using System.Windows.Threading;

namespace shoot_the_dummy
{
    public partial class MainWindow : Window
    {
        ImageBrush backgroundImage = new ImageBrush(); 
        ImageBrush ghostSprite = new ImageBrush(); 
        ImageBrush aimImage = new ImageBrush(); 

        DispatcherTimer DummyMoveTimer = new DispatcherTimer(); 
        DispatcherTimer showGhostTimer = new DispatcherTimer(); 

        int topCount = 0; 
        int bottomCount = 0; 

        int score; 
        int miss; 

        List<int> topLocations; 
        List<int> bottomLocations; 

        List<Rectangle> removeThis = new List<Rectangle>(); 

        Random rand = new Random(); 

        public MainWindow()
        {
            InitializeComponent();

            this.Cursor = Cursors.None; 
                       
            backgroundImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/background.png"));
            MyCanvas.Background = backgroundImage;

            
            scopeImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/sniper-aim.png"));

            
            ghostSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/ghost.png"));

            
            DummyMoveTimer.Tick += DummyMoveTick;
            DummyMoveTimer.Interval = TimeSpan.FromMilliseconds(rand.Next(800, 2000));
            DummyMoveTimer.Start();

            
            showGhostTimer.Tick += ghostAnimation;
            showGhostTimer.Interval = TimeSpan.FromMilliseconds(20);
            showGhostTimer.Start();


            
            topLocations = new List<int> { 23, 270, 540, 23, 270, 540 };

            
            bottomLocations = new List<int> { 138, 128, 678, 138, 128, 678 };
        }

        private void ShootDummy(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle)
            {
                
                Rectangle activeRec = (Rectangle)e.OriginalSource; 

                MyCanvas.Children.Remove(activeRec); 

                score++; 

                if ((string)activeRec.Tag == "top")
                {
                    
                    topCount--;
                }
                else if ((string)activeRec.Tag == "bottom")
                {
                    
                    bottomCount--;
                }

                
                Rectangle ghostRec = new Rectangle
                {
                    Width = 60,
                    Height = 100,
                    Fill = ghostSprite,
                    Tag = "ghost"
                };

                
                Canvas.SetLeft(ghostRec, Mouse.GetPosition(MyCanvas).X - 40); 
                Canvas.SetTop(ghostRec, Mouse.GetPosition(MyCanvas).Y - 60); 

                MyCanvas.Children.Add(ghostRec); 
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point position = e.GetPosition(this);
            double pX = position.X;
            double pY = position.Y;

            Canvas.SetLeft(scopeImage, pX - (scopeImage.Width / 2));
            Canvas.SetTop(scopeImage, pY - (scopeImage.Height / 2));
        }

        private void ghostAnimation(object sender, EventArgs e)
        {
            scoreText.Content = "Pont: " + score; 
            missText.Content = "Tévesztés: " + miss; 

                        
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                
                if ((string)x.Tag == "ghost")
                {
                    
                    Canvas.SetTop(x, Canvas.GetTop(x) - 5);

                    if (Canvas.GetTop(x) < -180)
                    {
                        
                        removeThis.Add(x);

                    }
                }
            }

           
            foreach (Rectangle y in removeThis)
            {
                MyCanvas.Children.Remove(y);
            }
        }

        private void DummyMoveTick(object sender, EventArgs e)
        {
            removeThis.Clear(); 

            
            foreach (var i in MyCanvas.Children.OfType<Rectangle>())
            {
                
                if ((string)i.Tag == "top" || (string)i.Tag == "bottom")
                {
                    removeThis.Add(i); 

                    topCount--; 
                    bottomCount--; 
                    miss++; 
                }
            }


            
            if (topCount < 3)
            {
                
                ShowDummies(topLocations[rand.Next(0, 5)], 35, rand.Next(1, 4), "top");
                topCount++; 
            }


            if (bottomCount < 3)
            {
                
                ShowDummies(bottomLocations[rand.Next(0, 5)], 230, rand.Next(1, 4), "bottom");
                bottomCount++; 
            }
        }

        private void ShowDummies(int x, int y, int skin, string tag)
        {
            ImageBrush dummyBackground = new ImageBrush();

            
            switch (skin)
            {
                case 1:
                    dummyBackground.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dummy01.png"));
                    break;
                case 2:
                    dummyBackground.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dummy02.png"));
                    break;
                case 3:
                    dummyBackground.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dummy03.png"));
                    break;
                case 4:
                    dummyBackground.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dummy04.png"));
                    break;
            }

            Rectangle newRec = new Rectangle
            {
                Tag = tag,
                Width = 80,
                Height = 155,
                Fill = dummyBackground
            };

            Canvas.SetTop(newRec, y); 
            Canvas.SetLeft(newRec, x); 

            MyCanvas.Children.Add(newRec);

        }
    }
}
