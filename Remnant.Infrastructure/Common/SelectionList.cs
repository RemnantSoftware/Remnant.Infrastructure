using System.Collections.Generic;

namespace Remnant.Core.Common
{
	public class SelectionList<TItem> : List<TItem>
	{
		
		/// <summary>
		/// Construct a selection list
		/// </summary>
		/// <param name="name">The name of the list</param>
		/// <param name="list">The list to be added</param>
		public SelectionList(string name, IEnumerable<TItem> list) : base(list)
		{
			Name = name;
		}
		
		/// <summary>
		/// Specify a group name for the list of items for ex: Users
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Specify the index for the current selected item in the list
		/// </summary>
		public int SelectedIndex { get; set; }

		/// <summary>
		/// Use the selected key to identify a unique id for the selected item
		/// </summary>
		public int? SelectedKey { get; set; }

		/// <summary>
		/// Returns the selected item based on the selected index. Note the selected key is ignored!
		/// </summary>
		public TItem SelectedItem
		{
			get { return this[SelectedIndex]; }
		}
	}
}
