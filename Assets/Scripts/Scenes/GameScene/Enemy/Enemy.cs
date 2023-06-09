namespace Escaping.GameScene.Enemy
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    /// <summary>
    /// “G‚ÌƒNƒ‰ƒX
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        /// <summary>
        /// “G‚ÌˆÚ“®ƒAƒ‹ƒSƒŠƒYƒ€
        /// </summary>
        public void Move()
        {
            transform.position += transform.forward * Time.deltaTime;
        }

        /// <summary>
        /// ‚ ‚½‚Á‚½‚Æ‚«‚ÉŒÄ‚Î‚ê‚é
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
