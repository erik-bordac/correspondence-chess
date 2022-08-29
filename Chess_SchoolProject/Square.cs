using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Threading.Tasks;
using Chess_SchoolProject.ChessFigures;

namespace Chess_SchoolProject
{
	internal class Square : ObservableObject
	{
		public int Row { get; private set; }
		public int File { get; private set; }

		//private string content;

		//public string Content
		//{
		//	get { return content; }
		//	set { 
		//		content = value;
		//		OnPropertyChanged();
		//	}
		//}

		private IFigure content;

		public IFigure Content
		{
			get { return content; }
			set 
			{ 
				content = value;
				OnPropertyChanged();
			}
		}

		private string color;

		public string Color
		{
			get { return color; }
			set { 
				color = value;
				OnPropertyChanged();
			}
		}

		public Label Element { get; set; }

		public Square(int row, int file, string color, Label element=null, string content="")
		{
			Row = row;
			File = file;
			Color = color;
			Element = element;
			Content = null;
		}
	}	
}
