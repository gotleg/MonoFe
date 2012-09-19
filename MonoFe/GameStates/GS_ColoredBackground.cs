using System;
using System.Drawing;
using SdlDotNet.Core;
using SdlDotNet.Input;

namespace MonoFe.GameStates
{
	public class GS_ColoredBackground :IGameState
	{
		Color _bgColor;

		public GS_ColoredBackground ()
		{
		}

		#region IGameState implementation

		public void Init ()
		{
			_bgColor = Color.Azure;
		}

		public void Draw ()
		{
			ScreenManager.MainScreen.Fill(_bgColor);
		}

		public void Update ()
		{
			//throw new NotImplementedException ();
		}

		public void HandleEvents ()
		{
			if (!Events.Poll())
				return;
			// On quitte le programme quand on appuie sur Echap
			KeyboardState keyState = new KeyboardState();
			if (keyState.IsKeyPressed(Key.Escape))
				GameStateManager.Quit();
		}

		public void CleanUp ()
		{
			ScreenManager.MainScreen.Fill(Color.Black);
		}

		#endregion


	}
}

