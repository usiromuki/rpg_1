using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ステージ内でプレイヤーをカメラが追いかける
public class CameraController : MonoBehaviour
{
    Transform cameraTransform; // カメラのトランスフォームへの参照
    const float cameraTrackingDist = -1.0f;// この値分プレイヤーが画面の中心から離れた場合カメラが追尾する
    public Transform trackingTargetTransform; // 追尾対象(プレイヤー)のトランスフォームへの参照

    //  * 進行方向は右で左には戻れないようにするため右だけ考慮する 
    void Start ()
    {
        cameraTransform = this.transform;
    }	
	
	void Update ()
    {
        if (trackingTargetTransform.localPosition.x > 0)
        {
            this.transform.position = new Vector3(trackingTargetTransform.localPosition.x, this.transform.position.y, this.transform.position.z);
        }
    }
}
