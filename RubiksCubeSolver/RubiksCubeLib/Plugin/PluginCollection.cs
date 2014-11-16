using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
  /// <summary>
  /// Represents a collection of plugins
  /// </summary>
  /// <typeparam name="T">A pluginable type</typeparam>
  public class PluginCollection<T> where T : IPluginable
  {
    private List<T> plugins = new List<T>();

    /// <summary>
    /// Adds a plugin to the collection, if not already existing
    /// </summary>
    /// <param name="plugin"></param>
    /// <returns>Returns whether the plugin has been added successfully to the collection</returns>
    private bool Add(T plugin)
    {
      if (Contains(plugin.Name))
      {
        plugins.Add(plugin);
        return true;
      }
      else return false;
    }

    /// <summary>
    /// Add plugins from dll library to the collection
    /// </summary>
    /// <param name="fileName">Full path of the dll library</param>
    public void AddDll(string fileName)
    {
      List<T> pluginsFromFile = new PluginConnector<T>().GetPluginsFromDll(fileName);
      pluginsFromFile = pluginsFromFile.Where(p => Add(p)).ToList();
    }

    /// <summary>
    /// Add all detected plugins in a directory to the collection
    /// </summary>
    /// <param name="dirName">Full path of the directory</param>
    public void AddFolder(string dirName)
    {
      List<T> pluginsFromDir = new PluginConnector<T>().LoadPlugins(dirName);
      pluginsFromDir = pluginsFromDir.Where(p => Add(p)).ToList();
    }

    /// <summary>
    /// Gets all plugins
    /// </summary>
    /// <returns>All plugins in collection</returns>
    public IEnumerable<T> GetAll()
    {
      return plugins;
    }

    /// <summary>
    /// Gets or sets a specific plugin
    /// </summary>
    /// <param name="i">Index of specific item in collection</param>
    public T this[int i]
    {
      get { return plugins[i]; }
      set { plugins[i] = value; }
    }

    /// <summary>
    /// Gets the plugin from name
    /// </summary>
    /// <param name="name">Name of specific item in collection</param>
    public T this[string name]
    {
      get { return plugins.Find(p => p.Name == name); }
    }

    /// <summary>
    /// Gets the number of plugins in the collection
    /// </summary>
    public int Count { get { return plugins.Count; } }

    private T standardPlugIn;
    /// <summary>
    /// Gets or sets the standard plugin
    /// </summary>
    public T StandardPlugin
    {
      get
      {
        if (standardPlugIn == null)
        {
          if (Count > 0) return  plugins.First();
          else throw new Exception("No plugins available!");
        }
        else
        {
          return standardPlugIn;
        }
      }
      set { standardPlugIn = value; }
    }

    /// <summary>
    /// Check if collection already has a item with the same name
    /// </summary>
    /// <param name="name">Name to find in collection</param>
    /// <returns>True, if there is an equal name in the collection</returns>
    public bool Contains(string name)
    {
      return plugins.Find(p => p.Name == name) == null;
    }

    /// <summary>
    /// Clears the collection
    /// </summary>
    public void Clear()
    {
      plugins.Clear();
    }
  }
}
