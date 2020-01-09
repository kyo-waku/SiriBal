using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Touch Tools for Unity scripts
/// Add script into the generic parent game object and refer from scripts.
/// </summary>
public class TouchTools : MonoBehaviour
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
    private Vector3 _touchStartPosition;
    private Vector3 _touchEndPosition;
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
    public Vector3 touchStartPosition{
        get{
            return _touchStartPosition;
        }
        set{
            _touchStartPosition = value;
        }
    }

    public Vector3 touchEndPosition{
        get{
            return _touchEndPosition;
        }
        set{
            _touchEndPosition = value;
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
            touchStartPosition = Input.mousePosition;
        }
        // Just after the releasing
        else if (Input.GetMouseButtonUp(0))
        {
            touchPhaseEx = TouchPhaseExtended.Ended;
            touchEndPosition = Input.mousePosition;
        }
        // Mouse Dragging
        else if (Input.GetMouseButton(0))
        {
            touchPhaseEx = TouchPhaseExtended.Moved;
            touchEndPosition = Input.mousePosition;
        }
        else{
            touchPhaseEx = TouchPhaseExtended.None;
        }

        // --- SMART DEVICES ---
        #else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 position = new Vector3();
            position.x = touch.position.x;
            position.y = touch.position.y;
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchPhaseEx = TouchPhaseExtended.Began;
                    touchStartPosition = position;
                    break;
                case TouchPhase.Ended:
                    touchPhaseEx = TouchPhaseExtended.Ended;
                    touchEndPosition = position;
                    break;
                case TouchPhase.Moved:
                    touchPhaseEx = TouchPhaseExtended.Moved;
                    touchEndPosition = position;
                    break;
                default:
                    touchPhaseEx = TouchPhaseExtended.None;
                    break;
            }
        }
        #endif
    }
#endregion

}
