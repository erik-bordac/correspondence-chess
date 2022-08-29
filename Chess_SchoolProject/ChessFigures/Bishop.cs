using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_SchoolProject.ChessFigures
{
	class Bishop : IFigure
	{
		public string ImgPath { get; set; }
		public string Color { get; set; }

		public Bishop (string color)
		{
			Color = color;

			ImgPath = (Color == "W") ? "./Resources/Wbishop.png" : "./Resources/Bbishop.png";
		}

		public bool IsValidMove(Square source, Square target, ChessGame game)
		{
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
	}
}
