using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FCA RID: 4042
[Preserve]
public class XUiC_PoiList : XUiC_List<XUiC_PoiList.PoiListEntry>
{
	// Token: 0x17000EFC RID: 3836
	// (get) Token: 0x06007F12 RID: 32530 RVA: 0x0030671B File Offset: 0x0030491B
	// (set) Token: 0x06007F13 RID: 32531 RVA: 0x00306723 File Offset: 0x00304923
	public bool FilterSmallPois
	{
		get
		{
			return this.filterSmallPois;
		}
		set
		{
			if (this.filterSmallPois != value)
			{
				this.filterSmallPois = value;
				this.RebuildList(false);
			}
		}
	}

	// Token: 0x17000EFD RID: 3837
	// (get) Token: 0x06007F14 RID: 32532 RVA: 0x0030673C File Offset: 0x0030493C
	// (set) Token: 0x06007F15 RID: 32533 RVA: 0x00306744 File Offset: 0x00304944
	public int FilterTier
	{
		get
		{
			return this.filterTier;
		}
		set
		{
			if (this.filterTier != value)
			{
				this.filterTier = value;
				this.RebuildList(false);
			}
		}
	}

	// Token: 0x17000EFE RID: 3838
	// (get) Token: 0x06007F16 RID: 32534 RVA: 0x0030675D File Offset: 0x0030495D
	// (set) Token: 0x06007F17 RID: 32535 RVA: 0x00306765 File Offset: 0x00304965
	public int MinTier
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000EFF RID: 3839
	// (get) Token: 0x06007F18 RID: 32536 RVA: 0x0030676E File Offset: 0x0030496E
	// (set) Token: 0x06007F19 RID: 32537 RVA: 0x00306776 File Offset: 0x00304976
	public int MaxTier
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06007F1A RID: 32538 RVA: 0x00306780 File Offset: 0x00304980
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		if (GameManager.Instance != null)
		{
			DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
			if (dynamicPrefabDecorator != null)
			{
				List<PrefabInstance> list = new List<PrefabInstance>();
				dynamicPrefabDecorator.GetWorldPrefabs(list);
				foreach (PrefabInstance prefabInstance in list)
				{
					if (!this.openedBefore)
					{
						this.MinTier = Mathf.Min((int)prefabInstance.prefab.DifficultyTier, this.MinTier);
						this.MaxTier = Mathf.Max((int)prefabInstance.prefab.DifficultyTier, this.MaxTier);
					}
					if ((!this.filterSmallPois || prefabInstance.boundingBoxSize.Volume() >= 100) && (this.filterTier < 0 || (int)prefabInstance.prefab.DifficultyTier == this.filterTier))
					{
						this.allEntries.Add(new XUiC_PoiList.PoiListEntry(prefabInstance));
					}
				}
			}
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x06007F1B RID: 32539 RVA: 0x0030689C File Offset: 0x00304A9C
	public override void OnOpen()
	{
		base.OnOpen();
		if (!this.openedBefore || this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
		this.openedBefore = true;
	}

	// Token: 0x04005E70 RID: 24176
	[PublicizedFrom(EAccessModifier.Private)]
	public const int SmallPoiVolumeLimit = 100;

	// Token: 0x04005E71 RID: 24177
	[PublicizedFrom(EAccessModifier.Private)]
	public bool filterSmallPois = true;

	// Token: 0x04005E72 RID: 24178
	[PublicizedFrom(EAccessModifier.Private)]
	public int filterTier = -1;

	// Token: 0x04005E73 RID: 24179
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openedBefore;

	// Token: 0x02000FCB RID: 4043
	[Preserve]
	public class PoiListEntry : XUiListEntry<XUiC_PoiList.PoiListEntry>
	{
		// Token: 0x06007F1D RID: 32541 RVA: 0x003068DD File Offset: 0x00304ADD
		public PoiListEntry(PrefabInstance _prefabInstance)
		{
			this.prefabInstance = _prefabInstance;
		}

		// Token: 0x06007F1E RID: 32542 RVA: 0x003068EC File Offset: 0x00304AEC
		public override int CompareTo(XUiC_PoiList.PoiListEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			int num = string.Compare(this.prefabInstance.name, _otherEntry.prefabInstance.name, StringComparison.OrdinalIgnoreCase);
			if (num != 0)
			{
				return num;
			}
			num = this.prefabInstance.boundingBoxPosition.x - _otherEntry.prefabInstance.boundingBoxPosition.x;
			if (num != 0)
			{
				return num;
			}
			return this.prefabInstance.boundingBoxPosition.z - _otherEntry.prefabInstance.boundingBoxPosition.z;
		}

		// Token: 0x06007F1F RID: 32543 RVA: 0x00306968 File Offset: 0x00304B68
		public override bool MatchesSearch(string _searchString)
		{
			Prefab prefab = this.prefabInstance.prefab;
			if (prefab.PrefabName.ContainsCaseInsensitive(_searchString) || prefab.LocalizedName.ContainsCaseInsensitive(_searchString))
			{
				return true;
			}
			Vector3i boundingBoxPosition = this.prefabInstance.boundingBoxPosition;
			BiomeDefinition biomeInWorld = GameManager.Instance.World.GetBiomeInWorld(boundingBoxPosition.x, boundingBoxPosition.z);
			return biomeInWorld != null && biomeInWorld.LocalizedName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04005E76 RID: 24182
		public readonly PrefabInstance prefabInstance;
	}

	// Token: 0x02000FCC RID: 4044
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PoiList.PoiListEntry>.XUiC_ListEntry
	{
		// Token: 0x06007F20 RID: 32544 RVA: 0x003069D8 File Offset: 0x00304BD8
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PoiList.PoiListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.prefabInstance.prefab.PrefabName : null) ?? "";
		}

		// Token: 0x06007F21 RID: 32545 RVA: 0x003069FF File Offset: 0x00304BFF
		[XuiXmlBinding("localizedname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLocalizedName()
		{
			XUiC_PoiList.PoiListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.prefabInstance.prefab.LocalizedName : null) ?? "";
		}

		// Token: 0x06007F22 RID: 32546 RVA: 0x00306A28 File Offset: 0x00304C28
		[XuiXmlBinding("coords")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingCoords()
		{
			if (this.entryData == null)
			{
				return "";
			}
			Vector3i boundingBoxPosition = this.entryData.prefabInstance.boundingBoxPosition;
			BiomeDefinition biomeInWorld = GameManager.Instance.World.GetBiomeInWorld(boundingBoxPosition.x, boundingBoxPosition.z);
			return "(" + ValueDisplayFormatters.WorldPos(boundingBoxPosition.ToVector3(), " ", false) + ")\n" + ((biomeInWorld != null) ? biomeInWorld.LocalizedName : null);
		}
	}
}
