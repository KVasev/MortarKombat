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

		public Shell(char symbol)
			: this()
		{
			c = symbol;
		}
	}
}
