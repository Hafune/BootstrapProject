using System;
using UnityEngine;
using Reflex.Scripts.Utilities;
using System.Collections.Generic;

namespace Reflex
{
    public class Context : IDisposable
    {
        public Context Parent { get; private set; }
        public IReadOnlyList<Context> Children => _children;

        public string Name { get; }
        private readonly List<Context> _children = new();
        private readonly CompositeDisposable _disposables = new();
        private readonly Dictionary<Type, IResolver> _resolvers = new();

        public Context(string name)
        {
            Name = name;
            InjectSelf();
        }

        public void Dispose()
        {
            for (int i = _children.Count - 1; i >= 0; i--)
                _children[i].Dispose();

            Debug.Log($"Disposing container: {Name}");
            _disposables.Dispose();

            if (Parent != null)
            {
                Parent._children.Remove(this);
                Parent = null;
            }
        }

        public Context Scope(string name)
        {
            var scoped = new Context(name);
            scoped.Parent = this;
            _children.Add(scoped);

            foreach (var pair in _resolvers)
            {
                scoped._resolvers[pair.Key] = pair.Value;
            }

            scoped.InjectSelf();
            return scoped;
        }

        private void InjectSelf()
        {
            BindInstance(this);
        }

        public void AddDisposable(IDisposable disposable)
        {
            _disposables.TryAdd(disposable);
        }

        public bool Contains(Type type)
        {
            return _resolvers.ContainsKey(type);
        }

        public bool Contains<T>()
        {
            return _resolvers.ContainsKey(typeof(T));
        }

        public void BindInstance(object instance)
        {
            BindInstanceAs(instance, instance.GetType());
        }

        public void BindInstanceAs<TContract>(TContract instance)
        {
            BindInstanceAs(instance, typeof(TContract));
        }

        public void BindInstanceAs(object instance, Type asType)
        {
            var resolver = new InstanceResolver(instance);
            _disposables.Add(resolver);
            _resolvers[asType] = resolver;
        }

        public TContract Resolve<TContract>()
        {
            return (TContract)Resolve(typeof(TContract));
        }

        public object Resolve(Type contract)
        {
            if (_resolvers.TryGetValue(contract, out var resolver))
            {
                return resolver.Resolve(this);
            }

            throw new UnknownContractException(contract);
        }

        public T Instantiate<T>(T original) where T : Component
        {
            return MonoInstantiate.Instantiate(original, null, this,
                parent => UnityEngine.Object.Instantiate(original, parent));
        }

        public T Instantiate<T>(T original, Transform container) where T : Component
        {
            return MonoInstantiate.Instantiate(original, container, this,
                parent => UnityEngine.Object.Instantiate(original, parent));
        }

        public T Instantiate<T>(T original, Transform container, bool worldPositionStays) where T : Component
        {
            return MonoInstantiate.Instantiate(original, container, this,
                parent => UnityEngine.Object.Instantiate(original, parent, worldPositionStays));
        }
        
        public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform container = null) where T : Component
        {
            return MonoInstantiate.Instantiate(original, container, this,
                parent => UnityEngine.Object.Instantiate(original, position, rotation, parent));
        }
    }
}