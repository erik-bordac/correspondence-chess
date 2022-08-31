using System.Windows.Controls;
using Chess_SchoolProject.ChessFigures;

namespace Chess_SchoolProject
{
	internal class Square : ObservableObject
	{
		public int Row { get; private set; }
		public int File { get; private set; }
		public bool EnPassantFlag { get; set; }

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
