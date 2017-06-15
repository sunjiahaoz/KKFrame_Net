using UnityEngine;
using System.Collections;
using KK.Game.Room.Msg.Common;

public class Demo2 : MonoBehaviour {

    void Start()
    {
        RoomNetManager.Instance.CreateInit("192.168.1.120", 6079);
    }

    void OnGUI()
    {
        if (GUILayout.Button("Click", GUILayout.Width(100), GUILayout.Height(100)))
        {
            CMD_GR_LogonByUserID msg = new CMD_GR_LogonByUserID();
            msg.dwPlazaVersion = 1376263;
            msg.bHasVideo = 0;
            msg.dwProcessVersion = 0;
            msg.dwUserID = 1745408;
            msg.szPassWord = "9db06bcff9248837f86d1a6bcf41c9e7";
            RoomNetManager.Instance.SendMessage(msg);
        }
    }
}
