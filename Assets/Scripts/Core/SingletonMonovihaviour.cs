namespace Escaping.Core
{
    using Cysharp.Threading.Tasks;
    using Escaping.Core.FileSystems;
    using UnityEngine;

    /// <summary>
    /// MonoBehaviour を継承したシングルトンクラス
    /// </summary>
    /// <typeparam name="T">継承するクラス</typeparam>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        public static T Instance { get; private set; }

        /// <summary>
        /// インスタンスが生成されているか
        /// </summary>
        public static bool IsCreated { get; private set; }

        /// <summary>
        /// インスタンスをプレハブから生成
        /// </summary>
        /// <param name="prefab">生成するプレハブ</param>
        public static async UniTask Create(string prefabPath, Transform parent = null)
        {
            if (Instance != null)
            {
                return;
            }

            var prefab = await FileLoader.LoadAssetAsync<T>(prefabPath);

            Instance = Instantiate(prefab, parent).GetComponent<T>();
            Instance.name = prefab.name;

            if (parent == null)
            {
                DontDestroyOnLoad(Instance);
            }

            IsCreated = true;

            Instance.OnCreatedInstance();
        }

        /// <summary>
        /// インスタンスを破棄
        /// </summary>
        public void Destroy()
        {
            Destroy(Instance);
        }

        /// <summary>
        /// インスタンス生成時に実行
        /// </summary>
        protected virtual void OnCreatedInstance() { }
    }
}
