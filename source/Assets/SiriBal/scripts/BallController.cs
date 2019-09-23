using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class BallContoroller : MonoBehaviour
{
    float timeOut=3.0f;
    float timeElapsed;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed >= timeOut) {
            Destroy(gameObject);
            //timeElapsed = 0.0f;
        }
    }
}
