using System;
using ZodiarkLib.Core.IO;

namespace ZodiarkLib.SaveSystem
{
    public interface ISaveManager
    {
        /// <summary>
        /// Init save system
        /// </summary>
        /// <param name="saveFolder"></param>
        /// <param name="reader"></param>
        /// <param name="writer"></param>
        /// Data/global_data/global_profile_meta , sound_global_data , graphic_setting_global_data ,
        ///                         profile_slot_data
        /// Data/profile/user_name_1_data/user_1_meta , inventory_data , stage_data
        /// Data/profile/user_name_2_data/user_2_meta , inventory_data
        void Initialize(string saveFolder, IReader reader, IWriter writer);
        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="appendPath"></param>
        void Save(object data , string appendPath = "");
        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="appendPath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Load<T>(string appendPath = "") where T : new();

        /// <summary>
        /// Load generic data
        /// </summary>
        /// <param name="type"></param>
        /// <param name="appendPath"></param>
        /// <returns></returns>
        object Load(Type type, string appendPath = "");
    }
}