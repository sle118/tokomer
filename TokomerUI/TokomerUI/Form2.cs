using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CerealPotter
{
    public partial class formSelectCurve : Form
    {
        public Tuple<string, string> Selection ;
        public formSelectCurve()
        {
            InitializeComponent();
        }
        public void SetCurves(Dictionary<string,List<string>> curves)
        {
            treeView1.Nodes.Clear();
            foreach (var dict in curves)
            {
                
                TreeNode treeNodeParent = new TreeNode(dict.Key);
                treeNodeParent.ImageIndex = 1;
                foreach (var name in dict.Value)
                {
                    TreeNode treeNodeCurve = new TreeNode(name)
                    {
                        ImageKey = "Graphics3D_16x.png"
                    };

                    treeNodeParent.Nodes.Add(treeNodeCurve);
                }


                treeView1.Nodes.Add(treeNodeParent);
            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e != null && e.Node.Parent!=null)
            {
                Selection = new Tuple<string, string>(e?.Node.Parent.Text, e?.Node.Text);
            }
            
        }
    }
}
