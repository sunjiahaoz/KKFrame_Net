/********************************************************************
	created:	2017/05/23 		
	file base:	ShortSocketPool.cs	
	author:		sunjiahaoz
	
	purpose:	短连接的Socket池
    主要用于短连接的Socket，通过NewSocket获得一个socket进行连接发送消息，
    当该Socket.Close调用之后会将该Socket回收。
*********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KK.Frame.Net
{   
    public class ShortSocketPool
    {
        private class ShortUSocket : USocket
        {
            public ShortUSocket() : base() { }
            public ShortUSocket(SocketListner listner, Protocal protocal) : base(listner, protocal) { }

            public int ID { get; set; }
            public bool IsInPool { get; set; }
            public System.Action<ShortUSocket> _actionClose = null;

            public override void Close(bool serverClose = false)
            {
                base.Close(serverClose);
                if (_actionClose != null)
                {
                    _actionClose(this);
                }
            }
        }

        Queue<ShortUSocket> _lstPool = new Queue<ShortUSocket>();
        int _nIDIndex = 0;
        public USocket NewSocket(SocketListner listener, Protocal protocal)
        {
            ShortUSocket socket = null;
            if (_lstPool.Count == 0)
            {
                socket = new ShortUSocket(listener, protocal);
                socket.ID = _nIDIndex++;
                Debug.Log("<color=cyan>" + "创建" + socket.ID + "号Socket" + "</color>");
            }
            else
            {
                socket = _lstPool.Dequeue();
                socket.setLister(listener);
                socket.setProtocal(protocal);
                Debug.Log("<color=cyan>" + "直接使用" + socket.ID + "号Socket" + "</color>");
            }
            socket.IsInPool = false;
            socket._actionClose = OnShortSocketClose;
            return socket;
        }

        void OnShortSocketClose(ShortUSocket socket)
        {
            Debug.Log("<color=cyan>"+socket.ID + "关闭了，入池 ~~"+"</color>");
            socket.IsInPool = true;
            _lstPool.Enqueue(socket);
        }
    }
}
