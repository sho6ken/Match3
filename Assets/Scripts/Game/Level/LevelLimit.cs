using System;

namespace Moh.Game {
    /// <summary>
    /// 關卡限制
    /// </summary>
    [Serializable]
    public class LevelLimit {
        /// <summary>
        /// 限制種類
        /// </summary>
        public LimitType type = LimitType.Moves;

        /// <summary>
        /// 限制數量
        /// </summary>
        public int value = 10;
    }
}