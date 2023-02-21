using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Gamepad : MonoBehaviour
{
    public void Awake()
    {
        bool active = SupportTouch();
        gameObject.SetActive(active);
    }

    public void Update()
    {
        int playerId = MyPlayerPrefs.GetPlayerId();
        int enemyId = MyPlayerPrefs.GetEnemyId();

        if (playerId != enemyId)
            gameObject.SetActive(false);
    }

    private bool SupportTouch()
    {
        bool support = false;
#if !UNITY_EDITOR && UNITY_WEBGL
          support = Application.isMobilePlatform;
#endif
#if !UNITY_EDITOR && UNITY_DESKTOP
        support = Input.touchSupported;
#endif
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        support = true;
#endif

        return support;
    }
}
