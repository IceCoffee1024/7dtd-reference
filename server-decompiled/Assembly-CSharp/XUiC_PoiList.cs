using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FAB RID: 4011
[Preserve]
public class XUiC_PoiList : XUiC_List<XUiC_PoiList.PoiListEntry>
{
	// Token: 0x17000ECA RID: 3786
	// (get) Token: 0x06007EB4 RID: 32436 RVA: 0x00301C27 File Offset: 0x002FFE27
	// (set) Token: 0x06007EB5 RID: 32437 RVA: 0x00301C2F File Offset: 0x002FFE2F
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

	// Token: 0x17000ECB RID: 3787
	// (get) Token: 0x06007EB6 RID: 32438 RVA: 0x00301C48 File Offset: 0x002FFE48
	// (set) Token: 0x06007EB7 RID: 32439 RVA: 0x00301C50 File Offset: 0x002FFE50
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

	// Token: 0x17000ECC RID: 3788
	// (get) Token: 0x06007EB8 RID: 32440 RVA: 0x00301C69 File Offset: 0x002FFE69
	// (set) Token: 0x06007EB9 RID: 32441 RVA: 0x00301C71 File Offset: 0x002FFE71
	public int MinTier
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000ECD RID: 3789
	// (get) Token: 0x06007EBA RID: 32442 RVA: 0x00301C7A File Offset: 0x002FFE7A
	// (set) Token: 0x06007EBB RID: 32443 RVA: 0x00301C82 File Offset: 0x002FFE82
	public int MaxTier
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06007EBC RID: 32444 RVA: 0x00301C8C File Offset: 0x002FFE8C
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

	// Token: 0x06007EBD RID: 32445 RVA: 0x00301DA8 File Offset: 0x002FFFA8
	public override void OnOpen()
	{
		base.OnOpen();
		if (!this.openedBefore || this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
		this.openedBefore = true;
	}

	// Token: 0x04005E0C RID: 24076
	[PublicizedFrom(EAccessModifier.Private)]
	public const int SmallPoiVolumeLimit = 100;

	// Token: 0x04005E0D RID: 24077
	[PublicizedFrom(EAccessModifier.Private)]
	public bool filterSmallPois = true;

	// Token: 0x04005E0E RID: 24078
	[PublicizedFrom(EAccessModifier.Private)]
	public int filterTier = -1;

	// Token: 0x04005E0F RID: 24079
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openedBefore;

	// Token: 0x02000FAC RID: 4012
	[Preserve]
	public class PoiListEntry : XUiListEntry<XUiC_PoiList.PoiListEntry>
	{
		// Token: 0x06007EBF RID: 32447 RVA: 0x00301DE9 File Offset: 0x002FFFE9
		public PoiListEntry(PrefabInstance _prefabInstance)
		{
			this.prefabInstance = _prefabInstance;
		}

		// Token: 0x06007EC0 RID: 32448 RVA: 0x00301DF8 File Offset: 0x002FFFF8
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

		// Token: 0x06007EC1 RID: 32449 RVA: 0x00301E74 File Offset: 0x00300074
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

		// Token: 0x04005E12 RID: 24082
		public readonly PrefabInstance prefabInstance;
	}

	// Token: 0x02000FAD RID: 4013
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PoiList.PoiListEntry>.XUiC_ListEntry
	{
		// Token: 0x06007EC2 RID: 32450 RVA: 0x00301EE4 File Offset: 0x003000E4
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PoiList.PoiListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.prefabInstance.prefab.PrefabName : null) ?? "";
		}

		// Token: 0x06007EC3 RID: 32451 RVA: 0x00301F0B File Offset: 0x0030010B
		[XuiXmlBinding("localizedname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLocalizedName()
		{
			XUiC_PoiList.PoiListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.prefabInstance.prefab.LocalizedName : null) ?? "";
		}

		// Token: 0x06007EC4 RID: 32452 RVA: 0x00301F34 File Offset: 0x00300134
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
