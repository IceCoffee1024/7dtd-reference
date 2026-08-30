using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x020000FF RID: 255
	[NativeHeader("Runtime/Misc/Cache.h")]
	[StaticAccessor("CacheWrapper", StaticAccessorType.DoubleColon)]
	public struct Cache : IEquatable<Cache>
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00007F80 File Offset: 0x00006180
		internal int handle
		{
			get
			{
				return this.m_Handle;
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00007F98 File Offset: 0x00006198
		public static bool operator ==(Cache lhs, Cache rhs)
		{
			return lhs.handle == rhs.handle;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00007FBC File Offset: 0x000061BC
		public static bool operator !=(Cache lhs, Cache rhs)
		{
			return lhs.handle != rhs.handle;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00007FE4 File Offset: 0x000061E4
		public override int GetHashCode()
		{
			return this.m_Handle;
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00007FFC File Offset: 0x000061FC
		public override bool Equals(object other)
		{
			return other is Cache && this.Equals((Cache)other);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00008028 File Offset: 0x00006228
		public bool Equals(Cache other)
		{
			return this.handle == other.handle;
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x0000804C File Offset: 0x0000624C
		public bool valid
		{
			get
			{
				return Cache.Cache_IsValid(this.m_Handle);
			}
		}

		// Token: 0x060004E3 RID: 1251
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool Cache_IsValid(int handle);

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x0000806C File Offset: 0x0000626C
		public bool ready
		{
			get
			{
				return Cache.Cache_IsReady(this.m_Handle);
			}
		}

		// Token: 0x060004E5 RID: 1253
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool Cache_IsReady(int handle);

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x0000808C File Offset: 0x0000628C
		public bool readOnly
		{
			get
			{
				return Cache.Cache_IsReadonly(this.m_Handle);
			}
		}

		// Token: 0x060004E7 RID: 1255
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool Cache_IsReadonly(int handle);

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x000080AC File Offset: 0x000062AC
		public string path
		{
			get
			{
				return Cache.Cache_GetPath(this.m_Handle);
			}
		}

		// Token: 0x060004E9 RID: 1257
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string Cache_GetPath(int handle);

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x000080CC File Offset: 0x000062CC
		public int index
		{
			get
			{
				return Cache.Cache_GetIndex(this.m_Handle);
			}
		}

		// Token: 0x060004EB RID: 1259
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int Cache_GetIndex(int handle);

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x000080EC File Offset: 0x000062EC
		public long spaceFree
		{
			get
			{
				return Cache.Cache_GetSpaceFree(this.m_Handle);
			}
		}

		// Token: 0x060004ED RID: 1261
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern long Cache_GetSpaceFree(int handle);

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x0000810C File Offset: 0x0000630C
		// (set) Token: 0x060004EF RID: 1263 RVA: 0x00008129 File Offset: 0x00006329
		public long maximumAvailableStorageSpace
		{
			get
			{
				return Cache.Cache_GetMaximumDiskSpaceAvailable(this.m_Handle);
			}
			set
			{
				Cache.Cache_SetMaximumDiskSpaceAvailable(this.m_Handle, value);
			}
		}

		// Token: 0x060004F0 RID: 1264
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern long Cache_GetMaximumDiskSpaceAvailable(int handle);

		// Token: 0x060004F1 RID: 1265
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Cache_SetMaximumDiskSpaceAvailable(int handle, long value);

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x0000813C File Offset: 0x0000633C
		public long spaceOccupied
		{
			get
			{
				return Cache.Cache_GetCachingDiskSpaceUsed(this.m_Handle);
			}
		}

		// Token: 0x060004F3 RID: 1267
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern long Cache_GetCachingDiskSpaceUsed(int handle);

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x0000815C File Offset: 0x0000635C
		// (set) Token: 0x060004F5 RID: 1269 RVA: 0x00008179 File Offset: 0x00006379
		public int expirationDelay
		{
			get
			{
				return Cache.Cache_GetExpirationDelay(this.m_Handle);
			}
			set
			{
				Cache.Cache_SetExpirationDelay(this.m_Handle, value);
			}
		}

		// Token: 0x060004F6 RID: 1270
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int Cache_GetExpirationDelay(int handle);

		// Token: 0x060004F7 RID: 1271
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Cache_SetExpirationDelay(int handle, int value);

		// Token: 0x060004F8 RID: 1272 RVA: 0x0000818C File Offset: 0x0000638C
		public bool ClearCache()
		{
			return Cache.Cache_ClearCache(this.m_Handle);
		}

		// Token: 0x060004F9 RID: 1273
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool Cache_ClearCache(int handle);

		// Token: 0x060004FA RID: 1274 RVA: 0x000081AC File Offset: 0x000063AC
		public bool ClearCache(int expiration)
		{
			return Cache.Cache_ClearCache_Expiration(this.m_Handle, expiration);
		}

		// Token: 0x060004FB RID: 1275
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool Cache_ClearCache_Expiration(int handle, int expiration);

		// Token: 0x04000346 RID: 838
		private int m_Handle;
	}
}
