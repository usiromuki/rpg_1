using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 会話パートでの処理
public class StoryManager : MySceneManager
{
    const bool isDevelopMode = true;

    public int storyNumber;

	void Start ()
    {
        StartCoroutine(FadeIn());
    }
	
	void Update ()
    {    
		if(isDevelopMode && Input.GetKeyUp(KeyCode.Q))
        {
            switch(storyNumber)
            {
                case 0:
                    TranditionScene(SCENES.STAGESELECT);
                    break;
                case 1:
                    TranditionScene(SCENES.STAGE_1);
                    break;
                case 2:
                    TranditionScene(SCENES.STAGE_2);
                    break;
                case 3:
                    TranditionScene(SCENES.STAGE_3);
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            TranditionScene(SCENES.STAGESELECT);
        }
    }
}
