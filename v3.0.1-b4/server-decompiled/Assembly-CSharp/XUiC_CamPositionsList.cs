using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000EBB RID: 3771
[Preserve]
public class XUiC_CamPositionsList : XUiC_List<XUiC_CamPositionsList.CamPerspectiveEntry>
{
	// Token: 0x17000DE1 RID: 3553
	// (get) Token: 0x0600762D RID: 30253 RVA: 0x002C6803 File Offset: 0x002C4A03
	// (set) Token: 0x0600762E RID: 30254 RVA: 0x002C680B File Offset: 0x002C4A0B
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

	// Token: 0x0600762F RID: 30255 RVA: 0x002C6824 File Offset: 0x002C4A24
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

	// Token: 0x06007630 RID: 30256 RVA: 0x002C6880 File Offset: 0x002C4A80
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
	}

	// Token: 0x06007631 RID: 30257 RVA: 0x002C689C File Offset: 0x002C4A9C
	public void UpdateList()
	{
		int page = base.Page;
		this.RebuildList(false);
		base.Page = page;
	}

	// Token: 0x06007632 RID: 30258 RVA: 0x002C68C0 File Offset: 0x002C4AC0
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

	// Token: 0x06007633 RID: 30259 RVA: 0x002C695C File Offset: 0x002C4B5C
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

	// Token: 0x06007634 RID: 30260 RVA: 0x002C6990 File Offset: 0x002C4B90
	public void OnVisibilityChanged(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		if (_visibleInScene)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x06007635 RID: 30261 RVA: 0x002C699C File Offset: 0x002C4B9C
	public void Add(string _name, string _comment)
	{
		CameraPerspectives.Perspective perspective = new CameraPerspectives.Perspective(_name, this.xui.playerUI.entityPlayer, _comment);
		this.perspectives.Perspectives.Add(perspective.Name, perspective);
		this.perspectives.Save();
		this.UpdateList();
	}

	// Token: 0x0400572D RID: 22317
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CameraPerspectives perspectives = new CameraPerspectives(false);

	// Token: 0x0400572E RID: 22318
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showAddCamPositionWindow;

	// Token: 0x02000EBC RID: 3772
	[Preserve]
	public class CamPerspectiveEntry : XUiListEntry<XUiC_CamPositionsList.CamPerspectiveEntry>
	{
		// Token: 0x06007638 RID: 30264 RVA: 0x002C6A0E File Offset: 0x002C4C0E
		public CamPerspectiveEntry(CameraPerspectives.Perspective _perspective)
		{
			this.Perspective = _perspective;
		}

		// Token: 0x06007639 RID: 30265 RVA: 0x002C6A1D File Offset: 0x002C4C1D
		public override int CompareTo(XUiC_CamPositionsList.CamPerspectiveEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return string.Compare(this.Perspective.Name, _otherEntry.Perspective.Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600763A RID: 30266 RVA: 0x002C6A40 File Offset: 0x002C4C40
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

		// Token: 0x0400572F RID: 22319
		public readonly CameraPerspectives.Perspective Perspective;
	}

	// Token: 0x02000EBD RID: 3773
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class CamPositionsListEntryController : XUiC_List<XUiC_CamPositionsList.CamPerspectiveEntry>.XUiC_ListEntry
	{
		// Token: 0x0600763B RID: 30267 RVA: 0x002C6A78 File Offset: 0x002C4C78
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

		// Token: 0x0600763C RID: 30268 RVA: 0x002C6AF4 File Offset: 0x002C4CF4
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_CamPositionsList.CamPerspectiveEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Perspective.Name : null) ?? "";
		}

		// Token: 0x0600763D RID: 30269 RVA: 0x002C6B16 File Offset: 0x002C4D16
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

		// Token: 0x0600763E RID: 30270 RVA: 0x002C6B41 File Offset: 0x002C4D41
		[XuiXmlBinding("direction")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingDirection()
		{
			XUiC_CamPositionsList.CamPerspectiveEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Perspective.Direction.ToCultureInvariantString() : null) ?? "";
		}

		// Token: 0x0600763F RID: 30271 RVA: 0x002C6B68 File Offset: 0x002C4D68
		[XuiXmlBinding("comment")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingComment()
		{
			XUiC_CamPositionsList.CamPerspectiveEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Perspective.Comment : null) ?? "";
		}

		// Token: 0x04005730 RID: 22320
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_CamPositionsList parentList;
	}
}
