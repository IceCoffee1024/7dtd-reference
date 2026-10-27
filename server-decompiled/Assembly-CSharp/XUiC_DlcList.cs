using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E1A RID: 3610
[Preserve]
public class XUiC_DlcList : XUiC_List<XUiC_DlcList.DlcEntry>
{
	// Token: 0x17000C4C RID: 3148
	// (get) Token: 0x06006DEA RID: 28138 RVA: 0x002AEDD2 File Offset: 0x002ACFD2
	[XuiXmlBinding("dlc_count")]
	public int DlcCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.allEntries.Count;
		}
	}

	// Token: 0x17000C4D RID: 3149
	// (get) Token: 0x06006DEB RID: 28139 RVA: 0x002AEDDF File Offset: 0x002ACFDF
	[XuiXmlBinding("paging_active")]
	public bool PagingRequired
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.allEntries.Count > base.PageLength;
		}
	}

	// Token: 0x06006DEC RID: 28140 RVA: 0x002AEDF4 File Offset: 0x002ACFF4
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

	// Token: 0x06006DED RID: 28141 RVA: 0x002AEE84 File Offset: 0x002AD084
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x040053F8 RID: 21496
	public static readonly List<XUiC_DlcList.DlcEntry> DlcEntries = new List<XUiC_DlcList.DlcEntry>
	{
		new XUiC_DlcList.DlcEntry("armorWraithWoodWitchSet", "Data/DlcInfo/banner_wraith_outfit", EntitlementSetEnum.WraithWoodWitch),
		new XUiC_DlcList.DlcEntry("armorScarletScoundrelSet", "Data/DlcInfo/banner_scarlet_outfit", EntitlementSetEnum.ScarletScoundrel),
		new XUiC_DlcList.DlcEntry("armorRedRidingRogueSet", "Data/DlcInfo/banner_redriding_outfit", EntitlementSetEnum.RedRidingRogue),
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

	// Token: 0x02000E1B RID: 3611
	[Preserve]
	public class DlcEntry : XUiListEntry<XUiC_DlcList.DlcEntry>
	{
		// Token: 0x06006DF0 RID: 28144 RVA: 0x002AF005 File Offset: 0x002AD205
		public DlcEntry(string _nameKey, string _imageResourceUri, EntitlementSetEnum _dlcSet)
		{
			this.Name = Localization.Get(_nameKey, false, null);
			this.ImageResourceUri = _imageResourceUri;
			this.DlcSet = _dlcSet;
		}

		// Token: 0x06006DF1 RID: 28145 RVA: 0x002AF029 File Offset: 0x002AD229
		public override int CompareTo(XUiC_DlcList.DlcEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return string.Compare(this.Name, _otherEntry.Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x040053F9 RID: 21497
		public readonly string Name;

		// Token: 0x040053FA RID: 21498
		public readonly string ImageResourceUri;

		// Token: 0x040053FB RID: 21499
		public readonly EntitlementSetEnum DlcSet;
	}

	// Token: 0x02000E1C RID: 3612
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class DlcListEntryController : XUiC_List<XUiC_DlcList.DlcEntry>.XUiC_ListEntry
	{
		// Token: 0x06006DF2 RID: 28146 RVA: 0x002AF044 File Offset: 0x002AD244
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

		// Token: 0x06006DF3 RID: 28147 RVA: 0x002AF0DE File Offset: 0x002AD2DE
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

		// Token: 0x06006DF4 RID: 28148 RVA: 0x002AF110 File Offset: 0x002AD310
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_DlcList.DlcEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Name : null) ?? "";
		}

		// Token: 0x06006DF5 RID: 28149 RVA: 0x002AF12D File Offset: 0x002AD32D
		[XuiXmlBinding("texture_uri")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingTextureUri()
		{
			XUiC_DlcList.DlcEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.ImageResourceUri : null) ?? "";
		}

		// Token: 0x06006DF6 RID: 28150 RVA: 0x002AF14A File Offset: 0x002AD34A
		[XuiXmlBinding("owned")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingOwned()
		{
			return this.entryData != null && EntitlementManager.Instance.HasEntitlement(this.entryData.DlcSet);
		}

		// Token: 0x040053FC RID: 21500
		[PublicizedFrom(EAccessModifier.Private)]
		public const float HoverDuration = 0.25f;

		// Token: 0x040053FD RID: 21501
		[PublicizedFrom(EAccessModifier.Private)]
		public const float HoverScale = 1.15f;

		// Token: 0x040053FE RID: 21502
		[PublicizedFrom(EAccessModifier.Private)]
		public TweenScale textureScaler;
	}
}
