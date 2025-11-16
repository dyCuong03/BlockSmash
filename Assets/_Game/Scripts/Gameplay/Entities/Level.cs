namespace BlockSmash.Entities
{
    using System.Collections.Generic;
    using BlockSmash.Extensions;
    using BlockSmash.Pooling;
    using UnityEngine;
    using VContainer;
    using Grid = BlockSmash.Models.Grid;

    public class Level : MonoBehaviour
    {
        [SerializeField] private int        gridWidth  = 8;
        [SerializeField] private int        gridHeight = 8;
        [SerializeField] private GameObject root;
        [SerializeField] private GameObject cellRoot;
        [SerializeField] private Cell       cellPrefab;
        [SerializeField] private Shape      shapePrefab;

        private Grid             grid;
        private List<GameObject> slots  = new();
        private List<Shape>      shapes = new();
        private List<Cell>       cells  = new();

        private IObjectPoolManager objectPoolManager;

        private void Awake()
        {
            this.objectPoolManager = this.GetCurrentContainer().Resolve<IObjectPoolManager>();
        }

        private void Start()
        {
            this.InitializeGrid();
            this.GenerateSlots();
            this.GenerateShape();
        }

        private void InitializeGrid()
        {
            this.grid = new Grid(this.gridWidth, this.gridHeight);

            const float SPACING_X = 1.18f;
            const float SPACING_Y  = 1.18f;

            var totalWidth  = (this.gridWidth - 1) * SPACING_X;
            var totalHeight = (this.gridHeight - 1) * SPACING_Y;

            var offset = new Vector3(totalWidth / 2f, totalHeight / 2f, 0f);

            foreach (var cell in this.grid.Cells)
            {
                var cellPos = new Vector3(cell.position.x * SPACING_X, cell.position.y * SPACING_Y, 0f);

                var centeredPos = cellPos - offset;

                this.cells.Add(
                    this.objectPoolManager.Spawn(
                        this.cellPrefab,
                        position: centeredPos,
                        parent: this.cellRoot.transform,
                        spawnInWorldSpace: false
                    )
                );
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

        private void GenerateShape()
        {
            foreach (var slot in this.slots)
            {
                this.shapes.Add(this.objectPoolManager.Spawn(this.shapePrefab, Vector3.zero, Quaternion.identity, slot.transform, spawnInWorldSpace: false));
            }
        }
    }
}