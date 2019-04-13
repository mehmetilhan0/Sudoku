using System;
using System.Windows.Forms;

namespace Bilge.Sudoku
{
	/// <summary>
	/// Entry point for the application.
	/// </summary>
	internal static class Program
	{

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmSudoku());
		}

	}
}
