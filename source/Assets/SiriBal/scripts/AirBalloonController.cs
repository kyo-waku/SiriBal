using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBalloonController : MonoBehaviour
{
    // define balloon paramater
    Rigidbody rb;
    int ActionMode;
    int BalloonHP=0;
    int BreakCount=3;
    float ActionSpan;
    float timeElapsed;

    //find GameDirector for scoring
    GameObject director;


    //LevelDesign
    public void SetParameter(int BalloonHP, int BreakCount){
        this.BalloonHP = BalloonHP;
        this.BreakCount = BreakCount;
    }

    //Decision Balloon's Action
    void GetAction(int ActionNo){
        float force=Random.Range(250.0f,750.0f);
        Vector3 UpperForce = new Vector3 (0.0f,force,0.0f);
        Vector3 sideForce = new Vector3 (force,0.0f, 0.0f);
        switch(ActionNo){
            case 0:
                //Rise
                rb.AddForce (UpperForce);
                break;
            
            case 1:
                rb.AddForce (-UpperForce);
                break;
            case 2:
                rb.AddForce (sideForce);
                break;
            
            case 3:
                rb.AddForce (-sideForce);
                break;
            
            case 4:
                
                break;
            
            default:
                Destroy(gameObject);
                break;
            
        }

    }
    
    // Start is called before the first frame update
    void Start()
    {
        //Get Rigid Body
        rb = this.GetComponent<Rigidbody>();
        
        //Find GameDirector for Scoring
        this.director=GameObject.Find("GameDirector");

        //Decision 1st ActionSpan
        ActionSpan=Random.Range(3,5);
        GetAction(0);

    }

    // Update is called once per frame
    void Update()
    {
        //ActionTimer
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= ActionSpan) {
            timeElapsed = 0.0f;//reset Timer
            ActionSpan=Random.Range(3,5);//reset Action Span
            //this.rigidbody.velocity=0.0f;
            ActionMode = Random.Range(0,5);
            GetAction(ActionMode);
        }

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
