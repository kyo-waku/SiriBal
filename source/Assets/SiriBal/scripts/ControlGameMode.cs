using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGameMode : MonoBehaviour
{
    private bool isShootingBall = true;

    // Properties
    public bool IsShootingBall{
        get{
            return this.isShootingBall;
        }
        private set{
            this.isShootingBall = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        IsShootingBall = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isShootingBall = !isShootingBall;
        }
    }

    public void ModeSwitcherButton()
    {
        isShootingBall = !isShootingBall;
    }
}
