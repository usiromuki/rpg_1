using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ステージセレクト画面での処理
public class StageSelectManager : MySceneManager
{
    public GameObject map;

    private int selectableStage = 0; // 解放済みのステージ
    private byte nowSelectStage = 0; // 現在選択しているステージ
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(FadeIn());
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // ------------------------------------
    // 入力の更新
    // ------------------------------------
    private void UpdateInput()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            InputRight();
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            InputLeft();
        }
        else if(Input.GetKeyDown(KeyCode.KeypadEnter)|| Input.GetKeyDown(KeyCode.Return))
        {
            InputEnter();
        }
    }

    private void InputRight()
    {
        switch (selectableStage)
        {
            case 0:
                // ステージが一つしか開放されていないので何も起きない
                break;
            case 1:
                switch (nowSelectStage)
                {
                    case 0:
                        nowSelectStage++;
                        break;
                    case 1:
                        nowSelectStage = 0;
                        break;
                }
                break;
            case 2:
                switch (nowSelectStage)
                {
                    case 0:
                        nowSelectStage++;
                        break;
                    case 1:
                        nowSelectStage++;
                        break;
                    case 2:
                        nowSelectStage = 0;
                        break;
                }
                break;
        }
    }

    private void InputLeft()
    {
        switch (selectableStage)
        {
            case 0:
                // ステージが一つしか開放されていないので何も起きない
                break;
            case 1:
                switch (nowSelectStage)
                {
                    case 0:
                        nowSelectStage = 1;
                        break;
                    case 1:
                        nowSelectStage--;
                        break;
                }
                break;
            case 2:
                switch (nowSelectStage)
                {
                    case 0:
                        nowSelectStage = 2;
                        break;
                    case 1:
                        nowSelectStage--;
                        break;
                    case 2:
                        nowSelectStage--;
                        break;
                }
                break;
        }
    }

    private void InputEnter()
    {
        switch (nowSelectStage)
        {
            case 0:
                TranditionScene(SCENES.STORY_1);
                break;
            case 1:
                TranditionScene(SCENES.STORY_2);
                break;
            case 2:
                TranditionScene(SCENES.STORY_3);
                break;
        }
    }
}
