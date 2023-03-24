namespace Escaping.Core
{
    /// <summary>
    /// シングルトンクラス
    /// </summary>
    /// <typeparam name="T">継承するクラス</typeparam>
    public abstract class Singleton<T>
        where T : Singleton<T>, new()
    {
        private static T m_Instance = null;

        /// <summary>
        /// インスタンス
        /// </summary>
        public static T Instance
        {
            get { return m_Instance ??= new T(); }
        }
    }
}