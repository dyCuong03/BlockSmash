namespace BlockSmash.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BlockSmash.Extensions;
    using BlockSmash.Managers;
    using BlockSmash.Models;
    using BlockSmash.Pooling;
    using BlockSmash.Services;
    using BlockSmash.Signals;
    using MessagePipe;
    using UnityEngine;
    using VContainer;
    using Grid = BlockSmash.Models.Grid;

    public class Level : Entities
    {
        [SerializeField] private int        gridWidth  = 8;
        [SerializeField] private int        gridHeight = 8;
        [SerializeField] private GameObject root;
        [SerializeField] private GameObject cellRoot;
        [SerializeField] private Cell       cellPrefab;
        [SerializeField] private Shape      shapePrefab;
        [SerializeField] public  Transform  placedRoot;

        private List<GameObject> slots  = new();
        private List<Shape>      shapes = new();
        private List<Cell>       cells  = new();

        private IObjectPoolManager             objectPoolManager;
        private ShapeService                   shapeService;
        private ISubscriber<ShapePlacedSignal> shapePlacedSubscriber;
        private GameManager                    gameManager;
        private ShapeService                   ShapeService          => this.shapeService ??= this.GetCurrentContainer().Resolve<ShapeService>();
        private IObjectPoolManager             ObjectPoolManager     => this.objectPoolManager ??= this.GetCurrentContainer().Resolve<IObjectPoolManager>();
        private ISubscriber<ShapePlacedSignal> ShapePlacedSubscriber => this.shapePlacedSubscriber ??= this.GetCurrentContainer().Resolve<ISubscriber<ShapePlacedSignal>>();

        private IDisposable shapeSubscription;

        public Grid        Grid   { get; private set; }
        public List<Shape> Shapes => this.shapes;

        protected override void OnSpawned()
        {
            this.shapeSubscription = this.ShapePlacedSubscriber.Subscribe(this.OnShapePlaced);
            this.InitializeGrid();
            if (this.slots.Count <= 0) this.GenerateSlots();
            this.GenerateShape();
        }

        private void OnShapePlaced(ShapePlacedSignal signal)
        {
            this.shapeRemaining.Remove(signal.shapeModel);
            this.GenerateShape();
            this.shapes.RemoveAll(shape =>
            {
                if (shape.Blocks.Count != 0) return false;
                this.objectPoolManager.Recycle(shape);
                return true;
            });
        }

        private void InitializeGrid()
        {
            this.Grid = new(this.gridWidth, this.gridHeight);

            const float SPACING_X = 1.18f;
            const float SPACING_Y = 1.18f;

            var totalWidth  = (this.gridWidth - 1) * SPACING_X;
            var totalHeight = (this.gridHeight - 1) * SPACING_Y;

            var offset = new Vector3(totalWidth / 2f, totalHeight / 2f, 0f);

            foreach (var cell in this.Grid.Cells)
            {
                var cellPos = new Vector3(cell.position.x * SPACING_X, cell.position.y * SPACING_Y, 0f);

                var centeredPos = cellPos - offset;

                var cellObj = this.ObjectPoolManager.Spawn(
                    this.cellPrefab,
                    position: centeredPos,
                    parent: this.cellRoot.transform,
                    spawnInWorldSpace: false);

                this.cells.Add(cellObj);
                cellObj.BindData(cell);
            }
        }

        private void GenerateSlots()
        {
            const int   SLOT_COUNT    = 3;
            const float SPACING       = 5;
            var         startPosition = Vector3.zero;

            for (var i = 0; i < SLOT_COUNT; i++)
            {
                var x            = startPosition.x + (i - (SLOT_COUNT - 1) / 2f) * SPACING;
                var slotPosition = Vector3.right * x;

                var slot = new GameObject($"Slot_{i}")
                {
                    transform =
                    {
                        parent        = this.root.transform,
                        localPosition = slotPosition
                    },
                };

                this.slots.Add(slot);
            }
        }

        private List<ShapeModel> shapeRemaining = new();

        private void GenerateShape()
        {
            foreach (var shp in from slot in this.slots
                                where slot.transform.childCount <= 0
                                select this.ObjectPoolManager.Spawn(
                                    this.shapePrefab,
                                    Vector3.zero,
                                    Quaternion.identity,
                                    slot.transform,
                                    spawnInWorldSpace: false
                                ))
            {
                this.shapes.Add(shp);

                shp.BindData(this, this.ShapeService.GetShapeModel());
                this.shapeRemaining.Add(shp.ShapeModel);
            }

            this.gameManager?.CheckLose(this.shapeRemaining);
        }

        protected override void OnRecycled()
        {
            base.OnRecycled();
            this.shapes.RemoveAll(shape =>
            {
                this.objectPoolManager.Recycle(shape);
                return true;
            });

            this.cells.RemoveAll(cell =>
            {
                this.objectPoolManager.Recycle(cell);
                return true;
            });
            this.gameManager = null;
            this.shapeSubscription.Dispose();
        }

        public void BindData(GameManager gameManager1)
        {
            this.gameManager = gameManager1;
        }
    }
}