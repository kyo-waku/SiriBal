using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingWatcher : MonoBehaviour
{
    float timeOut=3.0f;
    float timeElapsed;
    int Xlotate;
    int Ylotate;

    public void Shoot(Vector3 dir)
    {
        GetComponent<Rigidbody>().AddForce(dir);
        Xlotate　=　Random.Range(-5,5);
        Ylotate　=　Random.Range(-5,5);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //回転を加える
        transform.Rotate(new Vector3(Xlotate, Ylotate, 10));
        //時間で消える
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= timeOut) {
            Destroy(gameObject);
        }
    }
}
