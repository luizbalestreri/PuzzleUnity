using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputController : Singleton<InputController>
{
    TouchControl tc;
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnEndTouch;
    public Camera mainCamera;
    // Start is called before the first frame update
    void Awake(){
        tc = new TouchControl();
        mainCamera = Camera.main;
    }
    private void OnEnable() => tc.Enable();
    private void OnDisable() => tc.Disable();
    void Start(){
        tc.Touch.TouchPress.started += ctx => StartTouch(ctx);
        tc.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }
    private void StartTouch(InputAction.CallbackContext context) {
        //Debug.Log("Começou" + tc.Touch.TouchPosition.ReadValue<Vector2>());
        if (OnStartTouch != null) OnStartTouch(Utils.ScreenToWorld(mainCamera, tc.Touch.TouchPosition.ReadValue<Vector2>()), (float) context.startTime);
    }
    private void EndTouch(InputAction.CallbackContext context) {
        //Debug.Log("Touch Ended");
        if (OnEndTouch != null) OnEndTouch(tc.Touch.TouchPosition.ReadValue<Vector2>(), (float) context.time);
    }

    public Vector2 PrimaryPosition(){
        return (Utils.ScreenToWorld(mainCamera, tc.Touch.TouchPosition.ReadValue<Vector2>()));
    }
    void FixedUpdate()
    {
    }


}
