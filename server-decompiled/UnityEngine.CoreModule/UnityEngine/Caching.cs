using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000101 RID: 257
	[StaticAccessor("GetCachingManager()", StaticAccessorType.Dot)]
	[NativeHeader("Runtime/Misc/CachingManager.h")]
	public sealed class Caching
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060004FC RID: 1276
		// (set) Token: 0x060004FD RID: 1277
		public static extern bool compressionEnabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060004FE RID: 1278
		public static extern bool ready
		{
			[NativeName("GetIsReady")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x060004FF RID: 1279
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool ClearCache();

		// Token: 0x06000500 RID: 1280 RVA: 0x000081CC File Offset: 0x000063CC
		public static bool ClearCache(int expiration)
		{
			return Caching.ClearCache_Int(expiration);
		}

		// Token: 0x06000501 RID: 1281
		[NativeName("ClearCache")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool ClearCache_Int(int expiration);

		// Token: 0x06000502 RID: 1282 RVA: 0x000081E4 File Offset: 0x000063E4
		public static bool ClearCachedVersion(string assetBundleName, Hash128 hash)
		{
			bool flag = string.IsNullOrEmpty(assetBundleName);
			if (flag)
			{
				throw new ArgumentException("Input AssetBundle name cannot be null or empty.");
			}
			return Caching.ClearCachedVersionInternal(assetBundleName, hash);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00008212 File Offset: 0x00006412
		[NativeName("ClearCachedVersion")]
		internal static bool ClearCachedVersionInternal(string assetBundleName, Hash128 hash)
		{
			return Caching.ClearCachedVersionInternal_Injected(assetBundleName, ref hash);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0000821C File Offset: 0x0000641C
		public static bool ClearOtherCachedVersions(string assetBundleName, Hash128 hash)
		{
			bool flag = string.IsNullOrEmpty(assetBundleName);
			if (flag)
			{
				throw new ArgumentException("Input AssetBundle name cannot be null or empty.");
			}
			return Caching.ClearCachedVersions(assetBundleName, hash, true);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0000824C File Offset: 0x0000644C
		public static bool ClearAllCachedVersions(string assetBundleName)
		{
			bool flag = string.IsNullOrEmpty(assetBundleName);
			if (flag)
			{
				throw new ArgumentException("Input AssetBundle name cannot be null or empty.");
			}
			return Caching.ClearCachedVersions(assetBundleName, default(Hash128), false);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00008283 File Offset: 0x00006483
		internal static bool ClearCachedVersions(string assetBundleName, Hash128 hash, bool keepInputVersion)
		{
			return Caching.ClearCachedVersions_Injected(assetBundleName, ref hash, keepInputVersion);
		}

		// Token: 0x06000507 RID: 1287
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Hash128[] GetCachedVersions(string assetBundleName);

		// Token: 0x06000508 RID: 1288 RVA: 0x00008290 File Offset: 0x00006490
		public static void GetCachedVersions(string assetBundleName, List<Hash128> outCachedVersions)
		{
			bool flag = string.IsNullOrEmpty(assetBundleName);
			if (flag)
			{
				throw new ArgumentException("Input AssetBundle name cannot be null or empty.");
			}
			bool flag2 = outCachedVersions == null;
			if (flag2)
			{
				throw new ArgumentNullException("Input outCachedVersions cannot be null.");
			}
			outCachedVersions.AddRange(Caching.GetCachedVersions(assetBundleName));
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x000082D4 File Offset: 0x000064D4
		[Obsolete("Please use IsVersionCached with Hash128 instead.")]
		public static bool IsVersionCached(string url, int version)
		{
			return Caching.IsVersionCached(url, new Hash128(0U, 0U, 0U, (uint)version));
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x000082F8 File Offset: 0x000064F8
		public static bool IsVersionCached(string url, Hash128 hash)
		{
			bool flag = string.IsNullOrEmpty(url);
			if (flag)
			{
				throw new ArgumentException("Input AssetBundle url cannot be null or empty.");
			}
			return Caching.IsVersionCached(url, "", hash);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0000832C File Offset: 0x0000652C
		public static bool IsVersionCached(CachedAssetBundle cachedBundle)
		{
			bool flag = string.IsNullOrEmpty(cachedBundle.name);
			if (flag)
			{
				throw new ArgumentException("Input AssetBundle name cannot be null or empty.");
			}
			return Caching.IsVersionCached("", cachedBundle.name, cachedBundle.hash);
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00008371 File Offset: 0x00006571
		[NativeName("IsCached")]
		internal static bool IsVersionCached(string url, string assetBundleName, Hash128 hash)
		{
			return Caching.IsVersionCached_Injected(url, assetBundleName, ref hash);
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0000837C File Offset: 0x0000657C
		[Obsolete("Please use MarkAsUsed with Hash128 instead.")]
		public static bool MarkAsUsed(string url, int version)
		{
			return Caching.MarkAsUsed(url, new Hash128(0U, 0U, 0U, (uint)version));
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000083A0 File Offset: 0x000065A0
		public static bool MarkAsUsed(string url, Hash128 hash)
		{
			bool flag = string.IsNullOrEmpty(url);
			if (flag)
			{
				throw new ArgumentException("Input AssetBundle url cannot be null or empty.");
			}
			return Caching.MarkAsUsed(url, "", hash);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x000083D4 File Offset: 0x000065D4
		public static bool MarkAsUsed(CachedAssetBundle cachedBundle)
		{
			bool flag = string.IsNullOrEmpty(cachedBundle.name);
			if (flag)
			{
				throw new ArgumentException("Input AssetBundle name cannot be null or empty.");
			}
			return Caching.MarkAsUsed("", cachedBundle.name, cachedBundle.hash);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00008419 File Offset: 0x00006619
		internal static bool MarkAsUsed(string url, string assetBundleName, Hash128 hash)
		{
			return Caching.MarkAsUsed_Injected(url, assetBundleName, ref hash);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00008424 File Offset: 0x00006624
		[Obsolete("This function is obsolete and will always return -1. Use IsVersionCached instead.")]
		public static int GetVersionFromCache(string url)
		{
			return -1;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x00008438 File Offset: 0x00006638
		[Obsolete("Please use use Cache.spaceOccupied to get used bytes per cache.")]
		public static int spaceUsed
		{
			get
			{
				return (int)Caching.spaceOccupied;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000513 RID: 1299
		[Obsolete("This property is only used for the current cache, use Cache.spaceOccupied to get used bytes per cache.")]
		public static extern long spaceOccupied
		{
			[NativeName("GetCachingDiskSpaceUsed")]
			[StaticAccessor("GetCachingManager().GetCurrentCache()", StaticAccessorType.Dot)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x00008450 File Offset: 0x00006650
		[Obsolete("Please use use Cache.spaceOccupied to get used bytes per cache.")]
		public static int spaceAvailable
		{
			get
			{
				return (int)Caching.spaceFree;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000515 RID: 1301
		[Obsolete("This property is only used for the current cache, use Cache.spaceFree to get unused bytes per cache.")]
		public static extern long spaceFree
		{
			[NativeName("GetCachingDiskSpaceFree")]
			[StaticAccessor("GetCachingManager().GetCurrentCache()", StaticAccessorType.Dot)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000516 RID: 1302
		// (set) Token: 0x06000517 RID: 1303
		[Obsolete("This property is only used for the current cache, use Cache.maximumAvailableStorageSpace to access the maximum available storage space per cache.")]
		[StaticAccessor("GetCachingManager().GetCurrentCache()", StaticAccessorType.Dot)]
		public static extern long maximumAvailableDiskSpace
		{
			[NativeName("GetMaximumDiskSpaceAvailable")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeName("SetMaximumDiskSpaceAvailable")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000518 RID: 1304
		// (set) Token: 0x06000519 RID: 1305
		[StaticAccessor("GetCachingManager().GetCurrentCache()", StaticAccessorType.Dot)]
		[Obsolete("This property is only used for the current cache, use Cache.expirationDelay to access the expiration delay per cache.")]
		public static extern int expirationDelay
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00008468 File Offset: 0x00006668
		public static Cache AddCache(string cachePath)
		{
			bool flag = string.IsNullOrEmpty(cachePath);
			if (flag)
			{
				throw new ArgumentNullException("Cache path cannot be null or empty.");
			}
			bool flag2 = false;
			bool flag3 = cachePath.Replace('\\', '/').StartsWith(Application.streamingAssetsPath);
			if (flag3)
			{
				flag2 = true;
			}
			else
			{
				bool flag4 = !Directory.Exists(cachePath);
				if (flag4)
				{
					throw new ArgumentException("Cache path '" + cachePath + "' doesn't exist.");
				}
				bool flag5 = (File.GetAttributes(cachePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
				if (flag5)
				{
					flag2 = true;
				}
			}
			bool valid = Caching.GetCacheByPath(cachePath).valid;
			if (valid)
			{
				throw new InvalidOperationException("Cache with path '" + cachePath + "' has already been added.");
			}
			return Caching.AddCache(cachePath, flag2);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0000851C File Offset: 0x0000671C
		[NativeName("AddCachePath")]
		internal static Cache AddCache(string cachePath, bool isReadonly)
		{
			Cache cache;
			Caching.AddCache_Injected(cachePath, isReadonly, out cache);
			return cache;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00008534 File Offset: 0x00006734
		[StaticAccessor("CachingManagerWrapper", StaticAccessorType.DoubleColon)]
		[NativeThrows]
		[NativeName("Caching_GetCacheHandleAt")]
		public static Cache GetCacheAt(int cacheIndex)
		{
			Cache cache;
			Caching.GetCacheAt_Injected(cacheIndex, out cache);
			return cache;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0000854C File Offset: 0x0000674C
		[NativeThrows]
		[StaticAccessor("CachingManagerWrapper", StaticAccessorType.DoubleColon)]
		[NativeName("Caching_GetCacheHandleByPath")]
		public static Cache GetCacheByPath(string cachePath)
		{
			Cache cache;
			Caching.GetCacheByPath_Injected(cachePath, out cache);
			return cache;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00008564 File Offset: 0x00006764
		public static void GetAllCachePaths(List<string> cachePaths)
		{
			cachePaths.Clear();
			for (int i = 0; i < Caching.cacheCount; i++)
			{
				cachePaths.Add(Caching.GetCacheAt(i).path);
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x000085A4 File Offset: 0x000067A4
		[StaticAccessor("CachingManagerWrapper", StaticAccessorType.DoubleColon)]
		[NativeThrows]
		[NativeName("Caching_RemoveCacheByHandle")]
		public static bool RemoveCache(Cache cache)
		{
			return Caching.RemoveCache_Injected(ref cache);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x000085AD File Offset: 0x000067AD
		[StaticAccessor("CachingManagerWrapper", StaticAccessorType.DoubleColon)]
		[NativeName("Caching_MoveCacheBeforeByHandle")]
		[NativeThrows]
		public static void MoveCacheBefore(Cache src, Cache dst)
		{
			Caching.MoveCacheBefore_Injected(ref src, ref dst);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x000085B8 File Offset: 0x000067B8
		[NativeName("Caching_MoveCacheAfterByHandle")]
		[NativeThrows]
		[StaticAccessor("CachingManagerWrapper", StaticAccessorType.DoubleColon)]
		public static void MoveCacheAfter(Cache src, Cache dst)
		{
			Caching.MoveCacheAfter_Injected(ref src, ref dst);
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000522 RID: 1314
		public static extern int cacheCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x000085C4 File Offset: 0x000067C4
		[StaticAccessor("CachingManagerWrapper", StaticAccessorType.DoubleColon)]
		public static Cache defaultCache
		{
			[NativeName("Caching_GetDefaultCacheHandle")]
			get
			{
				Cache cache;
				Caching.get_defaultCache_Injected(out cache);
				return cache;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x000085DC File Offset: 0x000067DC
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x000085F1 File Offset: 0x000067F1
		[StaticAccessor("CachingManagerWrapper", StaticAccessorType.DoubleColon)]
		public static Cache currentCacheForWriting
		{
			[NativeName("Caching_GetCurrentCacheHandle")]
			get
			{
				Cache cache;
				Caching.get_currentCacheForWriting_Injected(out cache);
				return cache;
			}
			[NativeThrows]
			[NativeName("Caching_SetCurrentCacheByHandle")]
			set
			{
				Caching.set_currentCacheForWriting_Injected(ref value);
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000085FC File Offset: 0x000067FC
		[Obsolete("This function is obsolete. Please use ClearCache.  (UnityUpgradable) -> ClearCache()")]
		public static bool CleanCache()
		{
			return Caching.ClearCache();
		}

		// Token: 0x06000528 RID: 1320
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ClearCachedVersionInternal_Injected(string assetBundleName, ref Hash128 hash);

		// Token: 0x06000529 RID: 1321
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ClearCachedVersions_Injected(string assetBundleName, ref Hash128 hash, bool keepInputVersion);

		// Token: 0x0600052A RID: 1322
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsVersionCached_Injected(string url, string assetBundleName, ref Hash128 hash);

		// Token: 0x0600052B RID: 1323
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool MarkAsUsed_Injected(string url, string assetBundleName, ref Hash128 hash);

		// Token: 0x0600052C RID: 1324
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddCache_Injected(string cachePath, bool isReadonly, out Cache ret);

		// Token: 0x0600052D RID: 1325
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetCacheAt_Injected(int cacheIndex, out Cache ret);

		// Token: 0x0600052E RID: 1326
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetCacheByPath_Injected(string cachePath, out Cache ret);

		// Token: 0x0600052F RID: 1327
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool RemoveCache_Injected(ref Cache cache);

		// Token: 0x06000530 RID: 1328
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void MoveCacheBefore_Injected(ref Cache src, ref Cache dst);

		// Token: 0x06000531 RID: 1329
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void MoveCacheAfter_Injected(ref Cache src, ref Cache dst);

		// Token: 0x06000532 RID: 1330
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_defaultCache_Injected(out Cache ret);

		// Token: 0x06000533 RID: 1331
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_currentCacheForWriting_Injected(out Cache ret);

		// Token: 0x06000534 RID: 1332
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_currentCacheForWriting_Injected(ref Cache value);
	}
}
