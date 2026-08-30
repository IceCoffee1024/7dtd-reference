using System;
using UnityEngine.Scripting;

// Token: 0x02000E50 RID: 3664
[Preserve]
public class XUiC_WorldSelectionList : XUiC_List<XUiC_WorldSelectionList.Entry>
{
	// Token: 0x060070F9 RID: 28921 RVA: 0x002B5C62 File Offset: 0x002B3E62
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x060070FA RID: 28922 RVA: 0x002B5C71 File Offset: 0x002B3E71
	public override void OnClose()
	{
		base.OnClose();
	}

	// Token: 0x060070FB RID: 28923 RVA: 0x002B5C7C File Offset: 0x002B3E7C
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

	// Token: 0x02000E51 RID: 3665
	[Preserve]
	public class Entry : XUiListEntry<XUiC_WorldSelectionList.Entry>
	{
		// Token: 0x060070FD RID: 28925 RVA: 0x002B5D20 File Offset: 0x002B3F20
		public Entry(PathAbstractions.AbstractedLocation _location, VersionInformation _version)
		{
			this.Location = _location;
			this.Version = _version;
		}

		// Token: 0x060070FE RID: 28926 RVA: 0x002B5D36 File Offset: 0x002B3F36
		public override int CompareTo(XUiC_WorldSelectionList.Entry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return this.Location.CompareTo(_otherEntry.Location);
		}

		// Token: 0x060070FF RID: 28927 RVA: 0x002B5D4E File Offset: 0x002B3F4E
		public override bool MatchesSearch(string _searchString)
		{
			return this.Location.Name.EqualsCaseInsensitive(_searchString);
		}

		// Token: 0x040054F9 RID: 21753
		public readonly PathAbstractions.AbstractedLocation Location;

		// Token: 0x040054FA RID: 21754
		public readonly VersionInformation Version;
	}

	// Token: 0x02000E52 RID: 3666
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_WorldSelectionList.Entry>.XUiC_ListEntry
	{
		// Token: 0x06007100 RID: 28928 RVA: 0x002B5D61 File Offset: 0x002B3F61
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

		// Token: 0x06007101 RID: 28929 RVA: 0x002B5D81 File Offset: 0x002B3F81
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

		// Token: 0x06007102 RID: 28930 RVA: 0x002B5DA4 File Offset: 0x002B3FA4
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
