using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowingImage : MonoBehaviour {
    private Transform cameraTransform; // カメラのトランスフォーム

    void Start () {
        cameraTransform = GameObject.Find("Main Camera").gameObject.transform;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        this.transform.position = new Vector3(cameraTransform.localPosition.x, this.transform.position.y, this.transform.position.z);
    }
}
