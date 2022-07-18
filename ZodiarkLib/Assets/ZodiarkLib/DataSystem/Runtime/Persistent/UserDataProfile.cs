namespace ZodiarkLib.Data
{
    public class UserDataProfile : BasePersistentDataProfile<IPersistentData>
    {
        public UserDataProfile() : base("default_profile")
        {
            
        }
        
        public UserDataProfile(string profileName) : base(profileName)
        {
        }
    }   
}