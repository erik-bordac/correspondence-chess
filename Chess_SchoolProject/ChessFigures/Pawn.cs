using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chess_SchoolProject.ChessFigures
{
	class Pawn : IFigure
	{
		public bool HasMoved { get; set; }
		public string ImgPath { get; set; }
		public string Color { get; set; }

		public Pawn (string color)
		{
			Color = color;
			HasMoved = false;

			ImgPath = (Color == "W") ? "./Resources/Wpawn.png" : "./Resources/Bpawn.png";
		}

		public bool IsValidMove(Square source, Square target, ChessGame game)
		{
			if (target.Content != null && source.Content.Color == target.Content.Color) return false;

			// Helper variables
			int FileDiff = (source.File - target.File);
			int RowDiff = (source.Row - target.Row);

			if (Color == "B")
			{
				RowDiff = -RowDiff;
			}
			// attacking EnPassant square
			if (RowDiff == 1 && Math.Abs(FileDiff) == 1 && target.EnPassantFlag == true)
			{
				return true;
			}

			if (RowDiff == 2 && FileDiff == 0 && !HasMoved && target.Content == null)
			{
				//piece in front of pawn
				int rowVector = Color == "B" ? 1 : -1;

				if (game.gameArr[source.Row + rowVector][source.File].Content != null)
				{
					return false;
				}

				return true;
			}

			if (RowDiff == 1 && FileDiff == 0 && target.Content == null ||
				(target.Content != null && RowDiff == 1 && (FileDiff == 1 || FileDiff == -1) && target.Content.Color != source.Content.Color))	// attacking enemy piece
			{
				return true;
			}
			else return false;
		}

		public List<(int, int)> getValidMoves(Square source, ChessGame game)
		{
			var moves = new List<(int, int)>();

			int colorScalar = 1;
			if (Color == "W") colorScalar *= -1;

			for (int i = 1; i <= 2; i++)
			{
				if (game.gameArr[source.Row + (i * colorScalar)][source.File].Content == null)
				{
					break;
				}
				moves.Add((source.Row + (i * colorScalar), source.File));
			}

			return moves;
		}
	}
}
