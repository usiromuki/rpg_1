using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲームステージ内での処理
public class StageManager : MySceneManager
{
    // マップデータ
    private int[,] mapData = {
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                    {0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                    {0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,},
                 };
    private int mapXLength = 100;
    // マップ用のプレファブ名
    string mapPrefabName = "Prefabs/groundBlock";

    // ステージのゴールのオブジェクト
    public GameObject goalObject;

    static public bool isPlayerDead = false;
    static public bool isPlayerGoal = false;
    private bool transitionScene = false;

    // Use this for initialization
    void Start ()
    {
        // フェードイン
        StartCoroutine(FadeIn());
        // マップを生成する 
        float stageStartY = -1.5f;
        float stageStartX = -10f; 
        for (int height = 0; height < 4; height++)
        {
            for (int width = 0; width < mapXLength; width++)
            {
                if (mapData[height, width] == 1)
                {
                    Instantiate((GameObject)Resources.Load(mapPrefabName),new Vector3(stageStartX + width, stageStartY - height, 0.0f), Quaternion.identity);
                }
            }
        }
        // ゴールをマップの端に配置する
        goalObject.transform.position = new Vector3(stageStartX + (mapXLength*0.9f), goalObject.transform.position.y, goalObject.transform.position.z);
    }
	
	void Update ()
    {
		if(isPlayerDead && !transitionScene)
        {
            transitionScene = true;
            TranditionScene(SCENES.TITLE);
            isPlayerDead = false;
        }
        if(isPlayerGoal)
        {
            transitionScene = true;
            TranditionScene(SCENES.ENDING);
            isPlayerGoal = false;
        }
	}

    public void GotoStageSelect()
    {
        TranditionScene(SCENES.TITLE);
    }
}
