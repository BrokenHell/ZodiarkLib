using System;
using UnityEngine;

namespace ZodiarkLib.UI.Properties
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple = true)]
    public class DialogPickerProperty : PropertyAttribute
    {
        public Type baseType = typeof(BaseDialog);
    }   
}