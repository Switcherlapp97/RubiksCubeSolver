using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
  public class LayerMoveCollection : IList<LayerMove>, IMove
  {
    public string Name
    {
      get
      {
        return string.Join(", ", moves.Select(m => m.Name).ToArray());
      }
    }
    public bool MultipleLayers { get { return true; } }

    public static LayerMoveCollection operator &(LayerMoveCollection collection, LayerMove newMove)
    {
      collection.Add(newMove);
      return collection;
    }

    private List<LayerMove> moves = new List<LayerMove>();
    public int IndexOf(LayerMove item)
    {
      return moves.IndexOf(item);
    }

    public void Insert(int index, LayerMove item)
    {
      moves.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
      moves.RemoveAt(index);
    }

    public LayerMove this[int index]
    {
      get
      {
        return moves[index];
      }
      set
      {
        moves[index] = value;
      }
    }


    public void Add(LayerMove item)
    {
      CubeFlag flag = CubeFlag.None;
      foreach (LayerMove m in moves)
      {
        flag |= m.Layer;
      }
      if (CubeFlagService.IsPossibleMovement(flag))
      {
        moves.Add(item);
      }
      else throw new Exception("Impossible movement");
    }

    public void AddRange(IEnumerable<LayerMove> items)
    {
      foreach(LayerMove m in items)
      {
        Add(m);
      }
    }

    public void Clear()
    {
      moves.Clear();
    }

    public bool Contains(LayerMove item)
    {
      return moves.Contains(item);
    }

    public void CopyTo(LayerMove[] array, int arrayIndex)
    {
      moves.CopyTo(array, arrayIndex);
    }

    public int Count
    {
      get { return moves.Count; }
    }

    public bool IsReadOnly
    {
      get { return false; }
    }

    public bool Remove(LayerMove item)
    {
      return moves.Remove(item);
    }

    public IEnumerator<LayerMove> GetEnumerator()
    {
      return moves.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return moves.GetEnumerator();
    }
  }
}
