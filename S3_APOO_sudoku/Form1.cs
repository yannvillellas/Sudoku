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
            Create_Cells();
            Start_New_Game();
        }
        SudokuCell[,] cells = new SudokuCell[9, 9];
        private void Create_Cells()
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
                    cells[i, j].KeyPress += Cell_Key_Pressed;
                    panel1.Controls.Add(cells[i, j]);
                }
            }
        }
        private void Start_New_Game()
        {
            Load_Values();
            Show_Random_Values_Hints(15);
        }
        private void Cell_Key_Pressed(object? sender, KeyPressEventArgs e)
        {
            var cell = sender as SudokuCell;
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
        private void Load_Values()
        {
            //clear the values in each cells
            foreach (var cell in cells)
            {
                cell.value = 0;
                cell.Clear();
            }
            //this method will be called recursively 
            //until it finds suitable values for each cells
            Find_Value_For_Next_Cell(0, -1);
        }
        Random random = new();
        private bool Find_Value_For_Next_Cell(int i, int j)
        {
            //increment the i and j values to move to the next cell
            //and if the columsn ends move to the next row
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
                cells[i, j].value = value;

                //remove the allocated value from the list
                numsLeft.Remove(value);
            }
            while (!Is_Valid_Number(value, i, j) || !Find_Value_For_Next_Cell(i, j));
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
        private void Show_Random_Values_Hints(int hintsCount)
        {
            // Show value in random cells
            // The hints count is based on the level player choose
            for (int i = 0; i < hintsCount; i++)
            {
                var rX = random.Next(9);
                var rY = random.Next(9);

                // Style the hint cells differently and
                // lock the cell so that player can't edit the value
                cells[rX, rY].Text = cells[rX, rY].value.ToString();
                cells[rX, rY].ForeColor = Color.Black;
                cells[rX, rY].isLocked = true;
            }
        }
        private void Check_Button_Click(object sender, EventArgs e)
        {
            var wrongCells = new List<SudokuCell>();

            // Find all the wrong inputs
            foreach (var cell in cells)
            {
                if (!string.Equals(cell.value.ToString(), cell.Text))
                {
                    wrongCells.Add(cell);
                }
            }

            // Check if the inputs are wrong or the player wins 
            if (wrongCells.Any())
            {
                // Highlight the wrong inputs 
                wrongCells.ForEach(x => x.ForeColor = Color.Red);
                MessageBox.Show("Wrong inputs");
            }
            else
            {
                MessageBox.Show("You Wins");
            }
        }
        private void Clear_Button_Click(object sender, EventArgs e)
        {
            foreach (var cell in cells)
            {
                // Clear the cell only if it is not locked
                if (cell.isLocked == false)
                    cell.Clear();
            }
        }
        private void New_Game_Button_Click(object sender, EventArgs e)
        {
            Start_New_Game();
        }
    }
}