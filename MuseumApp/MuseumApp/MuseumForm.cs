using System;
using System.Linq;
using System.Windows.Forms;

namespace MuseumApp
{
    public partial class MuseumForm : Form
    {
        private DataGridView dgvMuseums;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnClose;

        public MuseumForm()
        {
            InitializeComponent();
            LoadMuseums();
        }

        private void InitializeComponent()
        {
            this.dgvMuseums = new DataGridView();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnClose = new Button();
            ((System.ComponentModel.ISupportInitialize)this.dgvMuseums).BeginInit();
            this.SuspendLayout();

            // dgvMuseums
            this.dgvMuseums.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMuseums.Location = new System.Drawing.Point(12, 12);
            this.dgvMuseums.Size = new System.Drawing.Size(400, 300);
            this.dgvMuseums.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvMuseums.MultiSelect = false;
            this.dgvMuseums.ReadOnly = true;
            this.dgvMuseums.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(430, 12);
            this.btnAdd.Size = new System.Drawing.Size(120, 40);
            this.btnAdd.Text = "Добавить";
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            // btnEdit
            this.btnEdit.Location = new System.Drawing.Point(430, 62);
            this.btnEdit.Size = new System.Drawing.Size(120, 40);
            this.btnEdit.Text = "Редактировать";
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(430, 112);
            this.btnDelete.Size = new System.Drawing.Size(120, 40);
            this.btnDelete.Text = "Удалить";
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            // btnClose
            this.btnClose.Location = new System.Drawing.Point(430, 272);
            this.btnClose.Size = new System.Drawing.Size(120, 40);
            this.btnClose.Text = "Закрыть";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // MuseumForm
            this.ClientSize = new System.Drawing.Size(570, 330);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvMuseums);
            this.Text = "Музеи (справочник)";
            this.StartPosition = FormStartPosition.CenterParent;

            ((System.ComponentModel.ISupportInitialize)this.dgvMuseums).EndInit();
            this.ResumeLayout(false);
        }

        private void LoadMuseums()
        {
            using (var context = new AppDbContext())
            {
                dgvMuseums.DataSource = context.Museums
                    .OrderBy(m => m.Name)
                    .Select(m => new { m.Id, Название = m.Name })
                    .ToList();
            }
            if (dgvMuseums.Columns["Id"] != null)
                dgvMuseums.Columns["Id"].Visible = false;
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            var dialog = new InputDialog("Введите название музея:");
            if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.InputText))
            {
                using (var context = new AppDbContext())
                {
                    context.Museums.Add(new Museum { Name = dialog.InputText.Trim() });
                    context.SaveChanges();
                }
                LoadMuseums();
            }
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvMuseums.CurrentRow == null)
            {
                MessageBox.Show("Выберите музей для редактирования.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = (int)dgvMuseums.CurrentRow.Cells["Id"].Value;
            string currentName = dgvMuseums.CurrentRow.Cells["Название"].Value?.ToString() ?? "";

            var dialog = new InputDialog("Редактировать название музея:", currentName);
            if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.InputText))
            {
                using (var context = new AppDbContext())
                {
                    var museum = context.Museums.Find(id);
                    if (museum != null)
                    {
                        museum.Name = dialog.InputText.Trim();
                        context.SaveChanges();
                    }
                }
                LoadMuseums();
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvMuseums.CurrentRow == null)
            {
                MessageBox.Show("Выберите музей для удаления.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = (int)dgvMuseums.CurrentRow.Cells["Id"].Value;
            string name = dgvMuseums.CurrentRow.Cells["Название"].Value?.ToString() ?? "";

            using (var context = new AppDbContext())
            {
                if (context.Exhibits.Any(ex => ex.MuseumId == id))
                {
                    MessageBox.Show($"Невозможно удалить музей \"{name}\", так как с ним связаны экспонаты.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show($"Удалить музей \"{name}\"?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var museum = context.Museums.Find(id);
                    if (museum != null)
                    {
                        context.Museums.Remove(museum);
                        context.SaveChanges();
                        LoadMuseums();
                    }
                }
            }
        }

        private void btnClose_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}