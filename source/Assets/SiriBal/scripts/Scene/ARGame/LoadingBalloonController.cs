using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBalloonController : MonoBehaviour
{
    private GameDirector GameDirector;
    //Vector3 LoadingBalloonInitForce = new Vector3(0, 50000, 0);
    Vector3 LoadingBalloonInitForce = new Vector3(-1000, 50000, 0);
    float SideForce = 2.0f;
    float timeOut = 6.0f;
    float timeElapsed = 0.0f;
    float Amplitude = 500;
    int ForceDirection = 1;


    // Start is called before the first frame update
    void Start()
    {
        GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        GetComponent<Rigidbody>().AddForce(Random.Range(-50000, 50000), 80000, 0);
        int RandomValue = Random.Range(0, 2);
        if (RandomValue == 0)
        {
            ForceDirection = -1;
        }
        else if (RandomValue == 1)
        {
            ForceDirection = 1;
        }
        else
        {
            Debug.Log("ExceptionError RandomForceDirection");
        }

        //GetComponent<Rigidbody>().AddForce(LoadingBalloonInitForce);
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        AddForceSinWave(rb);

        if (GameDirector.bMinLoadingBalloonPosYJudge1 == true)
        {
            if (this.transform.position.y < GameDirector.MinLoadingBalloonPosY)
            {
                GameDirector.MinLoadingBalloonPosY = this.transform.position.y;
            }
            GameDirector.bMinLoadingBalloonPosYJudge2 = true;
        }
        //暫定：時間で消える(そのうち画面外で消すように改良)
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= this.timeOut)
        {
            Destroy(gameObject);
        }
    }

    void AddForceSinWave(Rigidbody rb)
    {
        if (rb.velocity.x <= -Amplitude)
        {
            ForceDirection = 1;
            rb.AddForce(SideForce * ForceDirection * rb.mass * Amplitude * Vector3.right);
        }

        else if (-Amplitude < rb.velocity.x && rb.velocity.x < Amplitude)
        {
            rb.AddForce(SideForce * rb.mass * Mathf.Sqrt((Mathf.Pow(Amplitude, 2) - Mathf.Pow(rb.velocity.x, 2))) * ForceDirection * Vector3.right);
        }

        else if (Amplitude <= rb.velocity.x)
        {
            ForceDirection = -1;
            rb.AddForce(SideForce * ForceDirection * rb.mass * Amplitude * Vector3.right);
        }

        else
        {
            Debug.Log("ExceptionErrorAddForceSinWaveVelocity");
        }

    }

}
