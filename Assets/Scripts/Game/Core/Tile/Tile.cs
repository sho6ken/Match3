using UnityEngine;

namespace Moh.Game {
    /// <summary>
    /// 棋子
    /// </summary>
    public class Tile : TileBase {
        /// <summary>
        /// 棋種
        /// </summary>
        public override TileType type { get { return TileType.Tile; } }

        /// <summary>
        /// 附加效果種類
        /// </summary>
        [SerializeField]
        private BuffType _buffType = BuffType.None;

        /// <summary>
        /// 附加效果種類
        /// </summary>
        private BuffType buffType { get { return _buffType; } }
    }
}
