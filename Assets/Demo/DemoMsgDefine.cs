using UnityEngine;
using System.Collections;
using KK.Frame.Net;
using System;

namespace KK.MsgDef
{
    // 接收消息属性
    [AttributeUsage(AttributeTargets.Class)]
    public class ProtoRespNtfAttribute : Attribute
    {
        public short mainID { get; set; }
        public short subID { get; set; }
        public string strDeserialize { get; set; }
        public string strDefaultProcesser { get; set; }
    }

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
    }

    [ProtoRespNtf(mainID = 1, strDefaultProcesser ="Processer", strDeserialize = "Deserialize", subID = 103)]
    public class CMD_GP_AdditionScore : CMD_Base_RespNtf
    {
        public int dwLottery;  //奖券
        public int dwIngot;    //元宝
                               /// <summary>
                               /// VIP等级
                               /// </summary>
        public int dwVip;
        /// <summary>
        /// 记牌器时间
        /// </summary>
        public int dwCalCardTime;
        /// <summary>
        /// 抽奖剩余时间
        /// </summary>
        public int dwFreeLuckyTime;
        /// <summary>
        /// VIP 剩余天数
        /// </summary>
        public int dwVipOverTimes;
        /// <summary>
        /// 喇叭个数
        /// </summary>
        public int dwHornCount;

        /// <summary>
        /// VIP成长值
        /// </summary>
        public int VIP_Value;
        public int dwMinimum;

        /// <summary>
        /// VIP礼包等级对应是否领取 //10个
        /// </summary>
        public string szVipGift;
        /// <summary>
        /// VIP是否过期
        /// </summary>
        public byte bVipOverTime;
        /// <summary>
        /// VIP是否领取了金币
        /// </summary>
        public byte bVipGetScore;//0 未领取

        public int dwRecommondScore;                //推荐积分
        public int dwRecommondGet;                  //推荐积分（已领取）
        public byte bPhone;
        public int dwRoomCard;  // 房卡
        public bool bCardShared; // 是否已经获得分享房卡

        public CMD_Base_RespNtf Deserialize(ByteBuffer buf)
        {
            CMD_GP_AdditionScore t = this;
            // 读取buf填充结构体 todo
            t.dwLottery = buf.ReadInt();
            t.dwIngot = buf.ReadInt();
            t.dwVip = buf.ReadInt();
            t.dwCalCardTime = buf.ReadInt();
            t.dwFreeLuckyTime = buf.ReadInt();
            t.dwVipOverTimes = buf.ReadInt();
            t.dwHornCount = buf.ReadInt();
            t.VIP_Value = buf.ReadInt();
            t.dwMinimum = buf.ReadInt();
            t.szVipGift = buf.ReadString(10);
            t.bVipOverTime = buf.ReadByte();
            t.bVipGetScore = buf.ReadByte();
            t.dwRecommondScore = buf.ReadInt();
            t.dwRecommondGet = buf.ReadInt();
            t.bPhone = buf.ReadByte();
            buf.ReadBytes(3);
            t.dwRoomCard = buf.ReadInt();
            t.bCardShared = buf.ReadBool();
            buf.ReadBytes(3);

            return t;
        }
        public static void Processer(CMD_Base_RespNtf msgBase)
        {
            msgBase.Process();
            return;

            CMD_GP_AdditionScore msg = msgBase as CMD_GP_AdditionScore;
            // 处理数据 todo

            Debug.Log("<color=green>[log]</color>---" + msg.bVipGetScore);
        }

        public override void Process()
        {
            Debug.Log("<color=green>[log]</color>---" + "gaaggaagag");
            return;

            CMD_GP_AdditionScore msg = this as CMD_GP_AdditionScore;
            Debug.Log("<color=green>[log]</color>---" + msg.bVipGetScore);
        }

        public static CMD_Command cmd
        {
            get
            {
                return new CMD_Command(1, 103);
            }
        }
    }
}