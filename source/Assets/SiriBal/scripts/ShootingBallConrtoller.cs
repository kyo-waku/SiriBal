using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBallConrtoller : MonoBehaviour
{

    float timeOut=3.0f;
    float timeElapsed;
    public void Shoot(Vector3 dir)
    {
        GetComponent<Rigidbody>().AddForce(dir);
    }

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
            //timeE
        }
    }
}
