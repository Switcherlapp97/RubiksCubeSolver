using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
	/// <summary>
	/// Represents a collection of moves of specific layers
	/// </summary>
	public class LayerMoveCollection : IList<LayerMove>, IMove
	{

		// *** PROPERTIES ***

		/// <summary>
		/// Returns a connected strings of all LayerMove names
		/// </summary>
		public string Name
		{
			get
			{
				return string.Join(", ", _moves.Select(m => m.Name).ToArray());
			}
		}

		/// <summary>
		/// Returns true if MultipleLayers are allowed
		/// </summary>
		public bool MultipleLayers { get { return true; } }

		/// <summary>
		/// Returns the count of the moves
		/// </summary>
		public int Count
		{
			get { return _moves.Count; }
		}

		/// <summary>
		/// Returns true if this Layer is readonly
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}



		// *** OPERATORS ***

		/// <summary>
		/// Adds a single LayerMove to the given collection
		/// </summary>
		/// <param name="collection">Defines the collection to be expanded</param>
		/// <param name="newMove">Defines the additional LayerMove</param>
		/// <returns></returns>
		public static LayerMoveCollection operator &(LayerMoveCollection collection, LayerMove newMove)
		{
			LayerMoveCollection lmc = new LayerMoveCollection();
			lmc.AddRange(collection);
			lmc.Add(newMove);
			return lmc;
		}

		/// <summary>
		/// Adds a collection of LayerMoves to the given collection
		/// </summary>
		/// <param name="collection1">Defines the collection to be expanded</param>
		/// <param name="collection2">Defines the collection to be added</param>
		/// <returns></returns>
		public static LayerMoveCollection operator &(LayerMoveCollection collection1, LayerMoveCollection collection2)
		{
			LayerMoveCollection lmc = new LayerMoveCollection();
			lmc.AddRange(collection1);
			lmc.AddRange(collection2);
			return lmc;
		}



		// *** PRIVATE FIELDS

		//The list of the single moves 
		private List<LayerMove> _moves = new List<LayerMove>();




		// *** METHODS ***

		/// <summary>
		/// Returns the index of a item in the collection
		/// </summary>
		/// <param name="item">Defines the item</param>
		/// <returns></returns>
		public int IndexOf(LayerMove item)
		{
			return _moves.IndexOf(item);
		}

		/// <summary>
		/// Inserts an item in the collection at a index
		/// </summary>
		/// <param name="index">Defines the index where the item is meant to be inserted</param>
		/// <param name="item">Defines te</param>
		public void Insert(int index, LayerMove item)
		{
			_moves.Insert(index, item);
		}

		/// <summary>
		/// Removes the item at the given index
		/// </summary>
		/// <param name="index">Defines the index where the item is meant to be removed</param>
		public void RemoveAt(int index)
		{
			_moves.RemoveAt(index);
		}

		/// <summary>
		/// Returns the layermove at the given index
		/// </summary>
		/// <param name="index">Defines the index of the item</param>
		/// <returns></returns>
		public LayerMove this[int index]
		{
			get
			{
				return _moves[index];
			}
			set
			{
				_moves[index] = value;
			}
		}

		/// <summary>
		/// Adds an item at the end of the collection
		/// </summary>
		/// <param name="item">Defines the item which is meant to be added</param>
		/// <exception cref="System.Exception">Thrown if this movement would be impossible</exception>
		public void Add(LayerMove item)
		{
			CubeFlag flag = CubeFlag.None;
			foreach (LayerMove m in _moves)
			{
				flag |= m.Layer;
			}
			if (CubeFlagService.IsPossibleMove(flag))
			{
				_moves.Add(item);
			}
			else
				throw new Exception("Impossible movement");
		}

		/// <summary>
		/// Adds multiple items at the end of the collection
		/// </summary>
		/// <param name="items">Defines the items which are meant to be added</param>
		public void AddRange(IEnumerable<LayerMove> items)
		{
			foreach (LayerMove m in items)
				Add(m);
		}

		/// <summary>
		/// Adds multiple items at the end of the collection
		/// </summary>
		/// <param name="items">Defines the items which are meant to be added</param>
		public void AddRange(LayerMoveCollection items)
		{
			foreach (LayerMove m in items)
				Add(m);
		}

		/// <summary>
		/// Clears the collection
		/// </summary>
		public void Clear()
		{
			_moves.Clear();
		}

		/// <summary>
		/// Returns true if this collection contains the given item
		/// </summary>
		/// <param name="item">Defines the item to be searched for</param>
		/// <returns></returns>
		public bool Contains(LayerMove item)
		{
			return _moves.Contains(item);
		}

		/// <summary>
		/// Copies the entire collection into the given array starting at the given index
		/// </summary>
		/// <param name="array">The</param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(LayerMove[] array, int arrayIndex)
		{
			_moves.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Removes the given item out of the collection
		/// </summary>
		/// <param name="item">Defines the item to be removed</param>
		/// <returns></returns>
		public bool Remove(LayerMove item)
		{
			return _moves.Remove(item);
		}


		/// <summary>
		/// Returns the enumerator of the collection
		/// </summary>
		/// <returns></returns>
		public IEnumerator<LayerMove> GetEnumerator()
		{
			return _moves.GetEnumerator();
		}

		/// <summary>
		/// Returns the enumerator of the collection
		/// </summary>
		/// <returns></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _moves.GetEnumerator();
		}
	}
}
