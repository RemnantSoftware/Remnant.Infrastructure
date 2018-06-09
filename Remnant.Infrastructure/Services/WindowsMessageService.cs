using System;
using System.Runtime.InteropServices;

namespace Remnant.Core.Services
{
	[StructLayout(LayoutKind.Sequential)]
	struct MouseInput
	{
		public int dx;
		public int dy;
		public int mouseData;
		public int dwFlags;
		public int time;
		public IntPtr dwExtraInfo;
	}

	[StructLayout(LayoutKind.Sequential)]
	struct KeyboardInput
	{
		public short wVk;      //Virtual KeyCode (not needed here)
		public short wScan;    //Directx Keycode 
		public int dwFlags;    //This tells you what is use (Keyup, Keydown..)
		public int time;
		public IntPtr dwExtraInfo;
	}

	[StructLayout(LayoutKind.Sequential)]
	struct HardwareInput
	{
		public int uMsg;
		public short wParamL;
		public short wParamH;
	}

	[StructLayout(LayoutKind.Explicit)]
	struct Input
	{
		[FieldOffset(0)]
		public int type;
		[FieldOffset(4)]
		public MouseInput mi;
		[FieldOffset(4)]
		public KeyboardInput ki;
		[FieldOffset(4)]
		public HardwareInput hi;
	}

	[Flags]
	public enum KeyFlag
	{		
		KeyDown = 0x0000,
		ExtendedKey = 0x0001,
		KeyUp = 0x0002,
		UniCode = 0x0004,
		ScanCode = 0x0008
	}

	public static class WindowsMessageService
	{
		[DllImport("user32.dll")]
		private static extern UInt32 SendInput(UInt32 nInputs,[MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] Input[] pInputs, Int32 cbSize);

		public static void SendInput(short keycode, KeyFlag keyFlag)
		{
			var inputData = new Input[1];

			inputData[0].type = 1;
			inputData[0].ki.wScan = keycode;
			inputData[0].ki.dwFlags = (int)keyFlag;
			inputData[0].ki.time = 0;
			inputData[0].ki.dwExtraInfo = IntPtr.Zero;

			SendInput(1, inputData, Marshal.SizeOf(typeof(Input)));
		}

		public static void SendKey(short keyCode, KeyFlag keyFlag)
		{
			SendInput(keyCode, keyFlag | KeyFlag.ScanCode);
		}
	}
}
