using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200106F RID: 4207
[Preserve]
public class XUiC_SignStackGrid : XUiController
{
	// Token: 0x17000FAC RID: 4012
	// (get) Token: 0x06008529 RID: 34089 RVA: 0x0032C7D4 File Offset: 0x0032A9D4
	// (set) Token: 0x0600852A RID: 34090 RVA: 0x0032C7DC File Offset: 0x0032A9DC
	public int Length
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FAD RID: 4013
	// (get) Token: 0x0600852B RID: 34091 RVA: 0x0032C7E5 File Offset: 0x0032A9E5
	// (set) Token: 0x0600852C RID: 34092 RVA: 0x0032C7ED File Offset: 0x0032A9ED
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

	// Token: 0x0600852D RID: 34093 RVA: 0x0032C7FD File Offset: 0x0032A9FD
	public override void Init()
	{
		base.Init();
		this.signStackControllers = base.GetChildrenByType<XUiC_SignStack>(null);
		this.Length = this.signStackControllers.Length;
		this.IsDirty = false;
	}

	// Token: 0x0600852E RID: 34094 RVA: 0x0032C828 File Offset: 0x0032AA28
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

	// Token: 0x0600852F RID: 34095 RVA: 0x003012C2 File Offset: 0x002FF4C2
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
	}

	// Token: 0x06008530 RID: 34096 RVA: 0x002EE8C1 File Offset: 0x002ECAC1
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x06008531 RID: 34097 RVA: 0x0032C8DF File Offset: 0x0032AADF
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.SetSignIds(this.currentList, this.selectedId);
			this.IsDirty = false;
		}
	}

	// Token: 0x040062CC RID: 25292
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x040062CD RID: 25293
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x040062CE RID: 25294
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x040062CF RID: 25295
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SignStack[] signStackControllers;

	// Token: 0x040062D0 RID: 25296
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId selectedId;

	// Token: 0x040062D1 RID: 25297
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GlobalSignId> currentList;
}
