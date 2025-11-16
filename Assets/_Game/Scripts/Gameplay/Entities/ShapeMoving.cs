namespace BlockSmash.Entities
{
    using BlockSmash.Implements;
    using UnityEngine;

    public class ShapeMoving : MonoBehaviour, IInteractable
    {
        [SerializeField] private float moveSpeed = 20f;
        
        public void OnBeginDrag()
        {
            Debug.Log("OnBeginDrag");
        }

        public void OnDrag(Vector2 mousePosition)
        {
            Debug.Log("OnDrag");
            this.transform.position = Vector3.Lerp(this.transform.position, mousePosition, Time.deltaTime * this.moveSpeed);
        }

        public void OnEndDrag()
        {
            Debug.Log("OnEndDrag");
        }
    }
}