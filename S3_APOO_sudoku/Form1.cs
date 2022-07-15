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
            CreateCells();
            StartNewGame();
        }
        SudokuCell[,] cells = new SudokuCell[9, 9];
        private void CreateCells()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    //create 81 cells for with styles and locations based on the index
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
                    //assign key press event for each cells
                    cells[i, j].KeyPress += Cell_keyPressed;
                    panel1.Controls.Add(cells[i, j]);
                }
            }
        }
        private void StartNewGame()
        {
            Loadvalues();
        }
        private void Cell_keyPressed(object? sender, KeyPressEventArgs e)
        {
            SudokuCell cell = new();
            if (cell.isLocked) //do nothing if the cell is locked
                return;
            if (int.TryParse(e.KeyChar.ToString(), out int value)) //add the pressed key value in the cell only if it is a number
            {
                if (value == 0) //clear the cell value if pressed key is zero
                    cell.Clear();
                else
                    cell.Text = value.ToString();
                cell.ForeColor = SystemColors.ControlDarkDark;
            }
        }
        private void Loadvalues()
        {
            // Clear the values in each cells
            foreach (var cell in cells)
            {
                cell.value = 0;
                cell.Clear();
            }
            // This method will be called recursively 
            // until it finds suitable values for each cells
            FindValueForNextCell(0, -1);
        }
        Random random = new();
        private bool FindValueForNextCell(int i, int j)
        {
            // Increment the i and j values to move to the next cell
            // and if the columsn ends move to the next row
            if (++j > 8)
            {
                j = 0;

                // Exit if the line ends
                if (++i > 8)
                    return true;
            }
            var numsLeft = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int value;
            // Find a random and valid number for the cell and go to the next cell 
            // and check if it can be allocated with another random and valid number
            do
            {
                // If there is not numbers left in the list to try next, 
                // return to the previous cell and allocate it with a different number
                if (numsLeft.Count < 1)
                {
                    cells[i, j].value = 0;
                    return false;
                }

                // Take a random number from the numbers left in the list
                value = numsLeft[random.Next(0, numsLeft.Count)];
                cells[i, j].value = value;

                // Remove the allocated value from the list
                numsLeft.Remove(value);
            }
            while (!IsValidNumber(value, i, j) || !FindValueForNextCell(i, j));
            // TDO: Remove this line after testing
            cells[i, j].Text = value.ToString();
            return true;
        }
        private bool IsValidNumber(int value, int x, int y)
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
    }
}