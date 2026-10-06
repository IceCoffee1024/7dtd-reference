using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000AD1 RID: 2769
public sealed class SignPrioritizer
{
	// Token: 0x06005290 RID: 21136 RVA: 0x001FA468 File Offset: 0x001F8668
	public SignPrioritizer()
	{
		this._compareGroupOrder = new Comparison<int>(this.CompareGroupOrder);
	}

	// Token: 0x06005291 RID: 21137 RVA: 0x001FA50C File Offset: 0x001F870C
	public void Clear()
	{
		this._groups.Clear();
		this._canvasRefs.Clear();
		this._canvasRefsPacked.Clear();
		this._groupIndexById.Clear();
		this._groupOrder.Clear();
		this._availableSlots.Clear();
		this._groupCounts.Clear();
		this._groupOffsets.Clear();
	}

	// Token: 0x06005292 RID: 21138 RVA: 0x001FA571 File Offset: 0x001F8771
	public void RebuildRequests(HashSet<SignCanvas> canvases, Vector3 playerPos, SignTextureManager.TierSpec[] tiers, SignTextureStore store, List<SignBakeRequest> outRequests)
	{
		outRequests.Clear();
		if (tiers.Length == 0)
		{
			this.Clear();
			return;
		}
		this.BuildGroups(canvases, playerPos);
		this.PackCanvasRefs();
		this.SortGroupsByDistance();
		this.AssignTiersAndEmitRequests(tiers, store, outRequests);
		outRequests.Sort();
	}

	// Token: 0x06005293 RID: 21139 RVA: 0x001FA5AB File Offset: 0x001F87AB
	public GlobalSignId GetGroupSignId(int groupIndex)
	{
		return this._groups[groupIndex].SignId;
	}

	// Token: 0x06005294 RID: 21140 RVA: 0x001FA5BE File Offset: 0x001F87BE
	public void ApplyToGroupCanvases(int groupIndex, RenderTexture rt)
	{
		this.ApplyToCanvases(groupIndex, rt);
	}

	// Token: 0x06005295 RID: 21141 RVA: 0x001FA5C8 File Offset: 0x001F87C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildGroups(HashSet<SignCanvas> canvases, Vector3 playerPos)
	{
		this._groups.Clear();
		this._canvasRefs.Clear();
		this._groupIndexById.Clear();
		this._groupOrder.Clear();
		foreach (SignCanvas signCanvas in canvases)
		{
			if (!(signCanvas == null))
			{
				GlobalSignId displaySignId = signCanvas.DisplaySignId;
				if (displaySignId.IsValid)
				{
					float num = (signCanvas.GetWorldPosition() - playerPos).sqrMagnitude;
					num /= signCanvas.SizeSquared;
					int count;
					if (!this._groupIndexById.TryGetValue(displaySignId, out count))
					{
						count = this._groups.Count;
						this._groupIndexById.Add(displaySignId, count);
						SignPrioritizer.Group group = new SignPrioritizer.Group
						{
							SignId = displaySignId,
							MinDistanceSquared = num,
							TargetTier = -1,
							CanvasStart = 0,
							CanvasCount = 0
						};
						this._groups.Add(group);
						this._groupOrder.Add(count);
					}
					else
					{
						SignPrioritizer.Group group2 = this._groups[count];
						if (num < group2.MinDistanceSquared)
						{
							group2.MinDistanceSquared = num;
						}
						this._groups[count] = group2;
					}
					this._canvasRefs.Add(new SignPrioritizer.CanvasRef
					{
						Canvas = signCanvas,
						DistanceSquared = num,
						GroupIndex = count
					});
				}
			}
		}
	}

	// Token: 0x06005296 RID: 21142 RVA: 0x001FA76C File Offset: 0x001F896C
	[PublicizedFrom(EAccessModifier.Private)]
	public void PackCanvasRefs()
	{
		int count = this._groups.Count;
		SignPrioritizer.EnsureListSize<int>(this._groupCounts, count);
		SignPrioritizer.EnsureListSize<int>(this._groupOffsets, count);
		for (int i = 0; i < count; i++)
		{
			this._groupCounts[i] = 0;
		}
		for (int j = 0; j < this._canvasRefs.Count; j++)
		{
			List<int> groupCounts = this._groupCounts;
			int num = this._canvasRefs[j].GroupIndex;
			int num2 = groupCounts[num];
			groupCounts[num] = num2 + 1;
		}
		int num3 = 0;
		for (int k = 0; k < count; k++)
		{
			this._groupOffsets[k] = num3;
			SignPrioritizer.Group group = this._groups[k];
			group.CanvasStart = num3;
			group.CanvasCount = this._groupCounts[k];
			this._groups[k] = group;
			num3 += this._groupCounts[k];
		}
		SignPrioritizer.EnsureListSize<SignPrioritizer.CanvasRef>(this._canvasRefsPacked, num3);
		for (int l = 0; l < this._canvasRefs.Count; l++)
		{
			SignPrioritizer.CanvasRef canvasRef = this._canvasRefs[l];
			List<int> groupOffsets = this._groupOffsets;
			int num2 = canvasRef.GroupIndex;
			int num = groupOffsets[num2];
			groupOffsets[num2] = num + 1;
			int num4 = num;
			this._canvasRefsPacked[num4] = canvasRef;
		}
	}

	// Token: 0x06005297 RID: 21143 RVA: 0x001FA8D0 File Offset: 0x001F8AD0
	[PublicizedFrom(EAccessModifier.Private)]
	public int CompareGroupOrder(int a, int b)
	{
		return this._groups[a].MinDistanceSquared.CompareTo(this._groups[b].MinDistanceSquared);
	}

	// Token: 0x06005298 RID: 21144 RVA: 0x001FA907 File Offset: 0x001F8B07
	[PublicizedFrom(EAccessModifier.Private)]
	public void SortGroupsByDistance()
	{
		this._groupOrder.Sort(this._compareGroupOrder);
	}

	// Token: 0x06005299 RID: 21145 RVA: 0x001FA91C File Offset: 0x001F8B1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void AssignTiersAndEmitRequests(SignTextureManager.TierSpec[] tiers, SignTextureStore store, List<SignBakeRequest> outRequests)
	{
		this._availableSlots.Clear();
		for (int i = 0; i < tiers.Length; i++)
		{
			this._availableSlots.Add(tiers[i].ActiveCount);
		}
		int num = this._availableSlots.Count - 1;
		for (int j = 0; j < this._groupOrder.Count; j++)
		{
			int num2 = this._groupOrder[j];
			SignPrioritizer.Group group = this._groups[num2];
			group.TargetTier = num;
			this._groups[num2] = group;
			for (int k = 0; k <= num; k++)
			{
				if (store.Has(group.SignId, k))
				{
					store.MarkInUse(group.SignId, k);
				}
				else
				{
					outRequests.Add(new SignBakeRequest(num2, k, group.MinDistanceSquared));
				}
				List<int> availableSlots = this._availableSlots;
				int num3 = k;
				int num4 = availableSlots[num3];
				availableSlots[num3] = num4 - 1;
			}
			RenderTexture bestUpToTier = store.GetBestUpToTier(group.SignId, num);
			this.ApplyToCanvases(num2, bestUpToTier);
			if (num >= 0 && this._availableSlots[num] <= 0)
			{
				num--;
			}
		}
	}

	// Token: 0x0600529A RID: 21146 RVA: 0x001FAA4C File Offset: 0x001F8C4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyToCanvases(int groupIndex, RenderTexture rt)
	{
		SignPrioritizer.Group group = this._groups[groupIndex];
		int canvasStart = group.CanvasStart;
		int num = canvasStart + group.CanvasCount;
		for (int i = canvasStart; i < num; i++)
		{
			SignCanvas canvas = this._canvasRefsPacked[i].Canvas;
			if (canvas != null)
			{
				canvas.SetTexture(rt);
			}
		}
	}

	// Token: 0x0600529B RID: 21147 RVA: 0x001FAAA4 File Offset: 0x001F8CA4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void EnsureListSize<T>(List<T> list, int size)
	{
		while (list.Count < size)
		{
			list.Add(default(T));
		}
	}

	// Token: 0x0600529C RID: 21148 RVA: 0x001FAACC File Offset: 0x001F8CCC
	public void WriteGroupInfo(StringBuilder sb, Vector3 playerPos, SignTextureManager.TierSpec[] activeTiers)
	{
		int count = this._groups.Count;
		sb.AppendLine("**************************************************");
		sb.AppendLine(string.Format("GroupInfo ({0} groups)", count));
		sb.AppendLine("**************************************************");
		int num = 0;
		Dictionary<int, HashSet<Texture>> dictionary = new Dictionary<int, HashSet<Texture>>();
		for (int i = 0; i < this._groupOrder.Count; i++)
		{
			int num2 = this._groupOrder[i];
			SignPrioritizer.Group group = this._groups[num2];
			int canvasStart = group.CanvasStart;
			int num3 = canvasStart + group.CanvasCount;
			sb.AppendLine();
			sb.AppendLine(string.Format("--- Group {0} ---", num2));
			sb.AppendLine(string.Format("Sign ID: {0}", group.SignId));
			SignData signData;
			if (SignDataManager.Instance.TryGetSignData(group.SignId, out signData))
			{
				sb.AppendLine("Sign Name: " + signData.name);
			}
			sb.AppendLine(string.Format("MinDistanceSquared: {0}", group.MinDistanceSquared));
			sb.AppendLine(string.Format("TargetTier: {0}", group.TargetTier));
			sb.AppendLine(string.Format("CanvasCount: {0}", group.CanvasCount));
			sb.AppendLine(string.Format("CanvasStart: {0}", group.CanvasStart));
			for (int j = canvasStart; j < num3; j++)
			{
				sb.AppendLine();
				sb.AppendLine(string.Format("\t--- Canvas {0} (index {1}) ---", j - canvasStart, j));
				SignPrioritizer.CanvasRef canvasRef = this._canvasRefsPacked[j];
				SignCanvas canvas = canvasRef.Canvas;
				if (canvas == null)
				{
					sb.AppendLine("\tNULL");
				}
				else
				{
					float sqrMagnitude = (canvas.GetWorldPosition() - playerPos).sqrMagnitude;
					sb.AppendLine(string.Format("\tDistanceSquared (recorded): {0}", canvasRef.DistanceSquared));
					sb.AppendLine(string.Format("\tDistanceSquared (current): {0}", sqrMagnitude / canvas.SizeSquared));
					sb.AppendLine(string.Format("\t\tRaw: {0}", sqrMagnitude));
					sb.AppendLine(string.Format("\t\tSizeSquared: {0}", canvas.SizeSquared));
					Texture bakedTexture = canvas.BakedTexture;
					if (bakedTexture != null)
					{
						int width = bakedTexture.width;
						int resolution = activeTiers[group.TargetTier].Resolution;
						HashSet<Texture> hashSet;
						if (!dictionary.TryGetValue(width, out hashSet))
						{
							hashSet = new HashSet<Texture>();
							dictionary[width] = hashSet;
						}
						hashSet.Add(bakedTexture);
						if (width < resolution)
						{
							sb.AppendLine(string.Format("\tTexture: {0}px [BELOW TARGET: {1}px]", width, resolution));
						}
						else
						{
							sb.AppendLine(string.Format("\tTexture: {0}px", width));
						}
					}
					else
					{
						sb.AppendLine("\tTexture: NULL");
					}
					num++;
				}
			}
		}
		sb.AppendLine();
		sb.AppendLine("**************************************************");
		sb.AppendLine("GroupInfo: Canvas-Assigned Texture Counts By Size");
		sb.AppendLine("**************************************************");
		sb.AppendLine();
		foreach (KeyValuePair<int, HashSet<Texture>> keyValuePair in dictionary)
		{
			int key = keyValuePair.Key;
			HashSet<Texture> value = keyValuePair.Value;
			sb.AppendLine(string.Format("Size {0}: {1}", key, value.Count));
		}
		sb.AppendLine();
		sb.AppendLine("**************************************************");
		sb.AppendLine(string.Format("Finished LogGroupInfo ({0} active canvases)", num));
		sb.AppendLine("**************************************************");
	}

	// Token: 0x0400404D RID: 16461
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SignPrioritizer.Group> _groups = new List<SignPrioritizer.Group>(256);

	// Token: 0x0400404E RID: 16462
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SignPrioritizer.CanvasRef> _canvasRefs = new List<SignPrioritizer.CanvasRef>(1024);

	// Token: 0x0400404F RID: 16463
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SignPrioritizer.CanvasRef> _canvasRefsPacked = new List<SignPrioritizer.CanvasRef>(1024);

	// Token: 0x04004050 RID: 16464
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<GlobalSignId, int> _groupIndexById = new Dictionary<GlobalSignId, int>(256);

	// Token: 0x04004051 RID: 16465
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> _groupOrder = new List<int>(256);

	// Token: 0x04004052 RID: 16466
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> _availableSlots = new List<int>(8);

	// Token: 0x04004053 RID: 16467
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> _groupCounts = new List<int>(256);

	// Token: 0x04004054 RID: 16468
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> _groupOffsets = new List<int>(256);

	// Token: 0x04004055 RID: 16469
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Comparison<int> _compareGroupOrder;

	// Token: 0x02000AD2 RID: 2770
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Group
	{
		// Token: 0x04004056 RID: 16470
		public GlobalSignId SignId;

		// Token: 0x04004057 RID: 16471
		public float MinDistanceSquared;

		// Token: 0x04004058 RID: 16472
		public int TargetTier;

		// Token: 0x04004059 RID: 16473
		public int CanvasStart;

		// Token: 0x0400405A RID: 16474
		public int CanvasCount;
	}

	// Token: 0x02000AD3 RID: 2771
	[PublicizedFrom(EAccessModifier.Private)]
	public struct CanvasRef
	{
		// Token: 0x0400405B RID: 16475
		public SignCanvas Canvas;

		// Token: 0x0400405C RID: 16476
		public float DistanceSquared;

		// Token: 0x0400405D RID: 16477
		public int GroupIndex;
	}
}
