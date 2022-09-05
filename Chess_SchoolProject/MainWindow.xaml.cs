using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

		private void Img_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			// Store the mouse position
			startPoint = e.GetPosition(null);
		}

		private void Img_MouseMove(object sender, MouseEventArgs e)
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
			
			RemoveBorder(sourceLabel);
		}

		private void DEBUG(object sender, MouseButtonEventArgs e)
		{
			Label sourceLabel = sender as Label;
			Square source = (Square)sourceLabel.DataContext;

			MessageBox.Show(Game.gameArr[0][0].Content.GetType().Name.ToString());
		}

		private void Grid_Enter(object sender, DragEventArgs e)
		{
			Label sourceLabel = sender as Label;
			Square source = (Square)sourceLabel.DataContext;

			Square target = (Square)e.Data.GetData("Square");

			sourceLabel.BorderBrush = Brushes.Black;
			sourceLabel.BorderThickness = new Thickness(sourceLabel.ActualWidth / 15);
		}

		private void Grid_Leave(object sender, DragEventArgs e)
		{
			Label sourceLabel = sender as Label;

			RemoveBorder(sourceLabel);
		}

		private void RemoveBorder(Label element)
		{
			element.BorderBrush = null;
			element.BorderThickness = new Thickness(0);
		}

		private void Img_MouseEnter(object sender, MouseEventArgs e)
		{
			gridControl.Cursor = Cursors.Hand;
		}

		private void Img_MouseLeave(object sender, MouseEventArgs e)
		{
			gridControl.Cursor = Cursors.Arrow;
		}

		protected override void OnGiveFeedback(System.Windows.GiveFeedbackEventArgs e)
		{
			// Change cursor when drag dropping
			Mouse.SetCursor(Cursors.Hand);
			e.Handled = true;
		}

	}
}
