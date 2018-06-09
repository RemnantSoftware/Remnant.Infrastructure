namespace Remnant.Core.Common
{
	public class BoolValuePair<TValue>
	{
		public BoolValuePair()
		{			
		}

		public BoolValuePair(bool isTrue, TValue value)
		{
			IsTrue = isTrue;
			Value = value;
		}

		public bool IsTrue { get; set; }
		public TValue Value { get; set; }
	}
}
