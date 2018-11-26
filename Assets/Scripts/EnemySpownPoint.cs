using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemyを自動生成するポイントの処理
public class EnemySpownPoint : MonoBehaviour
{
    private float Conter = 0; // Enemyの生成の間隔用のカウンター
    private const float SpownTime = 3; // Enemyの生成の間隔のカウンター
    GameObject prefab;
    // Use this for initialization
    void Start ()
    {
        prefab = (GameObject)Resources.Load("Prefabs/Enemy_Type1");
    }
	
	// Update is called once per frame
	void Update ()
    {
        Conter+=Time.deltaTime;
        if (Conter >= SpownTime)
        {
            Instantiate(prefab, this.transform.position, Quaternion.identity);
            Conter = 0;
        }
    }
}
