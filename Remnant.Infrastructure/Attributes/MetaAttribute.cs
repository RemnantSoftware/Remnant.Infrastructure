using System;
using System.Text;

namespace Remnant.Core.Attributes
{
	[AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	public class MetaAttribute : Attribute
	{
		private int _length;
		private int _precision;
		private bool _lengthDefined;
		private bool _precisionDefined;
		
		public MetaAttribute()
		{
			IsVisible = true;
		}

		public int Length
		{
			get { return _length; }
			set 
			{ 
				_length = value;
				_lengthDefined = true;
			} 
		}

		public int Precision
		{
			get { return _precision; }
			set
			{
				_precision = value;
				_precisionDefined = true;
			}
		}

		public bool IsLengthDefined { get { return _lengthDefined; } }
		public bool IsPrecisionDefined { get { return _precisionDefined; } }

		public bool IsDateOnly { get; set; }
		public string Format { get; set; }
		public string DisplayName { get; set; }
		public string Description { get; set; }
		public bool IsRequired { get; set; }
		public bool IsNullable { get; set; }
		public bool IsVisible { get; set; }
		public bool IsSensitive { get; set; }
		public bool IsReadOnly { get; set; }
		public bool IsDisabled { get; set; }		
		public bool IsEmail { get; set; }
		public bool IsEnum { get; set; }
		public string DefaultValue { get; set; }
		public MetaType Type { get; set; }
		public bool IgnoreDisplayName { get; set; }

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("[Meta(");

      if (Length !=0 )
			  sb.Append("Length = " + Length + ",");

			if (Precision > 0)
				sb.Append("Precision = " + Precision + ",");

			if (!string.IsNullOrEmpty(DisplayName))
				sb.Append("DisplayName = \"" + DisplayName + "\",");

			if (!string.IsNullOrEmpty(Description))
				sb.Append("Description = \"" + Description + "\",");

			if (!string.IsNullOrEmpty(DefaultValue))
				sb.Append("DefaultValue = \"" + DefaultValue + "\",");

			if (!string.IsNullOrEmpty(Format))
				sb.Append("Format = \"" + Format + "\",");

			if (IsRequired)
				sb.Append("IsRequired = true,");
			
			if (IsRequired)
				sb.Append("IsNullable = true,");

			if (!IsVisible)
				sb.Append("IsVisible = false,");

			if (IsSensitive)
				sb.Append("IsSensitive = true,");

			if (IsReadOnly)
				sb.Append("IsReadOnly = true,");

			if (IsDateOnly)
				sb.Append("IsDateOnly = true,");

			if (IsDisabled)
				sb.Append("IsDisabled = true,");

			if (IsEmail)
				sb.Append("IsEmail = true,");

			if (IsEnum)
				sb.Append("IsEnum = true,");

			if (IgnoreDisplayName)
				sb.Append("IgnoreDisplayName = true,");

			if(sb[sb.Length-1] == ',')
				sb.Length--;

			sb.Append(")]");
			return sb.ToString();
		}
	}
}
