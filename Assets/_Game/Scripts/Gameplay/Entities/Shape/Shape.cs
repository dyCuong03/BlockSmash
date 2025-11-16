namespace BlockSmash.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BlockSmash.Extensions;
    using BlockSmash.Managers;
    using BlockSmash.Models;
    using BlockSmash.Pooling;
    using UnityEngine;
    using VContainer;

    [RequireComponent(typeof(Rigidbody2D))]
    public class Shape : Entities
    {
        [SerializeField] private Block      blockPrefab;
        [SerializeField] private GameObject root;
        [SerializeField] private GameObject colliderRoot;

        private IObjectPoolManager objectPoolManager;
        private GameManager        gameManager;

        protected override void OnInstantiated()
        {
            this.objectPoolManager = this.GetCurrentContainer().Resolve<IObjectPoolManager>();
            this.gameManager       = this.GetCurrentContainer().Resolve<GameManager>();
        }

        private ShapeModel  shapeModel;
        public  List<Block> Blocks { get; } = new();

        private const float BLOCK_SPACING = 1.17f;

        public bool IsPlaced { get; private set; } = false;
        
        public void BindData(ShapeModel data)
        {
            this.shapeModel = data;

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
                return true;
            }
        }

        protected override void OnSpawned()
        {
        }

        protected override void OnRecycled()
        {
        }
    }
}