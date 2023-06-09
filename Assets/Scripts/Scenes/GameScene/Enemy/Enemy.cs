namespace Escaping.GameScene.Enemy
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    /// <summary>
    /// �G�̃N���X
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        /// <summary>
        /// �G�̈ړ��A���S���Y��
        /// </summary>
        public void Move()
        {
            transform.position += transform.forward * Time.deltaTime;
        }

        /// <summary>
        /// ���������Ƃ��ɌĂ΂��
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
