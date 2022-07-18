using ZodiarkLib.Core;
using ZodiarkLib.SaveSystem.IO;
using ZodiarkLib.Utils;

namespace ZodiarkLib.Data.Extensions
{
    public static class PersistentProfileExtensions
    {
        public static UserDataProfile LoadProfileData(string defaultName)
        {
            var globalProfile = ServiceLocator.Get<GlobalDataProfile>();
            var profileMeta = globalProfile.SafeGet<UserProfileNameData>();

            var profileName = string.IsNullOrEmpty(profileMeta.lastLoadedProfile)
                ? defaultName
                : profileMeta.lastLoadedProfile;
            profileMeta.lastLoadedProfile = profileName;
            var userProfile = new UserDataProfile(profileName);
            userProfile.LazyInit();

            return userProfile;
        }
        
        public static void LazyInit(this GlobalDataProfile original)
        {
            original.Initialize(new BasePersistentDataProfile<IGlobalData>.Config()
            {
                reader = new JsonReader(),
                writer = new JsonWriter(),
                savePath = PathHelper.GetWritableFolder()
            });
            original.LoadMetadata();
            original.Load();
        }
        
        public static void LazyInit(this UserDataProfile original)
        {
            original.Initialize(new BasePersistentDataProfile<IPersistentData>.Config()
            {
                reader = new JsonReader(),
                writer = new JsonWriter(),
                savePath = PathHelper.GetWritableFolder()
            });
            original.LoadMetadata();
            original.Load();
            
            var globalProfile = ServiceLocator.Get<GlobalDataProfile>();
            var profileMeta = globalProfile.SafeGet<UserProfileNameData>();
            profileMeta.lastLoadedProfile = original.Metadata.name;
            if (!profileMeta.profileNames.Contains(original.Metadata.name))
            {
                profileMeta.profileNames.Add(original.Metadata.name);
            }

            globalProfile.SaveSingle<UserProfileNameData>();
        }

        public static UserDataProfile SwapProfile(this UserDataProfile original, string newProfileName)
        {
            var config = original.Configuration;
            var newProfile = new UserDataProfile(newProfileName);
            newProfile.Initialize(config);
            newProfile.LoadMetadata();
            newProfile.Load();
            ServiceLocator.Add(newProfile, true);

            var globalProfile = ServiceLocator.Get<GlobalDataProfile>();
            var profileMeta = globalProfile.SafeGet<UserProfileNameData>();
            profileMeta.lastLoadedProfile = newProfile.Metadata.name;
            if (!profileMeta.profileNames.Contains(newProfile.Metadata.name))
            {
                profileMeta.profileNames.Add(newProfile.Metadata.name);
            }

            globalProfile.SaveSingle<UserProfileNameData>();
            return newProfile;
        }
    }
}