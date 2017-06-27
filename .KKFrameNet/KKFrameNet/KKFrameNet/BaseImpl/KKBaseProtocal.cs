using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace KK.Frame.Net
{
    public class KKBaseProtocal : Protocal
    {
        public static object objLock = new object();
        private ByteBuffer frame;
        enum State
        {
            Receive,    // 接收数据
            Return,     // 逐帧返回
            End,            // 结束
        }
        State _curState = State.Receive;
        List<byte[]> _lscb = new List<byte[]>();
        int _nIndex = 0;
        public ByteBuffer TranslateFrame(byte[] cbSrc, int nLen)
        {
            try
            {
                while (true)
                {
                    switch (_curState)
                    {
                        case State.Receive:
                            {
                                _lscb.Clear();
                                if (!OnReceive(cbSrc, nLen))
                                {
                                    _curState = State.End;
                                    break;
                                }
                                else
                                {
                                    _nIndex = 0;
                                    _curState = State.Return;
                                }
                            }
                            break;
                        case State.Return:
                            {
                                if (_nIndex < _lscb.Count)
                                {
                                    ByteBuffer buf = new ByteBuffer(_lscb[_nIndex++]);
                                    return buf;
                                }
                                else
                                {
                                    _curState = State.End;
                                }
                            }
                            break;
                        case State.End:
                            {
                                _nIndex = 0;
                                _curState = State.Receive;
                                return null;
                            }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("<color=red>[Error]</color>---" + ex.Message);
            }
            return null;
        }

        private byte[] tempcon = null;
        public bool OnReceive(byte[] recedata, int length)             //解析收到的消息
        {
            try
            {
                //List<byte[]> outList = new List<byte[]>();
                if (tempcon != null && tempcon.Length > 0)
                {
                    byte[] temp = new byte[recedata.Length];
                    Buffer.BlockCopy(recedata, 0, temp, 0, recedata.Length);
                    recedata = new byte[temp.Length + tempcon.Length];
                    Buffer.BlockCopy(tempcon, 0, recedata, 0, tempcon.Length);
                    Buffer.BlockCopy(temp, 0, recedata, tempcon.Length, temp.Length);

                    length += tempcon.Length;
                    tempcon = null;
                    //Debug.Log("<color=magenta>" + "粘粘粘粘粘粘粘粘粘粘粘包处理**********" + "</color>"); 
                }

                ByteBuffer buf = new ByteBuffer(recedata);
                int start = 0;
                while (length - start >= HeaderLen())
                {
                    int size = buf.GetShort(start);
                    //Debug.Log("接收包大小:" + size+"真实包大小:"+ length+"起始位置:"+ start+"主消息:"+buf.GetShort(start+4) +":"+buf.GetShort(start+6));

                    if (size <= 0)
                        break;
                    if (size > length - start)
                    {
                        int addlen = length - start;//这个表示多出多少个包
                        tempcon = new byte[addlen];
                        Buffer.BlockCopy(recedata, start, tempcon, 0, addlen);
                        //Debug.Log("包不足大小" + addlen);
                        break;
                    }
                    else
                    {
                        byte[] temp = new byte[size + 256];
                        Buffer.BlockCopy(recedata, start, temp, 0, size);
                        lock (objLock)
                        {
                            //outList.Add(temp);
                            _lscb.Add(temp);
                        }
                        start += size;
                    }
                }
                if (length - start < HeaderLen())
                {
                    tempcon = new byte[length - start];
                    Buffer.BlockCopy(recedata, start, tempcon, 0, length - start);
                }
                return true;
                //return outList;
            }
            catch (System.Net.Sockets.SocketException e)
            {
                Debug.Log(e.Message + " errorcode =" + e.ErrorCode);
                //return null;
                return false;
            }
        }


        public int HeaderLen()
        {
            return NetDefine.HEAD_LEN;
        }
    }
}
