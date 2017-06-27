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
    /// <summary>
    /// 网络相关定义
    /// </summary>
    public class NetDefine
    {
        /// <summary>
        /// 校验字段
        /// </summary>
        public const byte m_CheckCode = 0;//效验字段
        /// <summary>
        /// 版本标识
        /// </summary>
        public const byte m_CheckVer = 65;//版本标识

        /// <summary>
        /// 消息头大小
        /// </summary>
        public const int HEAD_LEN = 8;
    }

    /// <summary>
    /// 消息ID
    /// 包括主ID与子ID两部分    
    /// </summary>
    [System.Serializable]
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
        /// <summary>
        /// 默认为空的一个ID
        /// 其中主，子ID均为-1
        /// </summary>
        public static CMD_Command none = new CMD_Command(-1, -1);
        /// <summary>
        /// 主命令码
        /// </summary>
        public short wMainCmdID;
        /// <summary>
        /// 子命令码
        /// </summary>
        public short wSubCmdID;        
    }

    /// <summary>
    /// 接收消息基本类
    /// </summary>
    public abstract class CMD_Base_RespNtf
    {
        /// <summary>
        /// 消息处理
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// 消息反序列化
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="nDataSize"></param>
        /// <returns></returns>
        public virtual CMD_Base_RespNtf Deserialize(ByteBuffer buf, int nDataSize)
        {
            return Deserialize(buf);
        }

        /// <summary>
        /// 消息反序列化
        /// </summary>
        /// <param name="buf">参与序列化的buff</param>
        /// <returns></returns>
        public abstract CMD_Base_RespNtf Deserialize(ByteBuffer buf);
    }

    /// <summary>
    /// 发送的消息基本类
    /// </summary>
    public abstract class CMD_Base_Req
    {
        public abstract short MainId { get; }
        public abstract short SubId { get; }

        /// <summary>
        /// 将实际消息结构数据写入buffer中
        /// </summary>
        /// <param name="buffer"></param>
        protected virtual void WriteData(ByteBuffer buffer)
        {

        }

        /// <summary>
        /// 计算并返回消息结构的长度        
        /// </summary>
        /// <returns></returns>
        protected virtual int GetDataSize()
        {
            throw new System.NotImplementedException();            
        }

        /// <summary>
        /// 序列化消息，返回一个可以发送出去的buffer
        /// </summary>
        /// <returns></returns>
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
