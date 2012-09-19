using System;

namespace MonoFe
{
	public class Game
	{
		private string _gameName = "";
		private string _plateform = "";
		private string _crc = "";
		private string _genre = "";
		private string _year = "";
		private string _developer = "";
		private string _filename = "";
		private string _screenOrientation = "";
		private string _control = "";
		private string _players = "";
		private string _region = "";

		#region get/set
		public string gameName {
			get {
				return _gameName;
			}

			set { _gameName = value;}
		}
		public string plateform {
			get {
				return _plateform;
			}
			
			set { _plateform = value;}
		}
		public string crc {
			get {
				return _crc;
			}
			
			set { _crc = value;}
		}
		public string genre {
			get {
				return _genre;
			}
			
			set { _genre = value;}
		}
		public string year {
			get {
				return _year;
			}
			
			set { _year = value;}
		}
		public string developer {
			get {
				return _developer;
			}
			
			set { _developer = value;}
		}
		public string filename {
			get {
				return _filename;
			}
			
			set { _filename = value;}
		}
		public string screenOrientation {
			get {
				return _screenOrientation;
			}
			
			set { _screenOrientation = value;}
		}
		public string control {
			get {
				return _control;
			}
			
			set { _control = value;}
		}
		public string players {
			get {
				return _players;
			}
			
			set { _players = value;}
		}
		public string region {
			get {
				return _region;
			}
			
			set { _region = value;}
		}
		#endregion
		public Game()
		{
		}
		/*
		public Game (string name, string plateform, string crc, string genre, string year, string developer, string filename, string screenOrientation, string control, string _players)
		{
			_name = name;
			_plateform = plateform;
			_crc = crc;
			_genre = genre;
			_year = year;
			_developer = developer;
			_filename = filename;
			_screenOrientation = screenOrientation;
			_control = control;
			_players = 	players;
		}
		*/
	}
}

