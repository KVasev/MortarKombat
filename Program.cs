using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace TW_Project
{
	class Program
	{
		static void Main(string[] args)
		{
			int width = 150, height = 50;
			int savedWidth = Console.WindowWidth, savedHeight = Console.WindowHeight,
				savedBufferWidth = Console.WindowWidth, savedBufferHeight = Console.WindowHeight;

			var shells = new Shell[]
			{
				new Shell( '@' ),
				new Shell( 'o' ),
				new Shell( 'O' )
			};

			try
			{
				Console.SetWindowSize(width, height);
				Console.SetBufferSize(width, height);
				Terrain terrain = new Terrain();
				//terrain.LoadFromFile("filename");
				terrain.StartTestLVL(width, 15);

				// TODO: da se dobavi menu
				// podava se samo 1 teren
				Game game = new Game();
				game.NewGame(shells, terrain, height);
			}
			finally
			{
				Console.ResetColor();
				Console.SetWindowSize(savedWidth, savedHeight);
				Console.SetBufferSize(savedBufferWidth, savedBufferHeight);
			}
		}
	}
}