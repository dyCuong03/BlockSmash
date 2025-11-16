namespace BlockSmash.Managers
{
    using System.Linq;
    using BlockSmash.Extensions;
    using BlockSmash.Implements;
    using UnityEngine;

    internal sealed class InputManager : MonoBehaviour
    {
        private const float DRAG_THRESHOLD = .1f;

        public bool IsSelected => this.selected is { };

        private new Camera         camera = null!;
        private     IInteractable? selected;
        private     Vector3        mouseDownPosition;
        private     bool           isDragging;

        private void Start()
        {
            this.camera     = Camera.main!;
            this.selected   = null;
            this.isDragging = false;
        }

        private void MouseDown()
        {
            this.mouseDownPosition = this.GetMousePosition();
            if (this.selected is { })
            {
                this.Drop();
                return;
            }
            var interactable = this.Raycast2DAtMousePosition<IInteractable>();
            if (interactable is null) return;

            this.selected = interactable;
            this.selected.OnBeginDrag();
        }

        private void MouseDrag()
        {
            if (this.selected is null) return;
            var mousePosition = this.GetMousePosition();

            if (Vector3.Distance(mousePosition, this.mouseDownPosition) > DRAG_THRESHOLD) this.isDragging = true;
            if (!this.isDragging) return;
            this.selected.OnDrag(this.GetMousePosition());
        }

        private void MouseUp()
        {
            if (this.selected is null) return;
            if (!this.isDragging) return;
            this.isDragging = false;
            this.Drop();
        }

        private void Drop()
        {
            this.selected?.OnEndDrag();
            this.selected = null;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.MouseDown();
            }
            if (Input.GetMouseButton(0))
            {
                this.MouseDrag();
            }
            if (Input.GetMouseButtonUp(0))
            {
                this.MouseUp();
            }
        }

        private T? Raycast2DAtMousePosition<T>()
        {
            var layerMask = 1 << LayerMask.NameToLayer(nameof(InputManager));
            var ray       = this.camera.ScreenPointToRay(Input.mousePosition);
            return Physics2D.RaycastAll(ray.origin, ray.direction, this.camera.farClipPlane, layerMask)
                .Select(hit => hit.rigidbody ? hit.rigidbody.GetComponentInParent<T>() : default)
                .FirstOrDefault(hit => hit is { });
        }

        private Vector3 GetMousePosition()
        {
            return this.camera.ScreenToWorldPoint(Input.mousePosition.WithZ(-this.camera.transform.position.z));
        }
    }
}