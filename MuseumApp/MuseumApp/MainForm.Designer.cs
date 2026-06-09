namespace MuseumApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnMuseums;
        private Button btnExhibits;
        private Button btnReport;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnMuseums = new Button();
            this.btnExhibits = new Button();
            this.btnReport = new Button();
            this.SuspendLayout();

            this.btnMuseums.Location = new System.Drawing.Point(50, 50);
            this.btnMuseums.Size = new System.Drawing.Size(200, 50);
            this.btnMuseums.Text = "Музеи (справочник)";
            this.btnMuseums.UseVisualStyleBackColor = true;
            this.btnMuseums.Click += new EventHandler(this.btnMuseums_Click);

            this.btnExhibits.Location = new System.Drawing.Point(50, 120);
            this.btnExhibits.Size = new System.Drawing.Size(200, 50);
            this.btnExhibits.Text = "Экспонаты (основная)";
            this.btnExhibits.UseVisualStyleBackColor = true;
            this.btnExhibits.Click += new EventHandler(this.btnExhibits_Click);

            this.btnReport.Location = new System.Drawing.Point(50, 190);
            this.btnReport.Size = new System.Drawing.Size(200, 50);
            this.btnReport.Text = "Отчёт";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new EventHandler(this.btnReport_Click);

            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnExhibits);
            this.Controls.Add(this.btnMuseums);
            this.Text = "Главное меню";
            this.ResumeLayout(false);
        }
    }
}