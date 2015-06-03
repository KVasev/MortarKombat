using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TW_Project
{
	public class Menu
	{
		const string helpString =
			@"<<<HELP>>>
Your objective is to destroy the evil illuminati triangle. Ammo is scarce and every bit counts , so aim carefully!
Use the UP and DOWN arrows to adjust the angle of the shot, and the LEFT and RIGHT arrows to adjust the power of the shot.
Press spacebar to shoot.
Every round the power and the direction of the wind changes.
There are 3 types of ammo - light , heavy and frag.
Good luck! Don't let the illuminati win!";
		private string[] menuScreen = new string[50];
		private void Draw()
		{
			for (int i = 0; i < menuScreen.GetLength(0); ++i)
			{
				Console.WriteLine(menuScreen[i]);
			}
		}

		private struct item
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
		private void PrintHelp()
		{
			Console.WriteLine(helpString);
			Console.WriteLine("Press any key to go back...");
			while (!Console.KeyAvailable) ;
			while (Console.KeyAvailable) Console.ReadKey(true);
		}

	    void PrintMenu(item[] items,int menuCursorPosition)
	    {
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
	    }
		public void Start(int width, int height)
		{
			var shells = new Shell[]
			{
				new Shell( '@' ),
				new Shell( 'o' ),
				new Shell( 'O' )
			};

			Terrain terrain = new Terrain();
			Game game = new Game();

			LoadFromFile(@"..\..\menu.txt");
			Draw();
			Console.SetCursorPosition(0, 0);
			//Console.Write(" ");

			int menuCursorPosition = 0;
			var items = new item[] {
				new item(18, 32, "NEW GAME"),
				new item(56, 32, "HELP"),
				new item(90, 32, "HIGHSCORE"),
				new item(128, 32, "EXIT")
			};

		    PrintMenu(items, menuCursorPosition);
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
							terrain.LoadFromFile("../../terrain-levels/map");
							//terrain.StartTestLVL(width, 15);

							game.NewGame(shells, terrain, height);
							Console.Clear();
							break;
						case 1:
							PrintHelp();
							break;
						case 2:
							break;
						case 3: quit = true;

							break;
					}
					Console.SetCursorPosition(0, 0);
					Draw();
				}
                PrintMenu(items, menuCursorPosition);
				while (Console.KeyAvailable) Console.ReadKey(true);
			}
			Console.ResetColor();
			Console.Clear();
		}
		private void LoadFromFile(string fileName)
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
