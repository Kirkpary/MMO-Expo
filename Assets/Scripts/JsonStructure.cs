using System;

namespace Com.Oregonstate.MMOExpo
{
    #region RoomJson
    [Serializable]
    public class Room
    {
        public string SceneName;
        public Booth[] Items;
    }

    [Serializable]
    public class Booth
    {
        public string BoothName;
    }
    #endregion

    #region RoomListJson
    [Serializable]
    public class RoomList
    {
        public string[] RoomNames;
    }
    #endregion
}