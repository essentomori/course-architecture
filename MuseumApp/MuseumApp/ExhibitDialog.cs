using System;
using System.Linq;
using System.Windows.Forms;

namespace MuseumApp
{
    public partial class ExhibitDialog : Form
    {
        private TextBox txtName;
        private ComboBox cmbMuseum;
        private NumericUpDown nudValue;
        private Button btnOk;
        private Button btnCancel;
        private Label lblName;
        private Label lblMuseum;
        private Label lblValue;

        public string ExhibitName => txtName.Text.Trim();
        public int SelectedMuseumId => (int)cmbMuseum.SelectedValue;
        public double ValueK => (double)nudValue.Value;

        public ExhibitDialog(string name = "", double value = 0, int museumId = 0)
        {
            InitializeComponent();
            LoadMuseums();

            txtName.Text = name;
            nudValue.Value = (decimal)value;

            if (museumId != 0 && cmbMuseum.Items.Count > 0)
                cmbMuseum.SelectedValue = museumId;
        }

        private void InitializeComponent()
        {
            this.txtName = new TextBox();
            this.cmbMuseum = new ComboBox();
            this.nudValue = new NumericUpDown();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.lblName = new Label();
            this.lblMuseum = new Label();
            this.lblValue = new Label();
            ((System.ComponentModel.ISupportInitialize)this.nudValue).BeginInit();
            this.SuspendLayout();

            this.lblName.Location = new System.Drawing.Point(12, 15);
            this.lblName.Size = new System.Drawing.Size(80, 25);
            this.lblName.Text = "Название:";

            this.txtName.Location = new System.Drawing.Point(100, 12);
            this.txtName.Size = new System.Drawing.Size(250, 27);

            this.lblMuseum.Location = new System.Drawing.Point(12, 55);
            this.lblMuseum.Size = new System.Drawing.Size(80, 25);
            this.lblMuseum.Text = "Музей:";

            this.cmbMuseum.Location = new System.Drawing.Point(100, 52);
            this.cmbMuseum.Size = new System.Drawing.Size(250, 28);
            this.cmbMuseum.DropDownStyle = ComboBoxStyle.DropDownList;

            this.lblValue.Location = new System.Drawing.Point(12, 95);
            this.lblValue.Size = new System.Drawing.Size(120, 25);
            this.lblValue.Text = "Стоимость (тыс. руб.):";

            this.nudValue.Location = new System.Drawing.Point(140, 92);
            this.nudValue.Size = new System.Drawing.Size(120, 27);
            this.nudValue.Minimum = 0;
            this.nudValue.Maximum = 1000000;
            this.nudValue.DecimalPlaces = 2;
            this.nudValue.ThousandsSeparator = true;

            this.btnOk.Location = new System.Drawing.Point(170, 135);
            this.btnOk.Size = new System.Drawing.Size(85, 35);
            this.btnOk.Text = "OK";
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);

            this.btnCancel.Location = new System.Drawing.Point(265, 135);
            this.btnCancel.Size = new System.Drawing.Size(85, 35);
            this.btnCancel.Text = "Отмена";
            this.btnCancel.DialogResult = DialogResult.Cancel;

            this.ClientSize = new System.Drawing.Size(370, 190);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.nudValue);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.cmbMuseum);
            this.Controls.Add(this.lblMuseum);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Text = "Экспонат";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            ((System.ComponentModel.ISupportInitialize)this.nudValue).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadMuseums()
        {
            using (var context = new AppDbContext())
            {
                var museums = context.Museums.OrderBy(m => m.Name).ToList();
                cmbMuseum.DataSource = museums;
                cmbMuseum.DisplayMember = "Name";
                cmbMuseum.ValueMember = "Id";
            }
        }

        private void btnOk_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Название экспоната не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }

            if (cmbMuseum.SelectedValue == null)
            {
                MessageBox.Show("Выберите музей.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }

            if (nudValue.Value < 0)
            {
                MessageBox.Show("Стоимость не может быть отрицательной.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
        }
    }
}