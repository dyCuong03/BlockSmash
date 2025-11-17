namespace BlockSmash.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using BlockSmash.Extensions;
    using BlockSmash.Managers;
    using BlockSmash.Models;
    using BlockSmash.Pooling;
    using BlockSmash.Signals;
    using MessagePipe;
    using UnityEngine;
    using VContainer;

    [RequireComponent(typeof(Rigidbody2D))]
    public class Shape : Entities
    {
        [SerializeField] private Block      blockPrefab;
        [SerializeField] private GameObject root;
        [SerializeField] private GameObject colliderRoot;

        private IObjectPoolManager            objectPoolManager;
        private GameManager                   gameManager;
        private IPublisher<ShapePlacedSignal> shapePublisher;

        protected override void OnInstantiated()
        {
            this.objectPoolManager = this.GetCurrentContainer().Resolve<IObjectPoolManager>();
            this.gameManager       = this.GetCurrentContainer().Resolve<GameManager>();
            this.shapePublisher    = this.GetCurrentContainer().Resolve<IPublisher<ShapePlacedSignal>>();
        }

        private Level       level;
        private ShapeModel  shapeModel;
        public  List<Block> Blocks { get; } = new();

        private const float BLOCK_SPACING = 1.17f;
        public        bool  IsPlaced { get; private set; } = false;
        public ShapeModel ShapeModel => this.shapeModel;

        public void BindData(Level level, ShapeModel data)
        {
            this.shapeModel = data;
            this.level      = level;

            foreach (var block in this.shapeModel.Blocks)
            {
                var pos = new Vector3(
                    block.Position.x * BLOCK_SPACING,
                    block.Position.y * BLOCK_SPACING,
                    0f
                );

                var blc = this.objectPoolManager.Spawn(
                    this.blockPrefab,
                    pos,
                    Quaternion.identity,
                    this.root.transform,
                    spawnInWorldSpace: false
                );

                blc.BindData(this, block);
                this.Blocks.Add(blc);

                var col = this.colliderRoot.AddComponent<BoxCollider2D>();
                col.size      = Vector2Int.one * (int)1.15;
                col.isTrigger = true;
                col.offset    = pos;
            }
        }

        public bool TryCollect()
        {
            if (!this.Blocks.All(block => block is { CanCollect: true })) return false;
            {
                if (!this.gameManager.TryPlace(this.Blocks.Select(block => block.CurrentCell.Position).ToList())) return false;
                this.IsPlaced = true;
                this.transform.SetParent(this.level.placedRoot);
                this.shapePublisher.Publish(new(this.shapeModel));
                return true;
            }
        }

        protected override void OnRecycled()
        {
            this.IsPlaced = false;
            
            this.Blocks.RemoveAll(block =>
            {
                this.objectPoolManager.Recycle(block);
                return true;
            });
            
            foreach (var col2d in this.colliderRoot.GetComponents<Collider2D>())
            {
                Destroy(col2d);
            }
        }

        public void RemoveBlocksInPositions(HashSet<Vector2Int> removedPositions)
        {
            foreach (var block in this.Blocks.Where(block => block.CurrentCell is { }
                    &&
                    removedPositions.Contains(block.CurrentCell.Position)).ToList())
            {
                block.OnRemoved();
                this.Blocks.Remove(block);
                this.objectPoolManager.Recycle(block);
            }
        }
    }
}