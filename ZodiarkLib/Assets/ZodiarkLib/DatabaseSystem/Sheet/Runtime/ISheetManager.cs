using System.Collections.Generic;

namespace ZodiarkLib.Database.Sheet
{
    public interface ISheetManager
    {
        /// <summary>
        /// Get all sheets
        /// </summary>
        IReadOnlyDictionary<System.Type, ISheet> Sheets { get; }

        /// <summary>
        /// Initialize and fetch data into sheets
        /// </summary>
        void Initialize(SheetCollectionSO collection);
        /// <summary>
        /// Add new sheet
        /// </summary>
        /// <param name="sheet"></param>
        void AddSheet(ISheet sheet);
        /// <summary>
        /// Remove sheet by type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void RemoveSheet<T>() where T : ISheet;
        /// <summary>
        /// Get sheet by type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        T GetSheet<T>() where T : ISheet;
    }   
}