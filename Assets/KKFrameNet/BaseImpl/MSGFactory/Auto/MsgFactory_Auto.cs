using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using KK.Frame.Util;

namespace KK.Frame.Net
{
    public class MsgFactory_Auto : IMsgFactory
    {
        public class MsgItem
        {
            public CMD_Command cmd;
            public Type msgRespNtf;
            public Type msgReq;
        }        
        
        public int groupId
        {
            get;
            set;
        }

        string _strSearchNameSpace = "";
        public MsgFactory_Auto(string strSearchNameSpace, int nGroupId)
        {
            _strSearchNameSpace = strSearchNameSpace;
            groupId = nGroupId;
        }
        
        Dictionary<CMD_Command, MsgItem> _dictItem = new Dictionary<CMD_Command, MsgItem>();
        public virtual void Init()
        {
            _dictItem.Clear();            
            List<Type> ls = ClassUtil.GetClasses(_strSearchNameSpace);
            if (ls.Count== 0)
            {
                Debug.LogWarning("<color=orange>[Warning]</color>---" + _strSearchNameSpace + "中找不到任何消息类型！");
                return;
            }
            for (int i = 0; i < ls.Count; ++i)
            {
                ProtoNetMsgAttribute attr = ClassUtil.GetAttribute(ls[i], typeof(ProtoNetMsgAttribute)) as ProtoNetMsgAttribute;
                if (attr == null)
                {
                    continue;
                }

                CMD_Command cmd = new CMD_Command(attr.mainID, attr.subID);
                if (!_dictItem.ContainsKey(cmd))
                {
                    _dictItem.Add(cmd, new MsgItem());
                }

                if (attr.msgTyp == ProtoNetMsgAttribute.MsgType.Req)
                {
                    _dictItem[cmd].msgReq = ls[i];
                }
                else
                {
                    _dictItem[cmd].msgRespNtf = ls[i];
                }
            }
        }

        public virtual CMD_Base_RespNtf CreateRespNtfMsg(CMD_Command cmd)
        {
            if (!_dictItem.ContainsKey(cmd))
            {
                return null;
            }

            try
            {
                CMD_Base_RespNtf msg = Activator.CreateInstance(_dictItem[cmd].msgRespNtf) as CMD_Base_RespNtf;
                return msg;
            }
            catch (System.Exception ex)
            {
            	Debug.LogError("<color=red>[Error]</color>---" + "CreateRespNtfMsg 创建失败！！：" + ex.Message);
                return null;
            }
        }

        public virtual CMD_Base_Req CreateReqMsg(CMD_Command cmd)
        {
            if (!_dictItem.ContainsKey(cmd))
            {
                return null;
            }

            try
            {
                CMD_Base_Req msg = Activator.CreateInstance(_dictItem[cmd].msgReq) as CMD_Base_Req;
                return msg;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("<color=red>[Error]</color>---" + "CreateReqMsg 创建失败！！：" + ex.Message);
                return null;
            }
        }

        public virtual void Combine(IMsgFactory factory)
        {
            if (groupId != factory.groupId)
            {
                Debug.LogWarning("<color=orange>[Warning]</color>---" + "groupId不同，不能合并");
                return;
            }

            MsgFactory_Auto fsrc = factory as MsgFactory_Auto;
            if (fsrc == null)
            {
                Debug.LogWarning("<color=orange>[Warning]</color>---" + "MsgFactory类型非法，合并失败");
                return;
            }
            ToolsUseful.CombineDict<CMD_Command, MsgItem>(_dictItem, fsrc._dictItem, false);
        }

        public virtual void UnCombine(IMsgFactory factory)
        {
            if (groupId != factory.groupId)
            {
                Debug.LogWarning("<color=orange>[Warning]</color>---" + "groupId不同，不需要取消合并");
                return;
            }
            MsgFactory_Auto fsrc = factory as MsgFactory_Auto;
            if (fsrc == null)
            {
                Debug.LogWarning("<color=orange>[Warning]</color>---" + "MsgFactory类型非法，取消合并失败");
                return;
            }
            foreach(var keyItem in fsrc._dictItem.Keys)
            {
                this._dictItem.Remove(keyItem);
            }
        }
    }
}
