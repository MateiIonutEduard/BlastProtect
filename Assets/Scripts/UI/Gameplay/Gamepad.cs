using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class Gamepad : MonoBehaviour
{
#if !UNITY_EDITOR && UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern bool IsMobile();
#endif
    public void Awake()
    {
        bool active = SupportTouch();
        gameObject.SetActive(active);
    }

    private bool SupportTouch()
    {
        bool support = false;
#if !UNITY_EDITOR && UNITY_WEBGL
          support = IsMobile();
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
