using System;

namespace KK.Frame.Net
{
	public abstract  class SocketListner
	{
		abstract public void OnMessage(USocket us, ByteBuffer bb);
		abstract public void OnClose(USocket us,bool fromRemote);
		abstract public void OnIdle(USocket us);
		abstract public void OnOpen(USocket us);
        // 处理错误，错误码为SocketCustomErrorCode
        // 如果eErrorCode 为 SocketCustomErrorCode.SystemCode 则nSysErrorCode有意义，表示系统Socket的错误
        // extraInfo一般为调试输出信息
        abstract public void OnError(USocket us, NetErrorCode eErrorCode, int nSysErrorCode = -1, string extraInfo = "");
	}
}

