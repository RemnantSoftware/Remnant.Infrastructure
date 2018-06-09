using System;

namespace Remnant.Core.Commands
{

	/// <summary>
	///  Derive from this base event command class to implement your command 
	/// when an event is fired including a callback to you with the output
	/// </summary>
	/// <typeparam name="TEventArgs">Specify the type of arguments for event: Ex: EventArgs, RoutedEventArgs</typeparam>
	/// /// <typeparam name="TOutput">Specify the output type for the callback </typeparam>
	public class EventCommand<TEventArgs, TOutput> : OutputCommand<TOutput>
		where TEventArgs : EventArgs
	{
		private readonly Action<TOutput> _callback;
	
		public EventCommand(Action<TOutput> callback) 
		{
			_callback = callback;
		}
	
		public void OnExecute(object sender, TEventArgs args)
		{
			if (Enabled)
				_callback(Execute());
		}		
	}

	
	/// <summary>
	///  Derive from this base event command class to implement your command when an event is fired
	///  Note: If not overridden the execute will do nothing
	/// </summary>
	/// <typeparam name="TEventArgs">Specify the type of arguments for event: Ex: EventArgs, RoutedEventArgs</typeparam>
	public class EventCommand<TEventArgs> : Command
		where TEventArgs : EventArgs
	{
		private readonly Action _callback;
		private readonly Action<TEventArgs> _callbackWithArgs;

		protected EventCommand()
		{			
		}

		public override void Execute()
		{
		}

		public EventCommand(Action callback)
		{
			_callback = callback;
		}

		public EventCommand(Action<TEventArgs> callback)
		{
			_callbackWithArgs = callback;
		}

		public void OnExecute(object sender, TEventArgs args)
		{
			if (Enabled)
			{
				Execute();
				if (_callback != null)
					_callback();
				if (_callbackWithArgs != null)
					_callbackWithArgs(args);
			}
		}
	}
}
