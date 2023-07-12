using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextFx
{
	public class ObjectPool<T> where T : Component
	{
		private class PoolObjectData
		{
			public T m_type_obj;

			public GameObject m_gameObject;

			public Transform m_transform;
		}

		private GameObject m_prefab_reference;

		private List<PoolObjectData> m_pool;

		private List<PoolObjectData> m_in_use_pool;

		private Transform m_pool_container;

		private string m_pool_name = string.Empty;

		private int m_total_pool_size;

		private Dictionary<string, PoolObjectData> m_obj_hash_lookup;

		private Action<T> m_object_creation_callback;

		private Action<T> m_object_recycle_steps_override;

		public ObjectPool(GameObject prefab, int start_pool_size, string poolNameOverride = "", Action<T> object_creation_callback = null, Action<T> object_recycle_steps_override = null)
		{
			m_prefab_reference = prefab;
			m_object_creation_callback = object_creation_callback;
			m_object_recycle_steps_override = object_recycle_steps_override;
			m_pool = new List<PoolObjectData>();
			m_in_use_pool = new List<PoolObjectData>();
			m_obj_hash_lookup = new Dictionary<string, PoolObjectData>();
			m_pool_name = ((!(poolNameOverride != string.Empty)) ? typeof(T).Name : poolNameOverride);
			m_pool_container = new GameObject("ObjectPool - " + m_pool_name).transform;
			for (int i = 0; i < start_pool_size; i++)
			{
				AddNewPoolItem();
			}
		}

		private PoolObjectData AddNewPoolItem()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(m_prefab_reference);
			Transform transform = gameObject.transform;
			T component = gameObject.GetComponent<T>();
			PoolObjectData poolObjectData = new PoolObjectData();
			poolObjectData.m_gameObject = gameObject;
			poolObjectData.m_transform = transform;
			poolObjectData.m_type_obj = component;
			PoolObjectData poolObjectData2 = poolObjectData;
			m_obj_hash_lookup.Add(component.GetHashCode().ToString(), poolObjectData2);
			m_pool.Add(poolObjectData2);
			transform.SetParent(m_pool_container);
			gameObject.SetActive(false);
			gameObject.name = m_pool_name + " #" + m_total_pool_size;
			m_total_pool_size++;
			if (m_object_creation_callback != null)
			{
				m_object_creation_callback(component);
			}
			return poolObjectData2;
		}

		public T GetObject(bool activateObject = true)
		{
			T val = (T)null;
			if (m_pool.Count > 0)
			{
				PoolObjectData poolObjectData = m_pool[0];
				poolObjectData.m_gameObject.SetActive(activateObject);
				val = poolObjectData.m_type_obj;
				m_pool.RemoveAt(0);
				m_in_use_pool.Add(poolObjectData);
			}
			else
			{
				PoolObjectData poolObjectData2 = AddNewPoolItem();
				m_pool.RemoveAt(0);
				m_in_use_pool.Add(poolObjectData2);
				poolObjectData2.m_gameObject.SetActive(activateObject);
				val = poolObjectData2.m_type_obj;
			}
			return val;
		}

		public void Recycle(T obj)
		{
			int hashCode = obj.GetHashCode();
			if (m_obj_hash_lookup.ContainsKey(hashCode.ToString()))
			{
				PoolObjectData poolObjectData = m_obj_hash_lookup[hashCode.ToString()];
				if (m_in_use_pool.Contains(poolObjectData))
				{
					poolObjectData.m_transform.SetParent(m_pool_container);
					if (m_object_recycle_steps_override != null)
					{
						m_object_recycle_steps_override(poolObjectData.m_type_obj);
					}
					else
					{
						poolObjectData.m_gameObject.SetActive(false);
					}
					m_in_use_pool.Remove(poolObjectData);
					m_pool.Add(poolObjectData);
				}
			}
			else
			{
				Debug.LogWarning("You're trying to \"recycle\" a pool object, which isn't already part of this pool");
			}
		}

		public void ResetPoolAll(Action<T> bespoke_callback = null)
		{
			foreach (PoolObjectData item in m_in_use_pool)
			{
				item.m_gameObject.SetActive(false);
				item.m_transform.parent = m_pool_container;
				m_pool.Add(item);
				if (bespoke_callback != null)
				{
					bespoke_callback(item.m_type_obj);
				}
			}
			m_in_use_pool = new List<PoolObjectData>();
		}
	}
}
