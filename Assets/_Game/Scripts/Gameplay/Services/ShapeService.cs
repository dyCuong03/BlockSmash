namespace BlockSmash.Services
{
    using BlockSmash.Models;
    using VContainer.Unity;

    public class ShapeService : IInitializable
    {
        private readonly ShapeCollection shapeCollection;

        public ShapeService(ShapeCollection shapeCollection)
        {
            this.shapeCollection = shapeCollection;
        }

        public void Initialize()
        {
        }
    }
}

