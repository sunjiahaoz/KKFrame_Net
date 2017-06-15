using UnityEngine;
using System.Collections;
using KK.Frame.Net;
using KK.Frame.Util;

[RequireComponent(typeof(MessageQueueHandler))]
public class LobbyNetManager : SingletonMonoBehaviour<LobbyNetManager>, INetManager
{

    MessageQueueHandler _queue = null;
    MessageQueueHandler msgQueue
    {
        get
        {
            if (_queue == null)
            {
                _queue = GetComponent<MessageQueueHandler>();
            }
            return _queue;
        }
    }
    ShortSocketPool _socketPool = null;
    SocketListner _listener = null;
    Protocal _protocal = null;    

    string _strIP = "118.190.62.68";
    public const int PORT_LOGIN = 9000;
    public const int PORT_LOBBY = 9001;
    public const int PORT_ROOMSTH = 9002;

    #region _INetManager_
    public void CreateInit(string strIP, int nIP, System.Object objExtend = null)
    {
        _strIP = strIP;        
        _socketPool = new ShortSocketPool();
        _listener = new KKBaseListener(msgQueue, MsgFactory.Lobby);
        _protocal = new KKBaseProtocal();
    }

    public void SendMessage(CMD_Base_Req req)
    {
        Debug.LogError("<color=orange>[Warning]</color>---" + "请使用LobbyNetManager的另一个SendMessage发送消息！！");
        //SendMessage(_strIP, _nPortLobby, req);
    }
    #endregion

    public virtual void SendMessage(int nPort, CMD_Base_Req req)
    {
        USocket socket = _socketPool.NewSocket(_listener, _protocal);
        socket.Connect(_strIP, nPort, (success) =>
        {
            if (!success)
            {
                return;
            }
            socket.Send(req.Serialize());
        });
    }



    //public void Login(string strAccount)
    //{
    //    CMD_Req_SUB_GP_LOGON_PHONE req = new CMD_Req_SUB_GP_LOGON_PHONE();
    //    req.dwPlazaVersion = 1376263;
    //    req.szAccounts = strAccount;
    //    req.szPassWord = "96e79218965eb72c92a549dd5a330112";
    //    req.szMac = "sh1000";
    //    req.szHD = "sh1000";

    //    SendMessage(_strIP, _nPortLogin, req);
    //}

}
