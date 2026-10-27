using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssetManagement
{
	// Token: 0x02001B87 RID: 7047
	public class AssetManagerLifetimed : AssetManager
	{
		// Token: 0x17001A85 RID: 6789
		// (get) Token: 0x0600D31B RID: 54043 RVA: 0x004C90C0 File Offset: 0x004C72C0
		public override int ActiveAssetCount
		{
			get
			{
				return this.assets.Count;
			}
		}

		// Token: 0x0600D31C RID: 54044 RVA: 0x004C90D0 File Offset: 0x004C72D0
		public AssetManagerLifetimed(int softMax, double timeout, int hardMax = 2147483647)
		{
			if (softMax < 0 || timeout < 0.0 || hardMax < 0)
			{
				throw new ArgumentException(string.Format("Arguments for {0} must be non-negative. {1}: {2}, {3}: {4} {5}: {6}", new object[] { "AssetManagerLifetimed", "softMax", softMax, "timeout", timeout, "hardMax", hardMax }));
			}
			if (softMax > hardMax)
			{
				throw new ArgumentException(string.Format("Invalid asset count limits for {0} {1} must be less than or equal to {2}. {3}: {4}, {5}: {6}", new object[] { "AssetManagerLifetimed", "softMax", "hardMax", "softMax", softMax, "hardMax", hardMax }));
			}
			this.softMax = softMax;
			this.timeout = timeout;
			this.hardMax = hardMax;
		}

		// Token: 0x0600D31D RID: 54045 RVA: 0x004C91C8 File Offset: 0x004C73C8
		[PublicizedFrom(EAccessModifier.Internal)]
		public override AssetHandle<T> TrackAsset<T>(string uri, LoadManager.AssetRequestTask<T> loadTask)
		{
			LinkedListNode<AssetManagerLifetimed.Entry> linkedListNode;
			if (!this.uriToEntryMap.TryGetValue(uri, out linkedListNode))
			{
				AssetManagerLifetimed.Entry entry = new AssetManagerLifetimed.Entry(uri, loadTask.GetHandle());
				linkedListNode = this.assets.AddLast(entry);
				this.uriToEntryMap.Add(uri, linkedListNode);
			}
			else
			{
				AssetManagerLifetimed.Entry value = linkedListNode.Value;
				value.UsageCount++;
				linkedListNode.Value = value;
			}
			return new AssetHandle<T>(this, uri, loadTask);
		}

		// Token: 0x0600D31E RID: 54046 RVA: 0x004C9234 File Offset: 0x004C7434
		[PublicizedFrom(EAccessModifier.Internal)]
		public override InstanceHandle<T> TrackInstance<T>(string uri, T instance)
		{
			LinkedListNode<AssetManagerLifetimed.Entry> linkedListNode;
			if (!this.uriToEntryMap.TryGetValue(uri, out linkedListNode))
			{
				throw new Exception("Expected uri to be referenced when tracking an instance");
			}
			AssetManagerLifetimed.Entry value = linkedListNode.Value;
			value.UsageCount++;
			linkedListNode.Value = value;
			return new InstanceHandle<T>(this, uri, instance);
		}

		// Token: 0x0600D31F RID: 54047 RVA: 0x004C9280 File Offset: 0x004C7480
		[PublicizedFrom(EAccessModifier.Internal)]
		public override void Release(string uri)
		{
			LinkedListNode<AssetManagerLifetimed.Entry> linkedListNode;
			if (!this.uriToEntryMap.TryGetValue(uri, out linkedListNode))
			{
				return;
			}
			AssetManagerLifetimed.Entry value = linkedListNode.Value;
			value.UsageCount--;
			if (value.UsageCount == 0)
			{
				value.DereferencedTime = Time.timeSinceLevelLoadAsDouble;
				this.assets.Remove(linkedListNode);
				this.assets.AddFirst(linkedListNode);
			}
			linkedListNode.Value = value;
		}

		// Token: 0x0600D320 RID: 54048 RVA: 0x004C92E4 File Offset: 0x004C74E4
		public void Update()
		{
			if (this.assets.Count < this.softMax)
			{
				return;
			}
			while (this.assets.Count > this.hardMax)
			{
				AssetManagerLifetimed.Entry value = this.assets.First.Value;
				if (value.UsageCount != 0)
				{
					IL_00C2:
					while (this.assets.Count > this.softMax)
					{
						AssetManagerLifetimed.Entry value2 = this.assets.First.Value;
						if (value2.UsageCount != 0 || Time.timeSinceLevelLoadAsDouble - value2.DereferencedTime < this.timeout)
						{
							break;
						}
						this.uriToEntryMap.Remove(value2.Uri);
						value2.Handle.Release();
						this.assets.RemoveFirst();
					}
					return;
				}
				this.uriToEntryMap.Remove(value.Uri);
				value.Handle.Release();
				this.assets.RemoveFirst();
			}
			goto IL_00C2;
		}

		// Token: 0x0600D321 RID: 54049 RVA: 0x004C93C8 File Offset: 0x004C75C8
		public override void Cleanup()
		{
			foreach (AssetManagerLifetimed.Entry entry in this.assets)
			{
				entry.Handle.Release();
			}
			this.assets.Clear();
			this.uriToEntryMap.Clear();
		}

		// Token: 0x0400A09F RID: 41119
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int softMax;

		// Token: 0x0400A0A0 RID: 41120
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hardMax;

		// Token: 0x0400A0A1 RID: 41121
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly double timeout;

		// Token: 0x0400A0A2 RID: 41122
		[PublicizedFrom(EAccessModifier.Private)]
		public LinkedList<AssetManagerLifetimed.Entry> assets = new LinkedList<AssetManagerLifetimed.Entry>();

		// Token: 0x0400A0A3 RID: 41123
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, LinkedListNode<AssetManagerLifetimed.Entry>> uriToEntryMap = new Dictionary<string, LinkedListNode<AssetManagerLifetimed.Entry>>();

		// Token: 0x02001B88 RID: 7048
		[PublicizedFrom(EAccessModifier.Private)]
		public struct Entry
		{
			// Token: 0x0600D322 RID: 54050 RVA: 0x004C9434 File Offset: 0x004C7634
			public Entry(string uri, LoadManager.IAssetHandle handle)
			{
				this.Uri = uri;
				this.Handle = handle;
				this.UsageCount = 1;
				this.DereferencedTime = 0.0;
			}

			// Token: 0x0400A0A4 RID: 41124
			public readonly string Uri;

			// Token: 0x0400A0A5 RID: 41125
			public readonly LoadManager.IAssetHandle Handle;

			// Token: 0x0400A0A6 RID: 41126
			public int UsageCount;

			// Token: 0x0400A0A7 RID: 41127
			public double DereferencedTime;
		}
	}
}
