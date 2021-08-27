using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ActionJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool DropBomb;
    public RectTransform rect;
    private float lastTime;
    private float val;

    public void Start()
    {
        DropBomb = false;
        gameObject.SetActive(true);
        lastTime = 0f;
        val = .5f;
    }

    public bool GetActionKey()
    {
        bool res = !DropBomb && lastTime >= .25f;
        lastTime = 0f;
        return res;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DropBomb = true;
        lastTime = Time.time;
        val = .5f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DropBomb = false;
        lastTime = Time.time - lastTime;
        rect.localScale = new Vector3(val, val, val);
    }

    public void Update()
    {
        if (DropBomb && val < .9f) val += .1f;
        if (!DropBomb && lastTime >= .25f) val = .5f;
        rect.localScale = new Vector3(val, val, val);
    }
}
