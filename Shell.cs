using System;
using System.Collections.Generic;
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

	class Shell
	{
		public Shell()
		{
		}
		public Shell(Shell other)
		{
			c = other.c;
		}

		public char c;
		public vector position;
		public vector lastPosition;
	}
}
