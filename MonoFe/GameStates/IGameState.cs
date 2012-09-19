using System;

namespace MonoFe.GameStates
{
	public interface IGameState
	{
		void Init();
		void Draw();
		void Update();
		void HandleEvents();
		void CleanUp();
	}
}

