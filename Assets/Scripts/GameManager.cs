// すべてのシーンで参照可能な処理や値の定義
using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum TRANSITION_STATE {
        NONE,
        PROCESSING,
    }
    static public TRANSITION_STATE transState = TRANSITION_STATE.NONE;
}
