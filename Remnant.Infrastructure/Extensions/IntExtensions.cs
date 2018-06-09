
namespace Remnant.Core.Extensions
{
  public static class IntExtensions
  {
    public static string ToBracketQuoted(this int source)
    {
			return source.ToString().ToBracketQuoted();
    }	
  }
}
