/********************************************************************
	created:	2017/06/14 		
	file base:	MessageQueueHandler.cs	
	author:		sunjiahaoz
	
	purpose:	消息队列
    主要是接收收到的网络消息放到队列里，通过每帧获取队列里的消息进行分发处理
    分发使用分发器Dispatcher
*********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace KK.Frame.Net
{  
    public class MessageQueueHandler : MonoBehaviour
    {
        static Queue<QueueItem> messageQueue;
        static Queue<QueueItem> errorQueue;
        public static System.Action<QueueItem> _actionDoCallBack = null;
        
        void Awake()
        {
            messageQueue = new Queue<QueueItem>();
            errorQueue = new Queue<QueueItem>();
        }

        // Update is called once per frame
        void Update()
        {
            if (messageQueue.Count > 0)
            {
                lock (messageQueue)
                {
                    QueueItem queueItem = messageQueue.Dequeue();
                    DoCallback(queueItem);
                }
            }
            else if (errorQueue.Count > 0)
            {
                lock (errorQueue)
                {
                    QueueItem errorItem = errorQueue.Dequeue();
                    Debug.Log("<color=#FFA300FF>"+errorItem._strMsg.TrimEnd('\0') + "</color>");
                }
            }
        }

        public static void PushQueue(short wMainCmdId, short wSubCmdId, CMD_Base_RespNtf msgBase)
        {
            lock (messageQueue)
            {
                messageQueue.Enqueue(new QueueItem(wMainCmdId, wSubCmdId, msgBase));
            }
        }

        /// <summary>
        /// 推送错误消息
        /// </summary>
        /// <param name="msg">Message.</param>
        /// <param name="state">State.</param>
        public static void PushError(string msg, short state = 0)
        {
            lock (errorQueue)
            {
                errorQueue.Enqueue(new QueueItem(-1, -1, null, msg));
            }
        }

        /// <summary>
        /// 回调方法执行
        /// </summary>
        /// <param name="param">Parameter.</param>
        private static void DoCallback(QueueItem item)
        {
            if (_actionDoCallBack != null)
            {
                _actionDoCallBack(item);
            }            
        }
    }

    public class QueueItem
    {
        //public short wDataSize;							//数据大小
        //public byte cbCheckCode;						//效验字段
        //public byte cbMessageVer;						//版本标识
        public CMD_Command _cmd;                            //命令码
        public CMD_Base_RespNtf _msgBase;
        public string _strMsg;

        public object Model;
        public QueueItem(short wMainCmdId, short wSubCmdId, CMD_Base_RespNtf msgBase, string msg = "")
        {
            _cmd = new CMD_Command(wMainCmdId, wSubCmdId);
            _msgBase = msgBase;
            _strMsg = msg;
        }
    }

}
