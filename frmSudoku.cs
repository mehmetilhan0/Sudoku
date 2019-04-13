using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Bilge.Sudoku
{
	public partial class frmSudoku : Form
	{

		#region Constructor...

		public frmSudoku()
		{
			this.InitializeComponent();		
			this.solutionDetails = new StringBuilder();
			this.CreateSudokuBoard();
		}

		#endregion

		#region Fields...
		private TextBox[,] graphicSudokuBoard;
		private StringBuilder solutionDetails;

		#endregion

		#region Methods...

		private void CreateSudokuBoard()
		{
			int sudokuTableMargin = this.btnSolve.Left;
			int x = sudokuTableMargin, y = sudokuTableMargin;
			int tabIndex = 0, textBoxHeight = 0;

			this.graphicSudokuBoard = new TextBox[9, 9];

			for (int row = 0; row < 9; row++)
			{
				x = sudokuTableMargin;

				for (int col = 0; col < 9; col++)
				{
					TextBox txt = new TextBox();
                    txt.BorderStyle = BorderStyle.FixedSingle;
                    txt.Font = new Font(txt.Font.FontFamily, 24F, FontStyle.Bold);
                    txt.MaxLength = 1;
                    txt.TextAlign = HorizontalAlignment.Center;
                    txt.Width = txt.Height;
                    txt.TextChanged += new EventHandler(this.txt_TextChanged);
                    txt.TabIndex = tabIndex++;
                    txt.Location = new Point(x, y);
					this.Controls.Add(txt);

					this.graphicSudokuBoard[row, col] = txt;
					textBoxHeight = txt.Height;

					x += txt.Width - 1;
					if (col % 3 == 2)
						x++;
				}

				y += textBoxHeight - 1;
				if (row % 3 == 2)
					y++; 
                
			}
		}

		private void txt_TextChanged(object sender, EventArgs e)
		{
			TextBox txt = (TextBox) sender;
			this.ValidateSudokuTextBox(txt);
			txt.BackColor = Color.SpringGreen;
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			for (int row = 0; row < 9; row++)
			{
				for (int col = 0; col < 9; col++)
				{
					this.graphicSudokuBoard[row, col].ReadOnly = false;
					this.graphicSudokuBoard[row, col].Text = string.Empty;


                    if (row % 2 == 0)
                    this.graphicSudokuBoard[row, col].BackColor = Color.Aqua;

                    if (row % 2 == 1)
                        this.graphicSudokuBoard[row, col].BackColor = Color.PowderBlue;
                }
			}

			this.graphicSudokuBoard[0, 0].Focus();

			this.solutionDetails.Length = 0;
			this.btnSolve.Enabled = true;
		}

		private void ValidateSudokuTextBox(TextBox txt)
		{
			if (txt.TextLength == 1)
			{
				if (txt.Text[0] < '1' || txt.Text[0] > '9')
				{
                    txt.Text = string.Empty;
				}
			}
			else if (txt.TextLength > 1)
			{
				if (txt.Text[0] < '1' || txt.Text[0] > '9')
				{
                    txt.Text = string.Empty;
				}
				else
				{
                    txt.Text = txt.Text.Substring(0, 1);
				}
			}
		}

		private void btnSolve_Click(object sender, EventArgs e)
		{

			// Prepare the board...
			SudokuTable sudokuTable = new SudokuTable();
			sudokuTable.SudokuStrategyUsed += 
				new SudokuStrategyUsedEventHandler(this.frmSudoku_SudokuStrategyUsed);
			solutionDetails.Length = 0;

			for (int row = 0; row < 9; row++)
			{
				for (int col = 0; col < 9; col++)
				{
					this.ValidateSudokuTextBox(this.graphicSudokuBoard[row, col]);

					if (this.graphicSudokuBoard[row, col].TextLength == 1)
					{
						try
						{
							sudokuTable.SetValue(row, col, Convert.ToInt32(this.graphicSudokuBoard[row, col].Text));
						}
						catch (ApplicationException ex)
						{
							this.graphicSudokuBoard[row, col].BackColor = Color.Pink;
							this.graphicSudokuBoard[row, col].Focus();
							MessageBox.Show(ex.Message);
							return;
						}
					}
				}
			}


			long ticksStart, ticksStop;

			try
			{
				ticksStart = DateTime.Now.Ticks;
				sudokuTable.Solve();
				ticksStop = DateTime.Now.Ticks;
			}
			catch (ApplicationException ex)
			{
				MessageBox.Show(ex.Message + Environment.NewLine + sudokuTable.ToString());
				return;
			}

			// Copy the results to show...
			for (int row = 0; row < 9; row++)
			{
				for (int col = 0; col < 9; col++)
				{
					this.graphicSudokuBoard[row, col].Text = sudokuTable[row, col].Value.ToString();
					this.graphicSudokuBoard[row, col].ReadOnly = true;
				}
			}

			this.btnSolve.Enabled = false;

		}

		private void frmSudoku_SudokuStrategyUsed(object sender, SudokuStrategyUsedEventArgs e)
		{
			this.solutionDetails.AppendFormat(
				"{0}: Cell [{1}, {2}] = {3}", e.Strategy, e.Row + 1, e.Col + 1, e.Value);
			this.solutionDetails.AppendLine();
		}

		private void frmSudoku_Load(object sender, EventArgs e)
		{
			this.btnClear.PerformClick();
		}

		#endregion


	}
}