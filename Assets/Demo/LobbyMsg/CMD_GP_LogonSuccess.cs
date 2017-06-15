using UnityEngine;
using System.Collections;
using KK.Frame.Net;

namespace KK.Game.Lobby.Msg
{
    [ProtoNetMsg(mainID = 1, subID = 100, msgTyp = ProtoNetMsgAttribute.MsgType.RespNtf)]
    public class CMD_GP_LogonSuccess : CMD_Base_RespNtf
    {
        public static CMD_Base_RespNtf CreateInstance()
        {
            return new CMD_GP_LogonSuccess();
        }

        public short wFaceID;                       //头像索引
        public byte cbGender;                       //用户性别
        public byte cbMember;                       //会员等级
        public int dwUserID;                        //用户 I D
        public int dwGameID;                        //游戏 I D
        public int dwExperience;                    //用户经验
        public int lUserScore;                      //用户银子
        public int lUserBank;                       //用户银行
        public byte bMoorMachine;                   //锁定信息
        public byte lineType;                       //线路类型
        public string szNotice;                     //公告内容 301
        public byte bShowTrust;                     //是否显示银行转账
        public byte bConfirmAccounts;//是否绑定过帐号
        public byte bBankExists;//是否创建过银行密码
        public byte bDayFirstLogon;					//今天是不是第一次登录 


        public override void Process()
        {
            Debug.Log("<color=green>[log]</color>---" + "CMD_GP_LogonSuccess:" + dwUserID);
        }

        public override CMD_Base_RespNtf Deserialize(ByteBuffer buf)
        {
            wFaceID = buf.ReadShort();
            cbGender = buf.ReadByte();
            cbMember = buf.ReadByte();
            dwUserID = buf.ReadInt();
            dwGameID = buf.ReadInt();
            dwExperience = buf.ReadInt();
            lUserScore = buf.ReadInt();
            lUserBank = buf.ReadInt();
            bMoorMachine = buf.ReadByte();
            lineType = buf.ReadByte();
            szNotice = buf.ReadString(301);
            bShowTrust = buf.ReadByte();
            bConfirmAccounts = buf.ReadByte();
            bBankExists = buf.ReadByte();
            bDayFirstLogon = buf.ReadByte();
            return this;
        }
    }
}
