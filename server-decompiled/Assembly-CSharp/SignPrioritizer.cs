using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000ADB RID: 2779
public sealed class SignPrioritizer
{
	// Token: 0x060052B4 RID: 21172 RVA: 0x001FB544 File Offset: 0x001F9744
	public SignPrioritizer()
	{
		this._compareGroupOrder = new Comparison<int>(this.CompareGroupOrder);
	}

	// Token: 0x060052B5 RID: 21173 RVA: 0x001FB5E8 File Offset: 0x001F97E8
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

	// Token: 0x060052B6 RID: 21174 RVA: 0x001FB64D File Offset: 0x001F984D
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

	// Token: 0x060052B7 RID: 21175 RVA: 0x001FB687 File Offset: 0x001F9887
	public GlobalSignId GetGroupSignId(int groupIndex)
	{
		return this._groups[groupIndex].SignId;
	}

	// Token: 0x060052B8 RID: 21176 RVA: 0x001FB69A File Offset: 0x001F989A
	public void ApplyToGroupCanvases(int groupIndex, RenderTexture rt)
	{
		this.ApplyToCanvases(groupIndex, rt);
	}

	// Token: 0x060052B9 RID: 21177 RVA: 0x001FB6A4 File Offset: 0x001F98A4
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

	// Token: 0x060052BA RID: 21178 RVA: 0x001FB848 File Offset: 0x001F9A48
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

	// Token: 0x060052BB RID: 21179 RVA: 0x001FB9AC File Offset: 0x001F9BAC
	[PublicizedFrom(EAccessModifier.Private)]
	public int CompareGroupOrder(int a, int b)
	{
		return this._groups[a].MinDistanceSquared.CompareTo(this._groups[b].MinDistanceSquared);
	}

	// Token: 0x060052BC RID: 21180 RVA: 0x001FB9E3 File Offset: 0x001F9BE3
	[PublicizedFrom(EAccessModifier.Private)]
	public void SortGroupsByDistance()
	{
		this._groupOrder.Sort(this._compareGroupOrder);
	}

	// Token: 0x060052BD RID: 21181 RVA: 0x001FB9F8 File Offset: 0x001F9BF8
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

	// Token: 0x060052BE RID: 21182 RVA: 0x001FBB28 File Offset: 0x001F9D28
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

	// Token: 0x060052BF RID: 21183 RVA: 0x001FBB80 File Offset: 0x001F9D80
	[PublicizedFrom(EAccessModifier.Private)]
	public static void EnsureListSize<T>(List<T> list, int size)
	{
		while (list.Count < size)
		{
			list.Add(default(T));
		}
	}

	// Token: 0x060052C0 RID: 21184 RVA: 0x001FBBA8 File Offset: 0x001F9DA8
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

	// Token: 0x0400406E RID: 16494
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SignPrioritizer.Group> _groups = new List<SignPrioritizer.Group>(256);

	// Token: 0x0400406F RID: 16495
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SignPrioritizer.CanvasRef> _canvasRefs = new List<SignPrioritizer.CanvasRef>(1024);

	// Token: 0x04004070 RID: 16496
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SignPrioritizer.CanvasRef> _canvasRefsPacked = new List<SignPrioritizer.CanvasRef>(1024);

	// Token: 0x04004071 RID: 16497
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<GlobalSignId, int> _groupIndexById = new Dictionary<GlobalSignId, int>(256);

	// Token: 0x04004072 RID: 16498
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> _groupOrder = new List<int>(256);

	// Token: 0x04004073 RID: 16499
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> _availableSlots = new List<int>(8);

	// Token: 0x04004074 RID: 16500
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> _groupCounts = new List<int>(256);

	// Token: 0x04004075 RID: 16501
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> _groupOffsets = new List<int>(256);

	// Token: 0x04004076 RID: 16502
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Comparison<int> _compareGroupOrder;

	// Token: 0x02000ADC RID: 2780
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Group
	{
		// Token: 0x04004077 RID: 16503
		public GlobalSignId SignId;

		// Token: 0x04004078 RID: 16504
		public float MinDistanceSquared;

		// Token: 0x04004079 RID: 16505
		public int TargetTier;

		// Token: 0x0400407A RID: 16506
		public int CanvasStart;

		// Token: 0x0400407B RID: 16507
		public int CanvasCount;
	}

	// Token: 0x02000ADD RID: 2781
	[PublicizedFrom(EAccessModifier.Private)]
	public struct CanvasRef
	{
		// Token: 0x0400407C RID: 16508
		public SignCanvas Canvas;

		// Token: 0x0400407D RID: 16509
		public float DistanceSquared;

		// Token: 0x0400407E RID: 16510
		public int GroupIndex;
	}
}
