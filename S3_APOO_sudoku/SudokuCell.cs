using System.Windows.Forms;

namespace S3_APOO_sudoku
{
    internal class SudokuCell : Button
    {
        public int value { get; set; }
        public bool isLocked { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public void Clear()
        {
            this.Text = string.Empty;
            this.isLocked = false;
        }
    }
}
