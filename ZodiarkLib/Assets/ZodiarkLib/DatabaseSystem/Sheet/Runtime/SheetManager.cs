using System;
using System.Collections.Generic;

namespace ZodiarkLib.Database.Sheet
{
    public sealed class SheetManager : ISheetManager
    {
        #region Fields

        private Dictionary<Type, ISheet> _sheets = new();

        #endregion

        #region Properties

        public IReadOnlyDictionary<Type, ISheet> Sheets => _sheets;
        
        #endregion

        #region Public Methods
        
        public void Initialize(SheetCollectionSO collection)
        {
            foreach (var sheetTxt in collection.Sheets)
            {
                var lines = ParseContent(sheetTxt.text);
                var type = Type.GetType(sheetTxt.name);
                if(type == null)
                    continue;
                var sheet = Activator.CreateInstance(type) as ISheet;
                if (sheet == null) 
                    continue;
                sheet.Load(lines);
                AddSheet(sheet);
            }
        }

        public void AddSheet(ISheet sheet)
        {
            var type = sheet.GetType();
            if (_sheets.ContainsKey(type))
            {
                return;
            }

            _sheets[type] = sheet;
        }

        public void RemoveSheet<T>() where T : ISheet
        {
            var type = typeof(T);
            _sheets.Remove(type);
        }

        public T GetSheet<T>() where T : ISheet
        {
            var type = typeof(T);
            if (_sheets.ContainsKey(type))
            {
                return (T)_sheets[type];
            }

            return default;
        }
        
        #endregion

        #region Private Methods

        public string[] ParseContent(string content)
        {
            var lines = new List<string>();
            var arrays = content.Split("\n");
            foreach (var s in arrays)
            {
                lines.Add(s.Replace("\r", ""));
            }

            return lines.ToArray();
        }

        #endregion
    }   
}