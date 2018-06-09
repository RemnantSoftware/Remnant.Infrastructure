using System;
using System.Collections.Generic;
using Remnant.Core.Services;

namespace Remnant.Core.Commands
{
	/// <summary>
	/// Derive from this abstract result command class to implement your command receiving input and returning a result
	/// </summary>
	public class InputOutputCommand<TInput, TOutput> : Command
	{
		#region Fields

		private List<string> _ignoreInput;

		#endregion

		/// <summary>
		/// If TInput is a class you can specify to ignore certain properties by adding their names.
		/// This provide the fuctionality to the executer of the command to ignore certain input.
		/// For example, a database update command does not require all fields to be updated
		/// </summary>
		public List<string> IgnoreInputs { get { return ReflectionService.LazyInit(ref _ignoreInput); } }

		/// <summary>
		/// If TInput is a class you can specify to ignore certain properties by adding their names.
		/// This provide the fuctionality to the executer of the command to ignore certain input.
		/// For example, a database update command does not require all fields to be updated
		/// </summary>
		/// <param name="inputName">The name of the property on the class that must be ignored as input</param>
		/// <returns></returns>
		public InputOutputCommand<TInput, TOutput> IgnoreInput(string inputName)
		{
			IgnoreInputs.Add(inputName);
			return this;
		}

		public virtual TOutput Execute(TInput input)
		{
			throw new NotImplementedException("Implement (override) Execute in derived class");
		}

	}
}
