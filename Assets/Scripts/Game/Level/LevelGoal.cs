using UnityEngine;

namespace Moh.Game {
    /// <summary>
    /// 關卡目標
    /// </summary>
    public abstract class LevelGoal : ScriptableObject {
        /// <summary>
        /// 目標種類
        /// </summary>
        public abstract GoalType type { get; }

        /// <summary>
        /// 是否完成目標
        /// </summary>
        /// <param name="achieve">關卡中完成的數量統計</param>
        public abstract bool Completed(LevelAchieve achieve);
    }
}