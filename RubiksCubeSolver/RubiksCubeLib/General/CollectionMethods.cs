using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{

	/// <summary>
	/// A static class to compare lists
	/// </summary>
	public static class CollectionMethods
	{

		// *** METHODS ***

		/// <summary>
		/// Returns true if both lists are equal even if they are scrambled (i.e. in a different order)
		/// </summary>
		/// <typeparam name="T">Defines the type of both lists</typeparam>
		/// <param name="list1">Defines the first list to be analyzed</param>
		/// <param name="list2">Defines the second list to be analyzed</param>
		/// <returns></returns>
		public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
		{
			var cnt = new Dictionary<T, int>();
			foreach (T s in list1)
			{
				if (cnt.ContainsKey(s))
				{
					cnt[s]++;
				}
				else
				{
					cnt.Add(s, 1);
				}
			}
			foreach (T s in list2)
			{
				if (cnt.ContainsKey(s))
				{
					cnt[s]--;
				}
				else
				{
					return false;
				}
			}
			return cnt.Values.All(c => c == 0);
		}

		public static Dictionary<T, T> Order<T>(List<T> standard, Dictionary<T, T> newOrder)
		{
			Dictionary<T, T> result = new Dictionary<T, T>();
			foreach (T t in standard)
			{
				result.Add(t, newOrder.ContainsKey(t) ? newOrder[t] : t);
			}
			return result;
		}

	}
}
