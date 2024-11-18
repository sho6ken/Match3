using System;
using UnityEngine;

namespace Moh.Game {
    /// <summary>
    /// 棋子基礎
    /// </summary>
    /// <remarks>數據部分</remarks>
    public abstract partial class TileBase : MonoBehaviour {
        /// <summary>
        /// 欄
        /// </summary>
        public int col { get; private set; }

        /// <summary>
        /// 列
        /// </summary>
        public int row { get; private set; }

        /// <summary>
        /// 棋種
        /// </summary>
        public abstract TileType type { get; }

        /// <summary>
        /// 棋色
        /// </summary>
        [SerializeField]
        private ColorType _color = ColorType.Red;

        /// <summary>
        /// 棋色
        /// </summary>
        public ColorType color { get { return _color; } }

        /// <summary>
        /// 清除
        /// </summary>
        public virtual void ClearModel() {
            col = 0;
            row = 0;
            _color = ColorType.None;

            Rename();
        }

        /// <summary>
        /// 改名
        /// </summary>
        protected virtual void Rename() {
            name = string.Format("{0}_{1}_{2}", col, row, Enum.GetName(color.GetType(), color));
        }

        /// <summary>
        /// 設定定位
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        public virtual void SetLocate(int col, int row) {
            this.col = col;
            this.row = row;

            Rename();
        }
    }
}
