using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTextController : MonoBehaviour
{
    public Transform cameraTransform;
    private static int updateCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        var pos = this.transform.position;
        pos = new Vector3(0, 0, 0.5f);
        this.transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        // 更新頻度が多すぎるとチャタるので、50回に1回だけ更新することにする
        ++updateCount;
        if (updateCount % 50 == 0)
        {
            // カメラの向いている方向に再描画して、回転もカメラの方向に合わせる
            cameraTransform = GameObject.Find("MainCamera").transform;
            var cameraPosition = cameraTransform.transform.position;
            var forward = Vector3.Scale(cameraTransform.transform.forward, new Vector3(1, 1, 1)).normalized;
            this.transform.position = cameraPosition + forward * 0.5f;
            this.transform.rotation = Quaternion.LookRotation(forward);

            updateCount = 0;
        }
    }
}