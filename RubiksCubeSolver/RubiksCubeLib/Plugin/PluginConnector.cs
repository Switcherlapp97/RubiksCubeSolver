using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
  /// <summary>
  /// Represents a connection for loading plugins from dll files
  /// </summary>
  /// <typeparam name="T">A pluginable type</typeparam>
  internal class PluginConnector<T> where T: IPluginable
  {
    /// <summary>
    /// Loads plugins from directory
    /// </summary>
    /// <param name="dirName">Full path of the directory</param>
    /// <returns>A collection of the detected compatible plugins</returns>
    public List<T> LoadPlugins(string dirName)
    {
      List<T> plugins = new List<T>();

      // Get dlls in plugin directory
      foreach (string fileOn in Directory.GetFiles(dirName))
      {
        FileInfo file = new FileInfo(fileOn);

        // Preliminary check, must be .dll
        if (file.Extension.Equals(".dll"))
        {
          // Add plugin to list
          plugins.AddRange(GetPluginsFromDll(file.FullName));
        }
      }
      return plugins;
    }

    /// <summary>
    /// Loads plugins from a dll library
    /// </summary>
    /// <param name="fileName">Full path of dll library</param>
    /// <returns>A collection of the detected compatible plugins</returns>
    public List<T> GetPluginsFromDll(string fileName)
    {
      List<T> plugins = new List<T>();
      System.Reflection.Assembly a = System.Reflection.Assembly.LoadFile(fileName);
      Type[] types = a.GetTypes();
      foreach (Type t in types)
      {
        try
        {
          object x = a.CreateInstance(t.FullName);
          plugins.Add((T)x);
        }
        catch { }
      }
      return plugins;
    }
  }
}
