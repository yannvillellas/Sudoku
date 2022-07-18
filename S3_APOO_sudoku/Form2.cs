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
            Create_Cells();
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
        private void Cell_Key_Pressed(object? sender, KeyPressEventArgs e)
        {
            var cell = sender as Cell;
            if (cell != null)
            {
                if (cell.isLocked) //do nothing if the cell is locked
                    return;
                if (int.TryParse(e.KeyChar.ToString(), out int value)) //add the pressed key value in the cell only if it is a number
                {
                    if (value == 0) //clear the cell value if pressed key is zero
                        cell.Clear();
                    else
                        cell.Text = value.ToString();
                    cell.ForeColor = Color.Black;
                    cell.isLocked = true;
                }
            }
        }
        Random random = new();
        private bool Find_Value_For_Next_Cell(int i, int j)
        {
            //increment the i and j values to move to the next cell
            //and if the column ends move to the next row
            if (++j > 8)
            {
                j = 0;

                //exit if the line ends
                if (++i > 8)
                    return true;
            }
            var numsLeft = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int value;
            //find a random and valid number for the cell and go to the next cell 
            //and check if it can be allocated with another random and valid number
            do
            {
                //if there is not numbers left in the list to try next, 
                //return to the previous cell and allocate it with a different number
                if (numsLeft.Count < 1)
                {
                    cells[i, j].value = 0;
                    return false;
                }

                //take a random number from the numbers left in the list
                value = numsLeft[random.Next(0, numsLeft.Count)];
                if (!cells[i,j].isLocked)
                {
                    cells[i, j].value = value;

                    //remove the allocated value from the list
                    numsLeft.Remove(value);
                }
            }
            while (!Is_Valid_Number(value, i, j) || !Find_Value_For_Next_Cell(i, j));
            if (!cells[i, j].isLocked) cells[i, j].Text = value.ToString();
            return true;
        }
        private bool Is_Valid_Number(int value, int x, int y)
        {
            for (int i = 0; i < 9; i++)
            {
                if (i != x && cells[i, y].value == value) //vertical
                    return false;
                if (i != y && cells[x, i].value == value) //horizontal
                    return false;
            }
            for (int i = x - (x % 3); i < x - (x % 3) + 3; i++) //square
            {
                for (int j = y - (y % 3); j < y - (y % 3) + 3; j++)
                {
                    if (i != x && j != y && cells[i, j].value == value)
                        return false;
                }
            }
            return true;
        }
        private void Clear_Button_Click(object sender, EventArgs e)
        {
            foreach (var cell in cells)
            {
                cell.Clear();
            }
        }
        private void Resolve_Button_Click(object sender, EventArgs e)
        {
            Find_Value_For_Next_Cell(0, -1);
        }
    }
}
