using UnityEngine;
using System.Collections;
using KK.Frame.Net;
using KK.Frame.Util;
using KK.MsgDef;

public class Demo : MonoBehaviour {

    void Start()
    {
        LobbyNetManager.Instance.CreateInit("118.190.62.68", 0);
    }

    void OnGUI()
    {
        if (GUILayout.Button("ClickConnect", GUILayout.Width(100), GUILayout.Height(100)))
        {
            CMD_Req_SUB_GP_LOGON_PHONE req = new CMD_Req_SUB_GP_LOGON_PHONE();
            req.dwPlazaVersion = 1376263;
            req.szAccounts = "sh1000";
            req.szPassWord = "96e79218965eb72c92a549dd5a330112";
            req.szMac = "sh1000";
            req.szHD = "sh1000";

            LobbyNetManager.Instance.SendMessage(LobbyNetManager.PORT_LOGIN, req);
        }
        if (GUILayout.Button("To2", GUILayout.Width(100), GUILayout.Height(100)))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("2");
        }
    }
	
}
