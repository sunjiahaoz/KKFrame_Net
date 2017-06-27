using UnityEngine;
using System.Collections;
using KK.Frame.Net;

namespace KK.Lobby.Msg
{
	[ProtoNetMsg(mainID = 1, subID = 222, msgTyp = ProtoNetMsgAttribute.MsgType.Req)]
	public class TestDll : CMD_Base_Req
	{
		public override short MainId { get { return 1; }}
		public override short SubId { get { return 222; }}

		// 消息字段///

		public byte asdf;  //
		public short ffff;  //
		public int wewe;  //

		protected override void WriteData(ByteBuffer buffer)
		{
			buffer.WriteByte(asdf);
			buffer.WriteBytes(new byte[3]);
			buffer.WriteShort(ffff);
			buffer.WriteBytes(new byte[2]);
			buffer.WriteInt(wewe);
		}

		protected override int GetDataSize()
		{
			return NetDefine.HEAD_LEN + 12;
		}
		public static CMD_Base_Req CreateInstance()
		{
			return new TestDll();
		}
	}
}
