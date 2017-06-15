using UnityEngine;
using System.Collections;

namespace KK.Frame.Net
{
    public class MsgFactory
    {
        public const int GROUPID_LOBBY = 1;
        public const int GROUPID_ROOM = 2;

        #region _Instance_
            public static IMsgFactory Lobby
            {
                get
                {
                    if (_FactoryLobby == null)
                    {
                    _FactoryLobby = new MsgFactory_Auto("", GROUPID_LOBBY);
                    }
                    return _FactoryLobby;
                }
            }
        public static IMsgFactory Room
        {
            get
            {
                if (_FactoryRoom == null)
                {
                    _FactoryRoom = new MsgFactory_Auto("", GROUPID_ROOM);
                }
                return _FactoryRoom;
            }
        }
        private static IMsgFactory _FactoryLobby;
        private static IMsgFactory _FactoryRoom;
        #endregion	
    }
}
