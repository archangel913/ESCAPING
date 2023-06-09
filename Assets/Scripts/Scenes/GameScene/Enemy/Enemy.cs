namespace Escaping.GameScene.Enemy
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    /// <summary>
    /// 敵のクラス
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        /// <summary>
        /// 敵の移動アルゴリズム
        /// </summary>
        public void Move()
        {
            transform.position += transform.forward * Time.deltaTime;
        }

        /// <summary>
        /// あたったときに呼ばれる
        /// </summary>
        /// <param name="collision">collision</param>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name == "Wall(Clone)")
            {
                transform.position -= transform.forward * Time.deltaTime;
                transform.Rotate(0, 45, 0, Space.World);
            }
        }
    }
}
