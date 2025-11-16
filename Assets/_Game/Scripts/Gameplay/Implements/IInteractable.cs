namespace BlockSmash.Implements
{
    using UnityEngine;

    public interface IInteractable
    {
        void OnBeginDrag();
        
        void OnDrag(Vector2 mousePosition);
        
        void OnEndDrag();
    }
}