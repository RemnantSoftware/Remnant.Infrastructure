using System;

namespace Remnant.Core.Commands
{
	/// <summary>
	/// Derive from this command class to implement more specific command types
	/// </summary>
	public class Command : ICommand
	{
		private readonly Action _callback;

		public Command()
		{
			Enabled = true;
		}

		public Command(Action callback)
			: this()
		{
			_callback = callback;
		}


		public virtual void Execute()
		{
		}

		public void OnExecute()
		{
			if (Enabled)
			{
				Execute();
				if (_callback != null)
					_callback();
			}
		}

		public bool Enabled { get; set; }

	}

}
