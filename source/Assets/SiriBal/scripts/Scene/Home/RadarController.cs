using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarController : MonoBehaviour
{
    private bool animationStatus = true;

    void Update()
    {
        if (animationStatus)
        {
            gameObject.transform.rotation *= Quaternion.AngleAxis(3, Vector3.down);
            if(gameObject.transform.rotation.y < 0)
            {
                animationStatus = false;
            }
        }
    }

    public void InitialzeRadar()
    {
        animationStatus = true;
        gameObject.transform.Rotate(new Vector3(0,1,0), 90);
    }
}
