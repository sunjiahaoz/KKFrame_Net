using UnityEngine;
using System.Collections;
using KK.Frame.Net;

namespace KK.Game.Lobby.Msg
{
    [ProtoNetMsg(mainID = 1, subID = 103, msgTyp = ProtoNetMsgAttribute.MsgType.RespNtf)]
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

        public override CMD_Base_RespNtf Deserialize(ByteBuffer buf)
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
            return this;
        }

        public override void Process()
        {
            CMD_GP_AdditionScore msg = this as CMD_GP_AdditionScore;
            Debug.Log("<color=green>[log]</color>---" + msg.bVipGetScore);
        }

        public static CMD_Base_RespNtf CreateInstance()
        {
            return new CMD_GP_AdditionScore();
        }
    }
}
