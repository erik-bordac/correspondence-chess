using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_SchoolProject.ChessFigures
{
	class Bishop : IFigure
	{
		public bool HasMoved { get; set; }
		public string ImgPath { get; set; }
		public string Color { get; set; }

		public Bishop (string color)
		{
			Color = color;

			ImgPath = (Color == "W") ? "./Resources/Wbishop.png" : "./Resources/Bbishop.png";
		}

		public bool IsValidMove(Square source, Square target, ChessGame game)
		{
			if (target.Content != null && source.Content.Color == target.Content.Color)
			{
				return false;
			}

			int rowDiff = target.Row - source.Row;
			int fileDiff = target.File - source.File;

			if (Math.Abs(rowDiff) != Math.Abs(fileDiff))
			{
				return false;
			}

			int rowStep = Math.Sign(rowDiff);
			int fileStep = Math.Sign(fileDiff);

			int currentRow = source.Row + rowStep;
			int currentFile = source.File + fileStep;

			while (currentRow != target.Row || currentFile != target.File)
			{
				if (game.gameArr[currentRow][currentFile].Content != null)
				{
					return false;
				}

				currentRow += rowStep;
				currentFile += fileStep;
			}

			return true;
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
