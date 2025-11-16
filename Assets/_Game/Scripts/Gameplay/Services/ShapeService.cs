namespace BlockSmash.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BlockSmash.Models;
    using UnityEngine;
    using VContainer.Unity;
    using Random = UnityEngine.Random;

    public class ShapeService : IInitializable
    {
        private readonly TextAsset jsonFile;
        public ShapeService(TextAsset jsonFile)
        {
            this.jsonFile = jsonFile;
        }
        
        private const int COLOR_COUNT = 8;

        private readonly List<ShapeModel> currentShapes = new();
        private ShapeCollection shapeCollection;
        
        public void Initialize()
        {
            if (this.jsonFile == null)
            {
                Debug.LogError($"{nameof(this.jsonFile)} is null");
                return;
            }
            this.shapeCollection = JsonUtility.FromJson<ShapeCollection>(this.jsonFile.text);;
        }

        public ShapeModel GetShapeModel()
        {
            if (this.shapeCollection.shapes == null || this.shapeCollection.shapes.Count == 0)
                throw new InvalidOperationException("No shapes available.");

            var usedShapeIds = this.currentShapes.Select(s => s.ShapeId).ToHashSet();
            var availableShapes = this.shapeCollection.shapes
                .Where(s => !usedShapeIds.Contains(s.shapeId))
                .ToList();

            if (availableShapes.Count == 0)
                throw new InvalidOperationException("All shapes are already used.");

            var randomShapeIndex = Random.Range(0, availableShapes.Count);
            var selectedShape    = availableShapes[randomShapeIndex];
            
            var allColors        = Enumerable.Range(0, COLOR_COUNT).ToList();

            var usedColors      = this.currentShapes.Select(s => s.ColorId).ToHashSet();
            var availableColors = allColors.Where(c => !usedColors.Contains(c)).ToList();

            if (availableColors.Count == 0)
                throw new InvalidOperationException("No available ColorPreset left.");

            var randomColorIndex = Random.Range(0, availableColors.Count);
            var selectedColor    = availableColors[randomColorIndex];

            var model = new ShapeModel(selectedShape, selectedColor);

            this.currentShapes.Add(model);

            return model;
        }

    }
}