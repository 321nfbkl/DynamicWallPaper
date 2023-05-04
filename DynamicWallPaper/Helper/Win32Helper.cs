using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace DynamicWallPaper.Helper
{
    public static class Win32Helper
    {
        /// <summary>
        ///  查找顶级窗口句柄
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="titleName">标题</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string className, string titleName);

        /// <summary>
        ///    查找子窗口句柄
        /// </summary>
        /// <param name="hwndParent">要查找窗口的父句柄</param>
        /// <param name="hwndChildAfter">从这个窗口后开始查找</param>
        /// <param name="className">窗口类名</param>
        /// <param name="title">窗口标题</param>
        /// <returns>找到返回窗口句柄，没找到返回0</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string className, string title);

        /// <summary>
        /// 枚举窗口
        /// </summary>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

        /// <summary>
        /// 改变指定子窗口的父窗口
        /// </summary>
        /// <param name="hwndChild">子窗口句柄</param>
        /// <param name="newParent">新的父窗口句柄 如果该参数是NULL，则桌面窗口就成为新的父窗口</param>
        /// <returns>如果函数成功，返回值为子窗口的原父窗口句柄；如果函数失败，返回值为NULL</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hwndChild, IntPtr newParent);

        /// <summary>
        /// 显示窗口异步
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">显示方式</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);
        public const int SW_SHOW = 5;
        public const int SW_HIDE = 0;
        public const int WS_SHOWNORMAL = 1;

        /// <summary>
        /// 激活窗口
        /// </summary>
        /// <param name="hWnd">激活窗口</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 销毁一个窗口
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int DestroyWindow(IntPtr hWnd);



        /// <summary>
        /// 该函数返回指定窗口的边框矩形的尺寸。该尺寸以相对于屏幕坐标左上角的屏幕坐标给出
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="rect">指向一个RECT结构的指针，该结构接收窗口的左上角和右下角的屏幕坐标</param>
        [DllImport("user32.dll")]
        public static extern void GetWindowRect(IntPtr hwnd, ref Rectangle rect);

        [DllImport("user32.dll")]
        public static extern void SetWindowPos(IntPtr hWnd, IntPtr hWndlnsertAfter, int x, int y, int cx, int cy, uint flag);
        public const int HWND_TOP = 0; // 在前面
        public const int HWND_BOTTOM = 1; // 在后面
        public const int HWND_TOPMOST = -1; // 在前面, 位于任何顶部窗口的前面
        public const int HWND_NOTOPMOST = -2; // 在前面, 位于其他顶部窗口的后面}


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_ABORTIFHUNG = 2,
            SMTO_BLOCK = 1,
            SMTO_ERRORONEXIT = 0x20,
            SMTO_NORMAL = 0,
            SMTO_NOTIMEOUTIFNOTHUNG = 8
        }

        /// <summary>
        /// 获取窗口句柄
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetWorkerW()
        {
            // 获取
            IntPtr windowHandle = FindWindow("Progman", null);

            IntPtr zero;
            // 重要消息 生成一个WorkerW 顶级窗口 桌面列表会随之搬家
            SendMessageTimeout(windowHandle, 0x52c, new IntPtr(0), IntPtr.Zero, SendMessageTimeoutFlags.SMTO_NORMAL, 0x3e8, out zero);
            IntPtr workerw = IntPtr.Zero;
            // 消息会生成两个WorkerW 顶级窗口 所以要枚举不包含“SHELLDLL_DefView”这个的 WorkerW 窗口 隐藏掉。
            EnumWindows(delegate (IntPtr tophandle, IntPtr topparamhandle)
            {
                if (FindWindowEx(tophandle, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
                {
                    workerw = FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", null);
                }
                return true;
            }, IntPtr.Zero);
            ShowWindow(workerw, SW_HIDE);
            return windowHandle;
        }

    }
}
