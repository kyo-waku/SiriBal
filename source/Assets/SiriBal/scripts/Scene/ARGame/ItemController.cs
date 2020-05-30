using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private float ItemDelTime = 0; //アイテム消滅までの経過時間
    private float ItemDelInterval = 10; //アイテム消滅の時間間隔

    // Start is called before the first frame update
    void Start()
    {
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
}
