using System;
using System.Web.Caching;

namespace umbraco.cms.businesslogic.cache
{
	/// <summary>
	/// Summary description for Cache.
	/// </summary>
	public class Cache
	{

		/// <summary>
		/// Clears everything in umbraco's runtime cache, which means that not only
		/// umbraco content is removed, but also other cache items from pages runing in
		/// the same application / website. Use with care :-)
		/// </summary>
		public static void ClearAllCache() 
		{
			System.Web.Caching.Cache c = System.Web.HttpRuntime.Cache;
			if (c != null) 
			{
				System.Collections.IDictionaryEnumerator cacheEnumerator = c.GetEnumerator();
				while ( cacheEnumerator.MoveNext() )
				{
					c.Remove(cacheEnumerator.Key.ToString());
				}
			}
		}
		
		/// <summary>
		/// Clears the item in umbraco's runtime cache with the given key 
		/// </summary>
		/// <param name="Key">Key</param>
		public static void ClearCacheItem(string Key) 
		{
			if (System.Web.HttpRuntime.Cache[Key] != null) 
			{
				System.Web.HttpRuntime.Cache.Remove(Key);
				System.Web.HttpContext.Current.Trace.Warn("Cache", "Item " + Key + " removed from cache");
			}
		}
		
		
		/// <summary>
		/// Clears all objects in the System.Web.Cache with the System.Type name as the
		/// input parameter. (using [object].GetType())
		/// </summary>
		/// <param name="TypeName">The name of the System.Type which should be cleared from cache ex "System.Xml.XmlDocument"</param>
		public static void ClearCacheObjectTypes(string TypeName) 
		{
			System.Web.Caching.Cache c = System.Web.HttpRuntime.Cache;
			try 
			{
				if (c != null) 
				{
					System.Collections.IDictionaryEnumerator cacheEnumerator = c.GetEnumerator();
					while ( cacheEnumerator.MoveNext() )
						if (cacheEnumerator.Key != null && c[cacheEnumerator.Key.ToString()] != null && c[cacheEnumerator.Key.ToString()].GetType() != null && c[cacheEnumerator.Key.ToString()].GetType().ToString() == TypeName)
							c.Remove(cacheEnumerator.Key.ToString());

				}
			} 
			catch (Exception CacheE) 
			{
				BusinessLogic.Log.Add(BusinessLogic.LogTypes.Error, BusinessLogic.User.GetUser(0), -1, "CacheClearing : " + CacheE.ToString());
			}
		}

		/// <summary>
		/// Retrieve all cached items
		/// </summary>
		/// <returns>A hastable containing all cacheitems</returns>
		public static System.Collections.Hashtable ReturnCacheItemsOrdred() 
		{
			System.Collections.Hashtable ht = new System.Collections.Hashtable();
			System.Web.Caching.Cache c = System.Web.HttpRuntime.Cache;
			if (c != null) 
			{
				System.Collections.IDictionaryEnumerator cacheEnumerator = c.GetEnumerator();
				while ( cacheEnumerator.MoveNext() )
				{
					if (ht[c[cacheEnumerator.Key.ToString()].GetType().ToString()] == null)
						ht.Add(c[cacheEnumerator.Key.ToString()].GetType().ToString(), new System.Collections.ArrayList());

					((System.Collections.ArrayList) ht[c[cacheEnumerator.Key.ToString()].GetType().ToString()]).Add(cacheEnumerator.Key.ToString());
				}
			}
			return ht;
		}


		public delegate TT GetCacheItemDelegate<TT>();

		public static TT GetCacheItem<TT>(string cacheKey, object syncLock,
			TimeSpan timeout, GetCacheItemDelegate<TT> getCacheItem)
		{
			return GetCacheItem(cacheKey, syncLock, null, timeout, getCacheItem);
		}

		public static TT GetCacheItem<TT>(string cacheKey, object syncLock,
			CacheItemRemovedCallback refreshAction, TimeSpan timeout,
			GetCacheItemDelegate<TT> getCacheItem)
		{
			return GetCacheItem(cacheKey, syncLock, CacheItemPriority.Normal, refreshAction, timeout, getCacheItem);
		}

		public static TT GetCacheItem<TT>(string cacheKey, object syncLock,
			CacheItemPriority priority, CacheItemRemovedCallback refreshAction, TimeSpan timeout,
			GetCacheItemDelegate<TT> getCacheItem)
		{
			return GetCacheItem(cacheKey, syncLock, priority, refreshAction, null, timeout, getCacheItem);
		}

		public static TT GetCacheItem<TT>(string cacheKey, object syncLock,
			CacheItemPriority priority, CacheItemRemovedCallback refreshAction,
			CacheDependency cacheDependency, TimeSpan timeout, GetCacheItemDelegate<TT> getCacheItem)
		{
			object result = System.Web.HttpRuntime.Cache.Get(cacheKey);
			if (result == null)
			{
				lock (syncLock)
				{
					result = System.Web.HttpRuntime.Cache.Get(cacheKey);
					if (result == null)
					{
						result = getCacheItem();
						System.Web.HttpRuntime.Cache.Add(cacheKey, result, cacheDependency,
							DateTime.Now.Add(timeout), TimeSpan.Zero, priority, refreshAction);
					}
				}
			}
			return (TT)result;
		}
	}
}
