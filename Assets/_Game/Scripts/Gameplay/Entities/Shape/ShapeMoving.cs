namespace BlockSmash.Entities
{
    using _Game.Scripts.Gameplay.Utils;
    using BlockSmash.Implements;
    using UnityEngine;

    public class ShapeMoving : MonoBehaviour, IInteractable
    {
        [SerializeField] private float moveSpeed = 20f;
        [SerializeField] private Shape shape;

        public void OnBeginDrag()
        {
            if(this.shape.IsPlaced) return;
        }

        public void OnDrag(Vector2 mousePosition)
        {
            if(this.shape.IsPlaced) return;
            this.transform.position = Vector3.Lerp(this.transform.position, mousePosition, Time.deltaTime * this.moveSpeed);
        }

        public void OnEndDrag()
        {
            if(this.shape.IsPlaced) return;
            if (this.shape.TryCollect())
            {
                GridUtils.AlignBlocksToCells(this.transform, this.shape.Blocks);
            }
            else
            {
                this.transform.localPosition =  Vector3.zero;
            }
        }
    }
}