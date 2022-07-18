using System.Collections.Generic;

namespace ZodiarkLib.Data
{
    [System.Serializable]
    public class UserProfileNameData : IGlobalData
    {
        public string lastLoadedProfile;
        public List<string> profileNames = new();
    }
}