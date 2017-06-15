using UnityEngine;
using System;
using System.Collections;

namespace KK.Frame.Net
{
    // 接收消息属性
    [AttributeUsage(AttributeTargets.Class)]
    public class ProtoNetMsgAttribute : Attribute
    {
        public enum MsgType
        {
            Req = 0,
            RespNtf = 1,
        }

        public MsgType msgTyp { get; set; }   // 0 为发送的消息结构，1为接收的消息结构
        public short mainID { get; set; }
        public short subID { get; set; }
        //public string strDeserialize { get; set; }
        //public string strDefaultProcesser { get; set; }
    }   

}
