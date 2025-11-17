namespace BlockSmash.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BlockSmash.Extensions;
    using BlockSmash.Models;
    using BlockSmash.Signals;
    using MessagePipe;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;
    using Random = UnityEngine.Random;

    public class ShapeService : IInitializable
    {
        private readonly TextAsset jsonFile;
        
        private ISubscriber<ShapePlacedSignal> shapePlacedSubscriber;
        private ISubscriber<ShapePlacedSignal> ShapePlacedSubscriber => this.shapePlacedSubscriber ??= this.GetCurrentContainer().Resolve<ISubscriber<ShapePlacedSignal>>();

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
            this.shapeCollection = JsonUtility.FromJson<ShapeCollection>(this.jsonFile.text);

            this.ShapePlacedSubscriber.Subscribe(this.OnShapePlacedAction);
        }

        private void OnShapePlacedAction(ShapePlacedSignal shapePlacedSignal)
        {
            this.currentShapes.Remove(shapePlacedSignal.shapeModel);
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

            var allColors       = Enumerable.Range(0, COLOR_COUNT).ToList();
            var usedColors      = this.currentShapes.Select(s => s.ColorId).ToHashSet();
            var availableColors = allColors.Where(c => !usedColors.Contains(c)).ToList();

            if (availableColors.Count == 0)
                throw new InvalidOperationException("No available colors left.");

            var randomShape = availableShapes[Random.Range(0, availableShapes.Count)];
            var randomColor = availableColors[Random.Range(0, availableColors.Count)];

            var model = new ShapeModel(randomShape, randomColor);
            this.currentShapes.Add(model);

            return model;
        }

        public void OnReplay()
        {
            this.currentShapes.Clear();
        }
    }
}