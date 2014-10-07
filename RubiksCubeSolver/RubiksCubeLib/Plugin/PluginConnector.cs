using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
  internal class PluginConnector<T> where T: IPluginable
  {
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
