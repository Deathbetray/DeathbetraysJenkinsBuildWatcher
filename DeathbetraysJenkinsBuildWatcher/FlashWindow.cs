using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeathbetraysJenkinsBuildWatcher
{
	public static class ExtensionMethods
	{
		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

		/// <summary>
		/// Stop flashing. The system restores the window to its original stae.
		/// </summary>
		private const uint FLASHW_STOP = 0;

		/// <summary>
		/// Flash the window caption.
		/// </summary>
		private const uint FLASHW_CAPTION = 1;

		/// <summary>
		/// Flash the taskbar button.
		/// </summary>
		private const uint FLASHW_TRAY = 2;

		/// <summary>
		/// Flash both the window caption and taskbar button.
		/// This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags.
		/// </summary>
		private const uint FLASHW_ALL = 3;

		/// <summary>
		/// Flash continuously, until the FLASHW_STOP flag is set.
		/// </summary>
		private const uint FLASHW_TIMER = 4;

		/// <summary>
		/// Flash continuously until the window comes to the foreground.
		/// </summary>
		private const uint FLASHW_TIMERNOFG = 12;

		[StructLayout(LayoutKind.Sequential)]
		private struct FLASHWINFO
		{
			/// <summary>
			/// The size of the structure in bytes.
			/// </summary>
			public uint cbSize;
			/// <summary>
			/// A Handle to the Window to be Flashed. The window can be either opened or minimized.
			/// </summary>
			public IntPtr hwnd;
			/// <summary>
			/// The Flash Status.
			/// </summary>
			public uint dwFlags;
			/// <summary>
			/// The number of times to Flash the window.
			/// </summary>
			public uint uCount;
			/// <summary>
			/// The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.
			/// </summary>
			public uint dwTimeout;
		}

		public static bool FlashStart(this Form _form, uint _count = uint.MaxValue)
		{
			return FlashInternal(_form, FLASHW_ALL | FLASHW_TIMERNOFG, _count);
		}

		public static bool FlashStop(this Form _form)
		{
			return FlashInternal(_form, FLASHW_STOP);
		}

		private static bool FlashInternal(this Form _form, uint _flags, uint _count = uint.MaxValue)
		{
			if (!Win2000OrLater)
			{
				return false;
			}

			FLASHWINFO fInfo = new FLASHWINFO();
			fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
			fInfo.hwnd = _form.Handle;
			fInfo.dwFlags = _flags;
			fInfo.uCount = _count;
			fInfo.dwTimeout = 0;

			return FlashWindowEx(ref fInfo);
		}

		private static bool Win2000OrLater
		{
			get { return System.Environment.OSVersion.Version.Major >= 5; }
		}
	}
}
