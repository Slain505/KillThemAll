using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Shared
{
	public class PopupFactory
	{
		private readonly Dictionary<Type, IPopup> instanceContainer;

		public PopupFactory()
		{
			instanceContainer = new Dictionary<Type, IPopup>();
		}

		public void RegisterInstance(IPopup newInstance)
		{
			var type = newInstance.GetType();
			if (!instanceContainer.ContainsKey(type))
			{
				newInstance.Init();
				instanceContainer.Add(type, newInstance);
			}
			else
			{
				Debug.LogError(
					$"PopupFactory :: Trying to register instance of {newInstance.GetType()} that already exists.");
			}
		}

		public void UnregisterInstance(IPopup newInstance)
		{
			var type = newInstance.GetType();
			if (instanceContainer.ContainsKey(type))
			{
				newInstance.Dispose();
				instanceContainer.Remove(type);
			}
		}

		public IPopup GetInstance<T>()
		{
			var type = typeof(T);
			if (instanceContainer.TryGetValue(type, out var popup))
			{
				return popup;
			}

			return null;
		}

		public void Dispose()
		{
			foreach (var popup in instanceContainer.Values)
			{
				popup.Dispose();
			}
		}
	}
}