namespace BlockSmash.Entities
{
    using System.Linq;
    using _Game.Scripts.Gameplay.Utils;
    using BlockSmash.Implements;
    using UnityEngine;
    
    public class ShapeMoving : MonoBehaviour, IInteractable
    {
        [SerializeField] private float moveSpeed = 20f;
        [SerializeField] private Shape shape;

        private Vector3 dragOffset;

        public void OnBeginDrag(Vector3 mousePosition)
        {
            if (this.shape.IsPlaced) return;
            this.dragOffset = this.transform.position - mousePosition;
        }

        public void OnDrag(Vector3 mousePosition)
        {
            if (this.shape.IsPlaced) return;
            this.transform.position = Vector3.Lerp(this.transform.position, mousePosition + this.dragOffset, Time.deltaTime * this.moveSpeed);
            this.shape.SetBlockState(ShapeState.Drag);
        }

        public void OnEndDrag()
        {
            if (this.shape.IsPlaced) return;
            this.shape.SetBlockState(ShapeState.Normal);
            if (this.shape.TryCollect())
            {
                GridUtils.AlignBlocksToCells(this.transform, this.shape.Blocks.Where(bl => !bl.IsComplete).ToList());
            }
            else
            {
                this.transform.localPosition = Vector3.zero;
            }
        }
    }
}