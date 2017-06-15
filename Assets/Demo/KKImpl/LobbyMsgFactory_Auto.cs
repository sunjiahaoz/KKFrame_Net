using UnityEngine;
using System.Collections;
using System;
using KK.Frame.Net;

public class LobbyMsgFactory_Auto : MsgFactoryMonoBase
{
    public string strSearchNameSpace = "";    
    IMsgFactory _factory;
    protected override IMsgFactory factory
    {
        get
        {
            if (_factory == null)
            {
                _factory = new MsgFactory_Auto(strSearchNameSpace, MsgFactory.GROUPID_LOBBY);                
            }
            return _factory;
        }
    }

    void Awake()
    {
        Init(MsgFactory.Lobby);
    }

    void OnDestroy()
    {
        UnInit(MsgFactory.Lobby);
    }

    protected override void RegisterNetMsg()
    {
        factory.Init();        
    }

    protected override void UnRegisterNetMsg()
    {        
    }
}
