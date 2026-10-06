using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200105F RID: 4191
[Preserve]
public class XUiC_SignStackGrid : XUiController
{
	// Token: 0x17000FA1 RID: 4001
	// (get) Token: 0x060084D7 RID: 34007 RVA: 0x0032AAD0 File Offset: 0x00328CD0
	// (set) Token: 0x060084D8 RID: 34008 RVA: 0x0032AAD8 File Offset: 0x00328CD8
	public int Length
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FA2 RID: 4002
	// (get) Token: 0x060084D9 RID: 34009 RVA: 0x0032AAE1 File Offset: 0x00328CE1
	// (set) Token: 0x060084DA RID: 34010 RVA: 0x0032AAE9 File Offset: 0x00328CE9
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

	// Token: 0x060084DB RID: 34011 RVA: 0x0032AAF9 File Offset: 0x00328CF9
	public override void Init()
	{
		base.Init();
		this.signStackControllers = base.GetChildrenByType<XUiC_SignStack>(null);
		this.Length = this.signStackControllers.Length;
		this.IsDirty = false;
	}

	// Token: 0x060084DC RID: 34012 RVA: 0x0032AB24 File Offset: 0x00328D24
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

	// Token: 0x060084DD RID: 34013 RVA: 0x002FF7A6 File Offset: 0x002FD9A6
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
	}

	// Token: 0x060084DE RID: 34014 RVA: 0x002ECFC5 File Offset: 0x002EB1C5
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x060084DF RID: 34015 RVA: 0x0032ABDB File Offset: 0x00328DDB
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.SetSignIds(this.currentList, this.selectedId);
			this.IsDirty = false;
		}
	}

	// Token: 0x0400628E RID: 25230
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x0400628F RID: 25231
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x04006290 RID: 25232
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006291 RID: 25233
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SignStack[] signStackControllers;

	// Token: 0x04006292 RID: 25234
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId selectedId;

	// Token: 0x04006293 RID: 25235
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GlobalSignId> currentList;
}
