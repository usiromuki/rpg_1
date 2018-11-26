using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ステージセレクト画面での処理
public class StageSelectManager : MySceneManager
{
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(FadeIn());
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnPressGoStage1()
    {
        TranditionScene(SCENES.STORY_1);
    }

    public void OnPressGoStage2()
    {
        TranditionScene(SCENES.STORY_2);
    }

    public void OnPressGoStage3()
    {
        TranditionScene(SCENES.STORY_3);
    }
}
