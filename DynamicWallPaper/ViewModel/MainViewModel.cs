using DynamicWallPaper.Helper;
using DynamicWallPaper.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Media3D;

namespace DynamicWallPaper.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        #region Properties


        private string mCurrentAudioPath=string.Empty;

        private static NotifyIcon trayIcon;

        private DynaicThemeWallpaper ThemeWallpaper = new DynaicThemeWallpaper();

        private bool mIsPlay = true;

        private string mSelectedPath= "pack://application:,,,/DynamicWallPaper;component/Image/nopic.bmp";
        /// <summary>
        /// 选中的图片
        /// </summary>
        public string SelectedPath
        {
            get => this.mSelectedPath;
            set => Set(ref this.mSelectedPath, value);
        }

        private string mSelectedAdapt;
        /// <summary>
        /// 选中的契合度
        /// </summary>
        public string SelectedAdapt
        {
            get => this.mSelectedAdapt;
            set
            {
                if (Set(ref this.mSelectedAdapt, value))
                {
                    AdaptScreen(value);
                }
            }
        }

        /// <summary>
        /// 契合度列表
        /// </summary>
        public IList<string> AdaptList { get; set; } = new ObservableCollection<string>();
        #endregion

        #region Command
        public ICommand SelectedPicCommand { get; set; }

        public ICommand ApplyPicCommand { get; set; }

        public ICommand ApplyDynaicPicCommand { get; set; }

        public ICommand SelectedDynaicPicCommand { get; set; }
        #endregion


        public MainViewModel()
        {
            this.SelectedPicCommand = new RelayCommand(SelectedPic);
            this.ApplyPicCommand = new RelayCommand(ApplyPic);
            this.SelectedDynaicPicCommand = new RelayCommand(SelectedDynaicPic);
            this.ApplyDynaicPicCommand = new RelayCommand(ApplyDynaicPic);
            this.AdaptList.Add("居中");
            this.AdaptList.Add("拉伸");
            this.AdaptList.Add("平铺");
        }

        /// <summary>
        /// 应用动态壁纸
        /// </summary>
        private void ApplyDynaicPic()
        {
            if (mCurrentAudioPath != null)
            {

                ThemeWallpaper.ChangeSource(new Uri(mCurrentAudioPath));
            }
        }

        /// <summary>
        /// 选择动态壁纸
        /// </summary>
        private void SelectedDynaicPic()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "视频|*.mp4;*.wmv";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog()==true)
            {
                mCurrentAudioPath = openFileDialog.FileName;
                App.mMainView.media.Stop();
                App.mMainView.media.Source = new Uri(mCurrentAudioPath);
                App.mMainView.media.Play();
                mIsPlay = true;
            }
        }

        /// <summary>
        /// 应用壁纸
        /// </summary>
        private void ApplyPic()
        {
            SystemParametersInfo(20, 1, SelectedPath, 1);
            ThemeWallpaper.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 选择壁纸
        /// </summary>
        private void SelectedPic()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                this.SelectedPath = openFileDialog.FileName;
            }
        }

        /// <summary>
        /// 适应方式
        /// </summary>
        /// <param name="value"></param>
        private void AdaptScreen(string value)
        {
            RegistryKey TRegKey;
            TRegKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);
            if (value == "居中")
            {
                //通过修改注册表实现
                if (TRegKey != null)
                {
                    TRegKey.SetValue("WallpaperStyle", "0");
                    TRegKey.SetValue("TileWallpaper", "0");
                }
            }
            else if (value == "拉伸")
            {
                if (TRegKey != null)
                {
                    TRegKey.SetValue("WallpaperStyle", "2");
                    TRegKey.SetValue("TileWallpaper", "0");
                }
            }
            else if (value == "平铺")
            {
                if (TRegKey != null)
                {
                    TRegKey.SetValue("WallpaperStyle", "1");
                    TRegKey.SetValue("TileWallpaper", "1");
                }
            }
        }
    }
}