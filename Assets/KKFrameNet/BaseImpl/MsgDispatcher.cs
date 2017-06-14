﻿/********************************************************************
	created:	2017/06/14 		
	file base:	MsgDispatcher.cs	
	author:		sunjiahaoz
	
	purpose:	消息分发器
    包括消息的解析--->放到队列--->分发处理
    其中消息的解析需要在之前对各个消息的解析函数进行注册；
    以及处理也需要在之前对各个消息的监听函数进行注册；
    这两种注册都支持动态的添加、删除
*********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KK.Frame.Net
{   
    [RequireComponent(typeof(MessageQueueHandler))]
    public class MsgDispatcher : MonoBehaviour
    {
        MessageQueueHandler _msgQueue;
        protected virtual MessageQueueHandler msgQueue
        {
            get
            {
                if (_msgQueue == null)
                {
                    _msgQueue = GetComponent<MessageQueueHandler>();
                    if (_msgQueue == null)
                    {
                        _msgQueue = gameObject.AddComponent<MessageQueueHandler>();
                    }
                }
                return _msgQueue;
            }
        }
        protected virtual void Start()
        {
            msgQueue._actionDoCallBack += _msgQueueDoCallBack;
        }
        protected virtual void OnDestroy()
        {
            msgQueue._actionDoCallBack -= _msgQueueDoCallBack;
        }

        /// <summary>
        /// 解析并分发
        /// </summary>
        /// <param name="nMainID">消息主ID</param>
        /// <param name="nSubID">消息副ID</param>
        /// <param name="buf">网络数据</param>
        public virtual void Dispatcher(short nMainID, short nSubID, ByteBuffer buf)
        {
            // 解析
            CMD_Command cmd = new CMD_Command(nMainID, nSubID);
            CMD_Base_RespNtf msg = DeserializeMsg(cmd, buf);
            if (msg == null)
            {
                return;
            }
            // push进队列
            msgQueue.PushQueue(nMainID, nSubID, msg);
        }

        #region _Register_
        /// <summary>
        /// 注册消息ID对应的解析器与处理器
        /// </summary>
        /// <param name="nMainID"></param>
        /// <param name="nSubID"></param>
        /// <param name="deserializer"></param>
        /// <param name="processer"></param>
        public virtual void RegisterDPer(short nMainID, short nSubID, Deserialize deserializer, Processer processer)
        {
            RegisterDPer(new CMD_Command(nMainID, nSubID), deserializer, processer);
        }
        /// <summary>
        /// 注册消息ID对应的解析器与处理器
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="deserializer"></param>
        /// <param name="processer"></param>
        public virtual void RegisterDPer(CMD_Command cmd, Deserialize deserializer, Processer processer)
        {
            RegisterDeserializer(cmd, deserializer);
            RegisterProcesser(cmd, processer);
        }
        /// <summary>
        /// 反注册消息id对应的解析器与处理器
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="processer"></param>
        public virtual void UnRegisterDPer(CMD_Command cmd, Processer processer)
        {
            UnRegisterDeserializer(cmd);
            UnRegisterProcesser(cmd, processer);
        }
        /// <summary>
        /// 反注册消息id对应的解析器与处理器
        /// </summary>
        /// <param name="nMainID"></param>
        /// <param name="nSubID"></param>
        /// <param name="processer"></param>
        public virtual void UnRegisterDPer(short nMainID, short nSubID, Processer processer)
        {
            UnRegisterDPer(new CMD_Command(nMainID, nSubID), processer);
        }
        #endregion
        #region _解析器_
        public delegate CMD_Base_RespNtf Deserialize(ByteBuffer buf);
        protected Dictionary<CMD_Command, Deserialize> _dictDeserialize = new Dictionary<CMD_Command, Deserialize>();

        /// <summary>
        /// 添加一个新的解析
        /// 如果该CMD已经存在解析函数了，则会覆盖
        /// </summary>
        /// <param name="cmd">commandID</param>
        /// <param name="deserializer">解析函数</param>
        public virtual void RegisterDeserializer(CMD_Command cmd, Deserialize deserializer)
        {
            if (_dictDeserialize.ContainsKey(cmd))
            {
                Debug.LogWarning("<color=orange>[Warning]</color>---" + "已经存在对"+cmd+"的解析，进行覆盖");
                _dictDeserialize[cmd] = deserializer;                
            }
            else
            {
                _dictDeserialize.Add(cmd, deserializer);
            }
        }

        /// <summary>
        /// 移除一个解析函数
        /// </summary>
        /// <param name="cmd"></param>
        public virtual void UnRegisterDeserializer(CMD_Command cmd)
        {
            _dictDeserialize.Remove(cmd);
        }

        /// <summary>
        /// 通过Command，解析buf中的消息
        /// </summary>
        /// <param name="cmd">commandID</param>
        /// <param name="buf">要解析的网络数据</param>
        /// <returns>解析出的消息结构，如果不存在该消息的解析函数，则返回null</returns>
        protected virtual CMD_Base_RespNtf DeserializeMsg(CMD_Command cmd, ByteBuffer buf)
        {
            if (!_dictDeserialize.ContainsKey(cmd))
            {
                return null;
            }
            return _dictDeserialize[cmd](buf);
        }
        #endregion
        #region _处理器_
        public delegate void Processer(CMD_Base_RespNtf msgBase);
        Dictionary<CMD_Command, Processer> _dictProcesser = new Dictionary<CMD_Command, Processer>();

        public virtual void RegisterProcesser(CMD_Command cmd, Processer processer)
        {
            if (!_dictProcesser.ContainsKey(cmd))
            {
                _dictProcesser.Add(cmd, null);
            }
            _dictProcesser[cmd] += processer;
        }

        public virtual void UnRegisterProcesser(CMD_Command cmd, Processer processer)
        {
            if (!_dictProcesser.ContainsKey(cmd))
            {
                return;
            }
            _dictProcesser[cmd] -= processer;
        }

        protected virtual void ProcessMsg(CMD_Command cmd, CMD_Base_RespNtf msgBase)
        {
            if (!_dictProcesser.ContainsKey(cmd))
            {
                return;
            }
            if (msgBase == null)
            {
                Debug.LogWarning("<color=#FFA300FF>没有" + cmd + " 的消息反序列化操作结构体" + "</color>");
                return;
            }
            msgBase.Process();
            //_dictProcesser[cmd](msgBase);
        }

        protected virtual void _msgQueueDoCallBack(QueueItem item)
        {
            ProcessMsg(item._cmd, item._msgBase);
        }
        #endregion
    }
}