using UnityEngine;
using System.Collections;
using KK.Frame.Net;

namespace KK.Game.Lobby.Msg
{
    [ProtoNetMsg(mainID = 1, subID = 104, msgTyp = ProtoNetMsgAttribute.MsgType.RespNtf)]
    public class CMD_GP_PhoneCtrl : CMD_Base_RespNtf
    {
        byte show;
        public override void Process()
        {
            Debug.Log("<color=green>[log]</color>---" + "show:" + show);
        }

        public override CMD_Base_RespNtf Deserialize(ByteBuffer buf)
        {
            show = buf.ReadByte();
            return this;
        }

        public static CMD_Base_RespNtf CreateInstance()
        {
            return new CMD_GP_PhoneCtrl();
        }
    }
}
