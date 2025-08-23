using System;
using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Mgr;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class OldGameRoot : MonoBehaviour
{
    private void Awake()
    {
    }

    private void OnEnable()
    {
    }

    private void Start()
    {
    }


    private void Update()
    {
    }

    //BuffMgr只在这里驱动
    private void FixedUpdate()
    {
        BuffSystem.getInstance().FixedUpdateBuffs();
    }

    private void OnDisable()
    {
    }


    private void OnDestroy()
    {
    }
}