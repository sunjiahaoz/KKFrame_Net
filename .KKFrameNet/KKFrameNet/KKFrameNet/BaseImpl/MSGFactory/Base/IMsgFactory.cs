using UnityEngine;
using System.Collections;

namespace KK.Frame.Net
{
    public interface IMsgFactory
    {
        /// <summary>
        /// 相同GroupID的Factory才可以混合
        /// </summary>
        int groupId
        {
            get;
            set;
        }
        // 初始化
        void Init();

        /// <summary>
        /// 获得一个RespNtf消息
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        CMD_Base_RespNtf CreateRespNtfMsg(CMD_Command cmd);
        // 获取一个req消息
        //CMD_Base_Req CreateReqMsg(CMD_Command cmd);

        /// <summary>
        /// 合并，如果有相同CMD_command的，则会覆盖掉
        /// </summary>
        /// <param name="factory"></param>
        void Combine(IMsgFactory factory);
        /// <summary>
        /// 取消合并，之前如果合并时覆盖掉的不会还原
        /// </summary>
        /// <param name="factory"></param>
        void UnCombine(IMsgFactory factory);
    }
}
