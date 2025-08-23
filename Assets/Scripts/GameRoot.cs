using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using XJXInterface;
using XJXMgr;
using XJXSystem;

public class GameRoot : MonoBehaviour
{
	List<SystemBase> systemBases = new List<SystemBase>();
	List<IStart> startFuncList = new List<IStart>();
	List<IUpdate> updateFuncList = new List<IUpdate>();
	List<IFixedUpdate> fixedUpdateFuncList = new List<IFixedUpdate>();
	List<ILateUpdate> lateUpdatesFuncList = new List<ILateUpdate>();

	void Awake()
	{
		// 直接添加系统实例
		RegisterSystem<AgentSystem>();
		RegisterSystem<BuffSystem>();
		InitializeSystems();
		// 注册系统数据管理器
		lateUpdatesFuncList.Add(SystemDataMgr.Instance);
	}

	private void InitializeSystems()
	{
		foreach (var system in systemBases)
		{
			system.Initialize();
			if (system is IStart startSystem)
			{
				startFuncList.Add(startSystem);
			}

			if (system is IUpdate updateSystem)
			{
				updateFuncList.Add(updateSystem);
			}

			if (system is IFixedUpdate fixedUpdateSystem)
			{
				fixedUpdateFuncList.Add(fixedUpdateSystem);
			}
		}
	}

	void Start()
	{
		foreach (var startInterface in startFuncList)
		{
			startInterface.Start();
		}

	}


	void Update()
	{
		//Debug.Log("GameRoot Update"+Time.time);
		foreach (var updateInterface in updateFuncList)
		{
			updateInterface.Update(Time.time);
		}
	}

	void FixedUpdate()
	{
		//Debug.Log("GameRoot FixedUpdate"+Time.time);
		foreach (var fixedUpdateInterface in fixedUpdateFuncList)
		{
			fixedUpdateInterface.FixedUpdate(Time.time);
		}
	}


	void LateUpdate()
	{
		foreach (var lateUpdateInterface in lateUpdatesFuncList)
		{
			lateUpdateInterface.LateUpdate(Time.time);
		}
	}

	#region register system

	private void RegisterSystem<T>() where T : SystemBase
	{
		var instance = GetSystemInstance<T>();
		if (instance != null)
		{
			systemBases.Add(instance);
		}
	}

	private T GetSystemInstance<T>() where T : SystemBase
	{
		var type = typeof(T);
		var instanceProperty = type.GetProperty("Instance");
		return instanceProperty?.GetValue(null) as T;
	}

	#endregion

}