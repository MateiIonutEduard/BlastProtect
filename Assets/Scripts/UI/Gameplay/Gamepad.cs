using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamepad : MonoBehaviour
{
    public void Awake()
    {
        bool active = SupportTouch();
        gameObject.SetActive(active);
    }

    private bool SupportTouch()
    {
        bool support = false;
        #if UNITY_DESKTOP
          support = Input.touchSupported;
#endif
#if UNITY_ANDROID
          support = true;
#endif
#if UNITY_IOS
          support = true;
#endif

        return support;
    }
}
