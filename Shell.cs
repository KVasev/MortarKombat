using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TW_Project
{
	enum SHELL_TYPE
	{
		SMALL = 0,
		MEDIUM = 1,
		LARGE = 2
	}

	public struct Shell
	{
		public char c;
		public vector position;
		public vector lastPosition;
	}

	public static class ShellLoader
	{
		public static List<Shell> Load(string fileName)
		{
			List<Shell> toReturn = new List<Shell>();

			// 4etene ot faila
			using (var file = new StreamReader(fileName))
			{
			}

			return toReturn;
		}
	}
}
