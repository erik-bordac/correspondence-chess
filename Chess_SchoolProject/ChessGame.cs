using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Chess_SchoolProject.ChessFigures;

namespace Chess_SchoolProject
{

	class ChessGame
	{

		public List<List<Square>> gameArr;
		public ItemsControl gridControl { get; set; }


		string Turn = "W";
		Square EnPassantAttackSquare;
		Square EnPassantRemoveSquare;

		public Square Wking;
		public Square Bking;

		public ChessGame(ItemsControl list)
		{
			gridControl = list;
			
			gameArr = new List<List<Square>>();
			
			for(int i = 0; i < 8; i++)
			{
				gameArr.Add(new List<Square>());
				for (int j = 0; j < 8; j++)
				{
					Square s = new Square(i, j, (i + j) % 2 == 0 ? "#77ABB7" : "#476D7C");
					gameArr[i].Add(s);
				}
			}

			InitializeFigures();

			gridControl.ItemsSource = gameArr;
		}

		private void InitializeFigures()
		{
			// black back-rank
			gameArr[0][4].Content = new King("B");
			Bking = gameArr[0][4];
			gameArr[0][3].Content = new Queen("B");
			gameArr[0][0].Content = new Rook("B");
			gameArr[0][7].Content = new Rook("B");
			gameArr[0][1].Content = new Knight("B");
			gameArr[0][6].Content = new Knight("B");
			gameArr[0][2].Content = new Bishop("B");
			gameArr[0][5].Content = new Bishop("B");
			// black pawns
			for (int i = 0; i < 8; i++)
			{
				gameArr[1][i].Content = new Pawn("B");
			}

			// white back-rank
			gameArr[7][4].Content = new King("W");
			Wking = gameArr[7][4];
			gameArr[7][3].Content = new Queen("W");
			gameArr[7][0].Content = new Rook("W");
			gameArr[7][7].Content = new Rook("W");
			gameArr[7][1].Content = new Knight("W");
			gameArr[7][6].Content = new Knight("W");
			gameArr[7][2].Content = new Bishop("W");
			gameArr[7][5].Content = new Bishop("W");
			// white pawns
			for (int i = 0; i < 8; i++)
			{
				gameArr[6][i].Content = new Pawn("W");
			}
		}

		public void Move(Square moveFrom, Square moveTo)
		{
			if (moveFrom == moveTo) return;

			// Check players turn
			if (moveFrom.Content.Color != Turn) return;

			if (moveFrom.Content.IsValidMove(moveFrom, moveTo, this))
			{

				if (moveFrom.Content is King) MoveKing(moveFrom, moveTo);
				else if (moveFrom.Content is Pawn) MovePawn(moveFrom, moveTo);
				else MovePiece(moveFrom, moveTo);

				King wk = Wking.Content as King;
				King bk = Bking.Content as King;

				if (Turn == "W")
				{
					if (bk.IsInCheck(Bking, this))
					{
						MessageBox.Show("BCheck");
					}
				}
				else
				{
					if(wk.IsInCheck(Wking, this))
					{
						MessageBox.Show("WCheck");
					}
				}


				// Change player turn
				if (Turn == "W")
				{
					Turn = "B";
				} else
				{
					Turn = "W";
				}
				return;
			}

		}
		private void MoveKing(Square moveFrom, Square moveTo)
		{
			// castle movement handling
			if (moveTo.Content is Rook &&
				moveFrom.Content.Color == moveTo.Content.Color &&
				!moveTo.Content.HasMoved &&
				!moveFrom.Content.HasMoved)
			{
				RemoveEnPasssantRefs();

				moveFrom.Content.HasMoved = true;
				moveTo.Content.HasMoved = true;

				if (moveFrom.File < moveTo.File)
				{
					// update king refference
					if (moveFrom.Content.Color == "W") { Wking = gameArr[moveFrom.Row][moveTo.File + 2]; } 
					else { Bking = gameArr[moveFrom.Row][moveTo.File + 2]; }

					gameArr[moveFrom.Row][moveFrom.File + 2].Content = moveFrom.Content;
					moveFrom.Content = null;
					gameArr[moveFrom.Row][moveTo.File - 2].Content = moveTo.Content;
					moveTo.Content = null;
				}
				else
				{
					// update king refference
					if (moveFrom.Content.Color == "W") { Wking = gameArr[moveFrom.Row][moveTo.File - 2]; } 
					else { Bking = gameArr[moveFrom.Row][moveTo.File - 2]; }

					gameArr[moveFrom.Row][moveFrom.File - 2].Content = moveFrom.Content;
					moveFrom.Content = null;
					gameArr[moveFrom.Row][moveTo.File + 3].Content = moveTo.Content;
					moveTo.Content = null;
				}
			} else
			{
				// update king refference
				{
					if (moveFrom.Content.Color == "W")
					{
						Wking = moveTo;
					} else
					{
						Bking = moveTo;
					}
				}

				MovePiece(moveFrom, moveTo);
			}


		}

		private void MovePawn(Square moveFrom, Square moveTo)
		{
			if (Math.Abs(moveFrom.Row - moveTo.Row) == 2)  // en passant move
			{
				RemoveEnPasssantRefs();

				if (moveFrom.Content.Color == "W")
				{
					EnPassantAttackSquare = gameArr[moveFrom.Row - 1][moveFrom.File];
				}
				else
				{
					EnPassantAttackSquare = gameArr[moveFrom.Row + 1][moveFrom.File];
				}

				EnPassantAttackSquare.EnPassantFlag = true;
				EnPassantRemoveSquare = moveTo;

				moveFrom.Content.HasMoved = true;
				moveTo.Content = moveFrom.Content;
				moveFrom.Content = null;

			}
			else if (moveTo == EnPassantAttackSquare)   // en passant attack
			{
				EnPassantAttackSquare.Content = moveFrom.Content;
				moveFrom.Content = null;
				EnPassantRemoveSquare.Content = null;

				RemoveEnPasssantRefs();
			}
			else MovePiece(moveFrom, moveTo);
		}

		private void MovePiece(Square moveFrom, Square moveTo)
		{
			RemoveEnPasssantRefs();

			moveFrom.Content.HasMoved = true;
			moveTo.Content = moveFrom.Content;
			moveFrom.Content = null;
		}

		private void RemoveEnPasssantRefs()
		{
			if (EnPassantRemoveSquare != null)
			{
				EnPassantAttackSquare.EnPassantFlag = false;
			}
			EnPassantAttackSquare = null;
			EnPassantRemoveSquare = null;
		}
	}
}
