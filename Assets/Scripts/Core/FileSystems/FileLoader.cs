namespace Escaping.Core.FileSystems
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Events;

    public static class FileLoader
    {
        /// <summary>
        /// Resources �ȉ��̃t�@�C����񓯊��œǂݍ���
        /// </summary>
        /// <typeparam name="T">�ǂݍ��ރI�u�W�F�N�g�̌^</typeparam>
        /// <param name="path">�t�@�C���p�X</param>
        /// <param name="onLoadDone">�ǂݍ��݊������ɌĂ΂�鏈��</param>
        /// <param name="loading">�ǂݍ��ݒ��ɌĂ΂�鏈��</param>
        /// <returns>�ǂݍ��񂾃I�u�W�F�N�g</returns>
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