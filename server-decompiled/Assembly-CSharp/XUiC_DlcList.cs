using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E0F RID: 3599
[Preserve]
public class XUiC_DlcList : XUiC_List<XUiC_DlcList.DlcEntry>
{
	// Token: 0x17000C46 RID: 3142
	// (get) Token: 0x06006DC5 RID: 28101 RVA: 0x002AD8AA File Offset: 0x002ABAAA
	[XuiXmlBinding("dlc_count")]
	public int DlcCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.allEntries.Count;
		}
	}

	// Token: 0x17000C47 RID: 3143
	// (get) Token: 0x06006DC6 RID: 28102 RVA: 0x002AD8B7 File Offset: 0x002ABAB7
	[XuiXmlBinding("paging_active")]
	public bool PagingRequired
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.allEntries.Count > base.PageLength;
		}
	}

	// Token: 0x06006DC7 RID: 28103 RVA: 0x002AD8CC File Offset: 0x002ABACC
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

	// Token: 0x06006DC8 RID: 28104 RVA: 0x002AD95C File Offset: 0x002ABB5C
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x040053D3 RID: 21459
	public static readonly List<XUiC_DlcList.DlcEntry> DlcEntries = new List<XUiC_DlcList.DlcEntry>
	{
		new XUiC_DlcList.DlcEntry("armorOktoberfestSet", "Data/DlcInfo/banner_octoberfest_outfit", EntitlementSetEnum.OktoberfestCosmetic),
		new XUiC_DlcList.DlcEntry("armorWorkingStiffSet", "Data/DlcInfo/banner_workingstiff_outfit", EntitlementSetEnum.WorkingStiffCosmetic),
		new XUiC_DlcList.DlcEntry("armorBigBeakSet", "Data/DlcInfo/banner_bigbeak_outfit", EntitlementSetEnum.HenpocalypseCosmetic),
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

	// Token: 0x02000E10 RID: 3600
	[Preserve]
	public class DlcEntry : XUiListEntry<XUiC_DlcList.DlcEntry>
	{
		// Token: 0x06006DCB RID: 28107 RVA: 0x002ADA98 File Offset: 0x002ABC98
		public DlcEntry(string _nameKey, string _imageResourceUri, EntitlementSetEnum _dlcSet)
		{
			this.Name = Localization.Get(_nameKey, false, null);
			this.ImageResourceUri = _imageResourceUri;
			this.DlcSet = _dlcSet;
		}

		// Token: 0x06006DCC RID: 28108 RVA: 0x002ADABC File Offset: 0x002ABCBC
		public override int CompareTo(XUiC_DlcList.DlcEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return string.Compare(this.Name, _otherEntry.Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x040053D4 RID: 21460
		public readonly string Name;

		// Token: 0x040053D5 RID: 21461
		public readonly string ImageResourceUri;

		// Token: 0x040053D6 RID: 21462
		public readonly EntitlementSetEnum DlcSet;
	}

	// Token: 0x02000E11 RID: 3601
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class DlcListEntryController : XUiC_List<XUiC_DlcList.DlcEntry>.XUiC_ListEntry
	{
		// Token: 0x06006DCD RID: 28109 RVA: 0x002ADAD8 File Offset: 0x002ABCD8
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

		// Token: 0x06006DCE RID: 28110 RVA: 0x002ADB72 File Offset: 0x002ABD72
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

		// Token: 0x06006DCF RID: 28111 RVA: 0x002ADBA4 File Offset: 0x002ABDA4
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_DlcList.DlcEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}

		// Token: 0x06006DD0 RID: 28112 RVA: 0x002ADBC1 File Offset: 0x002ABDC1
		[XuiXmlBinding("texture_uri")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingTextureUri()
		{
			XUiC_DlcList.DlcEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.ImageResourceUri : null) ?? "";
		}

		// Token: 0x06006DD1 RID: 28113 RVA: 0x002ADBDE File Offset: 0x002ABDDE
		[XuiXmlBinding("owned")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingOwned()
		{
			return this.entryData != null && EntitlementManager.Instance.HasEntitlement(this.entryData.DlcSet);
		}

		// Token: 0x040053D7 RID: 21463
		[PublicizedFrom(EAccessModifier.Private)]
		public const float HoverDuration = 0.25f;

		// Token: 0x040053D8 RID: 21464
		[PublicizedFrom(EAccessModifier.Private)]
		public const float HoverScale = 1.15f;

		// Token: 0x040053D9 RID: 21465
		[PublicizedFrom(EAccessModifier.Private)]
		public TweenScale textureScaler;
	}
}
