using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Common Tools for Unity scripts
/// Add script into the generic parent game object and refer from scripts.
/// </summary>
public class CommonTools : MonoBehaviour
{

#region Defines
    // TouchPhase enumeration doesn't have "None".
    // This enumeration defined to wrap the TouchPhase definition
    public enum TouchPhaseExtended
    {
        None = -1, Began, Moved, Stationary, Ended, Canceled,
    }
#endregion

#region Values
    private TouchPhaseExtended _touchPhaseEx;
    private Vector3 _touchPosition;
#endregion

#region Properties

    // TouchPhaseExtended
    public TouchPhaseExtended touchPhaseEx
    {
        get{ 
            return _touchPhaseEx;
        }
        set{
            _touchPhaseEx = value;
        }
    }

    // TouchPosition
    public Vector3 touchPosition{
        get{
            return _touchPosition;
        }
        set{
            _touchPosition = value;
        }
    }
#endregion

#region Processors
    // Auto update
    void Update()
    {
        // --- UNITY EDITOR ---
        #if UNITY_EDITOR
        // Just after the clicking
        if (Input.GetMouseButtonDown(0))
        {
            touchPhaseEx = TouchPhaseExtended.Began;
        }
        // Just after the releasing
        else if (Input.GetMouseButtonUp(0))
        {
            touchPhaseEx = TouchPhaseExtended.Ended;
        }
        // Mouse Dragging
        else if (Input.GetMouseButton(0))
        {
            touchPhaseEx = TouchPhaseExtended.Moved;
        }
        else{
            touchPhaseEx = TouchPhaseExtended.None;
        }
        
        // Update Position
        if (touchPhaseEx != TouchPhaseExtended.None)
        {
            touchPosition = Input.mousePosition;
        }

        // --- SMART DEVICES ---
        #else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchPhaseEx = TouchPhaseExtended.Began;
                    break;
                case TouchPhase.Ended:
                    touchPhaseEx = TouchPhaseExtended.Ended;
                    break;
                case TouchPhase.Moved:
                    touchPhaseEx = TouchPhaseExtended.Moved;
                    break;
                default:
                    touchPhaseEx = TouchPhaseExtended.None;
                    break;
            }

            if (touchPhaseEx != TouchPhaseExtended.None)
            {
                Vector3 position = new Vector3();
                position.x = touch.position.x;
                position.y = touch.position.y;
                touchPosition = position;
            }
        }
        #endif
    }
#endregion

}
