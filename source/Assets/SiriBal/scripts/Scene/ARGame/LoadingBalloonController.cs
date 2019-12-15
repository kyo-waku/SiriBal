using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBalloonController : MonoBehaviour
{
    Vector3 LoadingBalloonInitForce = new Vector3 (0, 50000,0);

// Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(LoadingBalloonInitForce);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
