using System;
using System.Windows.Forms;

namespace MuseumApp
{
    public partial class InputDialog : Form
    {
        private TextBox txtInput;
        private Button btnOk;
        private Button btnCancel;
        private Label lblPrompt;

        public string InputText => txtInput.Text;

        public InputDialog(string prompt, string defaultValue = "")
        {
            InitializeComponent();
            lblPrompt.Text = prompt;
            txtInput.Text = defaultValue;
        }

        private void InitializeComponent()
        {
            this.lblPrompt = new Label();
            this.txtInput = new TextBox();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.SuspendLayout();

            this.lblPrompt.Location = new System.Drawing.Point(12, 20);
            this.lblPrompt.Size = new System.Drawing.Size(350, 25);
            this.lblPrompt.Text = "Введите значение:";

            this.txtInput.Location = new System.Drawing.Point(12, 50);
            this.txtInput.Size = new System.Drawing.Size(350, 27);

            this.btnOk.Location = new System.Drawing.Point(190, 90);
            this.btnOk.Size = new System.Drawing.Size(80, 30);
            this.btnOk.Text = "OK";
            this.btnOk.DialogResult = DialogResult.OK;

            this.btnCancel.Location = new System.Drawing.Point(280, 90);
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.Text = "Отмена";
            this.btnCancel.DialogResult = DialogResult.Cancel;

            this.ClientSize = new System.Drawing.Size(380, 140);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.lblPrompt);
            this.Text = "Ввод данных";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}