using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireJoystick : Joystick {

    public bool isFire = false;

    private float inactiveAlpha = 1f;
    private float activeAlpha = 1f;
    private float screenWidth;
    private Vector3 iniPos;

    protected override void Start() {
        base.Start();
        GetComponent<CanvasGroup>().alpha = inactiveAlpha;
        screenWidth = Screen.width;
        iniPos = transform.position;
    }

    public override void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);
        GetComponent<CanvasGroup>().alpha = activeAlpha;
        //transform.position = eventData.position;
    }

    public override void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);
        isFire = true;
        GetComponent<CanvasGroup>().alpha = inactiveAlpha;
        //transform.position = iniPos;
    }
}
