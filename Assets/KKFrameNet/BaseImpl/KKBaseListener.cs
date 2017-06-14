using UnityEngine;
using System;
using System.Collections;

namespace KK.Frame.Net
{
    public class KKBaseListener : SocketListner
    {
        MsgDispatcher _dispatcher = null;
        public KKBaseListener(MsgDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        override public void OnMessage(USocket us, ByteBuffer bb) 
        {
            try
            {
                int nDataSize = bb.ReadShort();
                int nCheckCode = bb.ReadByte();
                int nMsgVer = bb.ReadByte();
                short nMainId = bb.ReadShort();
                short nSubId = bb.ReadShort();
                Debug.Log("<color=blue>" + string.Format("Message:{0},{1},{2},{3},{4}", nDataSize, nCheckCode, nMsgVer, nMainId, nSubId) + "</color>");
                _dispatcher.Dispatcher(nMainId, nSubId, bb);                
            }
            catch (System.Exception ex)
            {                
                Debug.LogError("<color=red>[Error]</color>---" + "KKBaseListener.OnMessage Error:" + ex.Message);            	
            }
        }

        override public void OnClose(USocket us, bool fromRemote)
        {
            Debug.Log("<color=green>[log]KKBaseListener</color>---" + "OnClose");
            //MessageQueueHandler.PushError("On Close + " + fromRemote);
        }

        override public void OnIdle(USocket us)
        {
            Debug.Log("<color=green>[log]KKBaseListener</color>---" + "OnIdle");
            //MessageQueueHandler.PushError("On OnIdle ");
        }

        override public void OnOpen(USocket us)
        {
            Debug.Log("<color=green>[log]KKBaseListener</color>---" + "OnOpen");
            //MessageQueueHandler.PushError("On OnOpen ");            
        }

        override public void OnError(USocket us, NetErrorCode eCode, int nSysErrorCode = -1, string err = "")
        {
            Debug.Log("<color=green>[log]KKBaseListener</color>---" + "OnError:" + eCode + ":" + nSysErrorCode + "->" + err);
            //MessageQueueHandler.PushError("On OnError " + nErrorCode + ":" + err);
        }
    }
}
