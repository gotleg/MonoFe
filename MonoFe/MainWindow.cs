using System;
using Gtk;
using System.Configuration;
using System.Collections.Generic;
using MyFe;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		//Test : try to load background image, not working yet
		if (!String.IsNullOrEmpty (ConfigurationSettings.AppSettings ["BackgroundImage"])) {
			
			Gtk.Image imgBackground = new Gtk.Image (ConfigurationSettings.AppSettings ["BackgroundImage"]);
			int Width = 800;
			int Height = 600;
			//win.GetSize (out Width, out Height);
			
			Gdk.Pixbuf pix = imgBackground.Pixbuf.ScaleSimple (Width, Height, Gdk.InterpType.Bilinear);
			//Gtk.Style style = win.Style;
			
		}
		
		// We load the mame game list
		GameParser gp = new GameParser ();
		List<Game> gameList = gp.loadMame(ConfigurationSettings.AppSettings ["MameInfos"]);
		List<Game> filteredGameList = gp.filterMameRoms(gameList,ConfigurationSettings.AppSettings["MameRomFolder"]);

		//Create date store to display in GTK# NodeView (GameTreeNode is defined in Game.cs)
		Gtk.NodeStore store;
		store = new Gtk.NodeStore (typeof(GameTreeNode));
		foreach (Game game in filteredGameList) {
			store.AddNode (new GameTreeNode (game.gameName, game.year, game.genre, game.filename));
		}
		//Create the NodeView
		NodeView nvGames = new NodeView (store);
		//Configure the displayed columns
		nvGames.AppendColumn ("Game", new Gtk.CellRendererText (), "text", 0);
		nvGames.AppendColumn ("Year", new Gtk.CellRendererText (), "text", 1);
		nvGames.AppendColumn ("Genre", new Gtk.CellRendererText (), "text", 2);
		nvGames.AppendColumn ("FileName", new Gtk.CellRendererText (), "text", 3);
		//Add the NodeView to the ScrollWindow Widget
		swGameList.Add(nvGames);
		swGameList.ShowAll();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void selectGame (object o, SelectionNotifyEventArgs args)
	{
		throw new System.NotImplementedException ();
	}
}
