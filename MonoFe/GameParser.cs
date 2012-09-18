using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using MyFe;

namespace MyFe
{
	public class GameParser
	{
		public GameParser ()
		{
		}

		public List<Game> loadMame (string fileName)
		{
			//Load mame rom index with extra infos from a file
			string[] lines = System.IO.File.ReadAllLines (fileName);
			int currentGameCount = 0;
			List<Game> gameList = new List<Game> ();

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
			return gameList;
		}

		public List<Game> filterMameRoms (List<Game> romIndex, string folder)
		{
			//Check roms present in rom folder and return the list with extra infos
			DirectoryInfo di = new DirectoryInfo(folder);
			FileInfo[] rgFiles = di.GetFiles("*.zip");
			List<String> foundRoms = new List<String>();
			List<Game> filteredGameList = new List<Game>();
			foreach(FileInfo fi in rgFiles)
			{
				foundRoms.Add(fi.Name.Remove(fi.Name.Length-4,4)); 
			}

			foreach(Game g in romIndex)
			{
				if (foundRoms.Contains(g.filename))
				{
					filteredGameList.Add(g);
				}
			}


			return filteredGameList;
		}
	}
}

