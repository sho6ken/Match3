using UnityEngine;

namespace Moh.Game {
    /// <summary>
    /// 積分目標
    /// </summary>
    [CreateAssetMenu(fileName = "ScoreGoal_積分", menuName = "墨/關卡/積分目標")]
    public class ScoreGoal : LevelGoal {
        /// <summary>
        /// 目標種類
        /// </summary>
        public override GoalType type { get { return GoalType.Score; } }

        /// <summary>
        /// 要求積分
        /// </summary>
        public int score = 100;

        /// <summary>
        /// 是否完成目標
        /// </summary>
        /// <param name="achieve">關卡中完成的數量統計</param>
        public override bool Completed(LevelAchieve achieve) {
            return achieve.score >= score;
        }
    }
}
