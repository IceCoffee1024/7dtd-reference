using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FBA RID: 4026
[Preserve]
public class XUiC_PoiList : XUiC_List<XUiC_PoiList.PoiListEntry>
{
	// Token: 0x17000EF1 RID: 3825
	// (get) Token: 0x06007EC3 RID: 32451 RVA: 0x00304BFF File Offset: 0x00302DFF
	// (set) Token: 0x06007EC4 RID: 32452 RVA: 0x00304C07 File Offset: 0x00302E07
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

	// Token: 0x17000EF2 RID: 3826
	// (get) Token: 0x06007EC5 RID: 32453 RVA: 0x00304C20 File Offset: 0x00302E20
	// (set) Token: 0x06007EC6 RID: 32454 RVA: 0x00304C28 File Offset: 0x00302E28
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

	// Token: 0x17000EF3 RID: 3827
	// (get) Token: 0x06007EC7 RID: 32455 RVA: 0x00304C41 File Offset: 0x00302E41
	// (set) Token: 0x06007EC8 RID: 32456 RVA: 0x00304C49 File Offset: 0x00302E49
	public int MinTier
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000EF4 RID: 3828
	// (get) Token: 0x06007EC9 RID: 32457 RVA: 0x00304C52 File Offset: 0x00302E52
	// (set) Token: 0x06007ECA RID: 32458 RVA: 0x00304C5A File Offset: 0x00302E5A
	public int MaxTier
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06007ECB RID: 32459 RVA: 0x00304C64 File Offset: 0x00302E64
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

	// Token: 0x06007ECC RID: 32460 RVA: 0x00304D80 File Offset: 0x00302F80
	public override void OnOpen()
	{
		base.OnOpen();
		if (!this.openedBefore || this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
		this.openedBefore = true;
	}

	// Token: 0x04005E35 RID: 24117
	[PublicizedFrom(EAccessModifier.Private)]
	public const int SmallPoiVolumeLimit = 100;

	// Token: 0x04005E36 RID: 24118
	[PublicizedFrom(EAccessModifier.Private)]
	public bool filterSmallPois = true;

	// Token: 0x04005E37 RID: 24119
	[PublicizedFrom(EAccessModifier.Private)]
	public int filterTier = -1;

	// Token: 0x04005E38 RID: 24120
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openedBefore;

	// Token: 0x02000FBB RID: 4027
	[Preserve]
	public class PoiListEntry : XUiListEntry<XUiC_PoiList.PoiListEntry>
	{
		// Token: 0x06007ECE RID: 32462 RVA: 0x00304DC1 File Offset: 0x00302FC1
		public PoiListEntry(PrefabInstance _prefabInstance)
		{
			this.prefabInstance = _prefabInstance;
		}

		// Token: 0x06007ECF RID: 32463 RVA: 0x00304DD0 File Offset: 0x00302FD0
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

		// Token: 0x06007ED0 RID: 32464 RVA: 0x00304E4C File Offset: 0x0030304C
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

		// Token: 0x04005E3B RID: 24123
		public readonly PrefabInstance prefabInstance;
	}

	// Token: 0x02000FBC RID: 4028
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PoiList.PoiListEntry>.XUiC_ListEntry
	{
		// Token: 0x06007ED1 RID: 32465 RVA: 0x00304EBC File Offset: 0x003030BC
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PoiList.PoiListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.prefabInstance.prefab.PrefabName : null) ?? "";
		}

		// Token: 0x06007ED2 RID: 32466 RVA: 0x00304EE3 File Offset: 0x003030E3
		[XuiXmlBinding("localizedname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLocalizedName()
		{
			XUiC_PoiList.PoiListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.prefabInstance.prefab.LocalizedName : null) ?? "";
		}

		// Token: 0x06007ED3 RID: 32467 RVA: 0x00304F0C File Offset: 0x0030310C
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
