using System;
using System.Collections.Generic;
using Remnant.Core.Creational;
using Remnant.Core.Services;

namespace Remnant.Core.Commands
{

	/// <summary>
	/// Represents all application commands.
	/// </summary>
	public static class CommandManager 
	{

		// <summary>
		// Contains the list of commands that are mapped to events
		// </summary>
		//private readonly Dictionary<ICommand> _commands = new List<BaseCommand>(); // maybe not needed


		//public void MapEventToCommand<TCommand>(Control control, object data = null)
		//  where TCommand : BaseCommand, new()
		//{
		//  Shield.Raise.AgainstNull(control);
		//  //MapEventToCommand<TCommand, Action>(item, null, data);
		//  var command = (BaseCommand)DomainsService.Instance.CreateInstance(typeof(TCommand), data);
		//  control.Click += command.OnExecute;      
		//  //_commands.Add(command);
		//}

		/// <summary>
		/// Map a click event to a command so that the command will be called when the event is fired.
		/// When the command is executed the passed callback is called on the view to process any results etc.
		/// Note: The callback can be any generic Action etc.
		/// </summary>
		/// <typeparam name="TCommand">The type of the command</typeparam>
		/// <typeparam name="TCallback">The type signature of callback</typeparam>
		/// <param name="control">The item whos click event must be hooked to the command</param>
		/// <param name="callback">The method pointer of the callback</param>
		/// <param name="parameter">The command parameters that will be used by the command to access additional data</param>
		/// <returns></returns>
		//public BaseCommand MapEventToCommand<TCommand, TCallback>(Control control, TCallback callback, ICommandParameter parameter)
		//  where TCommand : BaseCommand, new()
		//{
		//  Shield.Raise.AgainstNull(control);
		//  BaseCommand command;

		//  if (parameter == null)
		//    command = (BaseCommand)DomainsService.Instance.CreateInstance(typeof(TCommand), callback);
		//  else
		//    command = (BaseCommand)DomainsService.Instance.CreateInstance(typeof(TCommand), new object[] { callback, parameter });

		//  control.Click += command.OnBaseExecute;
		//  //_commands.Add(command);
		//  return command;
		//}

		/// <summary>
		/// Map a click event to a command so that the command will be called when the event is fired.
		/// When the command is executed the passed callback is called on the view to process any results etc.
		/// Note: The callback can be any generic Action etc.
		/// </summary>
		/// <param name="eventObject">The object which event must be hooked to the command</param>
		/// <param name="eventName">The name of the event that must be hooked to the command</param>
		/// <param name="command">The command to be called when event fires</param>
		public static void MapEvent(object eventObject, string eventName, ICommand command)
		{
			Shield.AgainstNull(eventObject).Raise();
			Shield.AgainstNullOrEmpty(eventName).Raise();

			// TODO: NEED TO FIX THE RETRIEVAL OF CORRECT EVENT WITH ARGS
			var eventInfo = eventObject.GetType().GetEvent(eventName);
			//var eventParam = eventInfo.EventHandlerType.GetMethod("Invoke").GetParameters()[1].ParameterType;
			//var d = Delegate.CreateDelegate(eventInfo.EventHandlerType, command,
				//command.GetType().GetMethod("OnExecute",new Type[] {eventParam}));
			var d = Delegate.CreateDelegate(eventInfo.EventHandlerType, command, "OnExecute", true);
			eventInfo.AddEventHandler(eventObject, d);
			
			//_commands.Add(command););		
		}

		//private void InterceptEvent(object sender, object args)
		//{
			
		//}

	}
}
