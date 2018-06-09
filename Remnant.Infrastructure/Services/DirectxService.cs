using System;
using System.Collections.Generic;

namespace Remnant.Core.Services
{

	public class DirectxKey
	{
		public string Key { get; set; }
		public int Decimal { get; set; }
		public short Hex { get; set; }
	}


	public static class DirectxService
	{
		private static readonly List<DirectxKey> _keys;

		static DirectxService()
		{
			_keys = new List<DirectxKey>
				{
					new DirectxKey {Key = "[", Decimal = 26, Hex = 0x1A},
					new DirectxKey {Key = "'", Decimal = 40, Hex = 0x28},
					new DirectxKey {Key = "]", Decimal = 27, Hex = 0x1B},
					new DirectxKey {Key = "/", Decimal = 53, Hex = 0x35},
					new DirectxKey {Key = ";", Decimal = 39, Hex = 0x27},
					new DirectxKey {Key = ".", Decimal = 52, Hex = 0x34},
					new DirectxKey {Key = "-", Decimal = 12, Hex = 0x0C},
					new DirectxKey {Key = @"\", Decimal = 43, Hex = 0x2B},
					new DirectxKey {Key = ",", Decimal = 51, Hex = 0x33},
					new DirectxKey {Key = "=", Decimal = 13, Hex = 0x0D},
					new DirectxKey {Key = "`", Decimal = 41, Hex = 0x29},
					new DirectxKey {Key = "0", Decimal = 11, Hex = 0x0B},
					new DirectxKey {Key = "1", Decimal = 2, Hex = 0x02},
					new DirectxKey {Key = "2", Decimal = 3, Hex = 0x03},
					new DirectxKey {Key = "3", Decimal = 4, Hex = 0x04},
					new DirectxKey {Key = "4", Decimal = 5, Hex = 0x05},
					new DirectxKey {Key = "5", Decimal = 6, Hex = 0x06},
					new DirectxKey {Key = "6", Decimal = 7, Hex = 0x07},
					new DirectxKey {Key = "7", Decimal = 8, Hex = 0x08},
					new DirectxKey {Key = "8", Decimal = 9, Hex = 0x09},
					new DirectxKey {Key = "9", Decimal = 10, Hex = 0x0A},
					new DirectxKey {Key = "A", Decimal = 30, Hex = 0x1E},
					new DirectxKey {Key = "B", Decimal = 48, Hex = 0x38},
					new DirectxKey {Key = "C", Decimal = 14, Hex = 0x2E},
					new DirectxKey {Key = "CAPSLOCK", Decimal = 58, Hex = 0x3A},
					new DirectxKey {Key = "D", Decimal = 32, Hex = 0x20},
					new DirectxKey {Key = "DOWN", Decimal = 208, Hex = 0xD0},
					new DirectxKey {Key = "E", Decimal = 18, Hex = 0x12},
					new DirectxKey {Key = "ENTER", Decimal = 28, Hex = 0x1C},
					new DirectxKey {Key = "ESCAPE", Decimal = 1, Hex = 0x01},
					new DirectxKey {Key = "F", Decimal = 33, Hex = 0x21},
					new DirectxKey {Key = "F1", Decimal = 059, Hex = 0x3B},
					new DirectxKey {Key = "F10", Decimal = 068, Hex = 0x44},
					new DirectxKey {Key = "F11", Decimal = 087, Hex = 0x57},
					new DirectxKey {Key = "F12", Decimal = 088, Hex = 0x58},
					new DirectxKey {Key = "F2", Decimal = 060, Hex = 0x3C},
					new DirectxKey {Key = "F3", Decimal = 061, Hex = 0x3D},
					new DirectxKey {Key = "F4", Decimal = 062, Hex = 0x3E},
					new DirectxKey {Key = "F5", Decimal = 063, Hex = 0x3F},
					new DirectxKey {Key = "F6", Decimal = 064, Hex = 0x40},
					new DirectxKey {Key = "F7", Decimal = 065, Hex = 0x41},
					new DirectxKey {Key = "F8", Decimal = 066, Hex = 0x42},
					new DirectxKey {Key = "F9", Decimal = 067, Hex = 0x43},
					new DirectxKey {Key = "G", Decimal = 034, Hex = 0x22},
					new DirectxKey {Key = "H", Decimal = 035, Hex = 0x23},
					new DirectxKey {Key = "I", Decimal = 023, Hex = 0x17},
					new DirectxKey {Key = "J", Decimal = 036, Hex = 0x24},
					new DirectxKey {Key = "K", Decimal = 037, Hex = 0x25},
					new DirectxKey {Key = "L", Decimal = 038, Hex = 0x26},
					new DirectxKey {Key = "LEFTALT", Decimal = 056, Hex = 0x38},
					new DirectxKey {Key = "LEFTARROW", Decimal = 203, Hex = 0xCB},
					new DirectxKey {Key = "LEFTCTRL", Decimal = 029, Hex = 0x1D},
					new DirectxKey {Key = "LEFTMB", Decimal = 256, Hex = 0x100},
					new DirectxKey {Key = "LEFTSHIFT", Decimal = 042, Hex = 0x2A},
					new DirectxKey {Key = "M", Decimal = 050, Hex = 0x32},
					new DirectxKey {Key = "MIDDLEMB", Decimal = 258, Hex = 0x102},
					new DirectxKey {Key = "MB3", Decimal = 259, Hex = 0x103},
					new DirectxKey {Key = "MB4", Decimal = 260, Hex = 0x104},
					new DirectxKey {Key = "MB5", Decimal = 261, Hex = 0x105},
					new DirectxKey {Key = "MB6", Decimal = 262, Hex = 0x106},
					new DirectxKey {Key = "MB7", Decimal = 263, Hex = 0x107},
					new DirectxKey {Key = "MOUSEWHEELDOWN", Decimal = 265, Hex = 0x109},
					new DirectxKey {Key = "MOUSEWHEELUP", Decimal = 264, Hex = 0x108},
					new DirectxKey {Key = "N", Decimal = 049, Hex = 0x31},
					new DirectxKey {Key = "NUM*", Decimal = 055, Hex = 0x37},
					new DirectxKey {Key = "NUM-", Decimal = 074, Hex = 0x4A},
					new DirectxKey {Key = "NUM/", Decimal = 181, Hex = 0xB5},
					new DirectxKey {Key = "NUM.", Decimal = 083, Hex = 0x53},
					new DirectxKey {Key = "NUM+", Decimal = 078, Hex = 0x4E},
					new DirectxKey {Key = "NUM0", Decimal = 082, Hex = 0x52},
					new DirectxKey {Key = "NUM1", Decimal = 079, Hex = 0x4F},
					new DirectxKey {Key = "NUM2", Decimal = 080, Hex = 0x50},
					new DirectxKey {Key = "NUM3", Decimal = 081, Hex = 0x52},
					new DirectxKey {Key = "NUM4", Decimal = 075, Hex = 0x4B},
					new DirectxKey {Key = "NUM5", Decimal = 076, Hex = 0x4C},
					new DirectxKey {Key = "NUM6", Decimal = 077, Hex = 0x4D},
					new DirectxKey {Key = "NUM7", Decimal = 071, Hex = 0x48},
					new DirectxKey {Key = "NUM8", Decimal = 072, Hex = 0x47},
					new DirectxKey {Key = "NUM9", Decimal = 073, Hex = 0x49},
					new DirectxKey {Key = "NUMENTER", Decimal = 156, Hex = 0x9C},
					new DirectxKey {Key = "NUMLOCK", Decimal = 069, Hex = 0x45},
					new DirectxKey {Key = "O", Decimal = 024, Hex = 0x18},
					new DirectxKey {Key = "P", Decimal = 025, Hex = 0x19},
					new DirectxKey {Key = "Q", Decimal = 016, Hex = 0x10},
					new DirectxKey {Key = "R", Decimal = 019, Hex = 0x13},
					new DirectxKey {Key = "RIGHTALT", Decimal = 184, Hex = 0xB8},
					new DirectxKey {Key = "RIGHTARROW", Decimal = 205, Hex = 0xCD},
					new DirectxKey {Key = "RIGHTCTRL", Decimal = 157, Hex = 0x9D},
					new DirectxKey {Key = "RIGHTMB", Decimal = 257, Hex = 0x101},
					new DirectxKey {Key = "RIGHTSHIFT", Decimal = 054, Hex = 0x36},
					new DirectxKey {Key = "S", Decimal = 031, Hex = 0x1F},
					new DirectxKey {Key = "SCROLLLOCK", Decimal = 070, Hex = 0x46},
					new DirectxKey {Key = "SPACEBAR", Decimal = 057, Hex = 0x39},
					new DirectxKey {Key = "T", Decimal = 020, Hex = 0x14},
					new DirectxKey {Key = "TAB", Decimal = 015, Hex = 0x0F},
					new DirectxKey {Key = "U", Decimal = 022, Hex = 0x16},
					new DirectxKey {Key = "UPARROW", Decimal = 200, Hex = 0xC8},
					new DirectxKey {Key = "V", Decimal = 047, Hex = 0x2F},
					new DirectxKey {Key = "W", Decimal = 017, Hex = 0x11},
					new DirectxKey {Key = "X", Decimal = 045, Hex = 0x2D},
					new DirectxKey {Key = "Y", Decimal = 021, Hex = 0x15},
					new DirectxKey {Key = "Z", Decimal = 044, Hex = 0x2C}
				};
		}

		public static DirectxKey GetDirectxKey(string key)
		{
			var result = _keys.Find(mapKey => mapKey.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
			return result;
		}

		public static List<DirectxKey> Keys { get { return _keys; } }

	}
}

