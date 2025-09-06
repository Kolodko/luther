using UnityEngine;

namespace Game.Core
{
    public class AreaBounds : MonoBehaviour
    {
        public Vector2 Size = new Vector2(10, 10);

        public Vector3 RandomPoint()
        {
            return transform.position + new Vector3(
                Random.Range(-Size.x * 0.5f, Size.x * 0.5f),
                0.1f,
                Random.Range(-Size.y * 0.5f, Size.y * 0.5f));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0, 1, 1, 0.3f);
            Gizmos.DrawCube(transform.position, new Vector3(Size.x, 0.02f, Size.y));
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(Size.x, 0.02f, Size.y));
        }
    }
}