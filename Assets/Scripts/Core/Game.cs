namespace Escaping.Core
{
    /// <summary>
    /// ゲームを管理するクラス
    /// </summary>
    public class Game : SingletonMonoBehaviour<Game>
    {
        /// <inheritdoc/>
        protected override async void OnCreatedInstance()
        {
            if (Fade.Instance == null)
            {
                await Fade.Create("Prefabs/Core/FadeImage", transform);

                Fade.Instance.Show();
            }
        }
    }
}
