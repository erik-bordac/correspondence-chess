using Chess_SchoolProject.ChessFigures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess_SchoolProject
{
	public partial class MainWindow : Window
	{

		Point startPoint;

		ChessGame Game;

		public MainWindow()
		{
			InitializeComponent();

			// Initialize chess game
			Game = new ChessGame(gridControl);
		}

		private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			// Store the mouse position
			startPoint = e.GetPosition(null);
		}

		private void Grid_MouseMove(object sender, MouseEventArgs e)
		{
			// Get the current mouse position
			Point mousePos = e.GetPosition(null);
			Vector diff = startPoint - mousePos;

			if (e.LeftButton == MouseButtonState.Pressed &&
				(Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
				Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
			{
				// Get the dragged item
				Image img = sender as Image;
				Label labelAncestor =
					FindAnchestor<Label>((DependencyObject)e.OriginalSource);


				// data behind grid square
				Square square = (Square)labelAncestor.DataContext;

				//MessageBox.Show(square.Color);

				// Initialize the drag drop operation
				DataObject dragData = new DataObject("Square", square);
				DragDrop.DoDragDrop(img, dragData, DragDropEffects.Move);
			}
		}

		private static T FindAnchestor<T>(DependencyObject current)
			where T : DependencyObject
		{
			do
			{
				if (current is T)
				{
					return (T)current;
				}
				current = VisualTreeHelper.GetParent(current);
			}
			while (current != null);
			return null;
		}

		private void Grid_Drop(object sender, DragEventArgs e)
		{

			Label sourceLabel = sender as Label;
			Square source = (Square)sourceLabel.DataContext;

			Square target = (Square)e.Data.GetData("Square");

			Game.Move(target, source);
		}

		private void DEBUG(object sender, MouseButtonEventArgs e)
		{
			Label sourceLabel = sender as Label;
			Square source = (Square)sourceLabel.DataContext;
			MessageBox.Show(source.Content.HasMoved.ToString());
		}
	}
}
