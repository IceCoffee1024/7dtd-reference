using System;
using UnityEngine.Scripting;

// Token: 0x02000E65 RID: 3685
[Preserve]
public class XUiC_WorldSelectionList : XUiC_List<XUiC_WorldSelectionList.Entry>
{
	// Token: 0x06007199 RID: 29081 RVA: 0x002B9186 File Offset: 0x002B7386
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
	}

	// Token: 0x0600719A RID: 29082 RVA: 0x002B9195 File Offset: 0x002B7395
	public override void OnClose()
	{
		base.OnClose();
	}

	// Token: 0x0600719B RID: 29083 RVA: 0x002B91A0 File Offset: 0x002B73A0
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

	// Token: 0x02000E66 RID: 3686
	[Preserve]
	public class Entry : XUiListEntry<XUiC_WorldSelectionList.Entry>
	{
		// Token: 0x0600719D RID: 29085 RVA: 0x002B9244 File Offset: 0x002B7444
		public Entry(PathAbstractions.AbstractedLocation _location, VersionInformation _version)
		{
			this.Location = _location;
			this.Version = _version;
		}

		// Token: 0x0600719E RID: 29086 RVA: 0x002B925A File Offset: 0x002B745A
		public override int CompareTo(XUiC_WorldSelectionList.Entry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return this.Location.CompareTo(_otherEntry.Location);
		}

		// Token: 0x0600719F RID: 29087 RVA: 0x002B9272 File Offset: 0x002B7472
		public override bool MatchesSearch(string _searchString)
		{
			return this.Location.Name.EqualsCaseInsensitive(_searchString);
		}

		// Token: 0x0400559D RID: 21917
		public readonly PathAbstractions.AbstractedLocation Location;

		// Token: 0x0400559E RID: 21918
		public readonly VersionInformation Version;
	}

	// Token: 0x02000E67 RID: 3687
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_WorldSelectionList.Entry>.XUiC_ListEntry
	{
		// Token: 0x060071A0 RID: 29088 RVA: 0x002B9285 File Offset: 0x002B7485
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

		// Token: 0x060071A1 RID: 29089 RVA: 0x002B92A5 File Offset: 0x002B74A5
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

		// Token: 0x060071A2 RID: 29090 RVA: 0x002B92C8 File Offset: 0x002B74C8
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
