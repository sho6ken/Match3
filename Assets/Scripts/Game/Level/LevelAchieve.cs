using System;
using System.Collections.Generic;

namespace Moh.Game {
    /// <summary>
    /// 完成數量統計
    /// </summary>
    /// <remarks>關卡中完成的數量都記錄在此處</remarks>
    [Serializable]
    public class LevelAchieve {
        /// <summary>
        /// 積分
        /// </summary>
        public int score = 0;

        /// <summary>
        /// 收集物
        /// </summary>
        public Dictionary<CollectType, int> collects = new Dictionary<CollectType, int>();
    }
}