﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Imaging;
using Chess_SchoolProject.ChessFigures;

namespace Chess_SchoolProject
{

	class ChessGame
	{
		string Turn = "W";
		public List<List<Square>> gameArr;
		public ItemsControl gridControl { get; set; }

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
			gameArr[0][3].Content = new Queen("B");
			gameArr[0][0].Content = gameArr[0][7].Content = new Rook("B");
			gameArr[0][1].Content = gameArr[0][6].Content = new Knight("B");
			gameArr[0][2].Content = gameArr[0][5].Content = new Bishop("B");
			// black pawns
			for (int i = 0; i < 8; i++)
			{
				gameArr[1][i].Content = new Pawn("B");
			}

			// white back-rank
			gameArr[7][4].Content = new King("W");
			gameArr[7][3].Content = new Queen("W");
			gameArr[7][0].Content = gameArr[7][7].Content = new Rook("W");
			gameArr[7][1].Content = gameArr[7][6].Content = new Knight("W");
			gameArr[7][2].Content = gameArr[7][5].Content = new Bishop("W");
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
				// Check for revealed check

				// castle movement handling
				if (moveFrom.Content is King && moveTo.Content is Rook && moveFrom.Content.Color == moveTo.Content.Color)
				{
					Rook tmp = (Rook)moveTo.Content;
					tmp.FirstMove = false;

					if (moveFrom.File < moveTo.File)
					{
						gameArr[moveFrom.Row][moveFrom.File + 2].Content = moveFrom.Content;
						moveFrom.Content = null;
						gameArr[moveFrom.Row][moveTo.File - 2].Content = moveTo.Content;
						moveTo.Content = null;
					} else
					{
						gameArr[moveFrom.Row][moveFrom.File - 2].Content = moveFrom.Content;
						moveFrom.Content = null;
						gameArr[moveFrom.Row][moveTo.File + 3].Content = moveTo.Content;
						moveTo.Content = null;
					}
				} else		// non castle movement handling
				{
					moveTo.Content = moveFrom.Content;
					moveFrom.Content = null;
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
	}
}
