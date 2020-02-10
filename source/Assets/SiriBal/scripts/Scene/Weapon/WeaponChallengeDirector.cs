using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic.Manager;

public class WeaponChallengeDirector : MonoBehaviour
{
    // members
    [SerializeField]
    private GameObject balloonPrefab;
    // properties


    // methods
    void Start()
    {
        switch(GameSceneManager.WeaponChallengeLevelNum)
        {
            case 1:
                CreateBalloon(GameObject.Find("Main Camera").transform.position);
                break;
            default:
                break;
        }
    }
    void Update()
    {
        
    }

    GameObject CreateBalloon(Vector3 atPosition)
	{
		return Instantiate (balloonPrefab, atPosition, Quaternion.identity);
	}

}
