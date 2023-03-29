using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_SchoolProject.ChessFigures
{
	class Rook : IFigure
	{
		public string ImgPath { get; set; }
		public string Color { get; set; }
		public bool HasMoved { get; set; }

		public Rook(string color)
		{
			Color = color;
			HasMoved = false;

			ImgPath = (Color == "W") ? "./Resources/Wrook.png" : "./Resources/Brook.png";
		}
		public bool IsValidMove(Square source, Square target, ChessGame game)
		{
			// Check if the source and target squares are the same or if the target square contains a piece of the same color
			if (source == target || (target.Content != null && source.Content.Color == target.Content.Color))
			{
				return false;
			}

			int rowDiff = Math.Abs(source.Row - target.Row);
			int fileDiff = Math.Abs(source.File - target.File);

			// Check if the move is diagonal
			if (rowDiff == fileDiff)
			{
				return false;
			}

			// Check if the move is horizontal or vertical
			if (source.Row == target.Row || source.File == target.File)
			{
				int direction = source.Row == target.Row ? Math.Sign(target.File - source.File) : Math.Sign(target.Row - source.Row);

				for (int i = 1; i < (source.Row == target.Row ? fileDiff : rowDiff); i++)
				{
					int checkedRow = source.Row + (source.Row == target.Row ? 0 : i * direction);
					int checkedFile = source.File + (source.File == target.File ? 0 : i * direction);

					if (game.gameArr[checkedRow][checkedFile].Content != null)
					{
						return false;
					}
				}

				return true;
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
