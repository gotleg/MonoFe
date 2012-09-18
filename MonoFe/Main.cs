using System;
using Gtk;
using System.Configuration;
using System.Collections.Generic;

namespace MyFe
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();



			if (ConfigurationSettings.AppSettings ["Fullscreen"] == "true") {
				win.Fullscreen ();
			}



		}


	}
}
