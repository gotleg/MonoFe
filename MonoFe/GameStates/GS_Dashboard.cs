using System;
using System.Drawing;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Input;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Drawing.Drawing2D;
using SdlDotNet.Audio;
using SdlDotNet.Particles;

namespace MonoFe.GameStates
{
	public class GS_Dashboard :IGameState
	{
		SdlDotNet.Graphics.Font fontNormal,fontLarge,fontXL,fontXXL;
		double animationSpeed = Convert.ToDouble (ConfigurationSettings.AppSettings["StarsAnimationSpeed"]);
		int currentEmulator;
		string[] emulators = new string[2];
		Surface _sfTitle;
		Surface sfFPS;
		Surface sfPlatefromName;
		Surface sfGameName;
		Surface sfGameGenre;
		Surface sfGameDeveloperYear;
		Surface sfGameImage;
		Surface sfBackgroundImage;
		Surface pixels;
		Point _ptTitle;
		Surface m_FontForegroundSurface;
		Surface m_FontShadowSurface;
		Surface sfError;
		Surface sfErrorBackground;
		List<Game> gameList;
		List<Surface> itemList = new List<Surface>();
		Sound changeGame ;
		Sound changeEmulator;
		bool displayError;
		string errorMessage = "";
		float[] starsX = new float[512];
		float[] starsY = new float[512];
		float[] starsZ = new float[512];
		float[] starsZV = new float[512];
		float[] starsOldScreenX = new float[512];
		float[] starsOldScreenY = new float[512];
		int[] starsScreenX = new int[512];
		int[] starsScreenY = new int[512];

		//List<Surface> stars = new List<Surface>();
		List<ParticlePixel> lstPixels = new List<ParticlePixel>();
		int currentIndex;
		int displayedItems = 20;
		int startX = 20;
		int startY = 280;
		int lineSpacing = 30;
		int time=0;
		Random rand;

		public GS_Dashboard ()
		{
		}

		#region IGameState implementation

		public void Init ()
		{
			sfError = new Surface(new Size(1,1));
			sfErrorBackground = new Surface(new Size(1,1));
			try {
				changeGame = new Sound (ConfigurationSettings.AppSettings["ChangeGameSound"]);
				changeEmulator = new Sound (ConfigurationSettings.AppSettings["ChangeEmulatorSound"]);
			} catch (Exception ex) {
				displayError = true;
				errorMessage += "Problem loading sounds, file may be missing... \n";
			}
			time = SdlDotNet.Core.Timer.TicksElapsed;
			emulators[0] = "Mame";
			emulators[1] = "Snes";
			rand = new Random ();
			currentEmulator = 0;
			try {
					_sfTitle = new Surface (ConfigurationSettings.AppSettings["ApplicationLogo"]);
			} catch (Exception ex) {
				displayError = true;
				errorMessage += "Problem loading logo, file may be missing... \n";
				_sfTitle = new Surface(new Size(1,1));
			}
			_ptTitle.X = (ScreenManager.ScreenSize.Width - 
				_sfTitle.Width) / 2;
			_ptTitle.Y = 10;
			//sfBackgroundImage = new Surface(ConfigurationSettings.AppSettings["BackgroundImage"]);
			currentIndex = 0;
			// We load the mame game list
			switchEmulator();

			fontNormal = new SdlDotNet.Graphics.Font (@"C:\Users\Got\Documents\Projects\MonoFe\MonoFe\Fonts\DIMITRI_.TTF", 22);
			fontLarge = new SdlDotNet.Graphics.Font (@"C:\Users\Got\Documents\Projects\MonoFe\MonoFe\Fonts\DIMITRI_.TTF", 26);
			fontXL = new SdlDotNet.Graphics.Font (@"C:\Users\Got\Documents\Projects\MonoFe\MonoFe\Fonts\DIMITRI_.TTF", 32);
			fontXXL = new SdlDotNet.Graphics.Font (@"C:\Users\Got\Documents\Projects\MonoFe\MonoFe\Fonts\DIMITRI_.TTF", 44);
			//init starfield
			for (int i=0;i<512;i++)
			{
				starsX[i] = rand.Next(-1000,1000);
				starsY[i] = rand.Next(-1000,1000);
				starsZ[i] = rand.Next(100,1000);
				starsZV[i] = rand.Next(1,8);
			}
			pixels = new Surface(new Size(1280,1024));
			if (displayError == true) {
				sfError = fontNormal.Render (errorMessage, Color.White);
				sfErrorBackground = new Surface(new Size(sfError.Width+40,sfError.Height+40));
				sfErrorBackground.Fill(Color.Gray);
				sfErrorBackground.Blit (sfError, new Point (((sfErrorBackground.Width -sfError.Width )/2), 20));
				ScreenManager.MainScreen.Blit (sfErrorBackground, new Point (((ScreenManager.MainScreen.Width -sfErrorBackground.Width )/2), 250));
			}
		}

		public void Draw ()
		{

			//Clear screen (not very clean, must find a better way)
			ScreenManager.MainScreen.Fill (Color.Black);
			pixels.Fill (Color.Black);
			lstPixels.Clear ();
			//First draw the stars background
			for (int i=0; i<512; i++) {

				//move the star closer
				starsZ [i] = (float)(starsZ [i] - (starsZV [i] * animationSpeed));

				if ((starsScreenX [i] < 0 ||
					starsScreenX [i] > ScreenManager.MainScreen.Width ||
					starsScreenY [i] < 0 ||
					starsScreenY [i] > ScreenManager.MainScreen.Height) ||
					starsZ [i] < 1) {
					starsX [i] = rand.Next (-1000, 1000);
					starsY [i] = rand.Next (-1000, 1000);
					starsZ [i] = rand.Next (400, 1000);
					starsZV [i] = rand.Next (1, 8);
				}

				//calculate screen coordinates
				starsScreenX [i] = Convert.ToInt32 (starsX [i] / starsZ [i] * 100 + (ScreenManager.MainScreen.Width / 2));
				starsScreenY [i] = Convert.ToInt32 (starsY [i] / starsZ [i] * 100 + (ScreenManager.MainScreen.Height / 2));
				string hexStarsColors = ConfigurationSettings.AppSettings["HexaRGBStarsColor"];
				string hexR = hexStarsColors.Substring(0,2);
				string hexG = hexStarsColors.Substring(2,2);
				string hexB = hexStarsColors.Substring(4,2);

				double maxR = Convert.ToDouble(int.Parse(hexR,System.Globalization.NumberStyles.HexNumber));
				double maxV = Convert.ToDouble(int.Parse(hexG,System.Globalization.NumberStyles.HexNumber));
				double maxB = Convert.ToDouble(int.Parse(hexB,System.Globalization.NumberStyles.HexNumber));
				double maxRVB = 255;
				double ratioR = maxR/maxRVB;
				double ratioV = maxV/maxRVB;
				double ratioB = maxB/maxRVB;
				int colorR = Convert.ToInt32 (maxR -(ratioR*((int)starsZ [i] * 0.255)));
				int colorV = Convert.ToInt32 (maxV - (ratioV*((int)starsZ [i] * 0.255)));
				int colorB = Convert.ToInt32 (maxB - (ratioB*(int)starsZ [i] * 0.255));
				//180-240-255
				ParticlePixel pp = new ParticlePixel (Color.FromArgb (colorR, colorV, colorB), starsScreenX [i], starsScreenY [i]);
				pp.Render (pixels);
				lstPixels.Add (pp);
			}
			ScreenManager.MainScreen.Blit (pixels, new Point (0, 0));
			//ScreenManager.MainScreen.Blit (sfBackgroundImage,new Point (0,0 ));
			ScreenManager.MainScreen.Blit (_sfTitle, _ptTitle);

			sfPlatefromName = fontXXL.Render (emulators [currentEmulator], Color.White);
			ScreenManager.MainScreen.Blit (sfPlatefromName, new Point (Convert.ToInt32 ((ScreenManager.MainScreen.Width - sfPlatefromName.Width) / 2), 200));

			int currentGame = 0;
			foreach (Surface s in itemList) {
				s.Dispose ();
			}
			itemList.Clear ();
			foreach (Game g in gameList) {
				if (currentGame >= currentIndex && currentGame < currentIndex + displayedItems) {
					if (currentGame == currentIndex) {
						string hexStarsColors = ConfigurationSettings.AppSettings["HexaRGBSelectedItemColor"];
						string hexSelectedItemR = hexStarsColors.Substring(0,2);
						string hexSelectedItemG = hexStarsColors.Substring(2,2);
						string hexSelectedItemB = hexStarsColors.Substring(4,2);

						itemList.Add (fontLarge.Render (g.gameName, Color.FromArgb(int.Parse(hexSelectedItemR,System.Globalization.NumberStyles.HexNumber),int.Parse(hexSelectedItemG,System.Globalization.NumberStyles.HexNumber),int.Parse(hexSelectedItemB,System.Globalization.NumberStyles.HexNumber))));
						sfGameName = fontXL.Render (g.gameName, Color.White);
						sfGameGenre = fontNormal.Render (g.genre, Color.White);
						sfGameDeveloperYear = fontNormal.Render (g.developer + " " + g.year, Color.White);
						ScreenManager.MainScreen.Blit (sfGameName, new Point (ScreenManager.MainScreen.Width - (sfGameName.Width + 20), 274));
						ScreenManager.MainScreen.Blit (sfGameGenre, new Point (ScreenManager.MainScreen.Width - (sfGameGenre.Width + 20), 310));
						ScreenManager.MainScreen.Blit (sfGameDeveloperYear, new Point (ScreenManager.MainScreen.Width - (sfGameDeveloperYear.Width + 20), 340));
						ScreenManager.MainScreen.Blit (itemList [currentGame - currentIndex], new Point (startX, startY + ((currentGame - currentIndex) * lineSpacing) - 2));

						//Load image
						//Check roms present in rom folder and return the list with extra infos
						DirectoryInfo di;
						FileInfo[] rgFiles;
						switch (currentEmulator) {
						case 0:
							di = new DirectoryInfo (ConfigurationSettings.AppSettings ["MameTitleImage"]);

							try {
								rgFiles = di.GetFiles (g.filename + ".png");
								if (rgFiles.Length > 0) {
									sfGameImage = new Surface (ConfigurationSettings.AppSettings ["MameTitleImage"] + "\\" + g.filename + ".png");
									ScreenManager.MainScreen.Blit (sfGameImage, new Point (ScreenManager.MainScreen.Width - (sfGameImage.Width + 20), 390));
								}
							} catch (Exception ex) {
								
							}		

							break;
						case 1:
							di = new DirectoryInfo (ConfigurationSettings.AppSettings ["SnesTitleImage"]);
							if (!string.IsNullOrEmpty (g.gameName) && !string.IsNullOrEmpty (g.region)) {
								try {
									rgFiles = di.GetFiles (g.gameName + " (" + g.region + ").png");
									if (rgFiles.Length > 0) {
										sfGameImage = new Surface (ConfigurationSettings.AppSettings ["SnesTitleImage"] + "\\" + g.gameName + " (" + g.region + ").png");
										ScreenManager.MainScreen.Blit (sfGameImage, new Point (ScreenManager.MainScreen.Width - (sfGameImage.Width + 20), 390));
									}
								} catch (Exception ex) {
									//file does not exists
								}

							}

							break;
						}
					} else {
						itemList.Add (fontNormal.Render (g.gameName, Color.White));
						ScreenManager.MainScreen.Blit (itemList [currentGame - currentIndex], new Point (startX, startY + ((currentGame - currentIndex) * lineSpacing)));
					}
				}
				currentGame++;
			}

			if ((SdlDotNet.Core.Timer.TicksElapsed - time) < 40) {
				System.Threading.Thread.Sleep (40 - (SdlDotNet.Core.Timer.TicksElapsed - time));
			}

			int fps = 1000 / (SdlDotNet.Core.Timer.TicksElapsed - time);
			time = SdlDotNet.Core.Timer.TicksElapsed;
			sfFPS = fontNormal.Render (Convert.ToString (fps) + " fps", Color.White);
			ScreenManager.MainScreen.Blit (sfFPS, new Point (ScreenManager.MainScreen.Width - (sfFPS.Width + 20), 20));

			ScreenManager.MainScreen.Blit (sfErrorBackground, new Point (((ScreenManager.MainScreen.Width -sfErrorBackground.Width )/2), 250));
		}

		public void Update ()
		{

		}

		public void HandleEvents ()
		{
			if (!Events.Poll ())
				return;
			// On change de GameState
			KeyboardState keyState = new KeyboardState ();
			if (keyState.IsKeyPressed (Key.Escape)) {
				//GameStateManager.ChangeState(new GS_ColoredBackground());
				GameStateManager.Quit ();
			}
			if (keyState.IsKeyPressed (Key.DownArrow))
				moveDownList ();
			if (keyState.IsKeyPressed (Key.UpArrow))
				moveUpList ();
			if (keyState.IsKeyPressed (Key.LeftArrow)) {
				if (currentEmulator == 0)
				{
					currentEmulator = emulators.Length-1;
				}
				else
				{
					currentEmulator--;
				}
				changeEmulator.Play();
				switchEmulator();
			}
			if (keyState.IsKeyPressed (Key.RightArrow)) {
				if (currentEmulator == emulators.Length-1)
				{
					currentEmulator = 0;
				}
				else
				{
					currentEmulator++;
				}
				changeEmulator.Play();
				switchEmulator();

			}
				
		}

		public void moveDownList ()
		{
			if (currentIndex < gameList.Count-1) {
				currentIndex++;
				changeGame.Play();
				Draw ();
			}
		}

		public void moveUpList ()
		{
			if (currentIndex > 0) {
				currentIndex--;
				changeGame.Play();
				Draw ();
			}
		}

		public void CleanUp ()
		{
			_sfTitle.Dispose();
			foreach (Surface s in itemList)
			{
				s.Dispose();
			}
		}

		#endregion

		public void switchEmulator ()
		{
			GameParser gp = new GameParser ();
			switch (currentEmulator) 
			{
			case 0:
				gameList = gp.loadMame (ConfigurationSettings.AppSettings ["MameInfos"]);
				gameList = gp.filterMameRoms(gameList,ConfigurationSettings.AppSettings["MameRomFolder"]);
				break;
			case 1:
				gameList = gp.loadSnes (ConfigurationSettings.AppSettings ["SnesInfos"]);
				gameList = gp.filterSnesRoms(gameList,ConfigurationSettings.AppSettings["SnesRomFolder"]);
				break;
			default:
				gameList = gp.loadMame (ConfigurationSettings.AppSettings ["MameInfos"]);
			//gameList = gp.filterMameRoms(gameList,ConfigurationSettings.AppSettings["MameRomFolder"]);
				break;
			}
			currentIndex=0;
		}
	}
}

