using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Definitions for stage properties.
namespace Weapon{
    public class Stage : MonoBehaviour
    {
        // getters
        public int BalloonLimit{get; internal set;}
        public List<Vector3> BalloonPositions{get; internal set;}
        public int TimeLimit{get; internal set;}
        public int ShootLimit{get; internal set;}
        public int ShootItemID{get; internal set;}
        public string GameDescription{get; internal set;}

        public Stage(int blim = 1, int tlim = 10, int slim = 5, int sid = 1, string gDes = "default"){
            BalloonLimit = blim;
            TimeLimit = tlim;
            ShootLimit = slim;
            ShootItemID = sid;
            GameDescription = gDes;
        }

        public void registerBalloonPositions(Vector3 position)
        {
            if( BalloonPositions == null ){
                BalloonPositions = new List<Vector3>();
            }
            if (BalloonPositions.Count <= BalloonLimit)
            {
                BalloonPositions.Add(position);
            }

            // 登録しすぎた場合のエラー処理は面倒なので一旦放置

        }
    }
}