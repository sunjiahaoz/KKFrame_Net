using UnityEngine;
using System.Collections;
using KK.Frame.Net;

namespace KK.Game.Room.Msg.Common
{
    [ProtoNetMsg(mainID = 1, subID = 100, msgTyp = ProtoNetMsgAttribute.MsgType.RespNtf)]
    public class ROOM_GR_LOGON_SUCCESS : CMD_Base_RespNtf
    {
        public static CMD_Base_RespNtf CreateInstance()
        {
            return new ROOM_GR_LOGON_SUCCESS();
        }

        int dwUserID;

        public override void Process()
        {
            Debug.Log("<color=green>[log]</color>---" + "登录成功:" + dwUserID);
        }

        public override CMD_Base_RespNtf Deserialize(ByteBuffer buf)
        {
            dwUserID = buf.ReadInt();
            return this;
        }
    }
}
