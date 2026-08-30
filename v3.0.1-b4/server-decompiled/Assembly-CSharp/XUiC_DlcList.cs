using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DEB RID: 3563
[Preserve]
public class XUiC_DlcList : XUiC_List<XUiC_DlcList.DlcEntry>
{
	// Token: 0x17000C12 RID: 3090
	// (get) Token: 0x06006D2E RID: 27950 RVA: 0x002A7086 File Offset: 0x002A5286
	[XuiXmlBinding("dlc_count")]
	public int DlcCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.allEntries.Count;
		}
	}

	// Token: 0x17000C13 RID: 3091
	// (get) Token: 0x06006D2F RID: 27951 RVA: 0x002A7093 File Offset: 0x002A5293
	[XuiXmlBinding("paging_active")]
	public bool PagingRequired
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.allEntries.Count > base.PageLength;
		}
	}

	// Token: 0x06006D30 RID: 27952 RVA: 0x002A70A8 File Offset: 0x002A52A8
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		foreach (XUiC_DlcList.DlcEntry dlcEntry in XUiC_DlcList.DlcEntries)
		{
			if (EntitlementManager.Instance.IsAvailableOnPlatform(dlcEntry.DlcSet) && EntitlementManager.Instance.IsEntitlementPurchasable(dlcEntry.DlcSet))
			{
				this.allEntries.Add(dlcEntry);
			}
		}
		base.RebuildList(_resetFilter);
	}

	// Token: 0x06006D31 RID: 27953 RVA: 0x002A7138 File Offset: 0x002A5338
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x040052F9 RID: 21241
	public static readonly List<XUiC_DlcList.DlcEntry> DlcEntries = new List<XUiC_DlcList.DlcEntry>
	{
		new XUiC_DlcList.DlcEntry("armorBeachwearSet", "Data/DlcInfo/banner_beachwear_outfit", EntitlementSetEnum.BeachCosmetic),
		new XUiC_DlcList.DlcEntry("armorClassicSurvivorSet", "Data/DlcInfo/banner_classicsurvivor_outfit", EntitlementSetEnum.ClassicSurvivorCosmetic),
		new XUiC_DlcList.DlcEntry("armorHolidayHatsSet", "Data/DlcInfo/banner_holidayhats", EntitlementSetEnum.ChristmasCosmetics),
		new XUiC_DlcList.DlcEntry("armorButcherSet", "Data/DlcInfo/banner_butcher_outfit", EntitlementSetEnum.ButcherCosmetic),
		new XUiC_DlcList.DlcEntry("armorHellreaverSet", "Data/DlcInfo/banner_hellreaver_outfit", EntitlementSetEnum.HellreaverCosmetic),
		new XUiC_DlcList.DlcEntry("armorPirateSet", "Data/DlcInfo/banner_pirate_outfit", EntitlementSetEnum.PirateCosmetic),
		new XUiC_DlcList.DlcEntry("armorHoarderSet", "Data/DlcInfo/banner_hoarder_outfit", EntitlementSetEnum.HoarderCosmetic),
		new XUiC_DlcList.DlcEntry("armorMarauderSet", "Data/DlcInfo/banner_marauder_outfit", EntitlementSetEnum.MarauderCosmetic),
		new XUiC_DlcList.DlcEntry("armorDesertSet", "Data/DlcInfo/banner_desert_outfit", EntitlementSetEnum.DesertCosmetic)
	};

	// Token: 0x02000DEC RID: 3564
	[Preserve]
	public class DlcEntry : XUiListEntry<XUiC_DlcList.DlcEntry>
	{
		// Token: 0x06006D34 RID: 27956 RVA: 0x002A722F File Offset: 0x002A542F
		public DlcEntry(string _nameKey, string _imageResourceUri, EntitlementSetEnum _dlcSet)
		{
			this.Name = Localization.Get(_nameKey, false, null);
			this.ImageResourceUri = _imageResourceUri;
			this.DlcSet = _dlcSet;
		}

		// Token: 0x06006D35 RID: 27957 RVA: 0x002A7253 File Offset: 0x002A5453
		public override int CompareTo(XUiC_DlcList.DlcEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return string.Compare(this.Name, _otherEntry.Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x040052FA RID: 21242
		public readonly string Name;

		// Token: 0x040052FB RID: 21243
		public readonly string ImageResourceUri;

		// Token: 0x040052FC RID: 21244
		public readonly EntitlementSetEnum DlcSet;
	}

	// Token: 0x02000DED RID: 3565
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class DlcListEntryController : XUiC_List<XUiC_DlcList.DlcEntry>.XUiC_ListEntry
	{
		// Token: 0x06006D36 RID: 27958 RVA: 0x002A726C File Offset: 0x002A546C
		public override void Init()
		{
			base.Init();
			XUiController childById = base.GetChildById("dlcImage");
			XUiV_Texture xuiV_Texture = ((childById != null) ? childById.ViewComponent : null) as XUiV_Texture;
			if (xuiV_Texture != null)
			{
				this.textureScaler = xuiV_Texture.UiTransform.gameObject.AddComponent<TweenScale>();
				this.textureScaler.enabled = false;
				this.textureScaler.from = Vector3.one;
				this.textureScaler.to = Vector3.one * 1.15f;
				this.textureScaler.duration = 0.25f;
				this.textureScaler.ResetToBeginning();
			}
		}

		// Token: 0x06006D37 RID: 27959 RVA: 0x002A7306 File Offset: 0x002A5506
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnHovered(bool _isOver)
		{
			base.OnHovered(_isOver);
			if (this.textureScaler == null)
			{
				return;
			}
			if (_isOver)
			{
				this.textureScaler.PlayForward();
				return;
			}
			this.textureScaler.PlayReverse();
		}

		// Token: 0x06006D38 RID: 27960 RVA: 0x002A7338 File Offset: 0x002A5538
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_DlcList.DlcEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}

		// Token: 0x06006D39 RID: 27961 RVA: 0x002A7355 File Offset: 0x002A5555
		[XuiXmlBinding("texture_uri")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingTextureUri()
		{
			XUiC_DlcList.DlcEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.ImageResourceUri : null) ?? "";
		}

		// Token: 0x06006D3A RID: 27962 RVA: 0x002A7372 File Offset: 0x002A5572
		[XuiXmlBinding("owned")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingOwned()
		{
			return this.entryData != null && EntitlementManager.Instance.HasEntitlement(this.entryData.DlcSet);
		}

		// Token: 0x040052FD RID: 21245
		[PublicizedFrom(EAccessModifier.Private)]
		public const float HoverDuration = 0.25f;

		// Token: 0x040052FE RID: 21246
		[PublicizedFrom(EAccessModifier.Private)]
		public const float HoverScale = 1.15f;

		// Token: 0x040052FF RID: 21247
		[PublicizedFrom(EAccessModifier.Private)]
		public TweenScale textureScaler;
	}
}
