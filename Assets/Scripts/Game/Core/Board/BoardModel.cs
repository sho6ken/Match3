using System.Collections.Generic;
using UnityEngine;

namespace Moh.Game {
    /// <summary>
    /// 棋盤數據
    /// </summary>
    public partial class BoardModel : MonoBehaviour {
        /// <summary>
        /// 棋盤外觀
        /// </summary>
        private IBoardView _view = null;

        /// <summary>
        /// 棋格列表
        /// </summary>
        private List<TileBase> _grids = null;

        /// <summary>
        /// 
        /// </summary>
        private void OnDestroy() {
            Clear();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="view">外觀實作</param>
        public void Init(IBoardView view) {
            _view = view;
        }

        /// <summary>
        /// 清除
        /// </summary>
        private void Clear() {
            if (_grids != null) {
                foreach (var tile in _grids) {
                    if (tile) {
                        Destroy(tile.gameObject);  // TODO: 改物件池
                    }
                }

                _grids.Clear();
            }
        }

        /// <summary>
        /// 取得棋子
        /// </summary>
        /// <param name="idx">棋格索引</param>
        private TileBase GetTile(int idx) {
            if (idx < 0 || idx >= _grids.Count) {
                return null;
            }

            return _grids[idx];
        }

        /// <summary>
        /// 取得棋子
        /// </summary>
        private TileBase GetTile(int col, int row) {
            if (col < 0 || col >= columns) {
                Debug.LogWarningFormat("get tile failed, col {0} out of range", col);
                return null;
            }

            if (row < 0 || row >= rows) {
                Debug.LogWarningFormat("get tile failed, row {0} out of range", row);
                return null;
            }

            var idx = Global.GetGridIdx(col, row);
            return _grids[idx];
        }

        /// <summary>
        /// 設定棋子
        /// </summary>
        private void SetTile(int col, int row, TileBase tile) {
            var name = tile != null ? tile.name : string.Empty;

            if (col < 0 || col >= columns) {
                Debug.LogWarningFormat("set tile {0} failed, col {1} out of range", name, col);
                return;
            }

            if (row < 0 || row >= rows) {
                Debug.LogWarningFormat("set tile {0} failed, row {1} out of range", name, row);
                return;
            }

            var idx = Global.GetGridIdx(col, row);
            _grids[idx] = tile;

            if (tile) {
                tile.SetLocate(col, row);
            }
        }

        /// <summary>
        /// 設定棋子
        /// </summary>
        /// <param name="idx">棋格索引</param>
        private void SetTile(int idx, TileBase tile) {
            var name = tile != null ? tile.name : string.Empty;

            if (idx < 0 || idx >= _grids.Count) {
                Debug.LogWarningFormat("set tile {0} failed, idx {1} out of range", name, idx);
                return;
            }

            _grids[idx] = tile;

            if (tile && Global.GetTileLocate(idx, out var col, out var row)) {
                tile.SetLocate(col, row);
            }
        }

        /// <summary>
        /// 開始關卡
        /// </summary>
        /// <param name="id">關卡編號</param>
        public void StartLevel(int id) {
            // 清除
            Clear();

            // 加載關卡資料
            Global.LoadLevel(id);

            //
            var count = Global.level.board.count;
            _grids = new List<TileBase>(count);

            // 生成盤面
            for (var i = 0; i < count; i++) {
                if (Global.GetTileLocate(i, out var col, out var row) == false) {
                    _grids.Add(null);
                    continue;
                }

                // TODO: 替換已連線棋子
                var tile = CreateTile();
                tile.SetLocate(col, row);

                _grids.Add(tile);
            }

            // 顯示盤面
            _view.ResetGrid(_grids);
        }

        /// <summary>
        /// 創建棋子
        /// </summary>
        private TileBase CreateTile() {
            // TODO: 要改用關卡初始盤面設定
            var min = (int)ColorType.Blue;
            var max = (int)ColorType.Yellow;
            var color = (ColorType)Random.Range(min, max + 1);

            return _view.CreateTile(TileType.Tile, color);
        }
    }
}
