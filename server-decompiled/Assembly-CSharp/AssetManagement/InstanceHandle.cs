using System;
using UnityEngine;

namespace AssetManagement
{
	// Token: 0x02001B55 RID: 6997
	public struct InstanceHandle<T> where T : global::UnityEngine.Object
	{
		// Token: 0x0600D202 RID: 53762 RVA: 0x004C54DE File Offset: 0x004C36DE
		public InstanceHandle(T instance)
		{
			this.Uri = null;
			this.Instance = instance;
			this.manager = null;
		}

		// Token: 0x0600D203 RID: 53763 RVA: 0x004C54F5 File Offset: 0x004C36F5
		public InstanceHandle(AssetManager manager, string uri, T instance)
		{
			this.Uri = uri;
			this.Instance = instance;
			this.manager = manager;
		}

		// Token: 0x0600D204 RID: 53764 RVA: 0x004C550C File Offset: 0x004C370C
		public InstanceHandle<T> Clone()
		{
			T t = global::UnityEngine.Object.Instantiate<T>(this.Instance);
			if (this.manager == null)
			{
				return new InstanceHandle<T>(t);
			}
			return this.manager.TrackInstance<T>(this.Uri, t);
		}

		// Token: 0x0600D205 RID: 53765 RVA: 0x004C5546 File Offset: 0x004C3746
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

		// Token: 0x0600D206 RID: 53766 RVA: 0x004C5580 File Offset: 0x004C3780
		public void Release()
		{
			AssetManager assetManager = this.manager;
			if (assetManager == null)
			{
				return;
			}
			assetManager.Release(this.Uri);
		}

		// Token: 0x04009FB3 RID: 40883
		public readonly string Uri;

		// Token: 0x04009FB4 RID: 40884
		public readonly T Instance;

		// Token: 0x04009FB5 RID: 40885
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AssetManager manager;
	}
}
