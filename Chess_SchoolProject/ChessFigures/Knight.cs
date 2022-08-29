using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_SchoolProject.ChessFigures
{
	class Knight : IFigure
	{
		public bool HasMoved { get; set; }
		public string ImgPath { get; set; }
		public string Color { get; set; }

		public Knight (string color)
		{
			Color = color;

			ImgPath = (Color == "W") ? "./Resources/Wknight.png" : "./Resources/Bknight.png";
		}

		public bool IsValidMove(Square source, Square target, ChessGame game)
		{
			if (target.Content != null && source.Content.Color == target.Content.Color) return false;

			if ( (Math.Abs(source.Row - target.Row) == 1 && Math.Abs(source.File - target.File) == 2) ||
				 (Math.Abs(source.Row - target.Row) == 2 && Math.Abs(source.File - target.File) == 1) )
			{
				return true;
			}
			else return false;
		}
	}
}
