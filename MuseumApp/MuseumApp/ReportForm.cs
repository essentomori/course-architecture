using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace MuseumApp
{
    public partial class ReportForm : Form
    {
        private TabControl tabControl;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private DataGridView dgvReport1;
        private DataGridView dgvReport2;
        private DataGridView dgvReport3;
        private Button btnClose;

        public ReportForm()
        {
            InitializeComponent();
            LoadReports();
        }

        private void InitializeComponent()
        {
            this.tabControl = new TabControl();
            this.tabPage1 = new TabPage();
            this.tabPage2 = new TabPage();
            this.tabPage3 = new TabPage();
            this.dgvReport1 = new DataGridView();
            this.dgvReport2 = new DataGridView();
            this.dgvReport3 = new DataGridView();
            this.btnClose = new Button();

            ((System.ComponentModel.ISupportInitialize)this.dgvReport1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.dgvReport2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.dgvReport3).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();

            // tabControl
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Dock = DockStyle.Top;
            this.tabControl.Size = new System.Drawing.Size(700, 380);
            this.tabControl.Location = new System.Drawing.Point(0, 0);

            // tabPage1
            this.tabPage1.Text = "1. Полный список";
            this.tabPage1.Controls.Add(this.dgvReport1);
            this.dgvReport1.Dock = DockStyle.Fill;
            this.dgvReport1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport1.ReadOnly = true;

            // tabPage2
            this.tabPage2.Text = "2. Количество по музеям";
            this.tabPage2.Controls.Add(this.dgvReport2);
            this.dgvReport2.Dock = DockStyle.Fill;
            this.dgvReport2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport2.ReadOnly = true;

            // tabPage3
            this.tabPage3.Text = "3. Средняя стоимость";
            this.tabPage3.Controls.Add(this.dgvReport3);
            this.dgvReport3.Dock = DockStyle.Fill;
            this.dgvReport3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport3.ReadOnly = true;

            // btnClose
            this.btnClose.Location = new System.Drawing.Point(290, 400);
            this.btnClose.Size = new System.Drawing.Size(120, 40);
            this.btnClose.Text = "Закрыть";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // ReportForm
            this.ClientSize = new System.Drawing.Size(700, 460);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl);
            this.Text = "Отчёт";
            this.StartPosition = FormStartPosition.CenterParent;

            ((System.ComponentModel.ISupportInitialize)this.dgvReport1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.dgvReport2).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.dgvReport3).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void LoadReports()
        {
            using (var context = new AppDbContext())
            {
                // Раздел 1: Полный список
                dgvReport1.DataSource = context.Exhibits
                    .Include(e => e.Museum)
                    .OrderBy(e => e.Name)
                    .Select(e => new
                    {
                        Название = e.Name,
                        Музей = e.Museum != null ? e.Museum.Name : "",
                        Стоимость = e.ValueK
                    })
                    .ToList();

                if (dgvReport1.Columns["Стоимость"] != null)
                    dgvReport1.Columns["Стоимость"].HeaderText = "Стоимость (тыс. руб.)";

                // Раздел 2: Количество по музеям
                dgvReport2.DataSource = context.Exhibits
                    .Where(e => e.Museum != null)
                    .GroupBy(e => e.Museum!.Name)
                    .Select(g => new
                    {
                        Музей = g.Key,
                        Количество = g.Count()
                    })
                    .OrderBy(r => r.Музей)
                    .ToList();

                // Раздел 3: Средняя стоимость по музеям (сортировка по убыванию)
                dgvReport3.DataSource = context.Exhibits
                    .Where(e => e.Museum != null)
                    .GroupBy(e => e.Museum!.Name)
                    .Select(g => new
                    {
                        Музей = g.Key,
                        СредняяСтоимость = Math.Round(g.Average(e => e.ValueK), 2)
                    })
                    .OrderByDescending(r => r.СредняяСтоимость)
                    .ToList();

                if (dgvReport3.Columns["СредняяСтоимость"] != null)
                    dgvReport3.Columns["СредняяСтоимость"].HeaderText = "Средняя стоимость (тыс. руб.)";
            }
        }

        private void btnClose_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}