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
			if (target.Content != null && source.Content.Color == target.Content.Color)
			{
				return false;
			}

			int rowDistance = Math.Abs(target.Row - source.Row);
			int fileDistance = Math.Abs(target.File - source.File);

			if (rowDistance == 0 && fileDistance == 0)
			{
				return false;
			}

			int rowDirection = Math.Sign(target.Row - source.Row);
			int fileDirection = Math.Sign(target.File - source.File);

			int currentRow = source.Row + rowDirection;
			int currentFile = source.File + fileDirection;

			while (currentRow != target.Row || currentFile != target.File)
			{
				if (game.gameArr[currentRow][currentFile].Content != null)
				{
					return false;
				}

				currentRow += rowDirection;
				currentFile += fileDirection;
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
