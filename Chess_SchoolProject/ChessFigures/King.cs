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
		public bool HasMoved { get; set; }
		public string ImgPath { get; set; }
		public string Color { get; set; }

		public King(string color)
		{
			Color = color;
			HasMoved = false;

			ImgPath = (Color == "W") ? "./Resources/Wking.png" : "./Resources/Bking.png";
		}

		public bool IsValidMove(Square source, Square target, ChessGame game)
		{
			// Check for castle
			if (target.Content is Rook && target.Content.Color == source.Content.Color && !HasMoved && !target.Content.HasMoved)
			{
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

		public bool IsInCheck(Square source, ChessGame game)
		{
			bool result;

			result = IsAttackedOnDiagonal(source, game);
			result = IsAttackedByKnight(source, game);
			result = IsAttackedOnRow(source, game);		// Rook and Queen
			
			return true;
		}

		private bool IsAttackedOnDiagonal(Square source, ChessGame game)
		{
			return false;
		}

		private bool IsAttackedByKnight(Square source, ChessGame game)
		{
			return false;
		}

		private bool IsAttackedOnRow(Square source, ChessGame game)
		{
			for(int i = 0; i<8; i++)
			{
				try
				{
					if (game.gameArr[source.Row][source.File - i].Content.Color == Color) break;
					else	// if Content is empty then NulLReferenceException is raised
					{
						if (game.gameArr[source.Row][source.File - i].Content is Rook ||
							game.gameArr[source.Row][source.File - i].Content is Queen)
						{ 
							MessageBox.Show("Check");
							return true;
						}
						else break;
					}
				}
				catch (NullReferenceException) { }
				catch (IndexOutOfRangeException) { break; }
			}

			return false;
		}
	}
}
