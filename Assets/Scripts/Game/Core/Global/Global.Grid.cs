using UnityEngine;

namespace Moh.Game {
    /// <summary>
    /// 全域
    /// </summary>
    /// <remarks>棋盤部分</remarks>
    public partial class Global {
        /// <summary>
        /// 原點位置
        /// </summary>
        /// <remarks>最左下棋格的顯示位置</remarks>
        public static Vector2 originPos = Vector2.zero;

        /// <summary>
        /// 取得棋格索引
        /// </summary>
        public static int GetGridIdx(TileBase tile) {
            return GetGridIdx(tile.col, tile.row);
        }

        /// <summary>
        /// 取得棋格索引
        /// </summary>
        public static int GetGridIdx(int col, int row) {
            return col + (row * level.board.columns);
        }

        /// <summary>
        /// 取得棋子定位
        /// </summary>
        /// <param name="idx">棋格索引</param>
        public static bool GetTileLocate(int idx, out int col, out int row) {
            col = 0;
            row = 0;

            if (idx < 0 || idx >= level.board.count) {
                return false;
            }

            row = idx / level.board.columns;
            col = idx - (row * level.board.columns);

            return true;
        }
    }
}