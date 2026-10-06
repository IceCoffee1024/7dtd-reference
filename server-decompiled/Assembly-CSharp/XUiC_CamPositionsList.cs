using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000EDF RID: 3807
[Preserve]
public class XUiC_CamPositionsList : XUiC_List<XUiC_CamPositionsList.CamPerspectiveEntry>
{
	// Token: 0x17000E18 RID: 3608
	// (get) Token: 0x060076C9 RID: 30409 RVA: 0x002CD0FF File Offset: 0x002CB2FF
	// (set) Token: 0x060076CA RID: 30410 RVA: 0x002CD107 File Offset: 0x002CB307
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

	// Token: 0x060076CB RID: 30411 RVA: 0x002CD120 File Offset: 0x002CB320
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

	// Token: 0x060076CC RID: 30412 RVA: 0x002CD17C File Offset: 0x002CB37C
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
	}

	// Token: 0x060076CD RID: 30413 RVA: 0x002CD198 File Offset: 0x002CB398
	public void UpdateList()
	{
		int page = base.Page;
		this.RebuildList(false);
		base.Page = page;
	}

	// Token: 0x060076CE RID: 30414 RVA: 0x002CD1BC File Offset: 0x002CB3BC
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

	// Token: 0x060076CF RID: 30415 RVA: 0x002CD258 File Offset: 0x002CB458
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

	// Token: 0x060076D0 RID: 30416 RVA: 0x002CD28C File Offset: 0x002CB48C
	public void OnVisibilityChanged(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		if (_visibleInScene)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x060076D1 RID: 30417 RVA: 0x002CD298 File Offset: 0x002CB498
	public void Add(string _name, string _comment)
	{
		CameraPerspectives.Perspective perspective = new CameraPerspectives.Perspective(_name, this.xui.playerUI.entityPlayer, _comment);
		this.perspectives.Perspectives.Add(perspective.Name, perspective);
		this.perspectives.Save();
		this.UpdateList();
	}

	// Token: 0x0400580A RID: 22538
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CameraPerspectives perspectives = new CameraPerspectives(false);

	// Token: 0x0400580B RID: 22539
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showAddCamPositionWindow;

	// Token: 0x02000EE0 RID: 3808
	[Preserve]
	public class CamPerspectiveEntry : XUiListEntry<XUiC_CamPositionsList.CamPerspectiveEntry>
	{
		// Token: 0x060076D4 RID: 30420 RVA: 0x002CD30A File Offset: 0x002CB50A
		public CamPerspectiveEntry(CameraPerspectives.Perspective _perspective)
		{
			this.Perspective = _perspective;
		}

		// Token: 0x060076D5 RID: 30421 RVA: 0x002CD319 File Offset: 0x002CB519
		public override int CompareTo(XUiC_CamPositionsList.CamPerspectiveEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			return string.Compare(this.Perspective.Name, _otherEntry.Perspective.Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060076D6 RID: 30422 RVA: 0x002CD33C File Offset: 0x002CB53C
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

		// Token: 0x0400580C RID: 22540
		public readonly CameraPerspectives.Perspective Perspective;
	}

	// Token: 0x02000EE1 RID: 3809
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class CamPositionsListEntryController : XUiC_List<XUiC_CamPositionsList.CamPerspectiveEntry>.XUiC_ListEntry
	{
		// Token: 0x060076D7 RID: 30423 RVA: 0x002CD374 File Offset: 0x002CB574
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

		// Token: 0x060076D8 RID: 30424 RVA: 0x002CD3F0 File Offset: 0x002CB5F0
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_CamPositionsList.CamPerspectiveEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Perspective.Name : null) ?? "";
		}

		// Token: 0x060076D9 RID: 30425 RVA: 0x002CD412 File Offset: 0x002CB612
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

		// Token: 0x060076DA RID: 30426 RVA: 0x002CD43D File Offset: 0x002CB63D
		[XuiXmlBinding("direction")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingDirection()
		{
			XUiC_CamPositionsList.CamPerspectiveEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Perspective.Direction.ToCultureInvariantString() : null) ?? "";
		}

		// Token: 0x060076DB RID: 30427 RVA: 0x002CD464 File Offset: 0x002CB664
		[XuiXmlBinding("comment")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingComment()
		{
			XUiC_CamPositionsList.CamPerspectiveEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.Perspective.Comment : null) ?? "";
		}

		// Token: 0x0400580D RID: 22541
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_CamPositionsList parentList;
	}
}
