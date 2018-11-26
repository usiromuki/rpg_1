using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebugInfoViewer : MonoBehaviour {

    static string debugInfo;
    public static Queue messageQueue = new Queue();
    public static List<string> messageList = new List<string>();
    public static void addDebugInfo(string text)
    {
        messageList.Add("\n" + text);
        if(messageList.Count > 20)
        {
            messageList.RemoveAt(0);
        }
    }
    Text viewInfo;

	// Use this for initialization
	void Start () {
        viewInfo = this.GetComponentInParent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        foreach(string message in  messageList)
        {
            viewInfo.text += message;
        }
    }
}
