using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
namespace S3_APOO_sudoku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            createCells();
        }
        SudokuCell[,] cells = new SudokuCell[9, 9];
        public void createCells()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    // Create 81 cells for with styles and locations based on the index
                    cells[i, j] = new SudokuCell
                    {
                        Font = new Font(SystemFonts.DefaultFont.FontFamily, 20),
                        Size = new Size(40, 40),
                        ForeColor = SystemColors.ControlDarkDark,
                        Location = new Point(i * 40, j * 40),
                        BackColor = ((i / 3) + (j / 3)) % 2 == 0 ? SystemColors.Control : Color.LightGray,
                        FlatStyle = FlatStyle.Flat
                    };
                    cells[i, j].FlatAppearance.BorderColor = Color.Black;
                    cells[i, j].x = i;
                    cells[i, j].y = j;
                    // Assign key press event for each cells
                    cells[i, j].KeyPress += cell_keyPressed;
                    panel1.Controls.Add(cells[i, j]);
                }
            }
        }
        private void cell_keyPressed(object sender, KeyPressEventArgs e)
        {
            SudokuCell cell = new();
            // Do nothing if the cell is locked
            if (cell.isLocked)
                return;
            // Add the pressed key value in the cell only if it is a number
            if (int.TryParse(e.KeyChar.ToString(), out int value))
            {
                // Clear the cell value if pressed key is zero
                if (value == 0)
                    cell.Clear();
                else
                    cell.Text = value.ToString();

                cell.ForeColor = SystemColors.ControlDarkDark;
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}