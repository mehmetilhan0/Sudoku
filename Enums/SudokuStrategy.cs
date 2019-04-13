namespace Bilge.Sudoku
{

	/// <summary>
	/// Enum the strategies used to solve the Sudoku board.
	/// </summary>
	internal enum SudokuStrategy
	{
		/// <summary>Search for cells with an unique possible value.</summary>
		UniqueValueForCell,

		/// <summary>Search for rows with an unique possible value.</summary>
		UniqueValueForRow,

		/// <summary>Search for columns with an unique possible value.</summary>
		UniqueValueForColumn,

		/// <summary>Search for square with an unique possible value.</summary>
		UniqueValueForSquare,

		/// <summary>Pick a random possible value.</summary>
		RandomPick

	}

}