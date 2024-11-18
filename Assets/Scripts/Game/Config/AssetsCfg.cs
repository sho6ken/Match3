using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Moh.Game {
    /// <summary>
    /// 資源配置
    /// </summary>
    [CreateAssetMenu(fileName = "AssetsCfg", menuName = "墨/配置/資源配置")]
    public class AssetsCfg : ScriptableObject {
        /// <summary>
        /// 藍棋
        /// </summary>
        public AssetReference blueTile;

        /// <summary>
        /// 綠棋
        /// </summary>
        public AssetReference greenTile;

        /// <summary>
        /// 橘棋
        /// </summary>
        public AssetReference orangeTile;

        /// <summary>
        /// 紫棋
        /// </summary>
        public AssetReference purpleTile;

        /// <summary>
        /// 紅棋
        /// </summary>
        public AssetReference redTile;

        /// <summary>
        /// 黃棋
        /// </summary>
        public AssetReference yellowTile;
    }
}