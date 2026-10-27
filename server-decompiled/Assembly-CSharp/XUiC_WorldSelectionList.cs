using System;
using UnityEngine.Scripting;

// Token: 0x02000E7F RID: 3711
[Preserve]
public class XUiC_WorldSelectionList : XUiC_List<XUiC_WorldSelectionList.Entry>
{
	// Token: 0x060071B6 RID: 29110 RVA: 0x002BDAA2 File Offset: 0x002BBCA2
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x060071B7 RID: 29111 RVA: 0x002BDAB1 File Offset: 0x002BBCB1
	public override void OnClose()
	{
		base.OnClose();
	}

	// Token: 0x060071B8 RID: 29112 RVA: 0x002BDABC File Offset: 0x002BBCBC
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		foreach (PathAbstractions.AbstractedLocation abstractedLocation in PathAbstractions.WorldsSearchPaths.GetAvailablePathsList(null, false, null))
		{
			GameUtils.WorldInfo worldInfo = GameUtils.WorldInfo.LoadWorldInfo(abstractedLocation);
			if (worldInfo != null)
			{
				this.allEntries.Add(new XUiC_WorldSelectionList.Entry(abstractedLocation, worldInfo.GameVersionCreated));
			}
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x02000E80 RID: 3712
	[Preserve]
	public class Entry : XUiListEntry<XUiC_WorldSelectionList.Entry>
	{
		// Token: 0x060071BA RID: 29114 RVA: 0x002BDB60 File Offset: 0x002BBD60
		public Entry(PathAbstractions.AbstractedLocation _location, VersionInformation _version)
		{
			this.Location = _location;
			this.Version = _version;
		}

		// Token: 0x060071BB RID: 29115 RVA: 0x002BDB76 File Offset: 0x002BBD76
		public override int CompareTo(XUiC_WorldSelectionList.Entry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return this.Location.CompareTo(_otherEntry.Location);
		}

		// Token: 0x060071BC RID: 29116 RVA: 0x002BDB8E File Offset: 0x002BBD8E
		public override bool MatchesSearch(string _searchString)
		{
			return this.Location.Name.EqualsCaseInsensitive(_searchString);
		}

		// Token: 0x040055F9 RID: 22009
		public readonly PathAbstractions.AbstractedLocation Location;

		// Token: 0x040055FA RID: 22010
		public readonly VersionInformation Version;
	}

	// Token: 0x02000E81 RID: 3713
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_WorldSelectionList.Entry>.XUiC_ListEntry
	{
		// Token: 0x060071BD RID: 29117 RVA: 0x002BDBA1 File Offset: 0x002BBDA1
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			if (this.entryData != null)
			{
				return this.entryData.Location.Name;
			}
			return "";
		}

		// Token: 0x060071BE RID: 29118 RVA: 0x002BDBC1 File Offset: 0x002BBDC1
		[XuiXmlBinding("location")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingLocation()
		{
			if (this.entryData == null)
			{
				return "";
			}
			return this.entryData.Location.GetLocationTypeDisplayString();
		}

		// Token: 0x060071BF RID: 29119 RVA: 0x002BDBE4 File Offset: 0x002BBDE4
		[XuiXmlBinding("version")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingVersion()
		{
			if (this.entryData == null)
			{
				return "";
			}
			if (!this.entryData.Version.IsValid)
			{
				return VersionInformation.EGameReleaseType.V.ToString() + " " + 3.ToString();
			}
			return this.entryData.Version.LongStringNoBuild;
		}
	}
}
