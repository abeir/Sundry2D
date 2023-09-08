using System;

namespace Sundry.Pool
{
    public interface IObjectPool<T> : IDisposable
    {

        public T Spawn();

        public void Despawn(T instance);

    }
}