using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001035 RID: 4149
[Preserve]
public class XUiC_SignStackGrid : XUiController
{
	// Token: 0x17000F32 RID: 3890
	// (get) Token: 0x0600839F RID: 33695 RVA: 0x0032269D File Offset: 0x0032089D
	// (set) Token: 0x060083A0 RID: 33696 RVA: 0x003226A5 File Offset: 0x003208A5
	public int Length
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F33 RID: 3891
	// (get) Token: 0x060083A1 RID: 33697 RVA: 0x003226AE File Offset: 0x003208AE
	// (set) Token: 0x060083A2 RID: 33698 RVA: 0x003226B6 File Offset: 0x003208B6
	public int Page
	{
		get
		{
			return this.page;
		}
		set
		{
			this.page = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x060083A3 RID: 33699 RVA: 0x003226C6 File Offset: 0x003208C6
	public override void Init()
	{
		base.Init();
		this.signStackControllers = base.GetChildrenByType<XUiC_SignStack>(null);
		this.Length = this.signStackControllers.Length;
		this.IsDirty = false;
	}

	// Token: 0x060083A4 RID: 33700 RVA: 0x003226F0 File Offset: 0x003208F0
	public void SetSignIds(List<GlobalSignId> signIdList, GlobalSignId newSelectedId)
	{
		this.xui.GetChildByType<XUiC_SignGalleryWindow>();
		int count = signIdList.Count;
		this.currentList = signIdList;
		this.selectedId = newSelectedId;
		for (int i = 0; i < this.Length; i++)
		{
			int num = i + this.Length * this.page;
			XUiC_SignStack xuiC_SignStack = this.signStackControllers[i];
			if (num < count)
			{
				xuiC_SignStack.SignId = signIdList[num];
				if (xuiC_SignStack.SignId == this.selectedId)
				{
					xuiC_SignStack.IsSelected = true;
				}
				else if (xuiC_SignStack.IsSelected)
				{
					xuiC_SignStack.IsSelected = false;
				}
			}
			else
			{
				xuiC_SignStack.SignId = GlobalSignId.InvalidId;
				if (xuiC_SignStack.IsSelected)
				{
					xuiC_SignStack.IsSelected = false;
				}
			}
		}
		this.IsDirty = false;
	}

	// Token: 0x060083A5 RID: 33701 RVA: 0x002F7F7E File Offset: 0x002F617E
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
	}

	// Token: 0x060083A6 RID: 33702 RVA: 0x002E5E28 File Offset: 0x002E4028
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x060083A7 RID: 33703 RVA: 0x003227A7 File Offset: 0x003209A7
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.SetSignIds(this.currentList, this.selectedId);
			this.IsDirty = false;
		}
	}

	// Token: 0x04006182 RID: 24962
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x04006183 RID: 24963
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x04006184 RID: 24964
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006185 RID: 24965
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SignStack[] signStackControllers;

	// Token: 0x04006186 RID: 24966
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId selectedId;

	// Token: 0x04006187 RID: 24967
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GlobalSignId> currentList;
}
