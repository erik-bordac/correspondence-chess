using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_SchoolProject.ChessFigures
{
	interface IFigure
	{
		string ImgPath { get; set; }
		string Color { get; set; }
		bool HasMoved { get; set; }
		bool IsValidMove(Square source, Square target, ChessGame game);
	}
}
