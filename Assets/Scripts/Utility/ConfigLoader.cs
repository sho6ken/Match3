using UnityEngine;

namespace Moh.Common {
    /// <summary>
    /// 加載配置檔
    /// </summary>
    /// <typeparam name="T">從resources中讀取, 且檔名必須與類名相同</typeparam>
    public class ConfigLoader<T> where T : ScriptableObject {
        /// <summary>
        /// 實例
        /// </summary>
        private static T _inst = null;

        /// <summary>
        /// 實例
        /// </summary>
        public static T inst {
            get {
                if (_inst == null) {
                    var name = typeof(T).Name;
                    _inst = Resources.Load<T>(name);

                    if (_inst == null) {
                        Debug.LogErrorFormat("load config {0} failed, file not found", name);
                    }
                }

                return _inst;
            }
        }

        /// <summary>
        /// 關閉
        /// </summary>
        public void Close() {
            _inst = null;
        }
    }   
}
