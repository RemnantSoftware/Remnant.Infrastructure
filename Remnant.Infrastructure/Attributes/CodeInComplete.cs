using System;

namespace Remnant.Core.Attributes
{
  /// <summary>
  /// Use this attribute to specify incomplete code
  /// </summary>
  public class CodeIncomplete : Attribute
  {
    public CodeIncomplete()
    {      
    }

    public CodeIncomplete(string link)
    {
      Link = link;
    }

    /// <summary>
    /// A reference to what is required or outstanding
    /// </summary>
    public string Link { get; set;}
  }
}
