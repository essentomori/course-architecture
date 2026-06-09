using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace MuseumApp
{
    public partial class ExhibitForm : Form
    {
        private DataGridView dgvExhibits;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnClose;

        public ExhibitForm()
        {
            InitializeComponent();
            LoadExhibits();
        }

        private void InitializeComponent()
        {
            this.dgvExhibits = new DataGridView();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnClose = new Button();
            ((System.ComponentModel.ISupportInitialize)this.dgvExhibits).BeginInit();
            this.SuspendLayout();

            this.dgvExhibits.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExhibits.Location = new System.Drawing.Point(12, 12);
            this.dgvExhibits.Size = new System.Drawing.Size(500, 350);
            this.dgvExhibits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvExhibits.MultiSelect = false;
            this.dgvExhibits.ReadOnly = true;
            this.dgvExhibits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.btnAdd.Location = new System.Drawing.Point(530, 12);
            this.btnAdd.Size = new System.Drawing.Size(120, 40);
            this.btnAdd.Text = "Добавить";
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            this.btnEdit.Location = new System.Drawing.Point(530, 62);
            this.btnEdit.Size = new System.Drawing.Size(120, 40);
            this.btnEdit.Text = "Редактировать";
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);

            this.btnDelete.Location = new System.Drawing.Point(530, 112);
            this.btnDelete.Size = new System.Drawing.Size(120, 40);
            this.btnDelete.Text = "Удалить";
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            this.btnClose.Location = new System.Drawing.Point(530, 322);
            this.btnClose.Size = new System.Drawing.Size(120, 40);
            this.btnClose.Text = "Закрыть";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            this.ClientSize = new System.Drawing.Size(670, 380);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvExhibits);
            this.Text = "Экспонаты (основная таблица)";
            this.StartPosition = FormStartPosition.CenterParent;

            ((System.ComponentModel.ISupportInitialize)this.dgvExhibits).EndInit();
            this.ResumeLayout(false);
        }

        private void LoadExhibits()
        {
            using (var context = new AppDbContext())
            {
                dgvExhibits.DataSource = context.Exhibits
                    .Include(e => e.Museum)
                    .OrderBy(e => e.Name)
                    .Select(e => new
                    {
                        e.Id,
                        Название = e.Name,
                        Музей = e.Museum != null ? e.Museum.Name : "",
                        Стоимость = e.ValueK
                    })
                    .ToList();
            }

            if (dgvExhibits.Columns["Id"] != null)
                dgvExhibits.Columns["Id"].Visible = false;
            if (dgvExhibits.Columns["Стоимость"] != null)
                dgvExhibits.Columns["Стоимость"].HeaderText = "Стоимость (тыс. руб.)";
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            var dialog = new ExhibitDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (var context = new AppDbContext())
                {
                    context.Exhibits.Add(new Exhibit
                    {
                        Name = dialog.ExhibitName,
                        MuseumId = dialog.SelectedMuseumId,
                        ValueK = dialog.ValueK
                    });
                    context.SaveChanges();
                }
                LoadExhibits();
            }
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvExhibits.CurrentRow == null)
            {
                MessageBox.Show("Выберите экспонат для редактирования.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = (int)dgvExhibits.CurrentRow.Cells["Id"].Value;
            string name = dgvExhibits.CurrentRow.Cells["Название"].Value?.ToString() ?? "";
            double value = Convert.ToDouble(dgvExhibits.CurrentRow.Cells["Стоимость"].Value);

            using (var context = new AppDbContext())
            {
                var exhibit = context.Exhibits.Find(id);
                if (exhibit == null) return;

                int museumId = exhibit.MuseumId;
                var dialog = new ExhibitDialog(name, value, museumId);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    exhibit.Name = dialog.ExhibitName;
                    exhibit.MuseumId = dialog.SelectedMuseumId;
                    exhibit.ValueK = dialog.ValueK;
                    context.SaveChanges();
                    LoadExhibits();
                }
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvExhibits.CurrentRow == null)
            {
                MessageBox.Show("Выберите экспонат для удаления.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = (int)dgvExhibits.CurrentRow.Cells["Id"].Value;
            string name = dgvExhibits.CurrentRow.Cells["Название"].Value?.ToString() ?? "";

            if (MessageBox.Show($"Удалить экспонат \"{name}\"?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (var context = new AppDbContext())
                {
                    var exhibit = context.Exhibits.Find(id);
                    if (exhibit != null)
                    {
                        context.Exhibits.Remove(exhibit);
                        context.SaveChanges();
                        LoadExhibits();
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