#if (!NETCOREAPP1_0 && !NETCOREAPP1_1 && !NETCOREAPP2_0 && !NETCOREAPP2_1)
using System.Drawing;

namespace Remnant.Core.Extensions
{
  public static class RectangleExtensions
  {
    public static Point Center(this Rectangle source)
    {
			return new Point(source.X + (source.Width / 2), source.Y + (source.Height / 2));
    }	
  }
}
#endif