using DynamicWallPaper.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DynamicWallPaper.View
{
    /// <summary>
    /// DynaicThemeWallpaper.xaml 的交互逻辑
    /// </summary>
    public partial class DynaicThemeWallpaper : Window
    {
        private MediaElement myPlayer;
        public DynaicThemeWallpaper()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;

            myPlayer = media;
            myPlayer.Margin = new Thickness(0, 0, 0, 0);

            this.GoFullscreen();

            myPlayer.UnloadedBehavior = MediaState.Manual;

            this.Left = 0;
            this.Top = 0;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
        }


        public void ChangeSource(Uri uri)
        {
            myPlayer.Stop();
            myPlayer.Source = uri;
            myPlayer.Play();
            this.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 改变指定子窗口的父窗口
        /// </summary>
        /// <param name="hwndChild">子窗口句柄</param>
        /// <param name="newParent">新的父窗口句柄 如果该参数是NULL，则桌面窗口就成为新的父窗口</param>
        /// <returns>如果函数成功，返回值为子窗口的原父窗口句柄；如果函数失败，返回值为NULL</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hwndChild, IntPtr newParent);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr thisIntPtr = new WindowInteropHelper(this).Handle;
            SetParent(thisIntPtr, Win32Helper.GetWorkerW());
            myPlayer.Play();
            this.Visibility = Visibility.Visible;   
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            myPlayer.Width = ActualWidth;
            myPlayer.Height = ActualHeight;
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            myPlayer.Position = TimeSpan.Zero;
            myPlayer.Play();
        }
    }
}
