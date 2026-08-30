using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000ED0 RID: 3792
[Preserve]
public class XUiC_CamPositionsList : XUiC_List<XUiC_CamPositionsList.CamPerspectiveEntry>
{
	// Token: 0x17000DFB RID: 3579
	// (get) Token: 0x060076CE RID: 30414 RVA: 0x002C9D27 File Offset: 0x002C7F27
	// (set) Token: 0x060076CF RID: 30415 RVA: 0x002C9D2F File Offset: 0x002C7F2F
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

	// Token: 0x060076D0 RID: 30416 RVA: 0x002C9D48 File Offset: 0x002C7F48
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

	// Token: 0x060076D1 RID: 30417 RVA: 0x002C9DA4 File Offset: 0x002C7FA4
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
	}

	// Token: 0x060076D2 RID: 30418 RVA: 0x002C9DC0 File Offset: 0x002C7FC0
	public void UpdateList()
	{
		int page = base.Page;
		this.RebuildList(false);
		base.Page = page;
	}

	// Token: 0x060076D3 RID: 30419 RVA: 0x002C9DE4 File Offset: 0x002C7FE4
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

	// Token: 0x060076D4 RID: 30420 RVA: 0x002C9E80 File Offset: 0x002C8080
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

	// Token: 0x060076D5 RID: 30421 RVA: 0x002C9EB4 File Offset: 0x002C80B4
	public void OnVisibilityChanged(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		if (_visibleInScene)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x060076D6 RID: 30422 RVA: 0x002C9EC0 File Offset: 0x002C80C0
	public void Add(string _name, string _comment)
	{
		CameraPerspectives.Perspective perspective = new CameraPerspectives.Perspective(_name, this.xui.playerUI.entityPlayer, _comment);
		this.perspectives.Perspectives.Add(perspective.Name, perspective);
		this.perspectives.Save();
		this.UpdateList();
	}

	// Token: 0x040057D2 RID: 22482
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CameraPerspectives perspectives = new CameraPerspectives(false);

	// Token: 0x040057D3 RID: 22483
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showAddCamPositionWindow;

	// Token: 0x02000ED1 RID: 3793
	[Preserve]
	public class CamPerspectiveEntry : XUiListEntry<XUiC_CamPositionsList.CamPerspectiveEntry>
	{
		// Token: 0x060076D9 RID: 30425 RVA: 0x002C9F32 File Offset: 0x002C8132
		public CamPerspectiveEntry(CameraPerspectives.Perspective _perspective)
		{
			this.Perspective = _perspective;
		}

		// Token: 0x060076DA RID: 30426 RVA: 0x002C9F41 File Offset: 0x002C8141
		public override int CompareTo(XUiC_CamPositionsList.CamPerspectiveEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return string.Compare(this.Perspective.Name, _otherEntry.Perspective.Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060076DB RID: 30427 RVA: 0x002C9F64 File Offset: 0x002C8164
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

		// Token: 0x040057D4 RID: 22484
		public readonly CameraPerspectives.Perspective Perspective;
	}

	// Token: 0x02000ED2 RID: 3794
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class CamPositionsListEntryController : XUiC_List<XUiC_CamPositionsList.CamPerspectiveEntry>.XUiC_ListEntry
	{
		// Token: 0x060076DC RID: 30428 RVA: 0x002C9F9C File Offset: 0x002C819C
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

		// Token: 0x060076DD RID: 30429 RVA: 0x002CA018 File Offset: 0x002C8218
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_CamPositionsList.CamPerspectiveEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Perspective.Name : null) ?? "";
		}

		// Token: 0x060076DE RID: 30430 RVA: 0x002CA03A File Offset: 0x002C823A
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

		// Token: 0x060076DF RID: 30431 RVA: 0x002CA065 File Offset: 0x002C8265
		[XuiXmlBinding("direction")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingDirection()
		{
			XUiC_CamPositionsList.CamPerspectiveEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Perspective.Direction.ToCultureInvariantString() : null) ?? "";
		}

		// Token: 0x060076E0 RID: 30432 RVA: 0x002CA08C File Offset: 0x002C828C
		[XuiXmlBinding("comment")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingComment()
		{
			XUiC_CamPositionsList.CamPerspectiveEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Perspective.Comment : null) ?? "";
		}

		// Token: 0x040057D5 RID: 22485
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_CamPositionsList parentList;
	}
}
