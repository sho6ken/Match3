using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Moh.Game {
    /// <summary>
    /// 棋盤數據
    /// </summary>
    /// <remarks>算法部分</remarks>
    public partial class BoardModel : MonoBehaviour {
        /// <summary>
        /// 有無連線
        /// </summary>
        private bool IsMatch(TileBase tile) {
            return IsMatch(tile.col, tile.row);
        }

        /// <summary>
        /// 有無連線
        /// </summary>
        private bool IsMatch(int col, int row) {
            return IsMatchH(col, row) || IsMatchV(col, row);
        }

        /// <summary>
        /// 有無橫向連線
        /// </summary>
        private bool IsMatchH(int col, int row) {
            return GetMatchesH(col, row).Count >= 3;
        }

        /// <summary>
        /// 有無直向連線
        /// </summary>
        private bool IsMatchV(int col, int row) {
            return GetMatchesV(col, row).Count >= 3;
        }

        /// <summary>
        /// 取得連線棋子
        /// </summary>
        private List<TileBase> GetMatches(TileBase tile) {
            return GetMatches(tile.col, tile.row);
        }

        /// <summary>
        /// 取得連線棋子
        /// </summary>
        private List<TileBase> GetMatches(int col, int row) {
            var res = GetMatchesH(col, row);
            res = res.Union(GetMatchesV(col, row)).ToList();
            return res;
        }

        /// <summary>
        /// 取得橫向連線棋子
        /// </summary>
        /// <returns>有連線, 則操作棋子必在容器開頭, 無連線則回傳空容器</returns>
        private List<TileBase> GetMatchesH(int col, int row) {
            var center = GetTile(col, row);
            var res = new List<TileBase>() { center };

            if (center.color == ColorType.None) {
                Debug.LogWarningFormat("tile {0} get matches h failed, tile's color is none", center.name);
                return res;
            }

            // 左
            for (var i = col - 1; i >= 0; i--) {
                if (IsTileFit(i, row, center.color, out var tile) == false) {
                    break;
                }

                res.Add(tile);
            }

            // 右
            for (var i = col + 1; i < columns; i++) {
                if (IsTileFit(i, row, center.color, out var tile) == false) {
                    break;
                }

                res.Add(tile);
            }

            // 未連線
            if (res.Count < 3) {
                res.Clear();
            }

            return res;
        }

        /// <summary>
        /// 取得直向連線棋子
        /// </summary>
        /// <returns>有連線, 則操作棋子必在容器開頭, 無連線則回傳空容器</returns>
        private List<TileBase> GetMatchesV(int col, int row) {
            var center = GetTile(col, row);
            var res = new List<TileBase>() { center };

            if (center.color == ColorType.None) {
                Debug.LogWarningFormat("tile {0} get matches v failed, tile's color is none", center.name);
                return res;
            }

            // 上
            for (var i = row + 1; i < rows; i++) {
                if (IsTileFit(col, i, center.color, out var tile) == false) {
                    break;
                }

                res.Add(tile);
            }

            // 下
            for (var i = row - 1; i >= 0; i--) {
                if (IsTileFit(col, i, center.color, out var tile) == false) {
                    break;
                }

                res.Add(tile);
            }

            // 未連線
            if (res.Count < 3) {
                res.Clear();
            }

            return res;
        }

        /// <summary>
        /// 棋子是否符合條件
        /// </summary>
        /// <param name="color">顏色限制</param>
        /// <param name="tile">該定位的棋子</param>
        private bool IsTileFit(int col, int row, ColorType color, out TileBase tile) {
            tile = GetTile(col, row);

            if (tile == null || tile.color != color) {
                return false;
            }

            return true;
        }
    }
}
