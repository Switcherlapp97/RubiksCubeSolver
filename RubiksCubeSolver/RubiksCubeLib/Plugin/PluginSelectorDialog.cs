using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RubiksCubeLib
{
  /// <summary>
  /// Represents a windwos form to select plugins
  /// </summary>
  /// <typeparam name="T">A pluginable type</typeparam>
  public class PluginSelectorDialog<T> : Form where T: IPluginable
  {
    private ListBox lbPlugins;
    private Button btnOK;
    private Button btnCancel;

    private PluginCollection<T> plugins;
    /// <summary>
    /// Gets the currently selected plugin
    /// </summary>
    public T SelectedPlugin { get; private set; }

    public PluginSelectorDialog(PluginCollection<T> plugins)
    {
      InitializeComponent();
      this.plugins = plugins;
      foreach (T plugin in this.plugins.GetAll())
      {
        lbPlugins.Items.Add(plugin.Name);
      }
    }

    #region Designer
    private void InitializeComponent()
    {
      this.lbPlugins = new System.Windows.Forms.ListBox();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // lbPlugins
      // 
      this.lbPlugins.FormattingEnabled = true;
      this.lbPlugins.Location = new System.Drawing.Point(12, 12);
      this.lbPlugins.Name = "lbPlugins";
      this.lbPlugins.Size = new System.Drawing.Size(258, 108);
      this.lbPlugins.TabIndex = 0;
      this.lbPlugins.SelectedIndexChanged += new System.EventHandler(this.lbPlugins_SelectedIndexChanged);
      // 
      // btnOK
      // 
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Location = new System.Drawing.Point(195, 125);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(12, 125);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // PluginSelectorDialog
      // 
      this.AcceptButton = this.btnOK;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(282, 160);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.lbPlugins);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PluginSelectorDialog";
      this.Text = "Select Plugin";
      this.Load += new System.EventHandler(this.PluginSelectorDialog_Load);
      this.ResumeLayout(false);
    }
    #endregion

    private void lbPlugins_SelectedIndexChanged(object sender, EventArgs e)
    {
      SelectedPlugin = plugins[lbPlugins.SelectedIndex];
      plugins.StandardPlugin = SelectedPlugin;
    }

    private void PluginSelectorDialog_Load(object sender, EventArgs e)
    {
      if (plugins.Count == 0)
      {
        MessageBox.Show("No plugins found!");
        this.Close();
      }
      else
      {
        lbPlugins.SelectedIndex = 0;
      }
    }
  }
}
