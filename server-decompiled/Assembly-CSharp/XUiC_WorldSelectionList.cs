using System;
using UnityEngine.Scripting;

// Token: 0x02000E74 RID: 3700
[Preserve]
public class XUiC_WorldSelectionList : XUiC_List<XUiC_WorldSelectionList.Entry>
{
	// Token: 0x06007191 RID: 29073 RVA: 0x002BC536 File Offset: 0x002BA736
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x06007192 RID: 29074 RVA: 0x002BC545 File Offset: 0x002BA745
	public override void OnClose()
	{
		base.OnClose();
	}

	// Token: 0x06007193 RID: 29075 RVA: 0x002BC550 File Offset: 0x002BA750
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

	// Token: 0x02000E75 RID: 3701
	[Preserve]
	public class Entry : XUiListEntry<XUiC_WorldSelectionList.Entry>
	{
		// Token: 0x06007195 RID: 29077 RVA: 0x002BC5F4 File Offset: 0x002BA7F4
		public Entry(PathAbstractions.AbstractedLocation _location, VersionInformation _version)
		{
			this.Location = _location;
			this.Version = _version;
		}

		// Token: 0x06007196 RID: 29078 RVA: 0x002BC60A File Offset: 0x002BA80A
		public override int CompareTo(XUiC_WorldSelectionList.Entry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return this.Location.CompareTo(_otherEntry.Location);
		}

		// Token: 0x06007197 RID: 29079 RVA: 0x002BC622 File Offset: 0x002BA822
		public override bool MatchesSearch(string _searchString)
		{
			return this.Location.Name.EqualsCaseInsensitive(_searchString);
		}

		// Token: 0x040055D4 RID: 21972
		public readonly PathAbstractions.AbstractedLocation Location;

		// Token: 0x040055D5 RID: 21973
		public readonly VersionInformation Version;
	}

	// Token: 0x02000E76 RID: 3702
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_WorldSelectionList.Entry>.XUiC_ListEntry
	{
		// Token: 0x06007198 RID: 29080 RVA: 0x002BC635 File Offset: 0x002BA835
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

		// Token: 0x06007199 RID: 29081 RVA: 0x002BC655 File Offset: 0x002BA855
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

		// Token: 0x0600719A RID: 29082 RVA: 0x002BC678 File Offset: 0x002BA878
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
