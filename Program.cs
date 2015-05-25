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
			//var shells = ShellLoader.Load("filename");
			var shells = new List<Shell>();
			// da se zaredqt snarqdite ( moje i masiv a ne list da se polzva )
			Shell shell = new Shell();
			shell.c = '@';
			shells.Add(shell);
			shell.c = 'o';
			shells.Add(shell);

			Terrain terrain = new Terrain();
			//terrain.LoadFromFile("filename");
			terrain.StartTestLVL();

			// TODO: da se dobavi menu
			// podava se samo 1 teren
			Game game = new Game();
			game.NewGame(shells, terrain);
		}
	}
}
