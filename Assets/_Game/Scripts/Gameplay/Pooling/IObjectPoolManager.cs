#nullable enable
namespace BlockSmash.Pooling
{
    using System;
    using UnityEngine;

    public interface IObjectPoolManager
    {
        public event Action<GameObject> Instantiated;

        public event Action<GameObject> Spawned;

        public event Action<GameObject> Recycled;

        public event Action<GameObject> CleanedUp;

        public void Load(GameObject prefab, int count = 1);

        public GameObject Spawn(GameObject prefab, Vector3? position = null, Quaternion? rotation = null, Transform? parent = null, bool spawnInWorldSpace = true);

        public void Recycle(GameObject instance);

        public void RecycleAll(GameObject prefab);

        public void Cleanup(GameObject prefab, int retainCount = 1);

        public void Unload(GameObject prefab);
    }
}