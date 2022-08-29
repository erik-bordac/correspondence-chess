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
		private bool FirstMove = true;
		public string ImgPath { get; set; }
		public string Color { get; set; }

		public Pawn (string color)
		{
			Color = color;

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


			if (RowDiff == 2 && FileDiff == 0 && FirstMove && target.Content == null)
			{
				//TODO: Implement enpassant

				//piece in front of pawn
				int rowVector = Color == "B" ? 1 : -1;

				if (game.gameArr[source.Row + rowVector][source.File].Content != null)
				{
					return false;
				}

				FirstMove = false;
				return true;
			}

			if (RowDiff == 1 && FileDiff == 0 && target.Content == null ||
				(target.Content != null && RowDiff == 1 && (FileDiff == 1 || FileDiff == -1) && target.Content.Color != source.Content.Color))	// attacking enemy piece
			{
				FirstMove = false;
				return true;
			}
			else return false;
		}
	}
}
