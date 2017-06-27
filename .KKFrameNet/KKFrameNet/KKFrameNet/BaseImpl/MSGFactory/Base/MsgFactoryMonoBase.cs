using UnityEngine;
using System.Collections;

namespace KK.Frame.Net
{
    public abstract class MsgFactoryMonoBase : MonoBehaviour
    {
        protected abstract IMsgFactory factory { get; }
        /// <summary>
        /// 初始化，请在建立连接之前进行调用以注册网络消息结构
        /// </summary>
        public virtual void Init(IMsgFactory mainFactory)
        {
            RegisterNetMsg();
            mainFactory.Combine(factory);
        }        

        /// <summary>
        /// 反初始化
        /// 建议在连接断开之后进行调用
        /// </summary>
        public virtual void UnInit(IMsgFactory mainFactory)
        {
            mainFactory.UnCombine(factory);
            UnRegisterNetMsg();
        }

        protected abstract void RegisterNetMsg();
        protected abstract void UnRegisterNetMsg();        
    }
}
