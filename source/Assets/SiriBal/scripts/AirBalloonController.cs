using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBalloonController : MonoBehaviour
{
    // define balloon paramater
    int BalloonHP=0;
    int BreakCount=3;

    //find GameDirector for scoring
    GameObject director;


    //LevelDesign
    public void SetParameter(int BalloonHP, int BreakCount){
        this.BalloonHP = BalloonHP;
        this.BreakCount = BreakCount;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        this.director=GameObject.Find("GameDirector");
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Detect Collision
    void OnCollisionEnter(Collision other){
        Debug.Log("Hit!");
        BalloonHP += 1;
        this.director.GetComponent<GameDirector>().HitCount();
        if(BalloonHP==BreakCount) {
            this.director.GetComponent<GameDirector>().DestroyCount();
            Destroy(gameObject);
        }
    }
}
