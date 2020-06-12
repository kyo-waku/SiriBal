using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private float ItemDelTime = 0; //アイテム消滅までの経過時間
    private float ItemDelInterval = 10; //アイテム消滅の時間間隔
    private const int RECOVERY = 50;

    private GameObject director;
    private GameObject disappearGraphic; //アイテム取得時の消滅グラフィック

    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("GameDirector");
        disappearGraphic = ItemData.Entity.ItemList[0].DisappearGraphic;
    }

    // Update is called once per frame
    void Update()
    {
        // 時間計測
        ItemDelTime += Time.deltaTime;

        // 定期的に増えるアイテム
        if (ItemDelTime > ItemDelInterval)
        {
            Destroy(gameObject);
            ItemDelTime = 0;
        }
    }

    //Detect Collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player_shoot") // プレイヤーの投げたものに当たる必要がある
        {
            // アイテム取得時の消滅グラフィック
            if (disappearGraphic != null)
            {
                Instantiate (disappearGraphic, transform.position,transform.rotation);
            }

            if (director == null)
            {
                director = GameObject.Find("GameDirector");
            }
            this.director.GetComponent<GameDirector>().RecoveryRate += RECOVERY; // とりあえず半分回復させてみる
            Destroy(gameObject);
        }
    }
}