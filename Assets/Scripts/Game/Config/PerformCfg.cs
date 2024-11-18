using UnityEngine;

namespace Moh.Game {
    /// <summary>
    /// 表演配置
    /// </summary>
    [CreateAssetMenu(fileName = "PerformCfg", menuName = "墨/配置/表演配置")]
    public class PerformCfg : ScriptableObject {
        /// <summary>
        /// 攝影機縮放
        /// </summary>
        public float zoom;

        /// <summary>
        /// 棋格寬
        /// </summary>
        public float gridW;

        /// <summary>
        /// 棋格高
        /// </summary>
        public float gridH;

        /// <summary>
        /// 互換秒數
        /// </summary>
        public float swapSec;

        /// <summary>
        /// 消除秒數
        /// </summary>
        public float crushSec;

        /// <summary>
        /// 掉落秒數
        /// </summary>
        public float fallSec;

        /// <summary>
        /// 連鎖停頓
        /// </summary>
        public float comboGap;
    }
}
