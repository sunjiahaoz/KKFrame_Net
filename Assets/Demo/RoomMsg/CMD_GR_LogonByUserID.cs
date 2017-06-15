using UnityEngine;
using System.Collections;
using KK.Frame.Net;
namespace KK.Game.Room.Msg.Common
{
    [ProtoNetMsg(mainID = 1, subID = 4, msgTyp = ProtoNetMsgAttribute.MsgType.Req)]
    public class CMD_GR_LogonByUserID : CMD_Base_Req
    {
        public override short MainId { get { return 1; } }
        public override short SubId { get { return 4; } }

        public int dwPlazaVersion;                      //广场版本
        public int dwProcessVersion;                    //进程版本
        public int dwUserID;                            //用户 I D
        public string szPassWord;               //登录密码 33
        public byte bHasVideo;							//有无视频


        protected override void WriteData(ByteBuffer buffer)
        {
            buffer.WriteInt(dwPlazaVersion);
            buffer.WriteInt(dwProcessVersion);
            buffer.WriteInt(dwUserID);
            buffer.WriteString(KKNetUtil.Get4scalPasswordCN(szPassWord, 33));
            buffer.WriteByte(bHasVideo);
            buffer.WriteBytes(new byte[2]);
        }

        protected override int GetDataSize()
        {
            return NetDefine.HEAD_LEN + 48;
        }
    }
}
