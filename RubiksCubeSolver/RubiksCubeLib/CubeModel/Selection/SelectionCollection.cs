using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.CubeModel
{
  public class SelectionCollection
  {
    private Dictionary<PositionSpec, Selection> selections;

    public SelectionCollection()
    {
      selections = new Dictionary<PositionSpec, Selection>();
    }

    public void Add(PositionSpec key, Selection value)
    {
      selections.Add(key, value);
    }

    public void Update(PositionSpec key, Selection newValue, bool asFlag = false)
    {
      if (selections.ContainsKey(key)) selections[key] = asFlag ? selections[key] | newValue : newValue; 
    }

    public void Reset()
    {
      foreach (PositionSpec key in selections.Keys.ToList())
      {
        selections[key] = Selection.None;
      }
    }

    public Selection this[PositionSpec key]
    {
      get
      {
        if (selections.ContainsKey(key)) return selections[key];
        else return Selection.None;
      }
      set
      {
        if (selections.ContainsKey(key)) selections[key] = value;
      }
    }
  }
}
