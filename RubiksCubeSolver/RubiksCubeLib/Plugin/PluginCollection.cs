using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
  public class PluginCollection<T> where T : IPluginable
  {
    private List<T> plugins = new List<T>();

    private bool Add(T plugin)
    {
      if (Contains(plugin.Name))
      {
        plugins.Add(plugin);
        return true;
      }
      else return false;
    }

    public void AddDll(string fileName)
    {
      List<T> pluginsFromFile = new PluginConnector<T>().GetPluginsFromDll(fileName);
      pluginsFromFile = pluginsFromFile.Where(p => Add(p)).ToList();
      switch (pluginsFromFile.Count)
      {
        case 0: System.Windows.Forms.MessageBox.Show("No plugins found");
          break;
        case 1: System.Windows.Forms.MessageBox.Show(string.Format("{0} plugin has been loaded successfully", pluginsFromFile.Count));
          break;
        default: System.Windows.Forms.MessageBox.Show(string.Format("{0} plugins have been loaded successfully", pluginsFromFile.Count));
          break;
      }
    }

    public void AddFolder(string dirName)
    {
      List<T> pluginsFromDir = new PluginConnector<T>().LoadPlugins(dirName);
      pluginsFromDir = pluginsFromDir.Where(p => Add(p)).ToList();
      switch(pluginsFromDir.Count) 
      {
        case 0: System.Windows.Forms.MessageBox.Show("No plugins found");
          break;
        case 1: System.Windows.Forms.MessageBox.Show(string.Format("{0} plugin has been loaded successfully", pluginsFromDir.Count));
          break;
        default: System.Windows.Forms.MessageBox.Show(string.Format("{0} plugins have been loaded successfully", pluginsFromDir.Count));
          break;
      }
    }

    public IEnumerable<T> GetAll()
    {
      return plugins;
    }

    public T this[int i]
    {
      get { return plugins[i]; }
      set { plugins[i] = value; }
    }

    public T this[string name]
    {
      get { return plugins.Find(p => p.Name == name); }
    }

    public int Count { get { return plugins.Count; } }

    public T StandardPlugin { get; set; }

    public bool Contains(string name)
    {
      return plugins.Find(p => p.Name == name) == null;
    }

    public void Clear()
    {
      plugins.Clear();
    }
  }
}
