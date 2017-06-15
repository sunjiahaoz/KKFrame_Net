using UnityEngine;
using System.Collections;
using KK.Frame.Net;
using System;

namespace KK.MsgDef
{    
    [ProtoNetMsg(mainID = 1, subID = 9, msgTyp = ProtoNetMsgAttribute.MsgType.Req)]
    public class CMD_Req_SUB_GP_LOGON_PHONE : CMD_Base_Req
    {
        public override short MainId { get { return 1; } }
        public override short SubId { get { return 9; } }

        // 消息字段///
        public int dwPlazaVersion;                  //广场版本
        public string szAccounts;       //登录帐号  32
        public string szPassWord;       //登录密码 33
        public string szMac;        //网卡 255
        public string szHD;     //硬盘 255        

        protected override void WriteData(ByteBuffer buffer)
        {
            buffer.WriteInt(dwPlazaVersion);
            buffer.WriteString(KKNetUtil.Get4scalPasswordCN(szAccounts, 32));
            buffer.WriteString(KKNetUtil.Get4scalPasswordCN(szPassWord, 33));
            buffer.WriteString(KKNetUtil.Get4scalPasswordCN(szMac, 255));
            buffer.WriteString(KKNetUtil.Get4scalPasswordCN(szHD, 255));
            buffer.WriteByte(0);
        }

        protected override int GetDataSize()
        {
            return NetDefine.HEAD_LEN + 580;
        }
        public static CMD_Base_Req CreateInstance()
        {
            return new CMD_Req_SUB_GP_LOGON_PHONE();
        }
    }

    

   
}