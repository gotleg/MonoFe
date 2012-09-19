using System;

namespace MonoFe.GameStates
{
	public sealed class GameStateManager
	{
		#region Objet Dynamique
		IGameState _currentState;
		IGameState _nextState;
		bool _bRunning;
		private GameStateManager()
		{
			_currentState = null;
			_nextState = null;
			_bRunning = true;
		}
		private void changeState(IGameState newState)
		{
			if (_currentState!=null)
				_currentState.CleanUp();
			_currentState = null;
			_nextState = newState;
		}
		private void checkState()
		{
			if ((_currentState!=null) ||
			    (_nextState==null))
				return;
			// Un nouvel état est utilisé
			_currentState = _nextState;
			_nextState = null;
			_currentState.Init();
		}
		private void handleEvents()
		{
			if (_currentState!=null)
				_currentState.HandleEvents();
		}
		private void update()
		{
			if (_currentState!=null)
				_currentState.Update();
		}
		private void draw()
		{
			if (_currentState!=null)
				_currentState.Draw();
		}
		private void quit()
		{
			_bRunning = false;
			_currentState.CleanUp();
			_currentState = null;
		}
		#endregion

		#region Objet Statique
		private static GameStateManager instance = new GameStateManager();
		public static bool Running
		{
			get {return instance._bRunning;}
		}
		public static void ChangeState(IGameState state)
		{
			instance.changeState(state);
		}
		public static void CheckState()
		{
			instance.checkState();
		}
		public static void HandleEvents()
		{
			instance.handleEvents();
		}
		public static void Update()
		{
			instance.update();
		}
		public static void Draw()
		{
			instance.draw();
		}
		public static void Quit()
		{
			instance.quit();
		}
		#endregion
}
}

