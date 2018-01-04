using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using HearthstoneDeckTracker.Utilities;

namespace HearthstoneDeckTracker.Tracker
{
    public static class Screenshot
    {
        [DllImport("user32.dll")]
        private static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [StructLayout(LayoutKind.Sequential)]

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static void GetScreenshot()
        {
            var process = Process.GetProcessesByName("Hearthstone").FirstOrDefault();

            if (process != null)
            {
                if (SetForegroundWindow(process.MainWindowHandle))
                {
                    RECT srcRect;

                    if (!process.MainWindowHandle.Equals(IntPtr.Zero))
                    {
                        if (GetWindowRect(process.MainWindowHandle, out srcRect))
                        {
                            int width = srcRect.Right - srcRect.Left;
                            int height = srcRect.Bottom - srcRect.Top;

                            Bitmap bmp = new Bitmap(width, height);
                            Graphics screenG = Graphics.FromImage(bmp);

                            try
                            {
                                screenG.CopyFromScreen(srcRect.Left, srcRect.Top, 0, 0, new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);

                                bmp.Save("HS.jpg", ImageFormat.Jpeg);
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex);
                            }
                            finally
                            {
                                screenG.Dispose();
                                bmp.Dispose();
                            }
                        }
                    }
                }
            }
        }
    }
}
