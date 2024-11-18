using System.Collections.Generic;
using UnityEngine;

namespace Moh.Game {
    /// <summary>
    /// 關卡資料
    /// </summary>
    [CreateAssetMenu(fileName = "LevelData_關卡編號", menuName = "墨/關卡/關卡資料")]
    public class LevelData : ScriptableObject {
        /// <summary>
        /// 關卡編號
        /// </summary>
        public int id = 0;

        /// <summary>
        /// 盤面配置
        /// </summary>
        public LevelBoard board = null;

        /// <summary>
        /// 關卡限制
        /// </summary>
        public LevelLimit limit = null;

        /// <summary>
        /// 關卡目標
        /// </summary>
        public List<LevelGoal> goals = null;
    }
}