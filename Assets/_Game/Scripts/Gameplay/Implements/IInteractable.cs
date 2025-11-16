namespace BlockSmash.Implements
{
    using UnityEngine;

    public interface IInteractable
    {
        void OnBeginDrag(Vector3 mousePosition);
        
        void OnDrag(Vector3 mousePosition);
        
        void OnEndDrag();
    }
}