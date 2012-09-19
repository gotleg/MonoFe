using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using MonoFe;

namespace MonoFe
{
	public class GameParser
	{
		public GameParser ()
		{
		}

		public List<Game> loadMame (string fileName)
		{
			//Load mame rom index with extra infos from a file
			List<Game> gameList = new List<Game> ();
			try {
				string[] lines = System.IO.File.ReadAllLines (fileName);
			
				int currentGameCount = 0;
			

				Game currentGame = null;
				foreach (string line in lines) {
					if (line == "*#*#*#*") {
						if (currentGameCount == 1) {
							gameList.Add (currentGame);
							currentGameCount = 0;
						} else {
							currentGame = new Game ();
							currentGameCount = 1;
						}
					}

					if (line.StartsWith ("Game: ")) {
						currentGame.gameName = line.Remove (0, 6);
					}
					if (line.StartsWith ("Platform: ")) {
						currentGame.plateform = line.Remove (0, 10);
					}
					if (line.StartsWith ("CRC: ")) {
						currentGame.crc = line.Remove (0, 5);
					}
					if (line.StartsWith ("Genre: ")) {
						currentGame.genre = line.Remove (0, 7);
					}
					if (line.StartsWith ("Release Year: ")) {
						currentGame.year = line.Remove (0, 14);
					}
					if (line.StartsWith ("Developer: ")) {
						currentGame.developer = line.Remove (0, 11);
					}
					if (line.StartsWith ("Game Filename: ")) {
						currentGame.filename = line.Remove (0, 15);
					}
					if (line.StartsWith ("Screen orientation : ")) {
						currentGame.screenOrientation = line.Remove (0, 21);
					}
					if (line.StartsWith ("Control: ")) {
						currentGame.control = line.Remove (0, 9);
					}
					if (line.StartsWith ("Players: ")) {
						currentGame.players = line.Remove (0, 9);
					}
				}
			} catch (Exception ex) {
				Console.WriteLine ("Problem loading MAME.txt db, file may be missing...");
			}
			return gameList;
		}

		public List<Game> filterMameRoms (List<Game> romIndex, string folder)
		{
			//Check roms present in rom folder and return the list with extra infos
			DirectoryInfo di = new DirectoryInfo (folder);
			FileInfo[] rgFiles = di.GetFiles ("*.zip");
			List<String> foundRoms = new List<String> ();
			List<Game> filteredGameList = new List<Game> ();
			foreach (FileInfo fi in rgFiles) {
				foundRoms.Add (fi.Name.Remove (fi.Name.Length - 4, 4)); 
			}

			foreach (Game g in romIndex) {
				if (foundRoms.Contains (g.filename)) {
					filteredGameList.Add (g);
				}
			}


			return filteredGameList;
		}

		public List<Game> loadSnes (string fileName)
		{
			//Load snes rom index with extra infos from a file
			List<Game> gameList = new List<Game> ();
			try {
				string[] lines = System.IO.File.ReadAllLines (fileName);
			
				int currentGameCount = 0;


				Game currentGame = null;
				int lineNumber = 0;
				bool startCountLine = false;
				foreach (string line in lines) {
					if (line == "*") {
						switch (currentGameCount) {
						case 0: 
							currentGame = new Game ();
							currentGameCount = 1;
							lineNumber = 0;
							startCountLine = true;
							break;
						case 1:
							currentGameCount = 2;
							lineNumber = 0;
							startCountLine = false;
							break;
						case 2: 
							gameList.Add (currentGame);
							currentGameCount = 0;
							break;
						}
					}
					if (startCountLine && line != "*") {
						lineNumber++;
						if (lineNumber == 2) {
							currentGame.gameName = line;
						}
					}

					if (line.StartsWith ("Game: ")) {
						currentGame.gameName = line.Remove (0, 6);
					}
					if (line.StartsWith ("Platform: ")) {
						currentGame.plateform = line.Remove (0, 10);
					}
					if (line.StartsWith ("CRC: ")) {
						currentGame.crc = line.Remove (0, 5);
					}
					if (line.StartsWith ("Genre: ")) {
						currentGame.genre = line.Remove (0, 7);
					}
					if (line.StartsWith ("Release Year: ")) {
						currentGame.year = line.Remove (0, 14);
					}
					if (line.StartsWith ("Developer: ")) {
						currentGame.developer = line.Remove (0, 11);
					}
					if (line.StartsWith ("Game Filename: ")) {
						currentGame.filename = line.Remove (0, 15);
					}
					if (line.StartsWith ("Screen orientation : ")) {
						currentGame.screenOrientation = line.Remove (0, 21);
					}
					if (line.StartsWith ("Control: ")) {
						currentGame.control = line.Remove (0, 9);
					}
					if (line.StartsWith ("Players: ")) {
						currentGame.players = line.Remove (0, 9);
					}
					if (line.StartsWith ("Region: ")) {
						currentGame.region = line.Remove (0, 8);
					}
				}
			} catch (Exception ex) {
				Console.WriteLine ("Problem loading SNES.txt db, file may be missing...");
			}
			return gameList;
		}

		public List<Game> filterSnesRoms (List<Game> romIndex, string folder)
		{
			//Check roms present in rom folder and return the list with extra infos
			DirectoryInfo di = new DirectoryInfo (folder);
			FileInfo[] rgFiles = di.GetFiles ("*.zip");
			List<String> foundRoms = new List<String> ();
			List<Game> filteredGameList = new List<Game> ();
			foreach (FileInfo fi in rgFiles) {
				foundRoms.Add (fi.Name.Remove (fi.Name.Length - 4, 4)); 
			}
			
			foreach (Game g in romIndex) {
				if (foundRoms.Contains (g.gameName + " (" + g.region + ")")) {
					filteredGameList.Add (g);
				}
			}
			
			
			return filteredGameList;
		}
		
	}
}

