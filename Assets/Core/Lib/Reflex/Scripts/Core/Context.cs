using System;
using UnityEngine;
using Reflex.Scripts.Utilities;
using System.Collections.Generic;
using Reflex.Scripts.Enums;

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
            foreach (var child in _children.Reversed())
            {
                child.Dispose();
            }

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
                parent => UnityEngine.Object.Instantiate(original, parent), MonoInjectionMode.Recursive);
        }

        public T Instantiate<T>(T original, Transform container = null,
            MonoInjectionMode injectionMode = MonoInjectionMode.Recursive) where T : Component
        {
            return MonoInstantiate.Instantiate(original, container, this,
                parent => UnityEngine.Object.Instantiate<T>(original, parent), injectionMode);
        }

        public T Instantiate<T>(T original, Transform container, bool worldPositionStays,
            MonoInjectionMode injectionMode = MonoInjectionMode.Recursive) where T : Component
        {
            return MonoInstantiate.Instantiate(original, container, this,
                parent => UnityEngine.Object.Instantiate<T>(original, parent, worldPositionStays), injectionMode);
        }

        public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform container,
            MonoInjectionMode injectionMode) where T : Component
        {
            return MonoInstantiate.Instantiate(original, container, this,
                parent => UnityEngine.Object.Instantiate<T>(original, position, rotation, parent), injectionMode);
        }
        
        public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform container = null) where T : Component
        {
            return MonoInstantiate.Instantiate(original, container, this,
                parent => UnityEngine.Object.Instantiate<T>(original, position, rotation, parent), MonoInjectionMode.Recursive);
        }
    }
}