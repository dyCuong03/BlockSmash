namespace BlockSmash.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using BlockSmash.Entities;
    using BlockSmash.Extensions;
    using BlockSmash.Models;
    using BlockSmash.Pooling;
    using BlockSmash.Services;
    using BlockSmash.Signals.BlockSmash.Signals;
    using MessagePipe;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public class GameManager : IStartable
    {
        private readonly IObjectPoolManager poolManager;
        private readonly Level              levelPrefab;
        private readonly Transform          parent;
        private readonly ShapeService       shapeService;

        public GameManager(IObjectPoolManager poolManager, Level levelPrefab, Transform parent, ShapeService shapeService)
        {
            this.poolManager  = poolManager;
            this.levelPrefab  = levelPrefab;
            this.parent       = parent;
            this.shapeService = shapeService;
        }

        private Level Level { get; set; }
        
        private IPublisher<GameLoseSignal> gameLosePublisher;
        private IPublisher<GameLoseSignal> GameLosePublisher => this.gameLosePublisher ??= this.GetCurrentContainer().Resolve<IPublisher<GameLoseSignal>>();

        public void Start()
        {
            this.Level = this.poolManager.Spawn(this.levelPrefab, Vector3.zero, Quaternion.identity, this.parent);
            this.Level.BindData(this);
        }

        public bool TryPlace(List<Vector2Int> positions)
        {
            if (!this.Level.Grid.TryPlaceShape(positions))
                return false;

            this.CheckAndRemoveFullLines();

            return true;
        }

        private void CheckAndRemoveFullLines()
        {
            var grid = this.Level.Grid;

            var removedCells = grid.RemoveFullLines();
            if (removedCells.Count == 0)
                return;

            var removedPositions = new HashSet<Vector2Int>(
                removedCells.Select(c => c.position)
            );

            foreach (var shape in this.Level.Shapes)
            {
                shape.RemoveBlocksInPositions(removedPositions);
            }
        }

        public bool CheckLose(List<ShapeModel> availableShapes)
        {
            foreach (var shape in availableShapes)
            {
                for (var x = 0; x <= this.Level.Grid.Width - 1; x++)
                {
                    for (var y = 0; y <= this.Level.Grid.Height - 1; y++)
                    {
                        var movedPositions = shape.Blocks
                            .Select(b => new Vector2Int(b.Position.x + x, b.Position.y + y))
                            .ToList();

                        if (this.Level.Grid.CanPlaceShape(movedPositions))
                            return false;
                    }
                }
            }
            
            this.GameLosePublisher.Publish(new ());

            return true;
        }

        public void Replay()
        {
            this.poolManager.Recycle(this.Level);
            this.Level = null;
            this.shapeService.OnReplay();
            this.Start();
        }
    }
}