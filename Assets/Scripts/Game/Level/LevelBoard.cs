using System;

namespace Moh.Game {
    /// <summary>
    /// 關卡盤面
    /// </summary>
    [Serializable]
    public class LevelBoard {
        /// <summary>
        /// 欄數
        /// </summary>
        public int columns = 7;

        /// <summary>
        /// 列數
        /// </summary>
        public int rows = 7;

        /// <summary>
        /// 格子總數
        /// </summary>
        public int count { get { return columns * rows; } }
    }
}