namespace BlockSmash.Signals
{
    using Models;

    public class ShapePlacedSignal
    {
        public ShapeModel shapeModel;
        public ShapePlacedSignal(ShapeModel shapeModel)
        {
            this.shapeModel = shapeModel;
        }
    }
}