namespace Remnant.Core.Common
{
	public class NameValue<TValue>
	{
		public NameValue()
		{			
		}

		public NameValue(string name, TValue value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; set; }
		public TValue Value { get; set; }
	}
}
