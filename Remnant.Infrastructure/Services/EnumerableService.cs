using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Remnant.Core.Services
{
	public static class EnumerableService
	{

		/// <summary>
		/// Removes all adjacent duplicates from the inputList
		/// </summary>
		/// <typeparam name="T">Any IEnumerable type</typeparam>
		/// <param name="inputList">The IEnumerable list to process</param>
		/// <returns>The IEnumerable with no adjacent duplicates</returns>
		public static List<T> RemoveAdjacentDuplicatesFromList<T>(IEnumerable<T> inputList)
		{
			var securityIdsNoAdjacentDuplicates = new List<T>();
			T prevObject = default(T);
			foreach (T securityId in inputList)
			{
				if (!securityId.Equals(prevObject))
					securityIdsNoAdjacentDuplicates.Add(securityId);

				prevObject = securityId;
			}
			return securityIdsNoAdjacentDuplicates;
		}
	}
}
