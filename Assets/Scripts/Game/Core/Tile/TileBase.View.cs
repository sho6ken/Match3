using System;
using Moh.Common;
using UnityEngine;

namespace Moh.Game {
    /// <summary>
    /// 棋子基礎
    /// </summary>
    /// <remarks>外觀部分</remarks>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public abstract partial class TileBase : MonoBehaviour {
        /// <summary>
        /// 標籤
        /// </summary>
        /// <remarks>給操作判斷觸控對象用</remarks>
        public static readonly string TAG = "Tile";

        /// <summary>
        /// 靜圖
        /// </summary>
        protected SpriteRenderer _sprite = null;

        /// <summary>
        /// 動畫
        /// </summary>
        protected Animator _anim = null;

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Awake() {
            gameObject.tag = TAG;

            _sprite = GetComponent<SpriteRenderer>();
            _anim = GetComponent<Animator>();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnDisable() {
            ClearModel();
            ClearView();
        }

        /// <summary>
        /// 清除
        /// </summary>
        public virtual void ClearView() {
            transform.localScale = Vector2.one;
            transform.localRotation = Quaternion.identity;

            SetOrder(0);
        }

        /// <summary>
        /// 設定顯示排序
        /// </summary>
        /// <remarks>越大顯示在越前面</remarks>
        public virtual void SetOrder(int value) {
            _sprite.sortingOrder = value;
        }

    #region 操作動畫
        /// <summary>
        /// 已選定
        /// </summary>
        public virtual void Selected() {
            _anim.SetTrigger("pressed");
        }

        /// <summary>
        /// 取消選定
        /// </summary>
        public virtual void Unselected() {
            _anim.SetTrigger("released");
        }

        /// <summary>
        /// 消除中
        /// </summary>
        public virtual void Crushing() {
            _anim.SetTrigger("crushing");
        }

        /// <summary>
        /// 掉落中
        /// </summary>
        public virtual void Falling() {
            _anim.SetTrigger("falling");
        }

        /// <summary>
        /// 提示中
        /// </summary>
        public virtual void Hinting() {
            _anim.SetTrigger("hinting");
        }

        /// <summary>
        /// 取消提示
        /// </summary>
        public virtual void Canceled() {
            _anim.SetTrigger("canceled");
        }

        /// <summary>
        /// 移動
        /// </summary>
        /// <param name="col">終點欄</param>
        /// <param name="row">終點列</param>
        /// <param name="sec">移動秒數</param>
        /// <param name="cb">完成回調</param>
        /// <param name="type">緩動方式</param>
        public virtual void Move(int col, int row, float sec, Action cb = null, LeanTweenType type = LeanTweenType.linear) {
            var cfg = ConfigLoader<PerformCfg>.inst;
            var pos = Global.originPos;

            var x = pos.x + (col * cfg.gridW);
            var y = pos.y + (row * cfg.gridH);

            Move(new Vector2(x, y), sec, cb, type);
        }

        /// <summary>
        /// 移動
        /// </summary>
        /// <param name="pos">終點位置</param>
        /// <param name="sec">移動秒數</param>
        /// <param name="cb">完成回調</param>
        /// <param name="type">緩動方式</param>
        public virtual void Move(Vector2 pos, float sec, Action cb = null, LeanTweenType type = LeanTweenType.linear) {
            if (Vector2.Distance(transform.position, pos) < 0.01f) {
                Debug.LogWarningFormat("tile {0} move to {1} failed, it's too close", name, pos.ToString());
                return;
            }

            var tween = LeanTween.move(gameObject, pos, sec);
            tween.setEase(type);

            if (cb != null) {
                tween.setOnComplete(cb);
            }
        }
    #endregion
    }
}
