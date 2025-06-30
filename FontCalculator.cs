#nullable enable
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinForms.FontSize
{
    public class FontCalculator
    {
        /// <summary>
        /// Get the font size for a certain screen resolution
        /// </summary>
        /// <param name="screen">The screen where a form is showed</param>
        /// <param name="designedHeight">The font heigt used in the WindowsForms designer</param>
        /// <remarks>
        /// You can get the screen with
        /// <code>Screen.FromControl(this)</code>
        /// Where this is the current form.
        /// </remarks>
        /// <returns></returns>
        public static float Calculate(Screen screen, float designedHeight)
        {
            uint dpi;

            GetDpi( screen, DpiType.Effective, out var _, out dpi );
            double ysize;
            GetSize( screen, out _, out ysize );
            int yres;
            GetRes( screen, out _, out yres );
            // Per definition dpi is 96 * Scaling
            // e.g. 144 for 150%
            var actualHeight = ysize / (double)yres * (double)dpi / 96d;
            // Reference Height is for a monitor with 8" height, 960px vertical with 100% scaling
            var refHeight = 8d / 960d;
            var fontSize = designedHeight * refHeight / actualHeight;
            return (float) fontSize;
        }

        public static void GetDpi( Screen screen, DpiType dpiType, out uint dpiX, out uint dpiY )
        {
            var pnt = new System.Drawing.Point( screen.Bounds.Left + 1, screen.Bounds.Top + 1 );
            var mon = MonitorFromPoint( pnt, 2 /*MONITOR_DEFAULTTONEAREST*/ );
            GetDpiForMonitor( mon, dpiType, out dpiX, out dpiY );
        }

        public static void GetSize( Screen screen, out double sizeX, out double sizeY )
        {
            var pnt = new System.Drawing.Point(screen.Bounds.Left + 1, screen.Bounds.Top + 1);
            var hdc = CreateDC(null, screen.DeviceName, null, IntPtr.Zero);
            sizeX = Math.Round( GetDeviceCaps( hdc, (int) DeviceCap.HORZSIZE ) / 25.4, 1 );
            sizeY = Math.Round( GetDeviceCaps( hdc, (int) DeviceCap.VERTSIZE ) / 25.4, 1 );
        }

        public static void GetRes( Screen screen, out int sizeX, out int sizeY )
        {
            var pnt = new System.Drawing.Point(screen.Bounds.Left + 1, screen.Bounds.Top + 1);
            var hdc = CreateDC(null, screen.DeviceName, null, IntPtr.Zero);
            sizeX = GetDeviceCaps( hdc, (int) DeviceCap.HORZRES );
            sizeY = GetDeviceCaps( hdc, (int) DeviceCap.VERTRES );
        }


        [DllImport( "User32.dll" )]
        private static extern IntPtr MonitorFromPoint( [In] System.Drawing.Point pt, [In] uint dwFlags );

        [DllImport( "Shcore.dll" )]
        private static extern IntPtr GetDpiForMonitor( [In] IntPtr hmonitor, [In] DpiType dpiType, [Out] out uint dpiX, [Out] out uint dpiY );

        [DllImport( "gdi32.dll", SetLastError = true )]
        static extern int GetDeviceCaps( IntPtr hdc, int nIndex );

        [DllImport( "gdi32.dll" )]
        static extern IntPtr CreateDC( string? lpszDriver, string lpszDevice, string? lpszOutput, IntPtr lpInitData );

        public enum DpiType
        {
            Effective = 0,
            Angular = 1,
            Raw = 2,
        }

        public enum DeviceCap
        {
            /// <summary>
            /// Horizontal size in millimeters
            /// </summary>
            HORZSIZE = 4,
            /// <summary>
            /// Vertical size in millimeters
            /// </summary>
            VERTSIZE = 6,
            /// <summary>
            /// Horizontal width in pixels
            /// </summary>
            HORZRES = 8,
            /// <summary>
            /// Vertical height in pixels
            /// </summary>
            VERTRES = 10,
        }

    }
}
