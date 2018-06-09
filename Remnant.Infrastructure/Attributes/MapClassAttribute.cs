using System;

namespace Remnant.Core.Attributes
{
  /// <summary>
  /// Use this attribute to map a class name to a member
  /// </summary>
  public class MapClassAttribute : Attribute
  {
    public MapClassAttribute(Type classType)
    {
      ClassType = classType;
    }

    /// <summary>
    /// Type defined to map member to a class
    /// </summary>
    public Type ClassType { get; set;}
  }
}
