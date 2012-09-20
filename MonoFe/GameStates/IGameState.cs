using System;

namespace MonoFe.GameStates
{
	/// <summary>
	/// I game state.
	/// </summary>
	public interface IGameState
	{
		void Init();
		void Draw();
		void Update();
		void HandleEvents();
		void CleanUp();
	}
}

