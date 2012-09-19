using System;
using System.Drawing;
using SdlDotNet.Graphics;
using System.Configuration;

namespace MonoFe
{
	public sealed class ScreenManager
	{
			#region Objet Statique
		private static ScreenManager instance = new
				ScreenManager ();

		public static Surface MainScreen {
			get { return instance._mainScreen;}
		}

		public static void Init ()
		{
			instance.init();
		}
		#endregion

		#region Objet Dynamique
		Surface _mainScreen;

		private ScreenManager ()
		{
		}

		private void init ()
		{
			//SdlDotNet.Input.Mouse.ShowCursor = false;
			_mainScreen = Video.SetVideoMode(Convert.ToInt32(ConfigurationSettings.AppSettings["ResX"]), Convert.ToInt32(ConfigurationSettings.AppSettings["ResY"]),false,false,Convert.ToBoolean(ConfigurationSettings.AppSettings["Fullscreen"]),true,true);
		}

		public static Size ScreenSize
		{
			get {return instance._mainScreen.Size;}
		}
		#endregion
	}
}


