using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using KK.Frame.Net;
using KK.MsgDef;
public class MsgRespNtfRegisterMonoBase : MonoBehaviour {

    void Awake()
    {
        Register();
    }

    void OnDisable()
    {
        UnRegister();
    }

    void OnDestroy()
    {
        
    }

    protected virtual void Register()
    {
        List<Type> ls = ClassUtil.GetClasses("KK.MsgDef");
        for (int i = 0; i < ls.Count; ++i)
        {
            ProtoRespNtfAttribute item = (ProtoRespNtfAttribute)ClassUtil.GetAttribute(ls[i], typeof(ProtoRespNtfAttribute));
            if (item != null)
            {
                
                CMD_Base_RespNtf msg = Activator.CreateInstance(ls[i]) as CMD_Base_RespNtf; //ls[i].Assembly.CreateInstance(ls[i].FullName) as CMD_Base_RespNtf;
                msg.Process();
            }
        }

        Debug.Log("<color=green>[log]</color>---" + "注册dispatcher");
        //LobbyNetManager.Instance.Dispatcher.RegisterDPer(CMD_GP_AdditionScore.cmd, CMD_GP_AdditionScore.Deserialize, CMD_GP_AdditionScore.Processer);
    }

    protected virtual void UnRegister()
    {
        Debug.Log("<color=green>[log]</color>---" + "反注册dispatcher");
        LobbyNetManager.Instance.Dispatcher.UnRegisterDPer(CMD_GP_AdditionScore.cmd, CMD_GP_AdditionScore.Processer);        
    }
}
