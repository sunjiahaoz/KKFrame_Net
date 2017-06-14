using UnityEngine;
using System.Collections;

namespace KK.Frame.Net
{
    public interface INetManager
    {
        // 初始化
        void CreateInit(Hashtable param);
        // 发送消息
        void SendMessage(CMD_Base_Req req);
    }
}
