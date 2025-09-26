namespace Evaluator.UI.Windows
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox display;
        private TableLayoutPanel panel;

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
            this.display = new TextBox();
            this.panel = new TableLayoutPanel();
            this.SuspendLayout();

            // display
            this.display.ReadOnly = true;
            this.display.BackColor = System.Drawing.Color.DarkGreen;
            this.display.ForeColor = System.Drawing.Color.White;
            this.display.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.display.Dock = DockStyle.Top;
            this.display.Height = 40;
            this.display.TextAlign = HorizontalAlignment.Right;

            // panel
            this.panel.RowCount = 5;
            this.panel.ColumnCount = 5;
            this.panel.Dock = DockStyle.Fill;
            this.panel.BackColor = System.Drawing.Color.Black;

            // Columnas iguales
            for (int i = 0; i < 5; i++)
                this.panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            // Filas iguales
            for (int i = 0; i < 5; i++)
                this.panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

            // ==== BOTONES ====
            AddButton("7", 0, 0);
            AddButton("8", 1, 0);
            AddButton("9", 2, 0);
            AddButton("(", 3, 0, true);
            AddButton(")", 4, 0, true);

            AddButton("4", 0, 1);
            AddButton("5", 1, 1);
            AddButton("6", 2, 1);
            AddButton("*", 3, 1, true);
            AddButton("/", 4, 1, true);

            AddButton("1", 0, 2);
            AddButton("2", 1, 2);
            AddButton("3", 2, 2);
            AddButton("+", 3, 2, true);
            AddButton("-", 4, 2, true);

            AddButton("0", 0, 3);
            AddButton(".", 1, 3);
            AddButton("^", 2, 3, true);
            AddButton("Delete", 3, 3, true, DeleteLast);
            AddButton("Clear", 4, 3, true, (s, e) => display.Text = "");

            // Botón =
            Button btnEquals = new Button();
            btnEquals.Text = "=";
            btnEquals.BackColor = System.Drawing.Color.Orange;
            btnEquals.Dock = DockStyle.Fill;
            btnEquals.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnEquals.Click += (s, e) => EvaluateExpression();
            this.panel.Controls.Add(btnEquals, 0, 4);
            this.panel.SetColumnSpan(btnEquals, 5);

            // Form
            this.ClientSize = new System.Drawing.Size(400, 450);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.display);
            this.Name = "Form1";
            this.Text = "Functions Evaluator";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void AddButton(string text, int col, int row, bool orange = false, EventHandler handler = null)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Dock = DockStyle.Fill;
            btn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);

            if (orange)
            {
                btn.BackColor = System.Drawing.Color.Orange;
                btn.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                btn.BackColor = System.Drawing.Color.White;
                btn.ForeColor = System.Drawing.Color.Black;
            }

            if (handler != null)
                btn.Click += handler;
            else
                btn.Click += (s, e) => AppendText(text);

            this.panel.Controls.Add(btn, col, row);
        }
    }
}
