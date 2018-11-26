using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 各種画面で共通の処理、シーンの管理など
public class MySceneManager : MonoBehaviour
{
    protected AsyncOperation sceneLoadingAsync;
    protected string fadeImageName = "FadeImage";

    public enum SCENES
    {
        AWAKE,
        TITLE,
        STAGESELECT,
        STORY_1,
        STORY_2,
        STORY_3,
        STAGE_1,
        STAGE_2,
        STAGE_3,
        PROLOG,
        ENDING,
    }
    
    protected void TranditionScene(SCENES scene)
    {
        sceneLoadingAsync = SceneManager.LoadSceneAsync(GetSceneName(scene), LoadSceneMode.Single);
        sceneLoadingAsync.allowSceneActivation = false;
        StartCoroutine(FadeOutAndTranditionScene());
    }

    protected string GetSceneName(SCENES scene)
    {
        string sceneName = "AwakeScene";

        switch (scene)
        {
            case SCENES.AWAKE:
                sceneName = "AwakeScene";
                break;
            case SCENES.TITLE:
                sceneName = "TitleScene";
                break;
            case SCENES.STAGESELECT:
                sceneName = "StageSelectScene";
                break;
            case SCENES.STAGE_1:
                sceneName = "Stage1Scene";
                break;
            case SCENES.STAGE_2:
                sceneName = "Stage2Scene";
                break;
            case SCENES.STAGE_3:
                sceneName = "Stage3Scene";
                break;
            case SCENES.STORY_1:
                sceneName = "Story1Scene";
                break;
            case SCENES.STORY_2:
                sceneName = "Story2Scene";
                break;
            case SCENES.STORY_3:
                sceneName = "Story3Scene";
                break;
            case SCENES.PROLOG:
                sceneName = "PrologScene";
                break;
            case SCENES.ENDING:
                sceneName = "EndingScene";
                break;
        }

        return sceneName;
    }

    protected IEnumerator FadeOutAndTranditionScene()
    {
        GameObject fadeImageObj = GameObject.Find(fadeImageName);
        float fadeImageAlpha = 0.0f;
        // FadeIn/Outの際に使用するオブジェクトがUITextでもSpriteでも動作するようにする
        if (fadeImageObj.GetComponent<Image>() != null)
        {
            while (fadeImageAlpha <= 1.0f)
            {
                fadeImageAlpha += 0.01f;
                fadeImageObj.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, fadeImageAlpha);
                yield return null;
            }
        }
        else if (fadeImageObj.GetComponent<SpriteRenderer>() != null)
        {
            while (fadeImageAlpha <= 1.0f)
            {
                fadeImageAlpha += 0.01f;
                fadeImageObj.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, fadeImageAlpha);
                yield return null;
            }
        }
        // シーンを変更する
        if (sceneLoadingAsync.progress >= 0.9f)
        {
            sceneLoadingAsync.allowSceneActivation = true;
            yield break;
        }
        else
        {
            yield return null;
        }
    }

    protected IEnumerator FadeIn()
    {
        GameObject fadeImageObj = GameObject.Find(fadeImageName);
        float fadeImageAlpha = 1.0f;
        // FadeIn/Outの際に使用するオブジェクトがUITextでもSpriteでも動作するようにする
        if (fadeImageObj.GetComponent<Image>() != null)
        {
            while (fadeImageAlpha >= 0.0f)
            {
                fadeImageAlpha -= 0.01f;
                fadeImageObj.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, fadeImageAlpha);
                yield return null;
            }
        }
        else if (fadeImageObj.GetComponent<SpriteRenderer>() != null)
        {
            while (fadeImageAlpha >= 0.0f)
            {
                fadeImageAlpha -= 0.01f;
                fadeImageObj.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, fadeImageAlpha);
                yield return null;
            }
        }
    }
}
