using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ZodiarkLib.Database.Sheet
{
    public sealed class SheetManager : ISheetManager
    {
        public static string LINE_SPLIT_RE = @"\r\n|\n\r";
        public static string PHASE_SPLIT_RE = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        
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

        private string[] ParseContent(string content)
        {
            var lines = Regex.Split(content, LINE_SPLIT_RE).ToList();
            return lines.ToArray();
        }

        #endregion
    }   
}