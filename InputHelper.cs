using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace StaxelAPI
{
	public static class InputHelper
	{
		static readonly HashSet<Keys> justHeld = new HashSet<Keys>();
		static readonly HashSet<Keys> isHeld = new HashSet<Keys>();

		internal static void Update()
		{
			KeyboardState keyState = Keyboard.GetState();
			Keys[] held = keyState.GetPressedKeys();

			justHeld.Clear();

			foreach (Keys key in isHeld.ToArray())
				if (!held.Contains(key))
					isHeld.Remove(key);

			foreach (Keys key in held)
				if (!isHeld.Contains(key))
				{
					justHeld.Add(key);
					isHeld.Add(key);
				}
		}

		public static bool AnyDown(params Keys[] keys)
		{
			foreach (Keys key in keys)
				if (justHeld.Contains(key))
					return true;

			return false;
		}

		public static bool AnyHeld(params Keys[] keys)
		{
			foreach (Keys key in keys)
				if (isHeld.Contains(key))
					return true;

			return false;
		}
	}
}
