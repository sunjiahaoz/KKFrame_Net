using UnityEngine;
using System.Collections;
using KK.Frame.Net;

public class RoomMsgFactory_Auto : MsgFactoryMonoBase
{
    public string strSearchNameSpace = "";
    IMsgFactory _factory;
    protected override IMsgFactory factory
    {
        get
        {
            if (_factory == null)
            {
                _factory = new MsgFactory_Auto(strSearchNameSpace, MsgFactory.GROUPID_ROOM);
            }
            return _factory;
        }
    }

    void Awake()
    {
        Init(MsgFactory.Room);
    }

    void OnDestroy()
    {
        UnInit(MsgFactory.Room);
    }

    protected override void RegisterNetMsg()
    {
        factory.Init();
    }

    protected override void UnRegisterNetMsg()
    {
    }
}
