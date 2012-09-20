using System;
using System.Drawing;
using SdlDotNet.Core;
using SdlDotNet.Input;
using System.Configuration;
using SdlDotNet.Graphics;

namespace MonoFe.GameStates
{
	/// <summary>
	/// G s_ colored background.
	/// </summary>
	public class GS_ColoredBackground :IGameState
	{
		Color _bgColor;
		SdlDotNet.Graphics.Font txtFont;
		Surface srTxt;
		int cpt;

		/// <summary>
		/// Initializes a new instance of the <see cref="MonoFe.GameStates.GS_ColoredBackground"/> class.
		/// </summary>
		public GS_ColoredBackground ()
		{
		}

		#region IGameState implementation
		/// <summary>
		/// Init this instance.
		/// </summary>
		public void Init ()
		{
			_bgColor = Color.Beige;
			txtFont = new SdlDotNet.Graphics.Font(ConfigurationSettings.AppSettings["FontFile"],30);
			cpt=0;
		}
		/// <summary>
		/// Draw this instance.
		/// </summary>
		public void Draw ()
		{
			cpt++;
			ScreenManager.MainScreen.Fill(_bgColor);
			srTxt = txtFont.Render(Convert.ToString(cpt),Color.Black);
			ScreenManager.MainScreen.Blit(srTxt,new Point(50,50));
		}
		/// <summary>
		/// Update this instance.
		/// </summary>
		public void Update ()
		{
			//throw new NotImplementedException ();
		}
		/// <summary>
		/// Handles the events.
		/// </summary>
		public void HandleEvents ()
		{
			if (!Events.Poll())
				return;
			// On quitte le programme quand on appuie sur Echap
			KeyboardState keyState = new KeyboardState();
			if (keyState.IsKeyPressed(Key.Escape))
				GameStateManager.Quit();
		}
		/// <summary>
		/// Cleans up.
		/// </summary>
		public void CleanUp ()
		{
			ScreenManager.MainScreen.Fill(Color.Black);
		}

		#endregion


	}
}

