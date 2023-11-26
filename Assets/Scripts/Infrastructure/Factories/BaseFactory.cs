using UnityEngine;

namespace Infrastructure.Factories
{
    public class BaseFactory
    {
        protected GameObject Load(string path)
            => Resources.Load<GameObject>(path);
    }
}