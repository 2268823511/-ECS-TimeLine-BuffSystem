using XJXInterface;

namespace XJXSystem
{
	//同步的系统基类
	public abstract class SystemBase : IGameSystem
	{
		private bool _isInitialized = false;
		private bool _isApplicationPaused = false;
		private bool _isApplicationFocused = true;
		private bool _disposed = false;
		// IsActive 的核心逻辑
		public virtual bool IsActive => _isInitialized && !_isApplicationPaused && _isApplicationFocused && !_disposed;


		public void Initialize()
		{
			OnInitialize();
			_isInitialized = true;
		}



		public void OnApplicationFocus(bool hasFocus)
		{
			OnFocusChanged(hasFocus);
			_isApplicationFocused = hasFocus;
		}

		public void OnApplicationPause(bool pauseStatus)
		{
			OnPauseChanged(pauseStatus);
			_isApplicationPaused = pauseStatus;
		}

		public void Dispose()
		{
			if (_disposed) return;

			try
			{
				OnDispose();
			}
			finally
			{
				_disposed = true;
				_isInitialized = false;
				_isApplicationPaused = false;
				_isApplicationFocused = false;
			}
		}

		protected virtual void OnInitialize() { }
		protected virtual void OnPauseChanged(bool isPaused) { }
		protected virtual void OnFocusChanged(bool hasFocus) { }
		protected virtual void OnDispose() { } // 子类重写进行清理


		#region ignore Func
		// public async Task InitializeAsync() //异步的不写
		// {

		// }
		#endregion
	}

}

