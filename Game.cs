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
			const string collisionBlocks = "#-|";
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

				if (terrain.UnsafeGetBlock(x, y) == '-' || terrain.UnsafeGetBlock(x, y) == '|')
				{
					return true;
				}

				// trqbva da e shell.type ili tam kakto go krustim :D
				switch (shell.c)
				{
					case 'o':
						//terrain.ChangeBlock(x, y, ' ');
						terrain.ChangeBlock(x, y, 'o');// DEBUG

						Console.SetCursorPosition(x, y + terrainOffset);
						Console.Write("o");
						break;
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
								if (terrain.GetBlock(j, i) == '#')
								{
									terrain.ChangeBlock(j, i, ' ');

									Console.SetCursorPosition(j, i + terrainOffset);
									Console.Write(' ');
								}
							}
						}
						terrain.ChangeBlock(x, y, '@');// DEBUG

						Console.SetCursorPosition(x, y + terrainOffset);
						Console.Write('@');
						break;
				}
				return true;
			}

			return false;
		}

		public void NewGame(Shell[] shells, Terrain terrain, int height)
		{
			const double toRad = Math.PI / 180.0;
			float deltaTime = 0.1f, time;
			int terrainOffset = height - terrain.GetHeight() - 1;

			vector pos = new vector(0, 8 + terrainOffset),
				velocity = new vector(5, -5);

			char trail = '.';
			int shotsLeft = 8;

			float angle = 45.0f;
			float shotForce = 5.0f;

			Random rnd = new Random();

			string emptyString = new string(' ', terrainOffset * terrain.GetWidth());
			while (shotsLeft > 0)
			{
				// TODO: da se vzeme vhoda za sledva6tiq iztrel
				// da se izbira snarqd i da se promenq skorosta(velocity)
				int windForce = rnd.Next(-5, 6);
				var currentShell = shells[0];
				currentShell.lastPosition = pos;

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
					Console.SetCursorPosition(15, height - 1);
					Console.Write("Shot angle: {0}", angle);
					Console.SetCursorPosition(35, height - 1);
					Console.Write("Shots left: {0}", shotsLeft);

					key = Console.ReadKey(false);
					switch (key.Key)
					{
						case ConsoleKey.UpArrow:
							if (angle < 85)
							{
								angle += 0.5f;
							}
							break;
						case ConsoleKey.LeftArrow:
							if (shotForce >= 1.0f)
							{
								shotForce -= 0.5f;
							}
							break;
						case ConsoleKey.RightArrow:
							if (shotForce <= 9.5f)
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
						case ConsoleKey.Spacebar:
							shotInput = true;
							break;
					}
					while (Console.KeyAvailable) Console.ReadKey(false);

					//Console.SetCursorPosition(0, height - 1);
					//Console.Write(new string(' ', terrain.GetWidth() - 1));

					//Console.SetCursorPosition(0, height - 1);
					//Console.Write("Shot force: {0}", shotForce);
					//Console.SetCursorPosition(15, height - 1);
					//Console.Write("Shot angle: {0}", angle);
					//Console.SetCursorPosition(35, height - 1);
					//Console.Write("Shots left: {0}", shotsLeft);

					Thread.Sleep(50);
				}

				velocity.X = (float)Math.Cos(angle * toRad) * (shotForce + windForce);
				velocity.Y = -(float)Math.Sin(angle * toRad) * (shotForce + windForce);

				time = 0;
				while (!TerrainCollisionDetection(time, pos, velocity, terrainOffset, ref currentShell, ref terrain))
				{
					DrawShellAndParabola(ref currentShell, trail, terrain.GetWidth());
					time += deltaTime;
					Thread.Sleep(20);
				}

				//angle += 15.0f;// DEBUG
				--shotsLeft;
			}
		}
	}
}
