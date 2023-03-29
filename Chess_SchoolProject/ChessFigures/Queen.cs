using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_SchoolProject.ChessFigures
{
	class Queen : IFigure
	{
		public bool HasMoved { get; set; }
		public string ImgPath { get; set; }
		public string Color { get; set; }

		public Queen(string color)
		{
			Color = color;

			ImgPath = (Color == "W") ? "./Resources/Wqueen.png" : "./Resources/Bqueen.png";
		}

		public bool IsValidMove(Square source, Square target, ChessGame game)
		{
			if (target.Content != null && source.Content.Color == target.Content.Color) return false;

			bool row1 = true;
			bool row2 = true;
			bool file1 = true;
			bool file2 = true;
			for (int i = 1; i < 8; i++)
			{
				if (row1)
				{
					int checkedRow = source.Row + i;

					if (checkedRow > 7) row1 = false;

					if (row1 && game.gameArr[checkedRow][source.File].Content != null) row1 = false;

					if (checkedRow == target.Row && source.File == target.File) return true;
				}
				if (row2)
				{
					int checkedRow = source.Row - i;

					if (checkedRow < 0) row2 = false;

					if (row2 && game.gameArr[checkedRow][source.File].Content != null) row2 = false;

					if (checkedRow == target.Row && source.File == target.File) return true;
				}

				if (file1)
				{
					int checkedFile = source.File + i;

					if (checkedFile > 7) file1 = false;

					if (file1 && game.gameArr[source.Row][checkedFile].Content != null) file1 = false;

					if (source.Row == target.Row && checkedFile == target.File) return true;
				}

				if (file2)
				{
					int checkedFile = source.File - i;

					if (checkedFile < 0) file2 = false;

					if (file2 && game.gameArr[source.Row][checkedFile].Content != null) file2 = false;

					if (source.Row == target.Row && checkedFile == target.File) return true;
				}
			}

			bool diagonal1 = true;
			bool diagonal2 = true;
			bool diagonal3 = true;
			bool diagonal4 = true;

			for (int i = 1; i < 8; i++)
			{
				if (diagonal1)
				{
					int checkedRow = source.Row + i; int checkedFile = source.File + i;

					if (checkedRow > 7 || checkedFile > 7) diagonal1 = false;

					if (diagonal1 && game.gameArr[checkedRow][checkedFile].Content != null) diagonal1 = false;

					if (checkedRow == target.Row && checkedFile == target.File) return true;
				}
				if (diagonal2)
				{
					int checkedRow = source.Row + i; int checkedFile = source.File - i;

					if (checkedRow > 7 || checkedFile > 7 || checkedFile < 0) diagonal2 = false;

					if (diagonal2 && game.gameArr[checkedRow][checkedFile].Content != null) diagonal2 = false;

					if (checkedRow == target.Row && checkedFile == target.File) return true;
				}
				if (diagonal3)
				{
					int checkedRow = source.Row - i; int checkedFile = source.File + i;

					if (checkedRow > 7 || checkedFile > 7 || checkedRow < 0) diagonal3 = false;

					if (diagonal3 && game.gameArr[checkedRow][checkedFile].Content != null) diagonal3 = false;

					if (checkedRow == target.Row && checkedFile == target.File) return true;
				}
				if (diagonal4)
				{
					int checkedRow = source.Row - i; int checkedFile = source.File - i;

					if (checkedRow > 7 || checkedFile > 7 || checkedRow < 0 || checkedFile < 0) diagonal4 = false;

					if (diagonal4 && game.gameArr[checkedRow][checkedFile].Content != null) diagonal4 = false;

					if (checkedRow == target.Row && checkedFile == target.File) return true;
				}
			}

			return false;
		}

		public List<(int, int)> getValidMoves(Square source, ChessGame game)
		{
			var moves = new List<(int, int)>();

			for (int i = 0; i < game.gameArr.Count; i++)
			{
				for (int j = 0; j < game.gameArr.Count; j++)
				{
					if (IsValidMove(source, game.gameArr[i][j], game))
					{
						moves.Add((i, j));
					}
				}
			}

			return moves;
		}
	}
}
