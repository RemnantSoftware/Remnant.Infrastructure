using System;

namespace Remnant.Core.Commands
{
	/// <summary>
	/// Derive from this abstract result command class to implement your command which returns a result
	/// </summary>
	public class OutputCommand<TOutput> : Command
	{

		public new virtual TOutput Execute()
		{
			throw new NotImplementedException("Please implement (override) Execute in the derived class");
		}
	
	}
}
