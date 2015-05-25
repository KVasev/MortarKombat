using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TW_Project
{
	public class Terrain
	{
		private char[,] field;

		public void Draw()
		{
			for (int i = 0; i < field.GetLength(0); ++i)
			{
				for (int j = 0; j < field.GetLength(1); ++j)
				{
					Console.Write(field[i, j]);
				}
				Console.WriteLine();
			}
		}

		public char GetBlock(int x, int y)
		{
			if (x >= 0 && x < field.GetLength(1) && y >= 0 && y < field.GetLength(0))
			{
				return field[y, x];
			}
			return '\0';
		}

		public char UnsafeGetBlock(int x, int y)
		{
			return field[y, x];
		}

		public int GetWidth()
		{
			return field.GetLength(1);
		}

		public int GetHeight()
		{
			return field.GetLength(0);
		}

		public void ChangeBlock(int x, int y, char c)
		{
			if (x >= 0 && x < field.GetLength(1) && y >= 0 && y < field.GetLength(0))
			{
				field[y, x] = c;
			}
		}

		public void StartTestLVL()
		{
			field = new char[16, 70];
			for (int i = 0; i < field.GetLength(0); ++i)
			{
				field[i, 25] = '#';
				field[i, 26] = '#';
				field[i, 27] = '#';
			}
			for (int i = 0; i < field.GetLength(1); ++i)
			{
				field[12, i] = '#';
				field[13, i] = '#';
				field[14, i] = '-';
			}
		}

		public void LoadFromFile(string fileName)
		{
			// 4etene ot faila
			using (var file = new StreamReader(fileName))
			{
			}
		}
	}
}
