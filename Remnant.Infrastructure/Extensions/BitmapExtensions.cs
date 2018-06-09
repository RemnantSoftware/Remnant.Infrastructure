#if (!NETCOREAPP1_0 && !NETCOREAPP1_1 && !NETCOREAPP2_0 && !NETCOREAPP2_1)
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Remnant.Core.Extensions
{
	public static class BitmapExtensions
	{
		public static byte[] ToByteArray(this Bitmap source)
		{
			var memoryStream = new MemoryStream();
			source.Save(memoryStream, ImageFormat.Jpeg);
			return memoryStream.ToArray();
		}
	}
}
#endif