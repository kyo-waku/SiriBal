using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBalloonWatcher : MonoBehaviour
{
    // define balloon paramater
    Rigidbody rb;
    int ActionMode;
    int BalloonHP=0;
    int BreakCount=3;
    float ActionSpan;
    float timeElapsed;
    float mutekiTime;
    bool mutekiFlag=false;

    //find GameDirector for scoring
    GameObject director;

    GameModeController controlGameMode;//for ReadGameMode


    //LevelDesign
    public void SetParameter(int BalloonHP, int BreakCount){
        this.BalloonHP = BalloonHP;
        this.BreakCount = BreakCount;
    }

    //Decision Balloon's Action
    void GetAction(int ActionNo){
        float force = Random.Range(250.0f,750.0f);
        Vector3 UpperForce = new Vector3 (0.0f,force,0.0f);
        Vector3 sideForce = new Vector3 (force,0.0f, 0.0f);
        switch(ActionNo){
            case 0:
                //Rise
                rb.AddForce (UpperForce);
                break;
            
            case 1:
                rb.AddForce (2.0f*UpperForce);
                break;
            case 2:
                rb.AddForce (sideForce);
                break;
            
            case 3:
                rb.AddForce (-sideForce);
                break;
            
            case 4:
                rb.AddForce (-UpperForce);
                break;
            
            default:
                Destroy(gameObject);
                break;
            
        }

    }
    
    // Start is called before the first frame update
    void Start()
    {
        controlGameMode = GameObject.Find ("ModeSwitcher").GetComponent<GameModeController>();

        //Get Rigid Body
        rb = this.GetComponent<Rigidbody>();
        
        //Find GameDirector for Scoring
        this.director = GameObject.Find("GameDirector");

        //Decision 1st ActionSpan
        ActionSpan = Random.Range(3,5);
    }

    // Update is called once per frame
    void Update()
    {
        switch(controlGameMode.GameMode)
        {
            case GameModeController.eGameMode.None:
                break;
            case GameModeController.eGameMode.Balloon:
                break;
            case GameModeController.eGameMode.WaitTime:
                break;
            case GameModeController.eGameMode.Shooting:
                //ActionTimer
                timeElapsed += Time.deltaTime;
                if(timeElapsed >= ActionSpan) {
                    timeElapsed = 0.0f;//reset Timer
                    ActionSpan = Random.Range(3,5);//reset Action Span
                    //this.rigidbody.velocity=0.0f;
                    ActionMode = Random.Range(0,5);
                    GetAction(ActionMode);
                }
                break;
        }
        
        if(mutekiFlag==true)
        {
            mutekiTime += Time.deltaTime;
            if(mutekiTime > 0.1f) mutekiFlag=false;
        }
        

    }

    //Detect Collision
    void OnCollisionEnter(Collision other)
    {
        if(controlGameMode.GameMode == GameModeController.eGameMode.Shooting)//get point! if gameMode is shooting
        {
            if (other.gameObject.tag == "player_shoot")
            {
                BalloonHP += 1;
                this.director.GetComponent<GameDirector>().HitCount();
                if(BalloonHP==BreakCount)
                {
                    this.director.GetComponent<GameDirector>().DestroyCount();
                    this.director.GetComponent<GameDirector>().BalloonCounter -= 1;
                    Destroy(gameObject);
                }
            }
            
        }
    }
}
