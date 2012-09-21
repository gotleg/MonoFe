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
		SdlDotNet.Graphics.Font fontXXSmall, fontXSmall,fontSmall, fontNormal, fontMedium, fontLarge, fontXL, fontXXL;
		SdlDotNet.Graphics.Font font8, font10,font12, font14, font16, font18, font20, font24,font28,font32,font36,font40;
		double animationSpeed = Convert.ToDouble (ConfigurationSettings.AppSettings ["StarsAnimationSpeed"]);
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
		Surface sfNbRoms;
		List<Game> gameList;
		List<Surface> itemList = new List<Surface> ();
		Sound changeGame ;
		Sound changeEmulator;
		Sound music;
		int[] itemX = new int[21];
		int[] itemY = new int[21];
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
		List<ParticlePixel> lstPixels = new List<ParticlePixel> ();
		int currentIndex;
		int displayedItems = 21;
		int startX = 20;
		int startY = 300;
		int lineSpacing = 40;
		int time = 0;
		Random rand;

		public GS_Dashboard ()
		{
		}

		#region IGameState implementation

		public void Init ()
		{
			//Init position of items in list
			/*
			itemY[0] = startY;
			itemY[1] = startY+12;//+12
			itemY[2] = startY+27;//+15
			itemY[3] = startY+42;//+15
			itemY[4] = startY+60;//+18
			itemY[5] = startY+80;//+20
			itemY[6] = startY+102;//+22
			itemY[7] = startY+126;//+24
			itemY[8] = startY+152;//+26
			itemY[9] = startY+184;//+32
			itemY[10] = startY+220;//+36 // center item
			itemY[11] = startY+266;//+46
			itemY[12] = startY+302;//+36
			itemY[13] = startY+334;//+32
			itemY[14] = startY+360;//+26
			itemY[15] = startY+384;//+24
			itemY[16] = startY+406;//+22
			itemY[17] = startY+426;//+20
			itemY[18] = startY+444;//+18
			itemY[19] = startY+459;//+15
			itemY[20] = startY+471;//+12
			*/

			itemY[0] = startY;
			itemY[1] = startY+16;//+16
			itemY[2] = startY+35;//+19
			itemY[3] = startY+54;//+19
			itemY[4] = startY+76;//+22
			itemY[5] = startY+100;//+24
			itemY[6] = startY+126;//+26
			itemY[7] = startY+154;//+28
			itemY[8] = startY+184;//+30
			itemY[9] = startY+220;//+36
			itemY[10] = startY+260;//+40 // center item
			itemY[11] = startY+310;//+50
			itemY[12] = startY+350;//+40
			itemY[13] = startY+386;//+36
			itemY[14] = startY+416;//+30
			itemY[15] = startY+444;//+28
			itemY[16] = startY+470;//+26
			itemY[17] = startY+494;//+24
			itemY[18] = startY+516;//+22
			itemY[19] = startY+535;//+19
			itemY[20] = startY+554;//+19

			itemX[0] = startX;
			itemX[1] = startX+3;
			itemX[2] = startX+6;
			itemX[3] = startX+9;
			itemX[4] = startX+12;
			itemX[5] = startX+15;
			itemX[6] = startX+18;
			itemX[7] = startX+21;
			itemX[8] = startX+24;
			itemX[9] = startX+27;
			itemX[10] = startX+30;// center item
			itemX[11] = startX+27;
			itemX[12] = startX+24;
			itemX[13] = startX+21;
			itemX[14] = startX+18;
			itemX[15] = startX+15;
			itemX[16] = startX+12;
			itemX[17] = startX+9;
			itemX[18] = startX+6;
			itemX[19] = startX+3;
			itemX[20] = startX;

			sfError = new Surface (new Size (1, 1));
			sfErrorBackground = new Surface (new Size (1, 1));
			try {
				changeGame = new Sound (ConfigurationSettings.AppSettings ["ChangeGameSound"]);
				changeEmulator = new Sound (ConfigurationSettings.AppSettings ["ChangeEmulatorSound"]);
				music = new Sound (ConfigurationSettings.AppSettings ["MusicSound"]);
			} catch (Exception ex) {
				displayError = true;
				errorMessage += "Problem loading sounds, file may be missing... \n";
			}
			if (ConfigurationSettings.AppSettings ["PlayMusic"] == "true") {
				music.Play (true);
			}
			time = SdlDotNet.Core.Timer.TicksElapsed;
			emulators [0] = "Mame";
			emulators [1] = "Snes";
			rand = new Random ();
			currentEmulator = 0;
			try {
				_sfTitle = new Surface (ConfigurationSettings.AppSettings ["ApplicationLogo"]);
			} catch (Exception ex) {
				displayError = true;
				errorMessage += "Problem loading logo, file may be missing... \n";
				_sfTitle = new Surface (new Size (1, 1));
			}
			_ptTitle.X = (ScreenManager.ScreenSize.Width - 
				_sfTitle.Width) / 2;
			_ptTitle.Y = 10;
			sfBackgroundImage = new Surface (ConfigurationSettings.AppSettings ["BackgroundImage"]);
			currentIndex = 0;
			// We load the mame game list
			switchEmulator ();
			fontXXSmall = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 8);
			fontXSmall = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 10);
			fontSmall = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 13);
			fontNormal = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 16);
			fontMedium = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 20);
			fontLarge = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 24);
			fontXL = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 32);
			fontXXL = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 44);

			font8 = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 8);
			font10 = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 10);
			font12 = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 12);
			font14 = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 14);
			font16 = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 16);
			font18 = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 18);
			font20 = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 20);
			font24 = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 24);
			font28 = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 28);
			font32 = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 32);
			font36 = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 36);
			font40 = new SdlDotNet.Graphics.Font (ConfigurationSettings.AppSettings ["FontFile"], 40);
			//init starfield
			for (int i=0; i<512; i++) {
				starsX [i] = rand.Next (-1000, 1000);
				starsY [i] = rand.Next (-1000, 1000);
				starsZ [i] = rand.Next (100, 1000);
				starsZV [i] = rand.Next (1, 8);
			}
			pixels = new Surface (new Size (1280, 1024));
			if (displayError == true) {
				sfError = fontNormal.Render (errorMessage, Color.White);
				sfErrorBackground = new Surface (new Size (sfError.Width + 40, sfError.Height + 40));
				sfErrorBackground.Fill (Color.Gray);
				sfErrorBackground.Blit (sfError, new Point (((sfErrorBackground.Width - sfError.Width) / 2), 20));
				ScreenManager.MainScreen.Blit (sfErrorBackground, new Point (((ScreenManager.MainScreen.Width - sfErrorBackground.Width) / 2), 250));
			}
		}

		public void Draw ()
		{

			//Clear screen (not very clean, must find a better way)
			ScreenManager.MainScreen.Fill (Color.Black);
			if (ConfigurationSettings.AppSettings ["AnimatedStarsBackground"] == "true") {
				//First draw the stars background
				pixels.Fill (Color.Black);
				lstPixels.Clear ();
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
					string hexStarsColors = ConfigurationSettings.AppSettings ["HexaRGBStarsColor"];
					string hexR = hexStarsColors.Substring (0, 2);
					string hexG = hexStarsColors.Substring (2, 2);
					string hexB = hexStarsColors.Substring (4, 2);
					
					double maxR = Convert.ToDouble (int.Parse (hexR, System.Globalization.NumberStyles.HexNumber));
					double maxV = Convert.ToDouble (int.Parse (hexG, System.Globalization.NumberStyles.HexNumber));
					double maxB = Convert.ToDouble (int.Parse (hexB, System.Globalization.NumberStyles.HexNumber));
					double maxRVB = 255;
					double ratioR = maxR / maxRVB;
					double ratioV = maxV / maxRVB;
					double ratioB = maxB / maxRVB;
					int colorR = Convert.ToInt32 (maxR - (ratioR * ((int)starsZ [i] * 0.255)));
					int colorV = Convert.ToInt32 (maxV - (ratioV * ((int)starsZ [i] * 0.255)));
					int colorB = Convert.ToInt32 (maxB - (ratioB * (int)starsZ [i] * 0.255));
					//180-240-255
					ParticlePixel pp = new ParticlePixel (Color.FromArgb (colorR, colorV, colorB), starsScreenX [i], starsScreenY [i]);
					pp.Render (pixels);
					lstPixels.Add (pp);
				}
				ScreenManager.MainScreen.Blit (pixels, new Point (0, 0));
			} else {
				ScreenManager.MainScreen.Blit (sfBackgroundImage, new Point (0, 0));
			}

			ScreenManager.MainScreen.Blit (_sfTitle, _ptTitle);
			switch (currentEmulator) {
			case 0:
				sfPlatefromName = new Surface (ConfigurationSettings.AppSettings ["MameLogo"]);
				break;
			case 1:
				sfPlatefromName = new Surface (ConfigurationSettings.AppSettings ["SnesLogo"]);
				break;
			}
			string txtNbRoms = "";
			if (gameList.Count <= 1)
				txtNbRoms = Convert.ToString(gameList.Count) + " rom";
			else
				txtNbRoms = Convert.ToString(gameList.Count) + " roms";
			sfNbRoms = new Surface(font24.Render(txtNbRoms,Color.White));
			ScreenManager.MainScreen.Blit (sfNbRoms, new Point (((ScreenManager.MainScreen.Width - sfNbRoms.Width) / 2), 250));
			//sfPlatefromName = fontXXL.Render (emulators [currentEmulator], Color.White);
			ScreenManager.MainScreen.Blit (sfPlatefromName, new Point (Convert.ToInt32 ((ScreenManager.MainScreen.Width - sfPlatefromName.Width) / 2), 150));

			int currentGame = 0;
			foreach (Surface s in itemList) {
				s.Dispose ();
			}
			itemList.Clear ();

			//items vides début
			int emptyLines = 0;
			if (currentIndex < 10) {
				for (int i=0; i<10-currentIndex; i++) {
					switch (itemList.Count)
					{
					case 0:
						itemList.Add (font8.Render (" ", Color.White));
						break;
					case 1:
						itemList.Add (font10.Render (" ", Color.White));
						break;
					case 2:
						itemList.Add (font12.Render (" ", Color.White));
						break;
					case 3:
						itemList.Add (font14.Render (" ", Color.White));
						break;
					case 4:
						itemList.Add (font16.Render (" ", Color.White));
						break;
					case 5:
						itemList.Add (font18.Render (" ", Color.White));
						break;
					case 6:
						itemList.Add (font20.Render (" ", Color.White));
						break;
					case 7:
						itemList.Add (font24.Render (" ", Color.White));
						break;
					case 8:
						itemList.Add (font28.Render (" ", Color.White));
						break;
					case 9:
						itemList.Add (font32.Render (" ", Color.White));
						break;
					}

					ScreenManager.MainScreen.Blit (itemList [itemList.Count - 1], new Point (itemX[itemList.Count - 1], itemY[itemList.Count - 1]));
					emptyLines++;
				}
			}
			foreach (Game g in gameList) {

				//We need to show 21 products
				//Selected product is always in the middle of the list
				// We can have 10 empty items when at the begining or the end of the list
				//So we display 10 items in top of the selected item and 10 at the bottom
				if (currentGame >= currentIndex - 10 && currentGame < (currentIndex + 11)) {
					if (currentGame == currentIndex) {
						//Selected Item
						string hexStarsColors = ConfigurationSettings.AppSettings ["HexaRGBSelectedItemColor"];
						string hexSelectedItemR = hexStarsColors.Substring (0, 2);
						string hexSelectedItemG = hexStarsColors.Substring (2, 2);
						string hexSelectedItemB = hexStarsColors.Substring (4, 2);
						
						itemList.Add (font40.Render (g.gameName, Color.FromArgb (int.Parse (hexSelectedItemR, System.Globalization.NumberStyles.HexNumber), int.Parse (hexSelectedItemG, System.Globalization.NumberStyles.HexNumber), int.Parse (hexSelectedItemB, System.Globalization.NumberStyles.HexNumber))));
						sfGameName = fontXL.Render (g.gameName, Color.White);
						sfGameGenre = fontNormal.Render (g.genre, Color.White);
						sfGameDeveloperYear = fontNormal.Render (g.developer + " " + g.year, Color.White);
						ScreenManager.MainScreen.Blit (sfGameName, new Point (ScreenManager.MainScreen.Width - (sfGameName.Width + 20), 294));
						ScreenManager.MainScreen.Blit (sfGameGenre, new Point (ScreenManager.MainScreen.Width - (sfGameGenre.Width + 20), 330));
						ScreenManager.MainScreen.Blit (sfGameDeveloperYear, new Point (ScreenManager.MainScreen.Width - (sfGameDeveloperYear.Width + 20), 360));
						ScreenManager.MainScreen.Blit (itemList [itemList.Count - 1], new Point (itemX[itemList.Count - 1], itemY[itemList.Count - 1]));
						
						//Load image
						//Check roms present in rom folder and return the list with extra infos
						DirectoryInfo di;
						FileInfo[] rgFiles;
						switch (currentEmulator) {
						case 0:
							di = new DirectoryInfo (ConfigurationSettings.AppSettings ["MameCovers"]);
							
							try {
								rgFiles = di.GetFiles (g.filename + ".png");
								if (rgFiles.Length > 0) {
									sfGameImage = new Surface (ConfigurationSettings.AppSettings ["MameCovers"] + "\\" + g.filename + ".png");
									ScreenManager.MainScreen.Blit (sfGameImage, new Point (ScreenManager.MainScreen.Width - (sfGameImage.Width + 20), 420));
								}
							} catch (Exception ex) {
								
							}		
							
							break;
						case 1:
							di = new DirectoryInfo (ConfigurationSettings.AppSettings ["SnesCovers"]);
							if (!string.IsNullOrEmpty (g.gameName) && !string.IsNullOrEmpty (g.region)) {
								try {
									rgFiles = di.GetFiles (g.gameName + " (" + g.region + ").png");
									if (rgFiles.Length > 0) {
										sfGameImage = new Surface (ConfigurationSettings.AppSettings ["SnesCovers"] + "\\" + g.gameName + " (" + g.region + ").png");
										ScreenManager.MainScreen.Blit (sfGameImage, new Point (ScreenManager.MainScreen.Width - (sfGameImage.Width + 20), 420));
									}
								} catch (Exception ex) {
									//file does not exists
								}
								
							}
							
							break;
						}
					} else {
						//Other items
						switch (itemList.Count)
						{
						case 0:
						case 20:
							itemList.Add (font8.Render (g.gameName, Color.White));
							break;
						case 1:
						case 19:
							itemList.Add (font10.Render (g.gameName, Color.White));
							break;
						case 2:
						case 18:
							itemList.Add (font12.Render (g.gameName, Color.White));
							break;
						case 3:
						case 17:
							itemList.Add (font14.Render (g.gameName, Color.White));
							break;
						case 4:
						case 16:
							itemList.Add (font16.Render (g.gameName, Color.White));
							break;
						case 5:
						case 15:
							itemList.Add (font18.Render (g.gameName, Color.White));
							break;
						case 6:
						case 14:
							itemList.Add (font20.Render (g.gameName, Color.White));
							break;
						case 7:
						case 13:
							itemList.Add (font24.Render (g.gameName, Color.White));
							break;
						case 8:
						case 12:
							itemList.Add (font28.Render (g.gameName, Color.White));
							break;
						case 9:
						case 11:
							itemList.Add (font32.Render (g.gameName, Color.White));
							break;
						}
						ScreenManager.MainScreen.Blit (itemList [itemList.Count - 1], new Point (itemX[itemList.Count - 1], (itemY[itemList.Count - 1])));
					}
					//3 cases
					//currentIndex < 5 must show emtpy items
					//currentIndex >= 5 && currentIndex <= (gameList.Length - 5)
					//currentIndex > (gameList.Length - 5) must show emtpy items
					/*
				if (currentGame >= currentIndex && currentGame < currentIndex + displayedItems) {


					if (currentGame == currentIndex) {
						//Selected Item
						string hexStarsColors = ConfigurationSettings.AppSettings ["HexaRGBSelectedItemColor"];
						string hexSelectedItemR = hexStarsColors.Substring (0, 2);
						string hexSelectedItemG = hexStarsColors.Substring (2, 2);
						string hexSelectedItemB = hexStarsColors.Substring (4, 2);

						itemList.Add (fontLarge.Render (g.gameName, Color.FromArgb (int.Parse (hexSelectedItemR, System.Globalization.NumberStyles.HexNumber), int.Parse (hexSelectedItemG, System.Globalization.NumberStyles.HexNumber), int.Parse (hexSelectedItemB, System.Globalization.NumberStyles.HexNumber))));
						sfGameName = fontXL.Render (g.gameName, Color.White);
						sfGameGenre = fontNormal.Render (g.genre, Color.White);
						sfGameDeveloperYear = fontNormal.Render (g.developer + " " + g.year, Color.White);
						ScreenManager.MainScreen.Blit (sfGameName, new Point (ScreenManager.MainScreen.Width - (sfGameName.Width + 20), 294));
						ScreenManager.MainScreen.Blit (sfGameGenre, new Point (ScreenManager.MainScreen.Width - (sfGameGenre.Width + 20), 330));
						ScreenManager.MainScreen.Blit (sfGameDeveloperYear, new Point (ScreenManager.MainScreen.Width - (sfGameDeveloperYear.Width + 20), 360));
						ScreenManager.MainScreen.Blit (itemList [currentGame - currentIndex], new Point (startX, startY + ((currentGame - currentIndex) * lineSpacing) - 2));

						//Load image
						//Check roms present in rom folder and return the list with extra infos
						DirectoryInfo di;
						FileInfo[] rgFiles;
						switch (currentEmulator) {
						case 0:
							di = new DirectoryInfo (ConfigurationSettings.AppSettings ["MameCovers"]);

							try {
								rgFiles = di.GetFiles (g.filename + ".png");
								if (rgFiles.Length > 0) {
									sfGameImage = new Surface (ConfigurationSettings.AppSettings ["MameCovers"] + "\\" + g.filename + ".png");
									ScreenManager.MainScreen.Blit (sfGameImage, new Point (ScreenManager.MainScreen.Width - (sfGameImage.Width + 20), 420));
								}
							} catch (Exception ex) {
								
							}		

							break;
						case 1:
							di = new DirectoryInfo (ConfigurationSettings.AppSettings ["SnesCovers"]);
							if (!string.IsNullOrEmpty (g.gameName) && !string.IsNullOrEmpty (g.region)) {
								try {
									rgFiles = di.GetFiles (g.gameName + " (" + g.region + ").png");
									if (rgFiles.Length > 0) {
										sfGameImage = new Surface (ConfigurationSettings.AppSettings ["SnesCovers"] + "\\" + g.gameName + " (" + g.region + ").png");
										ScreenManager.MainScreen.Blit (sfGameImage, new Point (ScreenManager.MainScreen.Width - (sfGameImage.Width + 20), 420));
									}
								} catch (Exception ex) {
									//file does not exists
								}

							}

							break;
						}
					} else {
						//Other items
						itemList.Add (fontNormal.Render (g.gameName, Color.White));
						ScreenManager.MainScreen.Blit (itemList [currentGame - currentIndex], new Point (startX, startY + ((currentGame - currentIndex) * lineSpacing)));
					}
				}*/

				}
				currentGame++;
			}
			/*
			//Add empty items i needed
			if (itemList.Count < 9) {
				for (int i=0; i<9-itemList.Count;i++)
				{
					itemList.Add (fontNormal.Render ("empty", Color.White));
					ScreenManager.MainScreen.Blit (itemList [itemList.Count-1], new Point (startX, startY + ((itemList.Count-1) * lineSpacing)));
				}
			}
*/

			if ((SdlDotNet.Core.Timer.TicksElapsed - time) < 40) {
				System.Threading.Thread.Sleep (40 - (SdlDotNet.Core.Timer.TicksElapsed - time));
			}

			int fps = 1000 / (SdlDotNet.Core.Timer.TicksElapsed - time);
			time = SdlDotNet.Core.Timer.TicksElapsed;
			sfFPS = fontNormal.Render (Convert.ToString (fps) + " fps", Color.White);
			ScreenManager.MainScreen.Blit (sfFPS, new Point (ScreenManager.MainScreen.Width - (sfFPS.Width + 20), 20));

			ScreenManager.MainScreen.Blit (sfErrorBackground, new Point (((ScreenManager.MainScreen.Width - sfErrorBackground.Width) / 2), 250));
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
			if (keyState.IsKeyPressed (Key.Space))
				GameStateManager.ChangeState (new GS_ColoredBackground ());
			                         
			if (keyState.IsKeyPressed (Key.DownArrow))
				moveDownList ();
			if (keyState.IsKeyPressed (Key.UpArrow))
				moveUpList ();
			if (keyState.IsKeyPressed (Key.LeftArrow)) {
				if (currentEmulator == 0) {
					currentEmulator = emulators.Length - 1;
				} else {
					currentEmulator--;
				}
				changeEmulator.Play ();
				switchEmulator ();
			}
			if (keyState.IsKeyPressed (Key.RightArrow)) {
				if (currentEmulator == emulators.Length - 1) {
					currentEmulator = 0;
				} else {
					currentEmulator++;
				}
				changeEmulator.Play ();
				switchEmulator ();

			}
				
		}

		public void moveDownList ()
		{
			if (currentIndex < gameList.Count - 1) {
				currentIndex++;
				changeGame.Play ();
				Draw ();
			}
		}

		public void moveUpList ()
		{
			if (currentIndex > 0) {
				currentIndex--;
				changeGame.Play ();
				Draw ();
			}
		}

		public void CleanUp ()
		{
			_sfTitle.Dispose ();
			foreach (Surface s in itemList) {
				s.Dispose ();
			}
		}

		#endregion

		public void switchEmulator ()
		{
			GameParser gp = new GameParser ();
			switch (currentEmulator) {
			case 0:
				gameList = gp.loadMame (ConfigurationSettings.AppSettings ["MameInfos"]);
				gameList = gp.filterMameRoms (gameList, ConfigurationSettings.AppSettings ["MameRomFolder"]);
				break;
			case 1:
				gameList = gp.loadSnes (ConfigurationSettings.AppSettings ["SnesInfos"]);
				gameList = gp.filterSnesRoms (gameList, ConfigurationSettings.AppSettings ["SnesRomFolder"]);
				break;
			default:
				gameList = gp.loadMame (ConfigurationSettings.AppSettings ["MameInfos"]);
			//gameList = gp.filterMameRoms(gameList,ConfigurationSettings.AppSettings["MameRomFolder"]);
				break;
			}

			currentIndex = 0;
		}
	}
}

