using Moh.Game;
using UnityEngine;

namespace Moh.Game {
    /// <summary>
    /// 收集物目標
    /// </summary>
    [CreateAssetMenu(fileName = "CollectGoal_種類_數量", menuName = "墨/關卡/收集物目標")]
    public class CollectGoal : LevelGoal {
        /// <summary>
        /// 目標種類
        /// </summary>
        public override GoalType type { get { return GoalType.Collect; } }

        /// <summary>
        /// 收集物種類
        /// </summary>
        public CollectType collectType = CollectType.RedTile;

        /// <summary>
        /// 收集物數量
        /// </summary>
        public int collectValue = 5;

        /// <summary>
        /// 是否完成目標
        /// </summary>
        /// <param name="achieve">關卡中完成的數量統計</param>
        public override bool Completed(LevelAchieve achieve) {
            if (achieve.collects.ContainsKey(collectType) == false) {
                return false;
            }

            return achieve.collects[collectType] >= collectValue;
        }
    }
}
