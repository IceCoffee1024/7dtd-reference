using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F92 RID: 3986
[Preserve]
public class XUiC_PoiList : XUiC_List<XUiC_PoiList.PoiListEntry>
{
	// Token: 0x17000E91 RID: 3729
	// (get) Token: 0x06007DC2 RID: 32194 RVA: 0x002FD453 File Offset: 0x002FB653
	// (set) Token: 0x06007DC3 RID: 32195 RVA: 0x002FD45B File Offset: 0x002FB65B
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

	// Token: 0x17000E92 RID: 3730
	// (get) Token: 0x06007DC4 RID: 32196 RVA: 0x002FD474 File Offset: 0x002FB674
	// (set) Token: 0x06007DC5 RID: 32197 RVA: 0x002FD47C File Offset: 0x002FB67C
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

	// Token: 0x17000E93 RID: 3731
	// (get) Token: 0x06007DC6 RID: 32198 RVA: 0x002FD495 File Offset: 0x002FB695
	// (set) Token: 0x06007DC7 RID: 32199 RVA: 0x002FD49D File Offset: 0x002FB69D
	public int MinTier
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E94 RID: 3732
	// (get) Token: 0x06007DC8 RID: 32200 RVA: 0x002FD4A6 File Offset: 0x002FB6A6
	// (set) Token: 0x06007DC9 RID: 32201 RVA: 0x002FD4AE File Offset: 0x002FB6AE
	public int MaxTier
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06007DCA RID: 32202 RVA: 0x002FD4B8 File Offset: 0x002FB6B8
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

	// Token: 0x06007DCB RID: 32203 RVA: 0x002FD5D4 File Offset: 0x002FB7D4
	public override void OnOpen()
	{
		base.OnOpen();
		if (!this.openedBefore || this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
		this.openedBefore = true;
	}

	// Token: 0x04005D45 RID: 23877
	[PublicizedFrom(EAccessModifier.Private)]
	public const int SmallPoiVolumeLimit = 100;

	// Token: 0x04005D46 RID: 23878
	[PublicizedFrom(EAccessModifier.Private)]
	public bool filterSmallPois = true;

	// Token: 0x04005D47 RID: 23879
	[PublicizedFrom(EAccessModifier.Private)]
	public int filterTier = -1;

	// Token: 0x04005D48 RID: 23880
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openedBefore;

	// Token: 0x02000F93 RID: 3987
	[Preserve]
	public class PoiListEntry : XUiListEntry<XUiC_PoiList.PoiListEntry>
	{
		// Token: 0x06007DCD RID: 32205 RVA: 0x002FD615 File Offset: 0x002FB815
		public PoiListEntry(PrefabInstance _prefabInstance)
		{
			this.prefabInstance = _prefabInstance;
		}

		// Token: 0x06007DCE RID: 32206 RVA: 0x002FD624 File Offset: 0x002FB824
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

		// Token: 0x06007DCF RID: 32207 RVA: 0x002FD6A0 File Offset: 0x002FB8A0
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

		// Token: 0x04005D4B RID: 23883
		public readonly PrefabInstance prefabInstance;
	}

	// Token: 0x02000F94 RID: 3988
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_PoiList.PoiListEntry>.XUiC_ListEntry
	{
		// Token: 0x06007DD0 RID: 32208 RVA: 0x002FD710 File Offset: 0x002FB910
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_PoiList.PoiListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.prefabInstance.prefab.PrefabName : null) ?? "";
		}

		// Token: 0x06007DD1 RID: 32209 RVA: 0x002FD737 File Offset: 0x002FB937
		[XuiXmlBinding("localizedname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLocalizedName()
		{
			XUiC_PoiList.PoiListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.prefabInstance.prefab.LocalizedName : null) ?? "";
		}

		// Token: 0x06007DD2 RID: 32210 RVA: 0x002FD760 File Offset: 0x002FB960
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
