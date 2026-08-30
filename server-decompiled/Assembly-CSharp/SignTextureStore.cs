using System;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000AB1 RID: 2737
public sealed class SignTextureStore
{
	// Token: 0x060051FE RID: 20990 RVA: 0x001F522C File Offset: 0x001F342C
	public void RebuildPools(IReadOnlyList<SignTextureManager.TierSpec> tiers, Material defaultMat)
	{
		using (SignTextureStore.s_SignTextureManagerRebuildPools.Auto())
		{
			this.Clear();
			this._pools.Clear();
			CommandBuffer commandBuffer = new CommandBuffer
			{
				name = "SignTech_CreateRT"
			};
			long num = 0L;
			for (int i = 0; i < tiers.Count; i++)
			{
				SignTextureManager.TierSpec tierSpec = tiers[i];
				SignTextureStore.Pool pool = new SignTextureStore.Pool();
				pool.Resolution = tierSpec.Resolution;
				this._pools.Add(pool);
				int totalCount = tierSpec.TotalCount;
				for (int j = 0; j < totalCount; j++)
				{
					RenderTexture renderTexture = SignTextureStore.CreateRT(tierSpec.Resolution, tierSpec.Resolution, "SignTech_Pool", commandBuffer, defaultMat);
					this.EnqueueFreeRT(i, renderTexture);
					num += (long)(tierSpec.Resolution * tierSpec.Resolution * 4);
				}
			}
			commandBuffer.Release();
			SignTextureStore.s_TotalTextureMemory.Value = num;
		}
	}

	// Token: 0x060051FF RID: 20991 RVA: 0x001F533C File Offset: 0x001F353C
	public void BeginFrameMarkAllNotInUse()
	{
		using (SignTextureStore.s_SignTextureManagerMarkUnused.Auto())
		{
			foreach (KeyValuePair<GlobalSignId, SignTextureStore.Entry> keyValuePair in this._entries)
			{
				keyValuePair.Value.InUse.Clear();
			}
		}
	}

	// Token: 0x06005200 RID: 20992 RVA: 0x001F53C4 File Offset: 0x001F35C4
	public bool Has(GlobalSignId signId, int tier)
	{
		bool flag;
		using (SignTextureStore.s_SignTextureManagerHas.Auto())
		{
			SignTextureStore.Entry entry;
			if (this._entries.TryGetValue(signId, out entry))
			{
				flag = entry.TierToRT.ContainsKey(tier);
			}
			else
			{
				flag = false;
			}
		}
		return flag;
	}

	// Token: 0x06005201 RID: 20993 RVA: 0x001F5424 File Offset: 0x001F3624
	public void MarkInUse(GlobalSignId signId, int tier)
	{
		using (SignTextureStore.s_SignTextureManagerMarkUsed.Auto())
		{
			SignTextureStore.Entry entry;
			if (this._entries.TryGetValue(signId, out entry))
			{
				entry.InUse.Add(tier);
				RenderTexture renderTexture;
				if (entry.TierToRT.TryGetValue(tier, out renderTexture) && !(renderTexture == null))
				{
					if (tier < this._pools.Count)
					{
						this._pools[tier].TryRemove(renderTexture);
					}
				}
			}
		}
	}

	// Token: 0x06005202 RID: 20994 RVA: 0x001F54B8 File Offset: 0x001F36B8
	public RenderTexture Get(GlobalSignId signId, int tier)
	{
		RenderTexture renderTexture2;
		using (SignTextureStore.s_SignTextureManagerGet.Auto())
		{
			SignTextureStore.Entry entry;
			RenderTexture renderTexture;
			if (this._entries.TryGetValue(signId, out entry) && entry.TierToRT.TryGetValue(tier, out renderTexture))
			{
				renderTexture2 = renderTexture;
			}
			else
			{
				renderTexture2 = null;
			}
		}
		return renderTexture2;
	}

	// Token: 0x06005203 RID: 20995 RVA: 0x001F5520 File Offset: 0x001F3720
	public RenderTexture GetBestUpToTier(GlobalSignId signId, int maxTier)
	{
		RenderTexture renderTexture;
		using (SignTextureStore.s_SignTextureManagerGetBestUpToTier.Auto())
		{
			SignTextureStore.Entry entry;
			if (!this._entries.TryGetValue(signId, out entry))
			{
				renderTexture = null;
			}
			else
			{
				int num = -1;
				RenderTexture renderTexture2 = null;
				foreach (KeyValuePair<int, RenderTexture> keyValuePair in entry.TierToRT)
				{
					int key = keyValuePair.Key;
					if (key <= maxTier && key > num)
					{
						num = key;
						renderTexture2 = keyValuePair.Value;
					}
				}
				renderTexture = renderTexture2;
			}
		}
		return renderTexture;
	}

	// Token: 0x06005204 RID: 20996 RVA: 0x001F55D8 File Offset: 0x001F37D8
	public RenderTexture AcquireForBake(int tier)
	{
		RenderTexture renderTexture;
		using (SignTextureStore.s_SignTextureManagerAcquireForBake.Auto())
		{
			RenderTexture renderTexture2;
			if (tier >= this._pools.Count)
			{
				renderTexture = null;
			}
			else if (!this._pools[tier].TryUnpool(out renderTexture2))
			{
				renderTexture = null;
			}
			else
			{
				SignTextureStore.RtOwner rtOwner;
				if (this._ownerByRT.TryGetValue(renderTexture2, out rtOwner))
				{
					SignTextureStore.Entry entry;
					RenderTexture renderTexture3;
					if (this._entries.TryGetValue(rtOwner.SignId, out entry) && entry.TierToRT.TryGetValue(rtOwner.Tier, out renderTexture3) && renderTexture3 == renderTexture2)
					{
						entry.TierToRT.Remove(rtOwner.Tier);
					}
					this._ownerByRT.Remove(renderTexture2);
				}
				renderTexture = renderTexture2;
			}
		}
		return renderTexture;
	}

	// Token: 0x06005205 RID: 20997 RVA: 0x001F56A8 File Offset: 0x001F38A8
	public void SetBaked(GlobalSignId signId, int tier, RenderTexture rt)
	{
		using (SignTextureStore.s_SignTextureManagerSetBaked.Auto())
		{
			if (!(rt == null))
			{
				SignTextureStore.Entry entry;
				if (!this._entries.TryGetValue(signId, out entry))
				{
					entry = new SignTextureStore.Entry();
					this._entries.Add(signId, entry);
				}
				entry.TierToRT[tier] = rt;
				entry.InUse.Add(tier);
				this._ownerByRT[rt] = new SignTextureStore.RtOwner(signId, tier);
			}
		}
	}

	// Token: 0x06005206 RID: 20998 RVA: 0x001F5740 File Offset: 0x001F3940
	public void InvalidateSign(GlobalSignId signId)
	{
		using (SignTextureStore.s_SignTextureManagerInvalidateSign.Auto())
		{
			SignTextureStore.Entry entry;
			if (this._entries.TryGetValue(signId, out entry))
			{
				foreach (KeyValuePair<int, RenderTexture> keyValuePair in entry.TierToRT)
				{
					int key = keyValuePair.Key;
					RenderTexture value = keyValuePair.Value;
					if (!(value == null))
					{
						if (key < this._pools.Count)
						{
							this._pools[key].TryRemove(value);
						}
						this._ownerByRT.Remove(value);
						this.EnqueueFreeRT(key, value);
					}
				}
				this._entries.Remove(signId);
			}
		}
	}

	// Token: 0x06005207 RID: 20999 RVA: 0x001F5834 File Offset: 0x001F3A34
	public void EvictUnused()
	{
		using (SignTextureStore.s_SignTextureManagerEvictUnused.Auto())
		{
			foreach (KeyValuePair<GlobalSignId, SignTextureStore.Entry> keyValuePair in this._entries)
			{
				SignTextureStore.Entry value = keyValuePair.Value;
				foreach (KeyValuePair<int, RenderTexture> keyValuePair2 in value.TierToRT)
				{
					int key = keyValuePair2.Key;
					RenderTexture value2 = keyValuePair2.Value;
					if (value2 != null && !value.InUse.Contains(key))
					{
						this.EnqueueFreeRT(key, value2);
					}
				}
			}
		}
	}

	// Token: 0x06005208 RID: 21000 RVA: 0x001F5928 File Offset: 0x001F3B28
	public void Clear()
	{
		using (SignTextureStore.s_SignTextureManagerClear.Auto())
		{
			foreach (KeyValuePair<GlobalSignId, SignTextureStore.Entry> keyValuePair in this._entries)
			{
				foreach (KeyValuePair<int, RenderTexture> keyValuePair2 in keyValuePair.Value.TierToRT)
				{
					this.ReleaseToPool(keyValuePair2.Key, keyValuePair2.Value);
				}
			}
			this._entries.Clear();
			this._ownerByRT.Clear();
			for (int i = 0; i < this._pools.Count; i++)
			{
				this._pools[i].DestroyTexturesAndClear();
			}
			this._pools.Clear();
			this._tiersToRemove.Clear();
			this._signsToRemove.Clear();
			this._bestTierScratch.Clear();
			SignTextureStore.s_TotalTextureMemory.Value = 0L;
		}
	}

	// Token: 0x06005209 RID: 21001 RVA: 0x001F5A70 File Offset: 0x001F3C70
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReleaseToPool(int tier, RenderTexture rt)
	{
		using (SignTextureStore.s_SignTextureManagerReleaseToPool.Auto())
		{
			if (!(rt == null))
			{
				if (tier >= this._pools.Count)
				{
					rt.Release();
					global::UnityEngine.Object.DestroyImmediate(rt);
				}
				else
				{
					this.EnqueueFreeRT(tier, rt);
				}
			}
		}
	}

	// Token: 0x0600520A RID: 21002 RVA: 0x001F5ADC File Offset: 0x001F3CDC
	[PublicizedFrom(EAccessModifier.Private)]
	public void EnqueueFreeRT(int tier, RenderTexture rt)
	{
		using (SignTextureStore.s_SignTextureManagerEnqueueFreeRT.Auto())
		{
			if (!(rt == null))
			{
				if (tier >= this._pools.Count)
				{
					rt.Release();
					global::UnityEngine.Object.DestroyImmediate(rt);
				}
				else
				{
					this._pools[tier].Add(rt);
				}
			}
		}
	}

	// Token: 0x0600520B RID: 21003 RVA: 0x001F5B50 File Offset: 0x001F3D50
	[PublicizedFrom(EAccessModifier.Private)]
	public static RenderTexture CreateRT(int w, int h, string name, CommandBuffer cmd, Material mat)
	{
		RenderTexture renderTexture2;
		using (SignTextureStore.s_SignTextureManagerCreateRT.Auto())
		{
			RenderTexture renderTexture = new RenderTexture(w, h, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB)
			{
				name = name,
				useMipMap = false,
				autoGenerateMips = false,
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear,
				antiAliasing = 1
			};
			renderTexture.Create();
			RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(renderTexture);
			cmd.Clear();
			cmd.SetRenderTarget(renderTargetIdentifier);
			cmd.ClearRenderTarget(true, true, Color.clear);
			cmd.Blit(BuiltinRenderTextureType.None, renderTargetIdentifier, mat, 0);
			Graphics.ExecuteCommandBuffer(cmd);
			renderTexture2 = renderTexture;
		}
		return renderTexture2;
	}

	// Token: 0x0600520C RID: 21004 RVA: 0x001F5C04 File Offset: 0x001F3E04
	public void WriteTextureStoreInfo(StringBuilder sb)
	{
		sb.AppendLine("**************************************************");
		sb.AppendLine(string.Format("TextureStore: Pools ({0})", this._pools.Count));
		sb.AppendLine("**************************************************");
		if (this._pools.Count > 0)
		{
			for (int i = 0; i < this._pools.Count; i++)
			{
				sb.AppendLine();
				sb.AppendLine(string.Format("Pool Tier {0}", i));
				SignTextureStore.Pool pool = this._pools[i];
				if (pool == null)
				{
					sb.AppendLine("\tNULL");
				}
				else
				{
					sb.AppendLine(string.Format("\tResolution: {0}", pool.Resolution));
					sb.AppendLine(string.Format("\tFree Count: {0}", pool.Count));
				}
			}
		}
		else
		{
			sb.AppendLine();
			sb.AppendLine("\tNONE");
		}
		sb.AppendLine();
		sb.AppendLine("**************************************************");
		sb.AppendLine(string.Format("TextureStore: Entries ({0})", this._entries.Count));
		sb.AppendLine("**************************************************");
		Dictionary<int, List<GlobalSignId>> dictionary = new Dictionary<int, List<GlobalSignId>>();
		Dictionary<int, List<GlobalSignId>> dictionary2 = new Dictionary<int, List<GlobalSignId>>();
		foreach (KeyValuePair<GlobalSignId, SignTextureStore.Entry> keyValuePair in this._entries)
		{
			SignTextureStore.Entry value = keyValuePair.Value;
			sb.AppendLine();
			sb.AppendLine(string.Format("{0}", keyValuePair.Key));
			bool flag = false;
			for (int j = 0; j < this._pools.Count; j++)
			{
				if (value.TierToRT.ContainsKey(j))
				{
					flag = true;
					if (value.InUse.Contains(j))
					{
						sb.AppendLine(string.Format("\t{0} (in use)", j));
						List<GlobalSignId> list;
						if (!dictionary.TryGetValue(j, out list))
						{
							list = new List<GlobalSignId>();
							dictionary[j] = list;
						}
						list.Add(keyValuePair.Key);
					}
					else
					{
						sb.AppendLine(string.Format("\t{0} (in reserve)", j));
						List<GlobalSignId> list2;
						if (!dictionary2.TryGetValue(j, out list2))
						{
							list2 = new List<GlobalSignId>();
							dictionary2[j] = list2;
						}
						list2.Add(keyValuePair.Key);
					}
				}
			}
			if (!flag)
			{
				sb.AppendLine("\tNONE");
			}
		}
		sb.AppendLine();
		sb.AppendLine("**************************************************");
		sb.AppendLine("TextureStore: IDs By Tier");
		sb.AppendLine("**************************************************");
		for (int k = 0; k < this._pools.Count; k++)
		{
			sb.AppendLine();
			sb.AppendLine(string.Format("Tier {0}:", k));
			List<GlobalSignId> list3;
			if (!dictionary.TryGetValue(k, out list3))
			{
				sb.AppendLine("\t--- In-Use (Count: 0) ---");
				sb.AppendLine("\tNONE");
			}
			else
			{
				sb.AppendLine(string.Format("\t--- In-Use (Count: {0}) ---", list3.Count));
				foreach (GlobalSignId globalSignId in list3)
				{
					sb.AppendLine(string.Format("\t{0}", globalSignId));
				}
			}
			sb.AppendLine();
			List<GlobalSignId> list4;
			if (!dictionary2.TryGetValue(k, out list4))
			{
				sb.AppendLine("\t--- Pooled (Count: 0) ---");
				sb.AppendLine("\tNONE");
			}
			else
			{
				sb.AppendLine(string.Format("\t--- Pooled (Count: {0}) ---", list4.Count));
				foreach (GlobalSignId globalSignId2 in list4)
				{
					sb.AppendLine(string.Format("\t{0}", globalSignId2));
				}
			}
		}
		sb.AppendLine();
	}

	// Token: 0x04003F90 RID: 16272
	public static readonly ProfilerMarker s_SignTextureManagerRebuildPools = new ProfilerMarker("SignTextureManager.SignTextureStore.RebuildPools");

	// Token: 0x04003F91 RID: 16273
	public static readonly ProfilerMarker s_SignTextureManagerMarkUnused = new ProfilerMarker("SignTextureManager.SignTextureStore.MarkUnused");

	// Token: 0x04003F92 RID: 16274
	public static readonly ProfilerMarker s_SignTextureManagerMarkUsed = new ProfilerMarker("SignTextureManager.SignTextureStore.MarkUsed");

	// Token: 0x04003F93 RID: 16275
	public static readonly ProfilerMarker s_SignTextureManagerHas = new ProfilerMarker("SignTextureManager.SignTextureStore.Has");

	// Token: 0x04003F94 RID: 16276
	public static readonly ProfilerMarker s_SignTextureManagerGet = new ProfilerMarker("SignTextureManager.SignTextureStore.Get");

	// Token: 0x04003F95 RID: 16277
	public static readonly ProfilerMarker s_SignTextureManagerGetBestUpToTier = new ProfilerMarker("SignTextureManager.SignTextureStore.GetBestUpToTier");

	// Token: 0x04003F96 RID: 16278
	public static readonly ProfilerMarker s_SignTextureManagerAcquireForBake = new ProfilerMarker("SignTextureManager.SignTextureStore.AcquireForBake");

	// Token: 0x04003F97 RID: 16279
	public static readonly ProfilerMarker s_SignTextureManagerSetBaked = new ProfilerMarker("SignTextureManager.SignTextureStore.SetBaked");

	// Token: 0x04003F98 RID: 16280
	public static readonly ProfilerMarker s_SignTextureManagerInvalidateSign = new ProfilerMarker("SignTextureManager.SignTextureStore.InvalidateSign");

	// Token: 0x04003F99 RID: 16281
	public static readonly ProfilerMarker s_SignTextureManagerEvictUnused = new ProfilerMarker("SignTextureManager.SignTextureStore.EvictUnused");

	// Token: 0x04003F9A RID: 16282
	public static readonly ProfilerMarker s_SignTextureManagerClear = new ProfilerMarker("SignTextureManager.SignTextureStore.Clear");

	// Token: 0x04003F9B RID: 16283
	public static readonly ProfilerMarker s_SignTextureManagerReleaseToPool = new ProfilerMarker("SignTextureManager.SignTextureStore.ReleaseToPool");

	// Token: 0x04003F9C RID: 16284
	public static readonly ProfilerMarker s_SignTextureManagerEnqueueFreeRT = new ProfilerMarker("SignTextureManager.SignTextureStore.EnqueueFreeRT");

	// Token: 0x04003F9D RID: 16285
	public static readonly ProfilerMarker s_SignTextureManagerCreateRT = new ProfilerMarker("SignTextureManager.SignTextureStore.CreateRT");

	// Token: 0x04003F9E RID: 16286
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<long> s_TotalTextureMemory = new ProfilerCounterValue<long>(ProfilerCategory.Scripts, "STM Texture Memory", ProfilerMarkerDataUnit.Bytes, ProfilerCounterOptions.FlushOnEndOfFrame);

	// Token: 0x04003F9F RID: 16287
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SignTextureStore.Pool> _pools = new List<SignTextureStore.Pool>(8);

	// Token: 0x04003FA0 RID: 16288
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<GlobalSignId, SignTextureStore.Entry> _entries = new Dictionary<GlobalSignId, SignTextureStore.Entry>(256);

	// Token: 0x04003FA1 RID: 16289
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<RenderTexture, SignTextureStore.RtOwner> _ownerByRT = new Dictionary<RenderTexture, SignTextureStore.RtOwner>(512);

	// Token: 0x04003FA2 RID: 16290
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> _tiersToRemove = new List<int>(8);

	// Token: 0x04003FA3 RID: 16291
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<GlobalSignId> _signsToRemove = new List<GlobalSignId>(16);

	// Token: 0x04003FA4 RID: 16292
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> _bestTierScratch = new List<int>(8);

	// Token: 0x02000AB2 RID: 2738
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Pool
	{
		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x0600520F RID: 21007 RVA: 0x001F61BD File Offset: 0x001F43BD
		public int Count
		{
			get
			{
				return this.FreeNodes.Count;
			}
		}

		// Token: 0x06005210 RID: 21008 RVA: 0x001F61CC File Offset: 0x001F43CC
		public void DestroyTexturesAndClear()
		{
			for (LinkedListNode<RenderTexture> linkedListNode = this.Free.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				RenderTexture value = linkedListNode.Value;
				if (value != null)
				{
					value.Release();
					global::UnityEngine.Object.DestroyImmediate(value);
				}
			}
			this.Free.Clear();
			this.FreeNodes.Clear();
		}

		// Token: 0x06005211 RID: 21009 RVA: 0x001F6224 File Offset: 0x001F4424
		public bool TryRemove(RenderTexture rt)
		{
			LinkedListNode<RenderTexture> linkedListNode;
			if (this.FreeNodes.TryGetValue(rt, out linkedListNode))
			{
				this.Free.Remove(linkedListNode);
				this.FreeNodes.Remove(rt);
				return true;
			}
			return false;
		}

		// Token: 0x06005212 RID: 21010 RVA: 0x001F6260 File Offset: 0x001F4460
		public void Add(RenderTexture rt)
		{
			if (this.FreeNodes.ContainsKey(rt))
			{
				return;
			}
			LinkedListNode<RenderTexture> linkedListNode = this.Free.AddLast(rt);
			this.FreeNodes.Add(rt, linkedListNode);
		}

		// Token: 0x06005213 RID: 21011 RVA: 0x001F6298 File Offset: 0x001F4498
		public bool TryUnpool(out RenderTexture rt)
		{
			LinkedListNode<RenderTexture> first = this.Free.First;
			if (first == null)
			{
				rt = null;
				return false;
			}
			rt = first.Value;
			this.TryRemove(rt);
			return true;
		}

		// Token: 0x04003FA5 RID: 16293
		public int Resolution;

		// Token: 0x04003FA6 RID: 16294
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LinkedList<RenderTexture> Free = new LinkedList<RenderTexture>();

		// Token: 0x04003FA7 RID: 16295
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<RenderTexture, LinkedListNode<RenderTexture>> FreeNodes = new Dictionary<RenderTexture, LinkedListNode<RenderTexture>>(128);
	}

	// Token: 0x02000AB3 RID: 2739
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Entry
	{
		// Token: 0x04003FA8 RID: 16296
		public readonly Dictionary<int, RenderTexture> TierToRT = new Dictionary<int, RenderTexture>(8);

		// Token: 0x04003FA9 RID: 16297
		public readonly HashSet<int> InUse = new HashSet<int>();
	}

	// Token: 0x02000AB4 RID: 2740
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly struct RtOwner
	{
		// Token: 0x06005216 RID: 21014 RVA: 0x001F630D File Offset: 0x001F450D
		public RtOwner(GlobalSignId signId, int tier)
		{
			this.SignId = signId;
			this.Tier = tier;
		}

		// Token: 0x04003FAA RID: 16298
		public readonly GlobalSignId SignId;

		// Token: 0x04003FAB RID: 16299
		public readonly int Tier;
	}
}
