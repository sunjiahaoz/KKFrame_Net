using UnityEngine;
using System.Collections;

namespace KK.Frame.Net
{
    public interface INetManager
    {
        // 初始化
        void CreateInit(string strIP, int nIP, System.Object objExtend = null);
        // 发送消息
        void SendMessage(CMD_Base_Req req);
    }
}
