using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// タイトルシーンでの処理の管理を行う
public class TitleSceneManager : MySceneManager
{
    // UIの管理
    private SpriteRenderer uiStart;
    private SpriteRenderer uiScore;
    private SpriteRenderer uiExit;

    // UIの画像
    private Sprite startAct;
    private Sprite startNgt;
    private Sprite scoreAct;
    private Sprite scoreNgt;
    private Sprite exitAct;
    private Sprite exitNgt;
    
    private const string titleUIPath = "Sprites/titleUI/";
    enum SELECT_UI
    {
        START,
        //SCORE,
        EXIT,
        SELECTEND
    };
    // 選択しているUI
    SELECT_UI nowSelect = SELECT_UI.START;

    public AudioClip cursorSE;
    private AudioSource audioSource;

    private bool transitionScene = false;

    private void Start()
    {
        StartCoroutine(FadeIn());
        uiStart = GameObject.Find("ui_start").GetComponent<SpriteRenderer>();
        //uiScore = GameObject.Find("ui_score").GetComponent<SpriteRenderer>();
        uiExit = GameObject.Find("ui_exit").GetComponent<SpriteRenderer>();

        startAct = Resources.Load<Sprite>(titleUIPath+ "start_act");
        startNgt = Resources.Load<Sprite>(titleUIPath + "start_ngt");
        scoreAct = Resources.Load<Sprite>(titleUIPath + "score_act");
        scoreNgt = Resources.Load<Sprite>(titleUIPath + "score_ngt");
        exitAct = Resources.Load<Sprite>(titleUIPath + "exit_act");
        exitNgt = Resources.Load<Sprite>(titleUIPath + "exit_ngt");

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = cursorSE;
    }
    
	void Update ()
    {
        UpdateUISelect();
        UpdateSceneTrans();
    }

    private void UpdateUISelect()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            audioSource.Play();
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                nowSelect++;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                nowSelect--;
                if (nowSelect < 0)
                {
                    nowSelect = SELECT_UI.EXIT;
                }
            }

            if (nowSelect == SELECT_UI.SELECTEND)
            {
                nowSelect = SELECT_UI.START;
            }
            switch (nowSelect)
            {
                case SELECT_UI.START:
                    uiStart.sprite = startAct;
                    //uiScore.sprite = scoreNgt;
                    uiExit.sprite = exitNgt;
                    break;
                    /*
                case SELECT_UI.SCORE:
                    uiStart.sprite = startNgt;
                    uiScore.sprite = scoreAct;
                    uiExit.sprite = exitNgt;
                    break;
                    */
                case SELECT_UI.EXIT:
                    uiStart.sprite = startNgt;
                    //uiScore.sprite = scoreNgt;
                    uiExit.sprite = exitAct;
                    break;
            }
        }
    }

    public void UpdateSceneTrans()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            audioSource.Play();
            switch (nowSelect)
            {
                case SELECT_UI.START:
                    InputStartButton();
                    break;
                //case SELECT_UI.SCORE:
                  //  InputContinueButton();
                   // break;
                case SELECT_UI.EXIT:
                    InputExitButton();
                    break;
            }
        }
    }
    
    public void InputStartButton()
    {
        if(!transitionScene)
        {
            TranditionScene(SCENES.STAGE_1);
            transitionScene = true;
        }
    }

    public void InputContinueButton()
    {
        TranditionScene(SCENES.STAGESELECT);
    }

    public void InputExitButton()
    {
        Application.Quit();
    }
}
