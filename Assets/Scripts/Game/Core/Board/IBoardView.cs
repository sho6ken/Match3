using System.Collections;
using System.Collections.Generic;

namespace Moh.Game {
    /// <summary>
    /// 棋盤外觀
    /// </summary>
    public interface IBoardView {
        /// <summary>
        /// 重置盤面
        /// </summary>
        /// <param name="tiles">盤面數據</param>
        void ResetGrid(List<TileBase> tiles);

        /// <summary>
        /// 創建棋子
        /// </summary>
        TileBase CreateTile(TileType type, ColorType color);

        /// <summary>
        /// 選擇棋子
        /// </summary>
        void SelectTile(TileBase tile);

        /// <summary>
        /// 取消選擇棋子
        /// </summary>
        void UnselectTile(TileBase tile);

        /// <summary>
        /// 棋子交換
        /// </summary>
        IEnumerator SwapTiles(TileBase tileA, TileBase tileB);

        /// <summary>
        /// 消除棋子
        /// </summary>
        /// <param name="tiles">消除列表</param>
        IEnumerator CrushTiles(List<TileBase> tiles);

        /// <summary>
        /// 棋子掉落
        /// </summary>
        /// <param name="tiles">掉落列表</param>
        /// <param name="ends">各棋掉落到何列</param>
        /// <remarks>從盤內到盤內</remarks>
        IEnumerator FallTiles(List<TileBase> tiles, List<int> ends);

        /// <summary>
        /// 填補棋子
        /// </summary>
        /// <param name="tiles">填補列表</param>
        /// <param name="orders">起始順序</param>
        /// <remarks>從盤外到盤內</remarks>
        IEnumerator StuffTiles(List<TileBase> tiles, List<int> orders);

        /// <summary>
        /// 連鎖表演
        /// </summary>
        /// <param name="combos">段數</param>
        IEnumerator PlayCombos(int combos);
    }
}
