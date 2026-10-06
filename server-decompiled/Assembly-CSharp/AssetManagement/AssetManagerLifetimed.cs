using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssetManagement
{
	// Token: 0x02001B56 RID: 6998
	public class AssetManagerLifetimed : AssetManager
	{
		// Token: 0x17001A3D RID: 6717
		// (get) Token: 0x0600D207 RID: 53767 RVA: 0x004C5598 File Offset: 0x004C3798
		public override int ActiveAssetCount
		{
			get
			{
				return this.assets.Count;
			}
		}

		// Token: 0x0600D208 RID: 53768 RVA: 0x004C55A8 File Offset: 0x004C37A8
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

		// Token: 0x0600D209 RID: 53769 RVA: 0x004C56A0 File Offset: 0x004C38A0
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

		// Token: 0x0600D20A RID: 53770 RVA: 0x004C570C File Offset: 0x004C390C
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

		// Token: 0x0600D20B RID: 53771 RVA: 0x004C5758 File Offset: 0x004C3958
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

		// Token: 0x0600D20C RID: 53772 RVA: 0x004C57BC File Offset: 0x004C39BC
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

		// Token: 0x0600D20D RID: 53773 RVA: 0x004C58A0 File Offset: 0x004C3AA0
		public override void Cleanup()
		{
			foreach (AssetManagerLifetimed.Entry entry in this.assets)
			{
				entry.Handle.Release();
			}
			this.assets.Clear();
			this.uriToEntryMap.Clear();
		}

		// Token: 0x04009FB6 RID: 40886
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int softMax;

		// Token: 0x04009FB7 RID: 40887
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hardMax;

		// Token: 0x04009FB8 RID: 40888
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly double timeout;

		// Token: 0x04009FB9 RID: 40889
		[PublicizedFrom(EAccessModifier.Private)]
		public LinkedList<AssetManagerLifetimed.Entry> assets = new LinkedList<AssetManagerLifetimed.Entry>();

		// Token: 0x04009FBA RID: 40890
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, LinkedListNode<AssetManagerLifetimed.Entry>> uriToEntryMap = new Dictionary<string, LinkedListNode<AssetManagerLifetimed.Entry>>();

		// Token: 0x02001B57 RID: 6999
		[PublicizedFrom(EAccessModifier.Private)]
		public struct Entry
		{
			// Token: 0x0600D20E RID: 53774 RVA: 0x004C590C File Offset: 0x004C3B0C
			public Entry(string uri, LoadManager.IAssetHandle handle)
			{
				this.Uri = uri;
				this.Handle = handle;
				this.UsageCount = 1;
				this.DereferencedTime = 0.0;
			}

			// Token: 0x04009FBB RID: 40891
			public readonly string Uri;

			// Token: 0x04009FBC RID: 40892
			public readonly LoadManager.IAssetHandle Handle;

			// Token: 0x04009FBD RID: 40893
			public int UsageCount;

			// Token: 0x04009FBE RID: 40894
			public double DereferencedTime;
		}
	}
}
