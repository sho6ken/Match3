using System.Collections;
using System.Collections.Generic;
using Moh.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Moh.Game {
    /// <summary>
    /// 一般棋盤
    /// </summary>
    public class NormBoard : MonoBehaviour, IBoardView {
        /// <summary>
        /// 表演配置
        /// </summary>
        private PerformCfg _perform { get { return ConfigLoader<PerformCfg>.inst; } }

        /// <summary>
        /// 棋格寬
        /// </summary>
        private float _gridW { get { return _perform.gridW; } }

        /// <summary>
        /// 棋格高
        /// </summary>
        private float _gridH { get { return _perform.gridH; } }

        /// <summary>
        /// 欄數
        /// </summary>
        private int columns { get { return Global.level.board.columns; } }

        /// <summary>
        /// 列數
        /// </summary>
        public int rows { get { return Global.level.board.rows; } }

        /// <summary>
        /// 重置盤面
        /// </summary>
        /// <param name="tiles">盤面數據</param>
        public void ResetGrid(List<TileBase> tiles) {
            // 盤面置中偏移量
            var offset = new Vector2();
            offset.x = columns / 2 * _gridW;
            offset.y = rows / 2 * _gridH;

            var count = tiles.Count;

            for (var i = 0; i < count; i++) {
                var tile = tiles[i];

                // 左下到右上
                var pos = new Vector2();
                pos.x = tile.col * _gridW - offset.x;
                pos.y = tile.row * _gridH - offset.y;

                tile.transform.position = pos;
                tile.transform.SetParent(transform);
            }

            AdjustCamera();

            // 紀錄原點位置
            Global.originPos = tiles[0].transform.position;
        }

        /// <summary>
        /// 調整攝影機
        /// </summary>
        private void AdjustCamera() {
            var weight = columns * _gridW;
            var rate = Screen.height / (float)Screen.width;
            Camera.main.orthographicSize = weight * _perform.zoom * rate * 0.5f;
        }

        /// <summary>
        /// 創建棋子
        /// </summary>
        public TileBase CreateTile(TileType type, ColorType color) {
            var cfg = ConfigLoader<AssetsCfg>.inst;
            AssetReference asset = null;

            switch (color) {
                case ColorType.Blue  : asset = cfg.blueTile;   break;
                case ColorType.Green : asset = cfg.greenTile;  break;
                case ColorType.Orange: asset = cfg.orangeTile; break;
                case ColorType.Purple: asset = cfg.purpleTile; break;
                case ColorType.Red   : asset = cfg.redTile;    break;
                case ColorType.Yellow: asset = cfg.yellowTile; break;
            }

            if (asset == null) {
                Debug.LogWarningFormat("norm grid create tile {0}_{1} failed, ", type, color);
                return null;
            }

            // TODO: 改物件池
            var handler = Addressables.InstantiateAsync(asset);
            var go = handler.WaitForCompletion();

            return go.GetComponent<Tile>();
        }

        /// <summary>
        /// 選擇棋子
        /// </summary>
        public void SelectTile(TileBase tile) {
            tile.Selected();
        }

        /// <summary>
        /// 取消選擇棋子
        /// </summary>
        public void UnselectTile(TileBase tile) {
            tile.Unselected();
        }

        /// <summary>
        /// 棋子交換
        /// </summary>
        public IEnumerator SwapTiles(TileBase tileA, TileBase tileB) {
            var posA = tileA.transform.position;
            var posB = tileB.transform.position;
            var sec = _perform.swapSec;

            tileA.SetOrder(1);
            tileA.Move(posB, sec, () => tileA.SetOrder(0));

            tileB.Move(posA, sec);

            yield return new WaitForSeconds(sec);
        }

        /// <summary>
        /// 消除棋子
        /// </summary>
        /// <param name="tiles">消除列表</param>
        public IEnumerator CrushTiles(List<TileBase> tiles) {
            foreach (var tile in tiles) {
                tile.Crushing();
            }

            yield return new WaitForSeconds(_perform.crushSec);
        }

        /// <summary>
        /// 棋子掉落
        /// </summary>
        /// <param name="tiles">掉落列表</param>
        /// <param name="ends">各棋掉落到何列</param>
        /// <remarks>從盤內到盤內</remarks>
        public IEnumerator FallTiles(List<TileBase> tiles, List<int> ends) {
            var sec = _perform.fallSec;
            var count = tiles.Count;

            for (var i = 0; i < count; i++) {
                var tile = tiles[i];

                tile.Move(tile.col, ends[i], sec, () => {
                    tile.Falling();
                }, LeanTweenType.easeInQuad);
            }

            yield return new WaitForSeconds(sec);
        }

        /// <summary>
        /// 填補棋子
        /// </summary>
        /// <param name="tiles">填補列表</param>
        /// <param name="orders">起始順序</param>
        /// <remarks>從盤外到盤內</remarks>
        public IEnumerator StuffTiles(List<TileBase> tiles, List<int> orders) {
            var sec = _perform.fallSec;
            var count = tiles.Count;

            for (var i = 0; i < count; i++) {
                var tile = tiles[i];

                // 從盤外入場
                var pos = new Vector2();
                pos.x = Global.originPos.x + (tile.col * _gridW);
                pos.y = Global.originPos.y + (rows + orders[i]) * _gridH;

                tile.transform.position = pos;
                tile.transform.SetParent(transform);

                tile.Move(tile.col, tile.row, sec, () => {
                    tile.Falling();
                }, LeanTweenType.easeInQuad);
            }

            yield return new WaitForSeconds(sec);
        }

        /// <summary>
        /// 連鎖表演
        /// </summary>
        /// <param name="combos">段數</param>
        public IEnumerator PlayCombos(int combos) {
            yield return new WaitForSeconds(_perform.comboGap);
        }
    }
}
