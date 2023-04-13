using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace DynamicWallPaper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            App.mMainView = this;
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        /// <summary>
        /// 程序启动时设置墙纸的显示方式为居中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //通过修改注册表实现
            RegistryKey TRegKey;
            TRegKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);
            if (TRegKey != null)
            {
                TRegKey.SetValue("WallpaperStyle", "0");
                TRegKey.SetValue("TileWallpaper", "0");
            }
        }

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(
          int uAction,
          int uParam,
          string lpvParam,
          int fuWinIni
          );
        private void ApplyPic_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            //openFileDialog.DefaultExt = ".mp4";
            //openFileDialog.Filter = "视频文件(.MP4)|*.mp4;";

            if (openFileDialog.ShowDialog() == true)
            {
                Width = SystemParameters.PrimaryScreenWidth; Height = SystemParameters.PrimaryScreenHeight; Left = 0; Top = 0;
                
                SystemParametersInfo(20, 1, openFileDialog.FileName, 1);
            }
            this.WindowState = WindowState.Minimized;
        }

      
    }
}
