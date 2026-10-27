using System;
using UnityEngine;

namespace AssetManagement
{
	// Token: 0x02001B86 RID: 7046
	public struct InstanceHandle<T> where T : global::UnityEngine.Object
	{
		// Token: 0x0600D316 RID: 54038 RVA: 0x004C9006 File Offset: 0x004C7206
		public InstanceHandle(T instance)
		{
			this.Uri = null;
			this.Instance = instance;
			this.manager = null;
		}

		// Token: 0x0600D317 RID: 54039 RVA: 0x004C901D File Offset: 0x004C721D
		public InstanceHandle(AssetManager manager, string uri, T instance)
		{
			this.Uri = uri;
			this.Instance = instance;
			this.manager = manager;
		}

		// Token: 0x0600D318 RID: 54040 RVA: 0x004C9034 File Offset: 0x004C7234
		public InstanceHandle<T> Clone()
		{
			T t = global::UnityEngine.Object.Instantiate<T>(this.Instance);
			if (this.manager == null)
			{
				return new InstanceHandle<T>(t);
			}
			return this.manager.TrackInstance<T>(this.Uri, t);
		}

		// Token: 0x0600D319 RID: 54041 RVA: 0x004C906E File Offset: 0x004C726E
		public void Destroy()
		{
			if (this.Instance)
			{
				global::UnityEngine.Object.Destroy(this.Instance);
			}
			AssetManager assetManager = this.manager;
			if (assetManager == null)
			{
				return;
			}
			assetManager.Release(this.Uri);
		}

		// Token: 0x0600D31A RID: 54042 RVA: 0x004C90A8 File Offset: 0x004C72A8
		public void Release()
		{
			AssetManager assetManager = this.manager;
			if (assetManager == null)
			{
				return;
			}
			assetManager.Release(this.Uri);
		}

		// Token: 0x0400A09C RID: 41116
		public readonly string Uri;

		// Token: 0x0400A09D RID: 41117
		public readonly T Instance;

		// Token: 0x0400A09E RID: 41118
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AssetManager manager;
	}
}
