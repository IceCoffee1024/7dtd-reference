using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.SceneManagement
{
	// Token: 0x0200031F RID: 799
	[NativeHeader("Runtime/Export/SceneManager/Scene.bindings.h")]
	[Serializable]
	public struct Scene
	{
		// Token: 0x0600204A RID: 8266
		[StaticAccessor("SceneBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsValidInternal(int sceneHandle);

		// Token: 0x0600204B RID: 8267
		[StaticAccessor("SceneBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetPathInternal(int sceneHandle);

		// Token: 0x0600204C RID: 8268
		[StaticAccessor("SceneBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPathAndGUIDInternal(int sceneHandle, string path, string guid);

		// Token: 0x0600204D RID: 8269
		[StaticAccessor("SceneBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetNameInternal(int sceneHandle);

		// Token: 0x0600204E RID: 8270
		[StaticAccessor("SceneBindings", StaticAccessorType.DoubleColon)]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetNameInternal(int sceneHandle, string name);

		// Token: 0x0600204F RID: 8271
		[StaticAccessor("SceneBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetGUIDInternal(int sceneHandle);

		// Token: 0x06002050 RID: 8272
		[StaticAccessor("SceneBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsSubScene(int sceneHandle);

		// Token: 0x06002051 RID: 8273
		[StaticAccessor("SceneBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetIsSubScene(int sceneHandle, bool value);

		// Token: 0x06002052 RID: 8274
		[StaticAccessor("SceneBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIsLoadedInternal(int sceneHandle);

		// Token: 0x06002053 RID: 8275
		[StaticAccessor("SceneBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Scene.LoadingState GetLoadingStateInternal(int sceneHandle);

		// Token: 0x06002054 RID: 8276
		[StaticAccessor("SceneBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIsDirtyInternal(int sceneHandle);

		// Token: 0x06002055 RID: 8277
		[StaticAccessor("SceneBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetDirtyID(int sceneHandle);

		// Token: 0x06002056 RID: 8278
		[StaticAccessor("SceneBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetBuildIndexInternal(int sceneHandle);

		// Token: 0x06002057 RID: 8279
		[StaticAccessor("SceneBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetRootCountInternal(int sceneHandle);

		// Token: 0x06002058 RID: 8280
		[StaticAccessor("SceneBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetRootGameObjectsInternal(int sceneHandle, object resultRootList);

		// Token: 0x06002059 RID: 8281 RVA: 0x00035AA1 File Offset: 0x00033CA1
		internal Scene(int handle)
		{
			this.m_Handle = handle;
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x0600205A RID: 8282 RVA: 0x00035AAC File Offset: 0x00033CAC
		public int handle
		{
			get
			{
				return this.m_Handle;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x0600205B RID: 8283 RVA: 0x00035AC4 File Offset: 0x00033CC4
		internal Scene.LoadingState loadingState
		{
			get
			{
				return Scene.GetLoadingStateInternal(this.handle);
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x0600205C RID: 8284 RVA: 0x00035AE4 File Offset: 0x00033CE4
		internal string guid
		{
			get
			{
				return Scene.GetGUIDInternal(this.handle);
			}
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x00035B04 File Offset: 0x00033D04
		public bool IsValid()
		{
			return Scene.IsValidInternal(this.handle);
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x0600205E RID: 8286 RVA: 0x00035B24 File Offset: 0x00033D24
		public string path
		{
			get
			{
				return Scene.GetPathInternal(this.handle);
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x0600205F RID: 8287 RVA: 0x00035B44 File Offset: 0x00033D44
		// (set) Token: 0x06002060 RID: 8288 RVA: 0x00035B61 File Offset: 0x00033D61
		public string name
		{
			get
			{
				return Scene.GetNameInternal(this.handle);
			}
			set
			{
				Scene.SetNameInternal(this.handle, value);
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06002061 RID: 8289 RVA: 0x00035B74 File Offset: 0x00033D74
		public bool isLoaded
		{
			get
			{
				return Scene.GetIsLoadedInternal(this.handle);
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06002062 RID: 8290 RVA: 0x00035B94 File Offset: 0x00033D94
		public int buildIndex
		{
			get
			{
				return Scene.GetBuildIndexInternal(this.handle);
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06002063 RID: 8291 RVA: 0x00035BB4 File Offset: 0x00033DB4
		public bool isDirty
		{
			get
			{
				return Scene.GetIsDirtyInternal(this.handle);
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06002064 RID: 8292 RVA: 0x00035BD4 File Offset: 0x00033DD4
		internal int dirtyID
		{
			get
			{
				return Scene.GetDirtyID(this.handle);
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06002065 RID: 8293 RVA: 0x00035BF4 File Offset: 0x00033DF4
		public int rootCount
		{
			get
			{
				return Scene.GetRootCountInternal(this.handle);
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06002066 RID: 8294 RVA: 0x00035C14 File Offset: 0x00033E14
		// (set) Token: 0x06002067 RID: 8295 RVA: 0x00035C31 File Offset: 0x00033E31
		public bool isSubScene
		{
			get
			{
				return Scene.IsSubScene(this.handle);
			}
			set
			{
				Scene.SetIsSubScene(this.handle, value);
			}
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x00035C44 File Offset: 0x00033E44
		public GameObject[] GetRootGameObjects()
		{
			List<GameObject> list = new List<GameObject>(this.rootCount);
			this.GetRootGameObjects(list);
			return list.ToArray();
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x00035C70 File Offset: 0x00033E70
		public void GetRootGameObjects(List<GameObject> rootGameObjects)
		{
			bool flag = rootGameObjects.Capacity < this.rootCount;
			if (flag)
			{
				rootGameObjects.Capacity = this.rootCount;
			}
			rootGameObjects.Clear();
			bool flag2 = !this.IsValid();
			if (flag2)
			{
				throw new ArgumentException("The scene is invalid.");
			}
			bool flag3 = !Application.isPlaying && !this.isLoaded;
			if (flag3)
			{
				throw new ArgumentException("The scene is not loaded.");
			}
			bool flag4 = this.rootCount == 0;
			if (!flag4)
			{
				Scene.GetRootGameObjectsInternal(this.handle, rootGameObjects);
			}
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x00035CFC File Offset: 0x00033EFC
		public static bool operator ==(Scene lhs, Scene rhs)
		{
			return lhs.handle == rhs.handle;
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x00035D20 File Offset: 0x00033F20
		public static bool operator !=(Scene lhs, Scene rhs)
		{
			return lhs.handle != rhs.handle;
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x00035D48 File Offset: 0x00033F48
		public override int GetHashCode()
		{
			return this.m_Handle;
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x00035D60 File Offset: 0x00033F60
		public override bool Equals(object other)
		{
			bool flag = !(other is Scene);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				Scene scene = (Scene)other;
				flag2 = this.handle == scene.handle;
			}
			return flag2;
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x00035D9C File Offset: 0x00033F9C
		internal void SetPathAndGuid(string path, string guid)
		{
			Scene.SetPathAndGUIDInternal(this.m_Handle, path, guid);
		}

		// Token: 0x04000ABA RID: 2746
		[HideInInspector]
		[SerializeField]
		private int m_Handle;

		// Token: 0x02000320 RID: 800
		internal enum LoadingState
		{
			// Token: 0x04000ABC RID: 2748
			NotLoaded,
			// Token: 0x04000ABD RID: 2749
			Loading,
			// Token: 0x04000ABE RID: 2750
			Loaded,
			// Token: 0x04000ABF RID: 2751
			Unloading
		}
	}
}
