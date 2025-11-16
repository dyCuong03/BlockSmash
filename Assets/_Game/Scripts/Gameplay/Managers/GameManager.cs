namespace BlockSmash.Managers
{
    using System.Collections.Generic;
    using BlockSmash.Entities;
    using BlockSmash.Pooling;
    using UnityEngine;
    using VContainer.Unity;

    public class GameManager : IStartable
    {
        private readonly IObjectPoolManager poolManager;
        private readonly Level              levelPrefab;
        private readonly Transform          parent;

        public GameManager(IObjectPoolManager poolManager, Level levelPrefab, Transform parent)
        {
            this.poolManager = poolManager;
            this.levelPrefab = levelPrefab;
            this.parent      = parent;
        }

        public Level Level { get; set; }

        public void Start()
        {
            this.Level = this.poolManager.Spawn(this.levelPrefab, Vector3.zero, Quaternion.identity, this.parent);
        }

        public bool CanPlace()
        {
            return false;
        }

        public bool TryPlace(List<Vector2Int> positions)
        {
            return this.Level.Grid.TryPlaceShape(positions);
        }

        public bool TryRemove()
        {
            return false;
        }
    }
}