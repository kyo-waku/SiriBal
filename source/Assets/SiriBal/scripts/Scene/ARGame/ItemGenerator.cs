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
			for (int i = 0; i < itemCount; i++) {
				//Vector3 RandomPosition = GenerateRandomPosition(30, 10, 30, 5);
				float RandomPositionX = Random.Range(-15,15)/5.0f;
				float RandomPositionY = -1;
				float RandomPositionZ = Random.Range(-15,15)/5.0f;
				Vector3 RandomPosition = new Vector3(RandomPositionX, RandomPositionY, RandomPositionZ);
				CreateItem(cameraTransform.position + RandomPosition);
			}
		#endif
	}

}
