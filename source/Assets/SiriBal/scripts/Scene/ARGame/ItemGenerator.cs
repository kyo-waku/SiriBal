using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGenerator : MonoBehaviour
{
	Transform cameraTransform;
	GameObject mainCamera;
    private GameObject itemPrefab;



    // Start is called before the first frame update
    void Start()
    {
		mainCamera = GameObject.Find("AR Camera");
		cameraTransform = mainCamera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	// atPosition座標にアイテムを置く
	GameObject CreateItem(Vector3 atPosition)
	{
        itemPrefab = ItemData.Entity.ItemList[0].ItemPrefab;
		return Instantiate (itemPrefab, atPosition, Quaternion.identity);
	}


	public void CreateRandomItem(int itemCount = 1)
	{
		cameraTransform = mainCamera.transform;
		//アイテムをランダムに生成
		# if UNITY_EDITOR
			//ランダム範囲
			for (int i = 0; i < itemCount; i++) {
				float RandomPositionX = Random.Range(-15,15)/10.0f;
				float RandomPositionY = Random.Range(-20,20)/10.0f;
				float RandomPositionZ = Random.Range(0,5);

				Vector3 RandomPosition = new Vector3(RandomPositionX, RandomPositionY, RandomPositionZ);
				CreateItem (new Vector3 (cameraTransform.position.x + 0.0f, cameraTransform.position.y, cameraTransform.position.z + 9.0f) + RandomPosition);
			}
		# else
			//ランダム範囲(暫定。実機見ながら調整する)
			for (int i = 0; i < balloonCount; i++) {
				Vector3 RandomPosition = GenerateRandomPosition(30, 10, 30, 5);
				CreateItem(cameraTransform.position + RandomPosition);
			}
		#endif
	}


	// 中央マージンつきランダム座標生成関数（正負の振り分けは完全ランダム）
    // hack:BalloonControllerの関数をコピー。共通化できたらより良い。
	private Vector3 GenerateRandomPosition(int xDist, int yDist, int zDist, int centerMargin = 0)
	{
		float x,y,z = 0f;

		// X
		if (Random.Range(-1, 1) > 0)
		{
			x = Random.Range(-1*xDist, -1*centerMargin)/10.0f;
		}
		else
		{
			x = Random.Range(centerMargin, xDist)/10.0f;
		}

		// Y
		if (Random.Range(-1, 1) > 0)
		{
			y = Random.Range(-1*yDist, -1*centerMargin)/10.0f;
		}
		else
		{
			y = Random.Range(centerMargin, yDist)/10.0f;
		}

		// Z
		if (Random.Range(-1, 1) > 0)
		{
			z = Random.Range(-1*zDist, -1*centerMargin)/10.0f;
		}
		else
		{
			z = Random.Range(centerMargin, zDist)/10.0f;
		}

		return new Vector3(x,y,z);
	}

}
