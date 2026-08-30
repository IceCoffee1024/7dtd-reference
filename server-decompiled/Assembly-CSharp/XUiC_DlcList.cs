using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E00 RID: 3584
[Preserve]
public class XUiC_DlcList : XUiC_List<XUiC_DlcList.DlcEntry>
{
	// Token: 0x17000C2B RID: 3115
	// (get) Token: 0x06006DCD RID: 28109 RVA: 0x002AA51A File Offset: 0x002A871A
	[XuiXmlBinding("dlc_count")]
	public int DlcCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.allEntries.Count;
		}
	}

	// Token: 0x17000C2C RID: 3116
	// (get) Token: 0x06006DCE RID: 28110 RVA: 0x002AA527 File Offset: 0x002A8727
	[XuiXmlBinding("paging_active")]
	public bool PagingRequired
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.allEntries.Count > base.PageLength;
		}
	}

	// Token: 0x06006DCF RID: 28111 RVA: 0x002AA53C File Offset: 0x002A873C
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

	// Token: 0x06006DD0 RID: 28112 RVA: 0x002AA5CC File Offset: 0x002A87CC
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x0400539C RID: 21404
	public static readonly List<XUiC_DlcList.DlcEntry> DlcEntries = new List<XUiC_DlcList.DlcEntry>
	{
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

	// Token: 0x02000E01 RID: 3585
	[Preserve]
	public class DlcEntry : XUiListEntry<XUiC_DlcList.DlcEntry>
	{
		// Token: 0x06006DD3 RID: 28115 RVA: 0x002AA6F1 File Offset: 0x002A88F1
		public DlcEntry(string _nameKey, string _imageResourceUri, EntitlementSetEnum _dlcSet)
		{
			this.Name = Localization.Get(_nameKey, false, null);
			this.ImageResourceUri = _imageResourceUri;
			this.DlcSet = _dlcSet;
		}

		// Token: 0x06006DD4 RID: 28116 RVA: 0x002AA715 File Offset: 0x002A8915
		public override int CompareTo(XUiC_DlcList.DlcEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return string.Compare(this.Name, _otherEntry.Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0400539D RID: 21405
		public readonly string Name;

		// Token: 0x0400539E RID: 21406
		public readonly string ImageResourceUri;

		// Token: 0x0400539F RID: 21407
		public readonly EntitlementSetEnum DlcSet;
	}

	// Token: 0x02000E02 RID: 3586
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class DlcListEntryController : XUiC_List<XUiC_DlcList.DlcEntry>.XUiC_ListEntry
	{
		// Token: 0x06006DD5 RID: 28117 RVA: 0x002AA730 File Offset: 0x002A8930
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

		// Token: 0x06006DD6 RID: 28118 RVA: 0x002AA7CA File Offset: 0x002A89CA
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

		// Token: 0x06006DD7 RID: 28119 RVA: 0x002AA7FC File Offset: 0x002A89FC
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_DlcList.DlcEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}

		// Token: 0x06006DD8 RID: 28120 RVA: 0x002AA819 File Offset: 0x002A8A19
		[XuiXmlBinding("texture_uri")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingTextureUri()
		{
			XUiC_DlcList.DlcEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.ImageResourceUri : null) ?? "";
		}

		// Token: 0x06006DD9 RID: 28121 RVA: 0x002AA836 File Offset: 0x002A8A36
		[XuiXmlBinding("owned")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingOwned()
		{
			return this.entryData != null && EntitlementManager.Instance.HasEntitlement(this.entryData.DlcSet);
		}

		// Token: 0x040053A0 RID: 21408
		[PublicizedFrom(EAccessModifier.Private)]
		public const float HoverDuration = 0.25f;

		// Token: 0x040053A1 RID: 21409
		[PublicizedFrom(EAccessModifier.Private)]
		public const float HoverScale = 1.15f;

		// Token: 0x040053A2 RID: 21410
		[PublicizedFrom(EAccessModifier.Private)]
		public TweenScale textureScaler;
	}
}
