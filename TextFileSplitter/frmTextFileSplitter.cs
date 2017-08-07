using System;
using System.IO;
using System.Windows.Forms;

namespace TextFileSplitter
{
    public partial class frmTextFileSplitter : Form
    {
        private string path;

        public frmTextFileSplitter()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {                
                path = ofd.FileName;
                txtFilePath.Text = path;
            }
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            var lines = File.ReadAllLines(path);
            int maxLines = (int)nudMaxLines.Value;
            string[] fileLines = new string[maxLines];

            string prefix = txtPrefix.Text;
            string suffix = txtSuffix.Text;

            int lastPeriod = path.LastIndexOf(".");
            string extension = path.Substring(lastPeriod + 1, path.Length - 1 - lastPeriod);
            string oldFilePath = path.Substring(0, path.LastIndexOf("\\") + 1);

            int count = 0;

            for (int i = 0, j = 0; i < lines.Length; i++, j++)
            {
                if (j == maxLines || i == lines.Length - 1)
                {
                    j = 0;
                    count++;
                    
                    string newFilePath = string.Format("{0}{1}{2}{3}.{4}", oldFilePath, prefix, count, suffix, extension);

                    File.WriteAllLines(newFilePath, fileLines);
                    fileLines = new string[maxLines];
                }

                fileLines[j] = lines[i];
            }            

            lines = null;
            fileLines = null;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            MessageBox.Show(string.Format("Finished splitting, created {0} files in {1}", count + 1, oldFilePath), "Finished", MessageBoxButtons.OK);
        }
    }
}
