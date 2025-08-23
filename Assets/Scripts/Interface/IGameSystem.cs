using System;

namespace XJXInterface
{
	public interface IGameSystem : IDisposable
	{
		bool IsActive { get; }
		void Initialize();
		void OnApplicationPause(bool pauseStatus);
    	void OnApplicationFocus(bool hasFocus);
	}

}
