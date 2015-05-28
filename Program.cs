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

			try
			{
				Console.SetWindowSize(width, height);
				Console.SetBufferSize(width, height);

				var menu = new Menu();
				menu.Start(width, height);
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