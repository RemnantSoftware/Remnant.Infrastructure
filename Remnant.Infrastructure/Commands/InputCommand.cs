using System;

namespace Remnant.Core.Commands
{
	/// <summary>
	/// Derive from this abstract result command class to implement your command which returns a result
	/// </summary>
	public class InputCommand<TInput> : Command
	{
		public InputCommand()
		{		
		}

		public virtual void Execute(TInput input)
		{
			throw new NotImplementedException("Implement (override) Execute in derived class");
		}
	
	}
}
