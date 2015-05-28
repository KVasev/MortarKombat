using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TW_Project
{
	public class Game
	{
		const char trail = '.';
		bool quitGame;

		private void DrawShellAndParabola(ref Shell shell, char trail, int terrainWidth)
		{
			Console.SetCursorPosition((int)shell.lastPosition.X, (int)shell.lastPosition.Y);
			Console.Write(trail);

			if (shell.position.Y > 0 && shell.position.X >= 0 && shell.position.X < terrainWidth)
			{
				Console.SetCursorPosition((int)shell.position.X, (int)shell.position.Y);
				Console.Write(shell.c);

				shell.lastPosition = shell.position;
			}
		}

		private bool TerrainCollisionDetection(float time, vector startingPosition, vector velocityVector, int terrainOffset, ref Shell shell, ref Terrain terrain)
		{
			const string collisionBlocks = @"#-|0/\_";
			const float G = 0.98f;
			//const float G = 9.8f;

			shell.position.X = velocityVector.X * time + startingPosition.X;
			shell.position.Y = 0.5f * G * time * time + velocityVector.Y * time + startingPosition.Y;

			vector direction = new vector(shell.position.X - shell.lastPosition.X, shell.position.Y - shell.lastPosition.Y);
			float directionOffset = 0.0f;

			int x = 0, y = 0;
			bool hitCollisionBlock = false;
			while (directionOffset < 1.0f)
			{
				x = (int)(direction.X * directionOffset + shell.lastPosition.X);
				y = (int)(direction.Y * directionOffset + shell.lastPosition.Y) - terrainOffset;

				if (x < 0 || x >= terrain.GetWidth() || y >= terrain.GetHeight())
				{
					return true;
				}

				if (y >= 0 && collisionBlocks.IndexOf(terrain.UnsafeGetBlock(x, y)) >= 0)
				{
					hitCollisionBlock = true;
					break;
				}

				directionOffset += 0.05f;
			}

			if (hitCollisionBlock)
			{
				shell.position.X = direction.X * directionOffset + shell.lastPosition.X;
				shell.position.Y = direction.Y * directionOffset + shell.lastPosition.Y;

				switch (terrain.UnsafeGetBlock(x, y))
				{
					case '0':
						quitGame = true;
						return true;
					case '-':
					case '|':
						return true;
				}
				// trqbva da e shell.type ili tam kakto go krustim :D
				switch (shell.c)
				{
					case 'o':
						break;
					case 'O':
					case '@':
						int startY = y - 1,
							startX = x - 1;
						int endY = y + 2,
							endX = x + 2;

						for (int i = startY; i < endY; ++i)
						{
							for (int j = startX; j < endX; ++j)
							{
								// proverka dali ne e po izdrujliv material
								switch (terrain.GetBlock(j, i))
								{
									case '\\':
									case '/':
									case '_':
									case '#':
										terrain.ChangeBlock(j, i, ' ');

										Console.SetCursorPosition(j, i + terrainOffset);
										Console.Write(' ');
										break;
									case '0':
										quitGame = true;
										return true;
								}
							}
						}
						break;
				}
				terrain.ChangeBlock(x, y, shell.c);// DEBUG

				Console.SetCursorPosition(x, y + terrainOffset);
				Console.Write(shell.c);
				return true;
			}

			return false;
		}

		public void NewGame(Shell[] shells, Terrain terrain, int height)
		{
			const double toRad = Math.PI / 180.0;
			float deltaTime = 0.1f, time;
			int terrainOffset = height - terrain.GetHeight() - 1;
			quitGame = false;

			vector pos = new vector(0, 8 + terrainOffset);
			//velocity = new vector(5, -5);

			int shotsLeft = 8,
				shellType = 0;

			float angle = 45.0f;
			float shotForce = 5.0f;

			Random rnd = new Random();

			string emptyString = new string(' ', terrainOffset * terrain.GetWidth());
			while (shotsLeft > 0 && !quitGame)
			{
				int windForce = rnd.Next(-5, 6);

				//Console.Clear();// ako cqloto pole e v masiv i ne se vika clear ne premigva ?
				//Console.SetCursorPosition(0, terrainOffset);
				Console.SetCursorPosition(0, 0);
				Console.Write(emptyString);
				terrain.Draw();

				bool shotInput = false;
				ConsoleKeyInfo key;
				while (!shotInput)
				{
					Console.SetCursorPosition(0, height - 1);
					Console.Write(new string(' ', terrain.GetWidth() - 1));

					Console.SetCursorPosition(0, height - 1);
					Console.Write("Shot force: {0}", shotForce);
					Console.SetCursorPosition(20, height - 1);
					Console.Write("Shot angle: {0}", angle);
					Console.SetCursorPosition(40, height - 1);
					Console.Write("Shots left: {0}", shotsLeft);

					Console.SetCursorPosition(60, height - 1);
					if (windForce < 0)
					{
						Console.Write("{0} wind", new string('<', -windForce));
					}
					else
					{
						Console.Write("wind {0}", new string('>', windForce));
					}

					Console.SetCursorPosition(80, height - 1);
					Console.Write("Shell type: {0}", shellType);

					key = Console.ReadKey(true);
					switch (key.Key)
					{
						case ConsoleKey.UpArrow:
							if (angle < 85)
							{
								angle += 0.5f;
							}
							break;
						case ConsoleKey.LeftArrow:
							if (shotForce > 1.0f)
							{
								shotForce -= 0.5f;
							}
							break;
						case ConsoleKey.RightArrow:
							if (shotForce < 15.0f)
							{
								shotForce += 0.5f;
							}
							break;
						case ConsoleKey.DownArrow:
							if (angle > 5)
							{
								angle -= 0.5f;
							}
							break;
						case ConsoleKey.OemComma:
							if (shellType > 0)
							{
								--shellType;
							}
							break;
						case ConsoleKey.OemPeriod:
							if (shellType < shells.Length - 1)
							{
								++shellType;
							}
							break;
						case ConsoleKey.Spacebar:
							shotInput = true;
							break;
					}
					while (Console.KeyAvailable) Console.ReadKey(true);

					Thread.Sleep(50);
				}

				var currentShell = shells[shellType];
				currentShell.lastPosition = pos;

				if (currentShell.c == 'O')
				{
					windForce -= 2;
				}
				else if (currentShell.c == '@')
				{
					windForce -= 1;
				}

				currentShell.velocity.X = (float)Math.Cos(angle * toRad) * (shotForce + windForce);
				currentShell.velocity.Y = -(float)Math.Sin(angle * toRad) * (shotForce + windForce);

				time = 0;
				var activeShells = new[]{
					currentShell
				};

				int atShell;
				while (true)
				{
					atShell = 0;
					while (atShell < activeShells.Length)
					{
						if (!TerrainCollisionDetection(time, pos, activeShells[atShell].velocity, terrainOffset, ref activeShells[atShell], ref terrain))
						{
							DrawShellAndParabola(ref activeShells[atShell], trail, terrain.GetWidth());
						}
						else
						{
							var temp = activeShells.ToList();
							temp.RemoveAt(atShell);
							activeShells = temp.ToArray();
						}
						++atShell;
					}

					if (activeShells.Length <= 0)
					{
						break;
					}

					if (Console.KeyAvailable)
					{
						key = Console.ReadKey(true);
						switch (key.Key)
						{
							case ConsoleKey.Spacebar:
								if (activeShells[0].c == 'O')
								{
									activeShells = new[]{
										new Shell('o', activeShells[0].position, activeShells[0].lastPosition, new vector( activeShells[0].velocity.X - 0.5f,activeShells[0].velocity.Y)),
										new Shell('o', activeShells[0].position, activeShells[0].lastPosition, activeShells[0].velocity),
										new Shell('o', activeShells[0].position, activeShells[0].lastPosition, new vector( activeShells[0].velocity.X + 0.5f,activeShells[0].velocity.Y))
									};
								}
								break;
						}
						while (Console.KeyAvailable) Console.ReadKey(true);
					}

					time += deltaTime;
					Thread.Sleep(20);
				}

				--shotsLeft;
			}
		}
	}
}