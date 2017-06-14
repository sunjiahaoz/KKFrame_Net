/********************************************************************
	created:	2017/06/14 		
	file base:	NetDefine.cs	
	author:		sunjiahaoz
	
	purpose:	定义基本的数据，常量
*********************************************************************/
using UnityEngine;
using System.Collections;

namespace KK.Frame.Net
{
    public class NetDefine
    {
        public const byte m_CheckCode = 0;//效验字段
        public const byte m_CheckVer = 65;//版本标识

        public const int HEAD_LEN = 8;
    }

    public struct CMD_Command
    {
        public CMD_Command(short cmdMainId, short cmdSubId)
        {
            wMainCmdID = cmdMainId;
            wSubCmdID = cmdSubId;
        }

        public override string ToString()
        {
            return string.Format("Commond({0},{1})", wMainCmdID, wSubCmdID);
        }

        public static CMD_Command none = new CMD_Command(-1, -1);
        public short wMainCmdID;                            //主命令码
        public short wSubCmdID;                         //子命令码
    }

    // 接收消息基本类
    public class CMD_Base_RespNtf
    {
    }
    // 发送的消息基本类
    public abstract class CMD_Base_Req
    {        
        public abstract short MainId { get; }
        public abstract short SubId { get; }

        protected virtual void WriteData(ByteBuffer buffer)
        {

        }

        protected virtual int GetDataSize()
        {
            return NetDefine.HEAD_LEN;//注意发送大小变化 字符串之间按4字节对齐
        }

        public ByteBuffer Serialize()
        {
            int nDataSize = GetDataSize();
            ByteBuffer buffer = new ByteBuffer();
            WriteHead(buffer, (short)nDataSize, MainId, SubId);
            WriteData(buffer);
            return buffer;
        }

        protected void WriteHead(ByteBuffer buffer, short size, short mainId, short subId)
        {
            if (buffer == null)
                return;
            buffer.WriteShort(size);
            buffer.WriteByte(NetDefine.m_CheckCode);//效验字段
            buffer.WriteByte(NetDefine.m_CheckVer);//版本标识
            buffer.WriteShort(mainId);
            buffer.WriteShort(subId);
        }
    }
}
