using System;
using System.Collections.Generic;
using System.Text;

namespace Remnant.Core.Creational
{
	/// <summary>
	/// Singleton pattern implementation
	/// </summary>
	/// <typeparam name="TClass">The name of the derived class</typeparam>
	/// <remarks>
	/// Author: Neill Verreynne
	/// Date: Mid 2009
	/// </remarks>
	public class Singleton<TClass>
		where TClass : class, new()
	{
		#region Fields

		private static readonly TClass _instance = new TClass();

		#endregion

		#region Constructors & Finalizors

		protected Singleton()
		{
		}

		public static TClass Instance
		{
			get
			{
				return _instance;
			}
		}

		#endregion
	}
}
