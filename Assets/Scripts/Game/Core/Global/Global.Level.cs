using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Moh.Game {
    /// <summary>
    /// 全域
    /// </summary>
    /// <remarks>關卡部分</remarks>
    public partial class Global {
        /// <summary>
        /// 關卡資料
        /// </summary>
        private static LevelData _level = null;

        /// <summary>
        /// 關卡資料
        /// </summary>
        public static LevelData level { get { return _level; } }

        /// <summary>
        /// 加載關卡資料
        /// </summary>
        public static void LoadLevel(int id) {
            if (_level && _level.id == id) {
                Debug.LogWarningFormat("load level {0} failed, id is playing", id);
                return;
            }

            if (_level) {
                Addressables.Release(_level);
            }

            var name = string.Format("LevelData_{0}", id);
            var handler = Addressables.LoadAssetAsync<LevelData>(name);
            _level = handler.WaitForCompletion();
        }
    }
}