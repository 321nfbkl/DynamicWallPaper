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
using System.Windows.Media.Media3D;

namespace DynamicWallPaper.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        #region Properties

        private string mSelectedPath= "pack://application:,,,/DynamicWallPaper;component/Image/nopic.bmp";
        /// <summary>
        /// ѡ�е�ͼƬ
        /// </summary>
        public string SelectedPath
        {
            get => this.mSelectedPath;
            set => Set(ref this.mSelectedPath, value);
        }

        private string mSelectedAdapt;
        /// <summary>
        /// ѡ�е����϶�
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
        /// ���϶��б�
        /// </summary>
        public IList<string> AdaptList { get; set; } = new ObservableCollection<string>();
        #endregion

        #region Command
        public ICommand SelectedPicCommand { get; set; }

        public ICommand ApplyPicCommand { get; set; }
        #endregion


        public MainViewModel()
        {
            this.SelectedPicCommand = new RelayCommand(SelectedPic);
            this.ApplyPicCommand = new RelayCommand(ApplyPic);
            this.AdaptList.Add("����");
            this.AdaptList.Add("����");
            this.AdaptList.Add("ƽ��");
        }

        /// <summary>
        /// Ӧ�ñ�ֽ
        /// </summary>
        private void ApplyPic()
        {
            SystemParametersInfo(20, 1, SelectedPath, 1);
        }

        /// <summary>
        /// ѡ���ֽ
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
        /// ��Ӧ��ʽ
        /// </summary>
        /// <param name="value"></param>
        private void AdaptScreen(string value)
        {
            RegistryKey TRegKey;
            TRegKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);
            if (value == "����")
            {
                //ͨ���޸�ע���ʵ��
                if (TRegKey != null)
                {
                    TRegKey.SetValue("WallpaperStyle", "0");
                    TRegKey.SetValue("TileWallpaper", "0");
                }
            }
            else if (value == "����")
            {
                if (TRegKey != null)
                {
                    TRegKey.SetValue("WallpaperStyle", "2");
                    TRegKey.SetValue("TileWallpaper", "0");
                }
            }
            else if (value == "ƽ��")
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