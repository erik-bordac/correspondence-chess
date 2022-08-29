using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Chess_SchoolProject.ChessFigures;

namespace Chess_SchoolProject.ChessFigures
{
	class King : IFigure
	{
		public string ImgPath { get; set; }
		public string Color { get; set; }

		public King(string color)
		{
			Color = color;

			ImgPath = (Color == "W") ? "./Resources/Wking.png" : "./Resources/Bking.png";
		}

		public bool IsValidMove(Square source, Square target, ChessGame game)
		{
			// Check for castle
			if (target.Content is Rook && target.Content.Color == source.Content.Color)
			{
				Rook rook = (Rook)target.Content;
				if (rook.FirstMove == false) return false;

				// Determine if there are no pieces between king/rook
				int startPoint = Math.Min(source.File, target.File);
				int endPoint = Math.Max(source.File, target.File);

				for (int i = startPoint + 1; i < endPoint; i++)
				{
					if (game.gameArr[source.Row][i].Content != null)
					{
						return false;
					}
				}


				return true;
			}

			if (Math.Abs(source.Row - target.Row) <= 1 && Math.Abs(source.File - target.File) <= 1)
			{
				return true;
			}
			else return false;
		}
	}
}
