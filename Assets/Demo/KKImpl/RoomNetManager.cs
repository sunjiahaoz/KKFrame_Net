using UnityEngine;
using System.Collections;
using KK.Frame.Net;

[RequireComponent(typeof(MessageQueueHandler))]
public class RoomNetManager : SingletonMonoBehaviour<RoomNetManager>, INetManager
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
    USocket _socket = null;
    SocketListner _listener = null;
    Protocal _protocal = null;

    string _strIP = "";
    int _nPort = 0;

    #region _INetManager_
    public void CreateInit(string strIP, int nPort, System.Object objExtend = null)
    {
        _strIP = strIP;
        _nPort = nPort;

        _listener = new KKBaseListener(msgQueue, MsgFactory.Room);
        _protocal = new KKBaseProtocal();
        _socket = new USocket(_listener, _protocal);

        _socket.Connect(_strIP, nPort);
    }

    public void SendMessage(CMD_Base_Req req)
    {
        _socket.Send(req.Serialize());
    }
    #endregion    
}
