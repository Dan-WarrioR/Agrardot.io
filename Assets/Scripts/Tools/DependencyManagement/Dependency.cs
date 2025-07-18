using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
	public static class Dependency
	{
		private static readonly Dictionary<Type, object> Instances = new();

		//////////////////////////////////////////////////

		#region Interface

		public static T Get<T>() where T : class
		{
			var type = typeof(T);

			if (Instances.TryGetValue(type, out var instance))
			{
				return instance as T;
			}

			return null;
		}

		public static void Register<T>(T instance) where T : class
		{
			Register(typeof(T), instance);
		}

		public static void Register(Type type, object instance)
		{
			if (instance == null)
			{
				Debug.LogError($"Dependency Instance cannot be null.");
			}

			if (!Instances.TryAdd(type, instance))
			{
				Debug.LogError($"{type.Name} is already registered. Only one instance per type is allowed.");
			}
		}

		public static void Unregister<T>(T instance) where T : class
		{
			Unregister(typeof(T), instance);
		}

		public static void Unregister(Type type, object instance)
		{
			if (instance == null)
			{
				Debug.LogError($"Dependency Instance cannot be null.");
			}

			if (!Instances.ContainsKey(type))
			{
				Debug.LogError($"{type.Name} was not registered. Instance must be registered before unregister.");
			}

			Instances.Remove(type);
		}

		#endregion

		//////////////////////////////////////////////////
	}
}