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
		private readonly string testRichTextBoxText = "The quick brown fox jumps over the lazy dog";
		private string[] words;
		private string currentWord = "";
		private int currentIndex = 0;
		public MainWindow()
		{
			InitializeComponent();
			type.Focus();
			Init();
		}

		private void Init()
		{
			Paragraph paragraph = new Paragraph();
			testRichTextBox.Document = new FlowDocument(paragraph);
			paragraph.Inlines.Add(testRichTextBoxText);
			words = testRichTextBoxText.Split(' ');
			currentWord = words.First();
		}

		private string GetNextWord()
		{
			Debug.WriteLine("Current word: " + currentWord);
			string str = testRichTextBoxText;
			int startIndex = str.IndexOf(currentWord, currentIndex);
			int endIndex = str.IndexOf(" ", startIndex);
			startIndex = endIndex + 1;
			endIndex = str.IndexOf(" ", endIndex + 1);
			currentIndex = startIndex - 1;
			//if (startIndex < currentIndex)
			//{
			//	Debug.WriteLine("Repetitive word found");
			//	startIndex = str.IndexOf(currentWord, startIndex);
			//	endIndex = str.IndexOf(" ", endIndex);
			//	currentIndex = endIndex;
			//}
			//else
			//{
			//	currentIndex = endIndex;
			//}
			Debug.WriteLine("Length of currentWord: " + (endIndex - startIndex));
			try
			{
				return str.Substring(startIndex, endIndex - startIndex);
			}
			catch (Exception e)
			{
				return str.Split(' ').Last();
			}
		}

		private void UpdateRichTextBox(string text)
		{
			//Highlight the entire word and color the specific portion of word
			FlowDocument flowDocument = new FlowDocument();
			Paragraph pr = new Paragraph();

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < currentIndex; i++)
			{
				sb.Append(words[i]);
				sb.Append(" ");
			}

			Debug.WriteLine(sb.ToString());
			
			Run run = new Run(sb.ToString());
			pr.Inlines.Add(run);

			int length = text.Length;
			run = new Run(words[currentIndex].Substring(0, length));
			run.Foreground = new SolidColorBrush(Colors.Red);
			run.FontStyle = FontStyles.Italic;

			pr.Inlines.Add(run);
			int length2 = words[currentIndex].Length;
			run = new Run(words[currentIndex].Substring(length, length2 - length) + " ");
			run.FontStyle = FontStyles.Italic;
			pr.Inlines.Add(run);

			//run = new Run(words[currentIndex] + " ");
			//run.Foreground = new SolidColorBrush(Colors.Red);
			//run.FontStyle = FontStyles.Italic;
			//pr.Inlines.Add(run);
			sb.Clear();

			for (int i = currentIndex + 1; i < words.Length; i++)
			{
				sb.Append(words[i]);
				sb.Append(" ");
			}

			Debug.WriteLine(sb.ToString());

			run = new Run(sb.ToString());
			pr.Inlines.Add(run);

			flowDocument.Blocks.Add(pr);
			testRichTextBox.Document = flowDocument;

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
				Debug.WriteLine("Space bar pressed");
				// To remove space from text
				string actualText = text.TrimEnd(' ');
				if (actualText == currentWord)
				{
					Debug.WriteLine("Clearing");
					textBox.Text = string.Empty;
					UpdateCurrentWord();
					Debug.WriteLine("Next current word: " + currentWord);
				}
			}

			if (currentWord.Contains(text))
			{
				//Debug.WriteLine("Partial text found, call UpdateRichTextBox");
				UpdateRichTextBox(text);
			}
		}

		private void UpdateCurrentWord()
		{
			Debug.WriteLine("Current word: " + currentWord);

			if (currentIndex >= words.Length - 1)
			{
				MessageBox.Show("Typing test over!");
				currentIndex = -1;
			}

			currentWord = words[currentIndex + 1];
			currentIndex++;
			
		}
	}
}
