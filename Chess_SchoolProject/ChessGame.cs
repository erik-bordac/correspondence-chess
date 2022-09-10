using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
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
		public string PieceToPromote { get; set; }

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
			// Set game state to match starting position

			// Starting player
			Turn = "W";

			ClearBoard();

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
			// if move is valid do it && determine checkmate, else do nothing

			if (moveFrom == moveTo) return;

			// Check players turn
			if (moveFrom.Content.Color != Turn) return;
			
			if (moveFrom.Content.IsValidMove(moveFrom, moveTo, this))
			{
				// copy
				IFigure moveFromContent = moveFrom.Content;
				bool moveFromHasMoved = moveFrom.Content.HasMoved;
				IFigure moveToContent = null;
				bool moveToHasMoved = false;
				if (moveTo.Content != null)
				{
					moveToContent = moveTo.Content;
					moveToHasMoved = moveTo.Content.HasMoved;
				}

				if (moveFrom.Content is King) MoveKing(moveFrom, moveTo);
				else if (moveFrom.Content is Pawn) MovePawn(moveFrom, moveTo);
				else MovePiece(moveFrom, moveTo);

				King wk = Wking.Content as King;
				King bk = Bking.Content as King;

				string checkmate_result = "";
				if (Turn == "W")
				{
					if (wk.IsInCheck(Wking, this))
					{
						UndoMove(moveFrom, moveFromContent, moveFromHasMoved, moveTo, moveToContent, moveToHasMoved);
						return;
					}
					if (bk.IsInCheck(Bking, this))
					{
						ChangeTurn();
						checkmate_result = run_cmd("..\\..\\checkmate.py", getFen());
						ChangeTurn();
					}
				}
				else
				{
					if (bk.IsInCheck(Bking, this))
					{
						UndoMove(moveFrom, moveFromContent, moveFromHasMoved, moveTo, moveToContent, moveToHasMoved);
						return;
					}
					if (wk.IsInCheck(Wking, this))
					{
						ChangeTurn();
						checkmate_result = run_cmd("..\\..\\checkmate.py", getFen());
						ChangeTurn();
					}
				}
				
				if (checkmate_result.Contains("True"))
				{ 
					MessageBox.Show("Player " + Turn + " Won");
					InitializeFigures(); 
					return; 
				}

				ChangeTurn();
				return;
			}

		}
		
		public void loadFen(string fen)
		{
			// Sets game state to match given FEN notation
			// Doesnt check if given fen is valid

			// clear board
			ClearBoard();

			string[] stringarr = fen.Split(' ');

			// Turn
			Turn = stringarr[1].Contains("b") ? "B" : "W";

			// Place pieces
			int row = 0;
			int file = 0;
			foreach (char ch in stringarr[0].ToCharArray())
			{
				switch (ch)
				{
					case '/':
						row++;
						file = 0;
						break;
					case 'r':
					case 'R':
						gameArr[row][file].Content = new Rook(char.IsUpper(ch) ? "W" : "B");
						gameArr[row][file].Content.HasMoved = true;
						break;
					case 'n':
					case 'N':
						gameArr[row][file].Content = new Knight(char.IsUpper(ch) ? "W" : "B");
						break;
					case 'b':
					case 'B':
						gameArr[row][file].Content = new Bishop(char.IsUpper(ch) ? "W" : "B");
						break;
					case 'k':
					case 'K':
						gameArr[row][file].Content = new King(char.IsUpper(ch) ? "W" : "B");
						gameArr[row][file].Content.HasMoved = true;
						if (char.IsUpper(ch))
						{
							Wking = gameArr[row][file];
						} else
						{
							Bking = gameArr[row][file];
						}
						break;
					case 'p':
					case 'P':
						gameArr[row][file].Content = new Pawn(char.IsUpper(ch) ? "W" : "B");
						if (!(row == 1 || row == 6)) gameArr[row][file].Content.HasMoved = true;
						break;
					case 'q':
					case 'Q':
						gameArr[row][file].Content = new Queen(char.IsUpper(ch) ? "W" : "B");
						break;
					default:        // number
						int n = int.Parse(ch.ToString());
						file += n - 1;
						break;
				}
				if (ch != '/') file++;
			}

			// Castling rights
			bool Q = stringarr[2].ToString().Contains("Q");
			bool K = stringarr[2].ToString().Contains("K");
			bool q = stringarr[2].ToString().Contains("q");
			bool k = stringarr[2].ToString().Contains("k");

			if (Q)
			{
				gameArr[7][0].Content.HasMoved = false;
				Wking.Content.HasMoved = false;
			}
			if (K)
			{
				gameArr[7][7].Content.HasMoved = false;
				Wking.Content.HasMoved = false;
			}

			if (q)
			{
				gameArr[0][0].Content.HasMoved = false;
				Bking.Content.HasMoved = false;
			}
			if (k)
			{
				gameArr[0][7].Content.HasMoved = false;
				Bking.Content.HasMoved = false;
			}

			// en passant
			if (stringarr[3] != "-")
			{
				char f = stringarr[3][0];
				int _file = (int)f - (int)'a';

				char r = stringarr[3][1];
				int _row = Math.Abs(int.Parse(r.ToString()) - 8);


				EnPassantAttackSquare = gameArr[_row][_file];
				EnPassantAttackSquare.EnPassantFlag = true;
				if (_row == 2)		// black en passant attack square
				EnPassantRemoveSquare = gameArr[_row + 1][_file];
				else				// white en passant atack square
				EnPassantRemoveSquare = gameArr[_row - 1][_file];

			}
		}

		public string getFen()
		{
			// Returns string with current position written in FEN notation

			string fen = "";
			for (int i = 0; i < 8 ; i++)
			{
				for(int j = 0; j < 8; j++)
				{

					if (gameArr[i][j].Content == null)
					{
						char[] chararr = fen.ToCharArray();
						if (Char.IsNumber(chararr[fen.Length - 1]))
						{
							int n = int.Parse(chararr[fen.Length - 1].ToString());
							n++;
							chararr[fen.Length - 1] = Char.Parse(n.ToString());
							fen = new string(chararr);
						} else
						{
							fen += "1";
						}

						continue;
					}

					IFigure content = gameArr[i][j].Content;
					switch (content.GetType().Name.ToString())
					{
						case "Rook":
							fen += content.Color == "W" ? "R" : "r";
							break;
						case "Knight":
							fen += content.Color == "W" ? "N" : "n";
							break;
						case "Bishop":
							fen += content.Color == "W" ? "B" : "b";
							break;
						case "Queen":
							fen += content.Color == "W" ? "Q" : "q";
							break;
						case "King":
							fen += content.Color == "W" ? "K" : "k";
							break;
						case "Pawn":
							fen += content.Color == "W" ? "P" : "p";
							break;
					}
				}
				if (i != 7) fen += "/";
			}

			fen += " " + Turn.ToLower() + " "; // player move
			
			// castling rights
			if (!Wking.Content.HasMoved && gameArr[7][7].Content is Rook && !gameArr[7][7].Content.HasMoved) fen += "K";
			if (!Wking.Content.HasMoved && gameArr[7][0].Content is Rook && !gameArr[7][0].Content.HasMoved) fen += "Q";
			
			if (!Bking.Content.HasMoved && gameArr[0][7].Content is Rook && !gameArr[0][7].Content.HasMoved) fen += "k";
			if (!Bking.Content.HasMoved && gameArr[0][0].Content is Rook && !gameArr[0][0].Content.HasMoved) fen += "q";
				
			// neither side can castle
			if (fen[fen.Length-1] == ' ')	// nothing is appended from castling rights
			{
				fen += "-";
			}
			fen += " ";

			// enpassant square
			if (EnPassantAttackSquare != null)
			{
				char row = (char)((int)'a' + EnPassantAttackSquare.File);
				string r = row.ToString();
				fen += r + (Math.Abs(EnPassantAttackSquare.Row-8)).ToString() + " ";
			} else
			{
				fen += "- ";
			}

			fen += "0 0";

			return fen;
		}

		private void UndoMove(Square moveFrom, IFigure moveFromContent, bool moveFromHasMoved,
							  Square moveTo, IFigure moveToContent, bool moveToHasMoved)
		{
			// Is being called when your move gives your king in check

			moveFrom.Content = moveFromContent;
			moveFrom.Content.HasMoved = moveFromHasMoved;

			moveTo.Content = moveToContent;
			if (moveTo.Content != null)
			{
				moveTo.Content.HasMoved = moveToHasMoved;
			}
			return;
		}
		private void MoveKing(Square moveFrom, Square moveTo)
		{
			// King movement handler

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
					if (moveFrom.Content.Color == "W") { Wking = gameArr[moveFrom.Row][moveFrom.File + 2]; } 
					else { Bking = gameArr[moveFrom.Row][moveFrom.File + 2]; }

					gameArr[moveFrom.Row][moveFrom.File + 2].Content = moveFrom.Content;
					moveFrom.Content = null;
					gameArr[moveFrom.Row][moveTo.File - 2].Content = moveTo.Content;
					moveTo.Content = null;
				}
				else
				{
					// update king refference
					if (moveFrom.Content.Color == "W") { Wking = gameArr[moveFrom.Row][moveFrom.File - 2]; } 
					else { Bking = gameArr[moveFrom.Row][moveFrom.File - 2]; }

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
			// Pawn movement handler

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
			} else if (moveTo.Row == 0 || moveTo.Row == 7)	// pawn promotion
			{
				IFigure piece = new Queen(Turn);
				switch (PieceToPromote)
				{
					case "queen":
						piece = new Queen(Turn);
						break;
					case "rook":
						piece = new Rook(Turn);
						break;
					case "bishop":
						piece = new Bishop(Turn);
						break;
					case "knight":
						piece = new Knight(Turn);
						break;
				}
				RemoveEnPasssantRefs();

				moveFrom.Content = null;
				piece.HasMoved = true;
				moveTo.Content = piece;
			}

			else MovePiece(moveFrom, moveTo);
		}

		private void MovePiece(Square moveFrom, Square moveTo)
		{
			// Generic piece movement handler

			RemoveEnPasssantRefs();

			moveFrom.Content.HasMoved = true;
			moveTo.Content = moveFrom.Content;
			moveFrom.Content = null;
		}

		private void RemoveEnPasssantRefs()
		{
			// Set all EnPassant variables to null
			// Used to unmark squares after EnPassant move is no
			// longer available.

			if (EnPassantRemoveSquare != null)
			{
				EnPassantAttackSquare.EnPassantFlag = false;
			}
			EnPassantAttackSquare = null;
			EnPassantRemoveSquare = null;
		}

		private void ClearBoard()
		{
			// Clear game board
			RemoveEnPasssantRefs();
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					gameArr[i][j].Content = null;
				}
			}
		}

		private void ChangeTurn()
		{
			Turn = (Turn == "W" ? "B" : "W");
		}

		private string run_cmd(string cmd, string args)
		{
			// Execute python script in cmd, return result from stdout

			ProcessStartInfo start = new ProcessStartInfo();
			start.FileName = "C:\\Users\\erikb\\AppData\\Local\\Microsoft\\WindowsApps\\python.exe";	// python.exe absolute path
			start.Arguments = string.Format("{0} \"{1}\"", cmd, args);
			start.UseShellExecute = false;
			start.RedirectStandardOutput = true;
			using (Process process = Process.Start(start))
			{
				using (StreamReader reader = process.StandardOutput)
				{
					string result = reader.ReadToEnd();
					return result;
				}
			}
		}
	}
}
