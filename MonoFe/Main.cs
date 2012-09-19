using System;
using System.Drawing;
using SdlDotNet;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Input;
using MonoFe.GameStates;
using SdlDotNet.Particles;

namespace MonoFe
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// Initialisation de la zone d'affichage
			ScreenManager.Init();
			// Chargement de l'état initial
			GameStateManager.ChangeState(new GS_Dashboard());
			ParticlePixel pp = new ParticlePixel();

			// Boucle principale
			while(GameStateManager.Running)
			{
				GameStateManager.CheckState();
				GameStateManager.HandleEvents();
				GameStateManager.Update();
				GameStateManager.Draw();
				ScreenManager.MainScreen.Update();
			}
		}
	}
}
