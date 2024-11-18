using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Moh.Game {
    /// <summary>
    /// 棋盤數據
    /// </summary>
    /// <remarks>邏輯部分</remarks>
    public partial class BoardModel : MonoBehaviour {
        /// <summary>
        /// 初選棋子
        /// </summary>
        private TileBase _first = null;

        /// <summary>
        /// 連鎖判定鎖
        /// </summary>
        private bool _locked = false;

        /// <summary>
        /// 欄數
        /// </summary>
        private int columns { get { return Global.level.board.columns; } }

        /// <summary>
        /// 列數
        /// </summary>
        public int rows { get { return Global.level.board.rows; } }

        /// <summary>
        /// 選擇
        /// </summary>
        public void Select(GameObject go) {
            var idx = _grids.FindIndex(tile => tile && tile.gameObject == go);

            if (idx == -1) {
                Debug.LogWarningFormat("select obj {0} failed, tile not found", go.name);
                return;
            }

            var tile = GetTile(idx);

            // 非有效棋子
            if (tile.gameObject.activeSelf == false) {
                return;
            }

            // 初選
            if (_first == null) {
                Select1st(tile);
                return;
            }
            // 取消初選
            if (tile == _first) {
                Select1st(null);
                return;
            }

            // 非四向則重新初選, 上下
            if (_first.col == tile.col && _first.row - 1 != tile.row && _first.row + 1 != tile.row) {
                Select1st(tile);
                return;
            }

            // 非四向則重新初選, 左右
            if (_first.row == tile.row && _first.col - 1 != tile.col && _first.col + 1 != tile.col) {
                Select1st(tile);
                return;
            }

            // 非四向則重新初選, 其他
            if (_first.col != tile.col && _first.row != tile.row) {
                Select1st(tile);
                return;
            }

            // 次選
            StartCoroutine(Select2nd(_first, tile));
            _first = null;
        }

        /// <summary>
        /// 初選
        /// </summary>
        private void Select1st(TileBase tile) {
            if (tile == _first) {
                return;
            }

            if (_first) {
                _view.UnselectTile(_first);
            }

            if (tile == null) {
                Debug.LogFormat("cancel 1st {0}", _first.name);
            }
            else {
                Debug.LogFormat("select 1st {0}", tile.name);
                _view.SelectTile(tile);
            }

            _first = tile;
        }

        /// <summary>
        /// 次選
        /// </summary>
        private IEnumerator Select2nd(TileBase tileA, TileBase tileB) {
            Debug.LogFormat("select 2nd {0}", tileB.name);
            _view.UnselectTile(tileA);

            // 互換
            yield return Swap(tileA, tileB);

            // 無消除
            if (IsMatch(tileA) == false && IsMatch(tileB) == false) {
                yield break;
            }

            // 消除
            yield return Crush(tileA, tileB);

            // 掉落與填補
            yield return FallAndStuff();

            // 連鎖
            yield return Combo();
        }

        /// <summary>
        /// 互換
        /// </summary>
        private IEnumerator Swap(TileBase tileA, TileBase tileB) {
            var idxA = Global.GetGridIdx(tileA);
            var idxB = Global.GetGridIdx(tileB);

            SetTile(idxA, tileB);
            SetTile(idxB, tileA);

            // 無消除恢復原狀
            if (IsMatch(tileA) == false && IsMatch(tileB) == false) {
                Debug.LogFormat("swap tile {0} failed, tile {1} not match", tileA.name, tileB.name);

                SetTile(idxA, tileA);
                SetTile(idxB, tileB);

                yield return _view.SwapTiles(tileA, tileB);
            }
            // 有消除
            else {
                Debug.LogFormat("swap tile {0} succeed, tile {1} is match", tileA.name, tileB.name);
            }

            yield return _view.SwapTiles(tileA, tileB);
        }

        /// <summary>
        /// 消除
        /// </summary>
        /// <remarks>由交換的兩點做展開</remarks>
        private IEnumerator Crush(TileBase tileA, TileBase tileB) {
            var tiles = GetMatches(tileA);
            tiles = tiles.Union(GetMatches(tileB)).ToList();
            yield return Crush(tiles);
        }

        /// <summary>
        /// 消除
        /// </summary>
        /// <remarks>全盤面處理</remarks>
        private IEnumerator Crush() {
            var tiles = new List<TileBase>();

            foreach (var tile in _grids) {
                if (tile == null) {
                    continue;
                }

                // 暫不處理其他種類
                if (tile.type != TileType.Tile) {
                    continue;
                }

                if (tile.color == ColorType.None) {
                    continue;
                }

                // 此棋已計算過
                if (tiles.Contains(tile)) {
                    continue;
                }

                tiles = tiles.Union(GetMatches(tile)).ToList();
            }

            yield return Crush(tiles);
        }

        /// <summary>
        /// 實作消除
        /// </summary>
        /// <param name="tiles">消除列表</param>
        private IEnumerator Crush(List<TileBase> tiles) {
            if (tiles.Count <= 0) {
                yield break;
            }

            foreach (var tile in tiles) {
                var idx = Global.GetGridIdx(tile);
                SetTile(idx, null);
            }

            yield return _view.CrushTiles(tiles);

            foreach (var tile in tiles) {
                Destroy(tile.gameObject);  // TODO: 改物件池
            }
        }

        /// <summary>
        /// 掉落與填補
        /// </summary>
        private IEnumerator FallAndStuff() {
            StartCoroutine(Fall());
            yield return Stuff();
        }

        /// <summary>
        /// 掉落
        /// </summary>
        /// <remarks>從盤內到盤內</remarks>
        private IEnumerator Fall() {
            var tiles = new List<TileBase>();
            var ends = new List<int>();

            for (var i = 0; i < columns; i++) {
                Fall(i, out var tempA, out var tempB);
                tiles.AddRange(tempA);
                ends.AddRange(tempB);
            }

            yield return _view.FallTiles(tiles, ends);
        }

        /// <summary>
        /// 掉落
        /// </summary>
        /// <param name="col">處理的欄</param>
        /// <param name="tiles">掉落列表</param>
        /// <param name="ends">各棋掉落到何列</param>
        private void Fall(int col, out List<TileBase> tiles, out List<int> ends) {
            tiles = new List<TileBase>();
            ends = new List<int>();

            for (int i = 0; i < rows; i++) {
                // 找空位
                if (GetTile(col, i)) {
                    continue;
                }

                // 找掉落物
                for (int k = i + 1; k < rows; k++) {
                    var tile = GetTile(col, k);

                    if (tile == null) {
                        continue;
                    }

                    tiles.Add(tile);
                    ends.Add(i);

                    SetTile(col, i, tile);
                    SetTile(col, k, null);

                    break;
                }
            }
        }

        /// <summary>
        /// 填補
        /// </summary>
        /// <remarks>從盤外到盤內</remarks>
        private IEnumerator Stuff() {
            var tiles = new List<TileBase>();
            var orders = new List<int>();

            for (var i = 0; i < columns; i++) {
                Stuff(i, out var tempA, out var tempB);
                tiles.AddRange(tempA);
                orders.AddRange(tempB);
            }

            yield return _view.StuffTiles(tiles, orders);
        }

        /// <summary>
        /// 填補
        /// </summary>
        /// <param name="col">處理的欄</param>
        /// <param name="tiles">填補列表</param>
        /// <param name="orders">起始順序</param>
        private void Stuff(int col, out List<TileBase> tiles, out List<int> orders) {
            tiles = new List<TileBase>();
            orders = new List<int>();

            var count = 0;

            for (int i = 0; i < rows; i++) {
                // 找空位
                if (GetTile(col, i)) {
                    continue;
                }

                // 新增棋子
                var tile = CreateTile();
                SetTile(col, i, tile);

                tiles.Add(tile);
                orders.Add(count++);
            }
        }

        /// <summary>
        /// 偵測盤面
        /// </summary>
        /// <returns>有無現存連線</returns>
        private bool Detect() {
            foreach (var tile in _grids) {
                // 任意連線即為連鎖
                if (IsMatch(tile)) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 連鎖
        /// </summary>
        private IEnumerator Combo() {
            // 只要一個協程處理連鎖就好
            if (_locked) {
                yield break;
            }

            try {
                _locked = true;
                var combos = 0;

                while (Detect()) {
                    // 連鎖表演
                    yield return _view.PlayCombos(++combos);

                    // 消除
                    yield return Crush();

                    // 掉落與填補
                    yield return FallAndStuff();

                    // 下幀再繼續
                    yield return null;
                }
            }
            finally {
                _locked = false;
            }
        }
    }
}
