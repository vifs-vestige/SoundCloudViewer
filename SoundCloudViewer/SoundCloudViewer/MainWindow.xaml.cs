using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SoundCloudViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static String SongName = "";
        public static String SongInfo = "";
        private SoundCloud SoundCloud;
        private int MainWidth = 292;

        public MainWindow()
        {
            InitializeComponent();

            SoundCloud = new SoundCloud();
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(Loop);
            timer.Start();
        }



        private void StartLoopingName()
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = canMain.ActualWidth;
            doubleAnimation.To = -txtSongName.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:8"));
            txtSongName.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
        }

        private void EndLoopingName()
        {
            txtSongName.BeginAnimation(Canvas.LeftProperty, null);
            txtSongName.Width = MainWidth;
        }

        private void StartLoopingInfo()
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = canMain.ActualWidth;
            doubleAnimation.To = -txtSongInfo.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:8"));
            txtSongInfo.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
        }

        private void EndLoopingInfo()
        {
            txtSongInfo.BeginAnimation(Canvas.LeftProperty, null);
            txtSongInfo.Width = MainWidth;
        }

        private void Loop(object sender, EventArgs e)
        {
            SoundCloud.GetInfo();
            UpdateUI();
        }
        

        private void UpdateUI()
        {
            if (SongName != "error" && SongInfo != "error")
            {
                var tempName = txtSongName.Text;
                var tempInfo = txtSongInfo.Text;
                if (tempName != SongName || tempInfo != SongInfo)
                {
                    SoundCloud.PrintSongInfo();
                    EndLoopingInfo();
                    EndLoopingName();
                    txtSongName.Text = SongName;
                    txtSongInfo.Text = SongInfo;
                    if (SongName.Count() > 30)
                    {
                        txtSongName.Width = SongName.Count() * 9.7;
                        StartLoopingName();
                    }
                    if (SongInfo.Count() > 39)
                    {
                        txtSongInfo.Width = SongInfo.Count() * 7.6;
                        StartLoopingInfo();
                    }
                }
            }
        }
    }
}