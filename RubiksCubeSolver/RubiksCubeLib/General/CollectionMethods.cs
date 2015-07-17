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

    public static Dictionary<TKey, TValue> TwoListsToDict<TKey, TValue>(IEnumerable<TKey> keys, IEnumerable<TValue> values)
    {
      Dictionary<TKey, TValue> newDict = new Dictionary<TKey, TValue>();
      List<TKey> lstKeys = keys.ToList();
      List<TValue> lstValues = values.ToList();

      if (lstKeys.Count != lstValues.Count) throw new Exception("The two collections don't have the same number of items!");

      for (int i = 0; i < lstKeys.Count; i++)
      {
        newDict.Add(lstKeys[i], lstValues[i]);
      }

      return newDict;
    }
	}
}
