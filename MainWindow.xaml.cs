using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace SpeedTester
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string currentWord = "";
		Paragraph paragraph;
		public MainWindow()
		{
			InitializeComponent();
			paragraph = new Paragraph();
			type.Focus();
			UpdateRichTextBox();
			//UpdateRichTextBox(paragraph);
		}

		private string GetNextWord()
		{
			string str = getStringFromRichTextBox(testRichTextBox);
			int startIndex = str.IndexOf(currentWord);
			int endIndex = str.IndexOf(" ", startIndex);
			return str.Substring(startIndex, endIndex - startIndex);
		}

		private void UpdateRichTextBox(Paragraph paragraph)
		{

		}

		private void UpdateRichTextBox()
		{
			Paragraph paragraph = new Paragraph();
			testRichTextBox.Document = new FlowDocument(paragraph);
			paragraph.Inlines.Add("The quick brown fox jumps over the lazy dog");
			currentWord = "The";
		}

		private string getStringFromRichTextBox(RichTextBox richTextBox)
		{
			TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
			return textRange.Text;
		}

		private void type_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			string text = textBox.Text;
			if (text.EndsWith(" "))
			{
				Debug.WriteLine("Clearing");
				textBox.Text = "";
				currentWord = GetNextWord();
			}

			if (currentWord.Contains(text))
			{
				Debug.WriteLine("Partial text found, call UpdateRichTextBox");
			}
		}
	}
}
