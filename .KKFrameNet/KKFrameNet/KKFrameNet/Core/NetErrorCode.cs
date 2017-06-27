using System.Collections;

namespace KK.Frame.Net
{
    public enum NetErrorCode
    {
        //
        // 不要定义小于Begin的枚举值，主要是与系统的SocketErrorCode区分开来
        //

        Begin = 99, // 在Begin与End之间赋值
        SystemCode,         // 系统Socket错误
        NoNameError,    // 其他错误      

        CannotConnect,  // 链接失败
        SendError,          // 发送错误
        CloseError, // 关闭Socket出错
        
        End = 400,
    }
}
