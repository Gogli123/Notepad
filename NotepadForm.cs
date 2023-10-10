using System;
using System.IO;
using System.Windows.Forms;

namespace Gr09_Notepad
{
    public partial class NotepadForm : Form
    {
        private string _filePath;
        private bool _isModified;

        public NotepadForm()
        {
            InitializeComponent();
        }

        private string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                if (value != null)
                {
                    this.Text = $"Notepad - {Path.GetFileName(value)}";
                }
                else
                {
                    this.Text = "Notepad - Untitled.txt";
                }
                _filePath = value;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(isSaveAs: true);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtContent.Undo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtContent.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtContent.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtContent.Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtContent.SelectAll();
        }

        private void changeColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                txtContent.BackColor = colorDialog1.Color;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A simple notepad application", "Information", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void txtContent_TextChanged(object sender, EventArgs e)
        {
            if (!_isModified) _isModified = true;
        }

        private void NotepadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ConfirmSave())
            {
                e.Cancel = true;
            }
        }

        private void NewFile()
        {
            if (!ConfirmSave())
            {
                return;
            }
            txtContent.Clear();
            FilePath = null;
            _isModified = false;
        }

        private void OpenFile()
        {
            if (!ConfirmSave())
            {
                return;
            }
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    txtContent.Text = File.ReadAllText(dlgOpen.FileName);
                    FilePath = dlgOpen.FileName;
                    _isModified = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool SaveFile(bool isSaveAs = false)
        {
            if (isSaveAs || FilePath == null)
            {
                dlgSave.FileName = Path.GetFileName(FilePath);
                if (dlgSave.ShowDialog() == DialogResult.OK)
                {
                    FilePath = dlgSave.FileName;
                }
                else
                {
                    return false;
                }
            }

            try
            {
                File.WriteAllText(FilePath, txtContent.Text);
                _isModified = false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool ConfirmSave()
        {
            if (!_isModified)
            {
                return true;
            }

            DialogResult result = MessageBox.Show(
                "Do you want to save changes?",
                "Confirmation",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button3);

            switch (result)
            {
                case DialogResult.Yes:
                    return SaveFile();
                case DialogResult.No:
                    return true;
                default:
                    return false;
            }
        }
    }
}
