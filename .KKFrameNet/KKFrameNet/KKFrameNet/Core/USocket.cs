using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace KK.Frame.Net
{
    public class USocket
    {
        private Socket clientSocket;
        private SocketListner listner;
        private Protocal protocal;
        private string ip;
        private int port;
        private int status;
        private bool asyc = false;//异步收取
        private bool serverClose = true;//服务器主动关闭
        public const int STATUS_INIT = 0;
        public const int STATUS_CONNECTING = 1;
        public const int STATUS_CONNECTED = 2;
        public const int STATUS_CLOSED = 3;        
        private byte[] buf;

        public System.Action<bool> _actionConnected = null; // 链接之后的回调，参数为链接是否成功        
        /**
         * 构造（但不完善，需要设置监听器和协议解析器）
         */
        public USocket()
        {            
            buf = new byte[1024 * 4];
        }
        /**
         * 构造
         */
        public USocket(SocketListner listner, Protocal protocal)
        {
            this.listner = listner;
            this.protocal = protocal;            
            buf = new byte[1024 * 4];
        }

        /**
         * 装入一个监听器
         */
        public void setLister(SocketListner listner)
        {
            this.listner = listner;
        }
        /**
         * 装入一个协议解析器
         */
        public void setProtocal(Protocal p)
        {
            this.protocal = p;
        }
        /**
         * 协议
         */
        public Protocal getProtocal()
        {
            return this.protocal;
        }
        public int getStatus()
        {
            return this.status;
        }
        public bool isAsyc()
        {
            return asyc;
        }
        public void setAsyc(bool a)
        {
            this.asyc = a;
        }
        public string getIp()
        {
            return this.ip;
        }
        public int getPort()
        {
            return this.port;
        }
        /**
         * 连接指定地址
         */
        public void Connect(string ip, int port, System.Action<bool> actionConnected = null)
        {
            this._actionConnected = actionConnected;
            this.status = STATUS_CONNECTING;
            this.ip = ip;
            this.port = port;
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.NoDelay = true;
            LingerOption linger = new LingerOption(false, 0);
            clientSocket.LingerState = linger;
            clientSocket.BeginConnect(this.ip, this.port, connected, this);            
        }

        /**
         * 关闭连接
         */
        public virtual void Close(bool serverClose=false)
        {   
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    this.status = STATUS_CLOSED;
                    this.serverClose = serverClose;
                }
            }
            catch (Exception e)
            {
                listner.OnError(this, NetErrorCode.CloseError, -1, e.Message);
            }
        }
        /**
         * 连接成功
         */        
        private void connected(IAsyncResult asyncConnect)
        {
            if (this.clientSocket.Connected)
            {
                this.clientSocket.EndConnect(asyncConnect);
                this.status = STATUS_CONNECTED;
                this.listner.OnOpen(this);                
                Thread thread = new Thread(new ThreadStart(receive));
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                this.listner.OnError(this, NetErrorCode.CannotConnect);
                this.Close(false);
            }

            if (_actionConnected != null)
            {
                _actionConnected(this.clientSocket.Connected);
            }
        }
        
        /**
         *发送
         */
        public IAsyncResult Send(ByteBuffer buf)
        {
            try
            {
                byte[] msg = buf.ToBytes();
                IAsyncResult asyncSend = clientSocket.BeginSend(msg, 0, buf.GetLength(), SocketFlags.None, sended, buf);
                return asyncSend;
            }
            catch(System.Net.Sockets.SocketException e)
            {
                listner.OnError(this, NetErrorCode.SystemCode, e.ErrorCode, e.Message);
                return null;
            }
            catch (Exception e)
            {
                listner.OnError(this, NetErrorCode.SendError, -1, e.Message);
                return null;
            }
        }
        /**
         * 发送成功的回调
         */
        private void sended(IAsyncResult ar)
        {            
            this.clientSocket.EndSend(ar);
        }
        /**
         * 接收数据
         */
        private void receive()
        {
            while (this.status == STATUS_CONNECTED)
            {
                try
                {
                    if (clientSocket.Poll(-1, SelectMode.SelectRead))
                    {
                        try
                        {
                            if (asyc)//异步收取
                            {
                                clientSocket.BeginReceive(buf, 0, buf.Length, SocketFlags.None, new AsyncCallback(onRecieved), clientSocket);
                            }
                            else //同步收取
                            {
                                int len = clientSocket.Receive(buf);
                                if (len > 0)
                                {
                                    while (true)
                                    {
                                        ByteBuffer frame = this.protocal.TranslateFrame(buf, len);
                                        if (frame != null)
                                        {
                                            this.listner.OnMessage(this, frame);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    this.Close(true);
                                }
                            }
                        }
                        catch (System.Net.Sockets.SocketException e)
                        {
                            this.status = STATUS_CLOSED;
                            this.listner.OnError(this, NetErrorCode.SystemCode, e.ErrorCode, e.StackTrace + e.Message);
                            this.Close(true);
                        }
                    }
                    else
                    {
                        this.Close(true);
                    }
                }
                catch (System.Exception ex)
                {
                    listner.OnError(this, NetErrorCode.NoNameError, -1, ex.Message);
                }                          
            }
            this.listner.OnClose(this, serverClose);            
        }
        /**
         * 异步收取信息
         */
        private void onRecieved(IAsyncResult ar)
        {
            try
            {
                Socket so = (Socket)ar.AsyncState;
                int len = so.EndReceive(ar);
                if (len > 0)
                {
                    while (true)
                    {                        
                        ByteBuffer frame = this.protocal.TranslateFrame(buf, len);
                        if (frame != null)
                        {
                            this.listner.OnMessage(this, frame);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    this.Close(true);
                }
            }
            catch (Exception e)
            {
                this.Close(true);
            }
        }
    }
}

