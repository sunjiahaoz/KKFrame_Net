using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KK.Frame.Util;

namespace KK.Frame.Net
{
    public class MsgFactory_Manual : IMsgFactory
    {  
        public virtual int groupId { get; set; }

        public delegate CMD_Base_Req CreateInstance_req();
        public delegate CMD_Base_RespNtf CreateInstance_respNtf();
        Dictionary<CMD_Command, CreateInstance_req> _dictCreateReq = new Dictionary<CMD_Command, CreateInstance_req>();
        Dictionary<CMD_Command, CreateInstance_respNtf> _dictCreateRespNtf = new Dictionary<CMD_Command, CreateInstance_respNtf>();


        public void Init()
        {
            _dictCreateReq.Clear();
            _dictCreateRespNtf.Clear();

            //_dictCreateReq.Add(new CMD_Command(1, 9), KK.MsgDef.CMD_Req_SUB_GP_LOGON_PHONE.CreateInstance);
            //_dictCreateRespNtf.Add(new CMD_Command(1, 103), KK.Game.Lobby.Msg.CMD_GP_AdditionScore.CreateInstance);
        }
        
        public virtual void Combine(IMsgFactory fact)
        {
            if (fact.groupId != groupId)
            {
                Debug.LogWarning("<color=orange>[Warning]</color>---" + "groupId不同，不能合并");
                return;
            }
            MsgFactory_Manual factory = fact as MsgFactory_Manual;            
            if (factory == null)
            {
                Debug.LogWarning("<color=orange>[Warning]</color>---" + "MsgFactory类型非法，合并失败");
                return;
            }
            ToolsUseful.CombineDict<CMD_Command, CreateInstance_req>(_dictCreateReq, factory._dictCreateReq, false);
            ToolsUseful.CombineDict<CMD_Command, CreateInstance_respNtf>(_dictCreateRespNtf, factory._dictCreateRespNtf, false);            
        }

        public virtual void UnCombine(IMsgFactory fact)
        {
            if (fact.groupId != groupId)
            {
                Debug.LogWarning("<color=orange>[Warning]</color>---" + "groupId不同，不能取消合并");
                return;
            }
            MsgFactory_Manual factory = fact as MsgFactory_Manual;
            if (factory == null)
            {
                Debug.LogWarning("<color=orange>[Warning]</color>---" + "MsgFactory类型非法，取消合并失败");
                return;
            }

            foreach (var keyItem in factory._dictCreateReq.Keys)
            {
                this._dictCreateReq.Remove(keyItem);
            }
            foreach (var keyItem in factory._dictCreateRespNtf.Keys)
            {
                this._dictCreateRespNtf.Remove(keyItem);
            }
        }
        

        public CMD_Base_RespNtf CreateRespNtfMsg(CMD_Command cmd)
        {
            if (_dictCreateRespNtf.ContainsKey(cmd))
            {
                return _dictCreateRespNtf[cmd]();
            }
            return null;
        }

        public CMD_Base_Req CreateReqMsg(CMD_Command cmd)
        {
            if (_dictCreateReq.ContainsKey(cmd))
            {
                return _dictCreateReq[cmd]();
            }
            return null;
        }
    }
}
