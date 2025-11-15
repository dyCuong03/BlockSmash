namespace BlockSmash.Extensions
{
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public static class DIExtensions
    {
        private static LifetimeScope? CurrentSceneContext;

        public static IObjectResolver GetCurrentContainer(this object _)
        {
            if (CurrentSceneContext == null) CurrentSceneContext = Object.FindObjectOfType<LifetimeScope>();
            return CurrentSceneContext.Container;
        }
    }
}