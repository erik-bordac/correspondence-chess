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
			if (target.Content is Rook &&
				target.Content.Color == Color &&
				!HasMoved &&
				!target.Content.HasMoved)
			{
				// Determine if there are no pieces between king/rook
				int startPoint = Math.Min(source.File, target.File);
				int endPoint = Math.Max(source.File, target.File);


				King k = source.Content as King;
				for (int i = startPoint + 1; i < endPoint; i++)
				{
					if (game.gameArr[source.Row][i].Content != null)
					{
						return false;
					} else
					{
						game.gameArr[source.Row][i].Content = k;
						if (k.IsInCheck(game.gameArr[source.Row][i], game))
						{
							game.gameArr[source.Row][i].Content = null;
							return false;
						}
						else game.gameArr[source.Row][i].Content = null;
					}
				}
				return true;
			}

			if (target.Content != null &&
				source.Content.Color == target.Content.Color) return false;

			if (Math.Abs(source.Row - target.Row) <= 1 && Math.Abs(source.File - target.File) <= 1)
			{
				return true;
			}
			else return false;
		}

		public bool IsInCheck(Square source, ChessGame game)
		{
			return IsAttackedByKnight(source, game) || 
				IsAttackedOnDiagonal(source, game) ||		// bishop and queen
				IsAttackedOnRow(source, game);				// rook and queen
		}

		private bool IsAttackedOnDiagonal(Square source, ChessGame game)
		{
			// pawn check
			try
			{
				if (Color == "W")
				{	

					Square target1 = game.gameArr[source.Row - 1][source.File - 1];
					Square target2 = game.gameArr[source.Row - 1][source.File + 1];
					if (target1.Content != null && target1.Content.Color != Color && target1.Content is Pawn)
					{
						return true;
					}
					if (target2.Content != null && target2.Content.Color != Color && target2.Content is Pawn)
					{
						return true;
					}
				} else
				{
					Square target1 = game.gameArr[source.Row + 1][source.File - 1];
					Square target2 = game.gameArr[source.Row + 1][source.File + 1];
					if (target1.Content != null && target1.Content.Color != Color && target1.Content is Pawn)
					{
						return true;
					}
					if (target2.Content != null && target2.Content.Color != Color && target2.Content is Pawn)
					{
						return true;
					}
				}
			} catch (ArgumentOutOfRangeException) { }
			
			// upleft
			for (int i = 1; i < 8; i++)
			{
				try
				{
					Square targetSquare = game.gameArr[source.Row - i][source.File - i];

					if (targetSquare.Content == null) continue;

					if (targetSquare.Content.Color == Color) break;
					else
					{
						if (targetSquare.Content is Bishop ||
							targetSquare.Content is Queen)
						{
							return true;
						}
						else break;
					}
				}
				catch (ArgumentOutOfRangeException) { break; }
			}

			// upright
			for (int i = 1; i < 8; i++)
			{
				try
				{
					Square targetSquare = game.gameArr[source.Row - i][source.File + i];

					if (targetSquare.Content == null) continue;

					if (targetSquare.Content.Color == Color) break;
					else
					{
						if (targetSquare.Content is Bishop ||
							targetSquare.Content is Queen)
						{
							return true;
						}
						else break;
					}
				}
				catch (ArgumentOutOfRangeException) { break; }
			}

			// downleft
			for (int i = 1; i < 8; i++)
			{
				try
				{
					Square targetSquare = game.gameArr[source.Row + i][source.File - i];

					if (targetSquare.Content == null) continue;

					if (targetSquare.Content.Color == Color) break;
					else
					{
						if (targetSquare.Content is Bishop ||
							targetSquare.Content is Queen)
						{
							return true;
						}
						else break;
					}
				}
				catch (ArgumentOutOfRangeException) { break; }
			}

			// downright
			for (int i = 1; i < 8; i++)
			{
				try
				{
					Square targetSquare = game.gameArr[source.Row + i][source.File + i];

					if (targetSquare.Content == null) continue;

					if (targetSquare.Content.Color == Color) break;
					else
					{
						if (targetSquare.Content is Bishop ||
							targetSquare.Content is Queen)
						{
							return true;
						}
						else break;
					}
				}
				catch (ArgumentOutOfRangeException) { break; }
			}

			return false;
		}
	
		private bool IsAttackedByKnight(Square source, ChessGame game)
		{
			(int, int)[] moves = { (2, 1), (2, -1), (-2, 1), (-2, -1), (1, 2), (1, -2), (-1, 2), (-1, -2) };

			foreach ((int, int)move in moves)
			{
				try
				{
					if (game.gameArr[source.Row + move.Item1][source.File + move.Item2].Content is Knight &&
						game.gameArr[source.Row + move.Item1][source.File + move.Item2].Content.Color != Color)
					{
						return true;
					}
				} catch (ArgumentOutOfRangeException) { continue; }

			}
			return false;
		}

		private bool IsAttackedOnRow(Square source, ChessGame game)
		{
			// rowleft
			for(int i = 1; i<8; i++)
			{
				try
				{
					Square targetSquare = game.gameArr[source.Row][source.File - i];

					if (targetSquare.Content == null) continue;

					if (targetSquare.Content.Color == Color) break;
					else
					{
						if (targetSquare.Content is Rook ||
							targetSquare.Content is Queen)
						{ 
							return true;
						}
						else break;
					}
				}
				catch (ArgumentOutOfRangeException) { break; }
			}

			// rowright
			for (int i = 1; i < 8; i++)
			{
				try
				{
					Square targetSquare = game.gameArr[source.Row][source.File + i];

					if (targetSquare.Content == null) continue;

					if (targetSquare.Content.Color == Color) break;
					else
					{
						if (targetSquare.Content is Rook ||
							targetSquare.Content is Queen)
						{
							return true;
						}
						else break;
					}
				}
				catch (ArgumentOutOfRangeException) { break; }
			}

			// columnup
			for (int i = 1; i<8; i++)
			{
				try
				{
					Square targetSquare = game.gameArr[source.Row - i][source.File];

					if (targetSquare.Content == null) continue;

					if (targetSquare.Content.Color == Color) break;
					else
					{
						if (targetSquare.Content is Rook ||
							targetSquare.Content is Queen)
						{ 
							return true;
						}
						else break;
					}
				}
				catch (ArgumentOutOfRangeException) { break; }
			}

			// columndown
			for (int i = 1; i<8; i++)
			{
				try
				{
					Square targetSquare = game.gameArr[source.Row + i][source.File];

					if (targetSquare.Content == null) continue;

					if (targetSquare.Content.Color == Color) break;
					else
					{
						if (targetSquare.Content is Rook ||
							targetSquare.Content is Queen)
						{ 
							return true;
						}
						else break;
					}
				}
				catch (ArgumentOutOfRangeException) { break; }
			}

			return false;
		}
	}
}
