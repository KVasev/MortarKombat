using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TW_Project
{
	public struct vector
	{
		public vector(float x = 0.0f, float y = 0.0f)
			: this()
		{
			X = x;
			Y = y;
		}
		public float X { get; set; }
		public float Y { get; set; }
	}
}
