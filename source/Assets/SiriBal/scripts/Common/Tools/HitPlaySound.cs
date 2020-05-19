using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlaySound : MonoBehaviour {

    public AudioClip sound;
    private GameObject ARCamera;

    void OnCollisionEnter(Collision collision) {
        //メインカメラの位置を見つける
        Vector3 tmp = GameObject.Find("AR Camera").transform.position;
        AudioSource.PlayClipAtPoint(sound, tmp);
        //Debug.Log("SE-played");
    }
}
