using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace S3_APOO_sudoku
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        Cell[,] cells = new Cell[9, 9];
        private void Create_Cells()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    //create 81 cells for with styles and locations based on the index
                    cells[i, j] = new Cell
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
                    //assign key press event for each cells
                    cells[i, j].KeyPress += Cell_Key_Pressed;
                    panel1.Controls.Add(cells[i, j]);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
