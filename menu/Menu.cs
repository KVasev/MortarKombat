using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Menu
{
	public class Menu
	{
		public static string[] menuScreen = new string[50];
		public static void Draw()
		{
			for (int i = 0; i < menuScreen.GetLength(0); ++i)
			{
				Console.WriteLine(menuScreen[i]);
			}
		}

		public struct item
		{
			public int x;
			public int y;
			public string text;
			public item(int x, int y, string text)
			{
				this.x = x;
				this.y = y;
				this.text = text;
			}
		}

		static void Main(string[] args)
		{

			int width = 150, height = 50;
			int savedWidth = Console.WindowWidth, savedHeight = Console.WindowHeight,
				savedBufferWidth = Console.WindowWidth, savedBufferHeight = Console.WindowHeight;
			try
			{
				Console.SetWindowSize(width, height);
				Console.SetBufferSize(width, height);


				LoadFromFile(@"..\..\menu.txt");
				Draw();
				Console.SetCursorPosition(0, 0);
				Console.Write(" ");

				int menuCursorPosition = 0;
				var items = new item[] {
				new item(18, 32, "NEW GAME"),
				new item(56, 32, "HELP"),
				new item(90, 32, "HIGHSCORE"),
				new item(128, 32, "EXIT")
			};
				Console.ResetColor();				
				bool selectedButton = false;
				bool quit = false;
				ConsoleKeyInfo keyMenu;
				while (!quit)
				{

					selectedButton = false;
					keyMenu = Console.ReadKey(true);
					switch (keyMenu.Key)
					{

						case ConsoleKey.LeftArrow:
							if (menuCursorPosition >= 1)
							{
								Console.ResetColor();
								menuCursorPosition--;		
							}
							break;
						case ConsoleKey.RightArrow:
							if (menuCursorPosition <= 2)
							{
								Console.ResetColor();
								menuCursorPosition++;

							}
							break;
						case ConsoleKey.Spacebar:
							selectedButton = true;
							Console.Clear();
							break;
					}
					if (selectedButton)
					{
						switch (menuCursorPosition)
						{
							case 0:
								break;
							case 1:
								break;
							case 2:
								break;
							case 3: quit = true;

								break;
						}
						Console.SetCursorPosition(0, 0);
						Draw();
					}
					for (int i = 0; i <= 3; i++)
					{
						if (i == menuCursorPosition)
						{
							Console.SetCursorPosition(items[i].x, items[i].y);
							Console.BackgroundColor = ConsoleColor.Yellow;
							Console.ForegroundColor = ConsoleColor.Black;
							Console.Write(items[i].text);
						}
						else
						{
							Console.SetCursorPosition(items[i].x, items[i].y);
							Console.BackgroundColor = ConsoleColor.Black;
							Console.ForegroundColor = ConsoleColor.White;
							Console.Write(items[i].text);
						}
					}
					while (Console.KeyAvailable) Console.ReadKey(true);
				}
			}
			finally
			{
				Console.ResetColor();
				Console.SetWindowSize(savedWidth, savedHeight);
				Console.SetBufferSize(savedBufferWidth, savedBufferHeight);

			}
		}
		public static void LoadFromFile(string fileName)
		{
			try
			{

				StreamReader reader = new StreamReader(fileName);
				try
				{
					using (reader)
					{
						int[] menuSize = reader.ReadLine().Split(' ').Select(int.Parse).ToArray();						
						for (int rows = 0; rows < 50; rows++)
						{
							menuScreen[rows] = reader.ReadLine();
							if (menuScreen[rows] == null)
							{
								break;
							}
							if (menuScreen[rows].Length > 145)
							{
								menuScreen[rows] = menuScreen[rows].Substring(0, 144);
							}
						}
					}
				}
				finally
				{
					reader.Close();
				}
			}
			catch (FileNotFoundException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
