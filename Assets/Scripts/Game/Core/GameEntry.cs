using UnityEngine;

namespace Moh.Game {
    /// <summary>
    /// 遊戲入口
    /// </summary>
    public class GameEntry : MonoBehaviour {
        /// <summary>
        /// 棋盤
        /// </summary>
        private BoardModel _board = null;

        /// <summary>
        /// 遊戲是否開始
        /// </summary>
        private bool _started = false;

        /// <summary>
        /// 遊戲是否結束
        /// </summary>
        private bool _finished = false;

        /// <summary>
        /// 
        /// </summary>
        private void Awake() {
            var go = new GameObject("Board");
            go.transform.SetParent(transform);

            // 建立棋盤
            _board = go.AddComponent<BoardModel>();
            _board.Init(go.AddComponent<NormBoard>());  // TODO: 依條件使用不同棋盤
        }

        /// <summary>
        /// 
        /// </summary>
        private void Start() {
            _board.StartLevel(1);  // TODO: for test
            _started = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDestroy() {
            Destroy(_board.gameObject);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Update() {
            if (_started == false || _finished) {
                return;
            }

            OnMouseDown();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnMouseDown() {
            if (Input.GetMouseButtonDown(0) == false) {
                return;
            }

            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider == null) {
                return;
            }

            var go = hit.collider.gameObject;

            if (go.tag == null || go.CompareTag(Tile.TAG) == false) {
                return;
            }

            _board.Select(go);
        }
    }
}
