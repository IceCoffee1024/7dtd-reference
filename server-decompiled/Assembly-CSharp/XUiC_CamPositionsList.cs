using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000EEA RID: 3818
[Preserve]
public class XUiC_CamPositionsList : XUiC_List<XUiC_CamPositionsList.CamPerspectiveEntry>
{
	// Token: 0x17000E1C RID: 3612
	// (get) Token: 0x060076EE RID: 30446 RVA: 0x002CE5BB File Offset: 0x002CC7BB
	// (set) Token: 0x060076EF RID: 30447 RVA: 0x002CE5C3 File Offset: 0x002CC7C3
	public bool ShowAddCamPositionWindow
	{
		get
		{
			return this.showAddCamPositionWindow;
		}
		set
		{
			if (value == this.showAddCamPositionWindow)
			{
				return;
			}
			this.showAddCamPositionWindow = value;
			base.RefreshBindings();
		}
	}

	// Token: 0x060076F0 RID: 30448 RVA: 0x002CE5DC File Offset: 0x002CC7DC
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("btnAddCamPosition");
		XUiV_Button xuiV_Button = ((childById != null) ? childById.ViewComponent : null) as XUiV_Button;
		if (xuiV_Button != null)
		{
			xuiV_Button.Controller.OnPress += delegate(XUiController _, int _)
			{
				this.ShowAddCamPositionWindow = !this.ShowAddCamPositionWindow;
			};
		}
		base.OnVisiblity += this.OnVisibilityChanged;
	}

	// Token: 0x060076F1 RID: 30449 RVA: 0x002CE638 File Offset: 0x002CC838
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
	}

	// Token: 0x060076F2 RID: 30450 RVA: 0x002CE654 File Offset: 0x002CC854
	public void UpdateList()
	{
		int page = base.Page;
		this.RebuildList(false);
		base.Page = page;
	}

	// Token: 0x060076F3 RID: 30451 RVA: 0x002CE678 File Offset: 0x002CC878
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		this.perspectives.Load();
		foreach (KeyValuePair<string, CameraPerspectives.Perspective> keyValuePair in this.perspectives.Perspectives)
		{
			string text;
			CameraPerspectives.Perspective perspective;
			keyValuePair.Deconstruct(out text, out perspective);
			CameraPerspectives.Perspective perspective2 = perspective;
			this.allEntries.Add(new XUiC_CamPositionsList.CamPerspectiveEntry(perspective2));
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x060076F4 RID: 30452 RVA: 0x002CE714 File Offset: 0x002CC914
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "cam_position_add_open")
		{
			_value = this.ShowAddCamPositionWindow.ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x060076F5 RID: 30453 RVA: 0x002CE748 File Offset: 0x002CC948
	public void OnVisibilityChanged(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		if (_visibleInScene)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x060076F6 RID: 30454 RVA: 0x002CE754 File Offset: 0x002CC954
	public void Add(string _name, string _comment)
	{
		CameraPerspectives.Perspective perspective = new CameraPerspectives.Perspective(_name, this.xui.playerUI.entityPlayer, _comment);
		this.perspectives.Perspectives.Add(perspective.Name, perspective);
		this.perspectives.Save();
		this.UpdateList();
	}

	// Token: 0x0400582F RID: 22575
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CameraPerspectives perspectives = new CameraPerspectives(false);

	// Token: 0x04005830 RID: 22576
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showAddCamPositionWindow;

	// Token: 0x02000EEB RID: 3819
	[Preserve]
	public class CamPerspectiveEntry : XUiListEntry<XUiC_CamPositionsList.CamPerspectiveEntry>
	{
		// Token: 0x060076F9 RID: 30457 RVA: 0x002CE7C6 File Offset: 0x002CC9C6
		public CamPerspectiveEntry(CameraPerspectives.Perspective _perspective)
		{
			this.Perspective = _perspective;
		}

		// Token: 0x060076FA RID: 30458 RVA: 0x002CE7D5 File Offset: 0x002CC9D5
		public override int CompareTo(XUiC_CamPositionsList.CamPerspectiveEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return string.Compare(this.Perspective.Name, _otherEntry.Perspective.Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060076FB RID: 30459 RVA: 0x002CE7F8 File Offset: 0x002CC9F8
		public override bool MatchesSearch(string _searchString)
		{
			if (string.IsNullOrEmpty(_searchString))
			{
				return true;
			}
			if (!this.Perspective.Name.ContainsCaseInsensitive(_searchString))
			{
				string comment = this.Perspective.Comment;
				return comment != null && comment.ContainsCaseInsensitive(_searchString);
			}
			return true;
		}

		// Token: 0x04005831 RID: 22577
		public readonly CameraPerspectives.Perspective Perspective;
	}

	// Token: 0x02000EEC RID: 3820
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class CamPositionsListEntryController : XUiC_List<XUiC_CamPositionsList.CamPerspectiveEntry>.XUiC_ListEntry
	{
		// Token: 0x060076FC RID: 30460 RVA: 0x002CE830 File Offset: 0x002CCA30
		public override void Init()
		{
			base.Init();
			this.parentList = base.GetParentByType<XUiC_CamPositionsList>();
			XUiC_SimpleButton xuiC_SimpleButton = base.GetChildById("camButton") as XUiC_SimpleButton;
			if (xuiC_SimpleButton != null)
			{
				xuiC_SimpleButton.OnPressed += delegate(XUiController _, int _)
				{
					base.GetEntry().Perspective.ToPlayer(this.xui.playerUI.entityPlayer);
				};
			}
			XUiController childById = base.GetChildById("btnDelete");
			XUiV_Button xuiV_Button = ((childById != null) ? childById.ViewComponent : null) as XUiV_Button;
			if (xuiV_Button != null)
			{
				xuiV_Button.Controller.OnPress += delegate(XUiController _, int _)
				{
					XUiC_CamPositionsList.CamPerspectiveEntry entry = base.GetEntry();
					this.parentList.perspectives.Perspectives.Remove(entry.Perspective.Name);
					this.parentList.perspectives.Save();
					this.parentList.UpdateList();
				};
			}
		}

		// Token: 0x060076FD RID: 30461 RVA: 0x002CE8AC File Offset: 0x002CCAAC
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_CamPositionsList.CamPerspectiveEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Perspective.Name : null) ?? "";
		}

		// Token: 0x060076FE RID: 30462 RVA: 0x002CE8CE File Offset: 0x002CCACE
		[XuiXmlBinding("position")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingPosition()
		{
			if (this.entryData != null)
			{
				return ValueDisplayFormatters.WorldPos(this.entryData.Perspective.Position, " ", false);
			}
			return "";
		}

		// Token: 0x060076FF RID: 30463 RVA: 0x002CE8F9 File Offset: 0x002CCAF9
		[XuiXmlBinding("direction")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingDirection()
		{
			XUiC_CamPositionsList.CamPerspectiveEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Perspective.Direction.ToCultureInvariantString() : null) ?? "";
		}

		// Token: 0x06007700 RID: 30464 RVA: 0x002CE920 File Offset: 0x002CCB20
		[XuiXmlBinding("comment")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingComment()
		{
			XUiC_CamPositionsList.CamPerspectiveEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Perspective.Comment : null) ?? "";
		}

		// Token: 0x04005832 RID: 22578
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_CamPositionsList parentList;
	}
}
