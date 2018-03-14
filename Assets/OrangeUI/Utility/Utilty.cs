using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace OrangeUtility
{
    public interface ILogTraceable
    {
        string GetLog();
        void ClearLog();
    }
    public static class Utilty 
    {
        public static string ToPath(this Transform go)
        {
            string path = go.gameObject.name;
            Transform current = go;
            int i = 0;
            while (current.parent != null && i < 100)
            {
                i++;
                path = current.parent.gameObject.name + "/" + path;
                current = current.parent;
            }
            return path;
        }

        public static string CallStack()
        {
            // 1:省略目前位置
            // true:顯示檔案資訊
            var stackTrace = new StackTrace(1, true);
            return stackTrace.ToString();
        }
    }
}
