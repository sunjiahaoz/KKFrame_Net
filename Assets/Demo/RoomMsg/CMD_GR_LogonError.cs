using UnityEngine;
using System.Collections;
using KK.Frame.Net;

namespace KK.Game.Room.Msg.Common
{
    [ProtoNetMsg(mainID = 1, subID = 101, msgTyp = ProtoNetMsgAttribute.MsgType.RespNtf)]
    public class CMD_GR_LogonError : CMD_Base_RespNtf
    {

        public int lErrorCode;                          //错误代码
        public string szErrorDescribe;				//错误消息 128

        public override void Process()
        {
            Debug.Log("<color=green>[log]</color>---" + "登录失败:" + szErrorDescribe.TrimEnd('\0'));
        }

        public override CMD_Base_RespNtf Deserialize(ByteBuffer buf)
        {
            lErrorCode = buf.ReadInt();
            szErrorDescribe = buf.ReadString(128);
            return this;
        }
    }
}
