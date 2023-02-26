namespace Escaping.Core.FileSystems
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Events;

    public static class FileLoader
    {
        /// <summary>
        /// Resources 以下のファイルを非同期で読み込む
        /// </summary>
        /// <typeparam name="T">読み込むオブジェクトの型</typeparam>
        /// <param name="path">ファイルパス</param>
        /// <param name="onLoadDone">読み込み完了時に呼ばれる処理</param>
        /// <param name="loading">読み込み中に呼ばれる処理</param>
        /// <returns>読み込んだオブジェクト</returns>
        public static async UniTask<T> LoadAssetAsync<T>(string path, UnityAction onLoadDone = null, UnityAction<float> loading = null) where T : Object
        {
            var request = Resources.LoadAsync<T>(path);

            await UniTask.WaitUntil(() => {
                loading?.Invoke(request.progress);

                return request.isDone;
            });

            onLoadDone?.Invoke();

            return request.asset as T;
        }
    }
}