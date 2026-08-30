using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200104E RID: 4174
[Preserve]
public class XUiC_SignStackGrid : XUiController
{
	// Token: 0x17000F6B RID: 3947
	// (get) Token: 0x06008491 RID: 33937 RVA: 0x00326EC5 File Offset: 0x003250C5
	// (set) Token: 0x06008492 RID: 33938 RVA: 0x00326ECD File Offset: 0x003250CD
	public int Length
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F6C RID: 3948
	// (get) Token: 0x06008493 RID: 33939 RVA: 0x00326ED6 File Offset: 0x003250D6
	// (set) Token: 0x06008494 RID: 33940 RVA: 0x00326EDE File Offset: 0x003250DE
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

	// Token: 0x06008495 RID: 33941 RVA: 0x00326EEE File Offset: 0x003250EE
	public override void Init()
	{
		base.Init();
		this.signStackControllers = base.GetChildrenByType<XUiC_SignStack>(null);
		this.Length = this.signStackControllers.Length;
		this.IsDirty = false;
	}

	// Token: 0x06008496 RID: 33942 RVA: 0x00326F18 File Offset: 0x00325118
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

	// Token: 0x06008497 RID: 33943 RVA: 0x002FC752 File Offset: 0x002FA952
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
	}

	// Token: 0x06008498 RID: 33944 RVA: 0x002E9CB4 File Offset: 0x002E7EB4
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x06008499 RID: 33945 RVA: 0x00326FCF File Offset: 0x003251CF
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.SetSignIds(this.currentList, this.selectedId);
			this.IsDirty = false;
		}
	}

	// Token: 0x04006249 RID: 25161
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x0400624A RID: 25162
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x0400624B RID: 25163
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x0400624C RID: 25164
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SignStack[] signStackControllers;

	// Token: 0x0400624D RID: 25165
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId selectedId;

	// Token: 0x0400624E RID: 25166
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GlobalSignId> currentList;
}
