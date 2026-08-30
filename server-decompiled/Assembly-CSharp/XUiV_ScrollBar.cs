using System;
using UnityEngine;

// Token: 0x0200115E RID: 4446
public class XUiV_ScrollBar : XUiView
{
	// Token: 0x170010D2 RID: 4306
	// (get) Token: 0x06008DC6 RID: 36294 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return null;
		}
	}

	// Token: 0x170010D3 RID: 4307
	// (get) Token: 0x06008DC7 RID: 36295 RVA: 0x0035804C File Offset: 0x0035624C
	public UIScrollBar ScrollBar
	{
		get
		{
			return this.scrollBar;
		}
	}

	// Token: 0x170010D4 RID: 4308
	// (get) Token: 0x06008DC8 RID: 36296 RVA: 0x00358054 File Offset: 0x00356254
	// (set) Token: 0x06008DC9 RID: 36297 RVA: 0x00358061 File Offset: 0x00356261
	public float ScrollPosition
	{
		get
		{
			return this.scrollBar.value;
		}
		set
		{
			this.scrollBar.value = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010D5 RID: 4309
	// (get) Token: 0x06008DCA RID: 36298 RVA: 0x00358075 File Offset: 0x00356275
	public override Vector3[] WorldCorners
	{
		get
		{
			if (this.background != null)
			{
				return this.background.WorldCorners;
			}
			XUiV_Sprite xuiV_Sprite = this.thumb;
			if (xuiV_Sprite == null)
			{
				return null;
			}
			return xuiV_Sprite.WorldCorners;
		}
	}

	// Token: 0x06008DCB RID: 36299 RVA: 0x00355D1D File Offset: 0x00353F1D
	public XUiV_ScrollBar(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008DCC RID: 36300 RVA: 0x0035809C File Offset: 0x0035629C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<UIScrollBar>();
	}

	// Token: 0x06008DCD RID: 36301 RVA: 0x003580AC File Offset: 0x003562AC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.scrollBar = this.uiTransform.gameObject.GetComponent<UIScrollBar>();
		this.scrollBar.value = 0f;
	}

	// Token: 0x06008DCE RID: 36302 RVA: 0x003580DC File Offset: 0x003562DC
	public override void InitView()
	{
		base.InitView();
		if (!base.Controller.TryGetChildView<XUiV_Sprite>("thumb", out this.thumb))
		{
			Log.Error("[XUi] XUiV_ScrollBar without child sprite 'thumb'. Hierarchy: " + base.GetXuiHierarchy());
			return;
		}
		if (!this.thumb.HasAnyEvent)
		{
			Log.Error("[XUi] XUiV_ScrollBar child sprite 'thumb' has no events enabled. Hierarchy: " + base.GetXuiHierarchy());
			return;
		}
		if (base.Controller.TryGetChildView<XUiV_Sprite>("background", out this.background) && !this.background.HasAnyEvent)
		{
			Log.Error("[XUi] XUiV_ScrollBar child sprite 'background' has no events enabled. Hierarchy: " + base.GetXuiHierarchy());
			return;
		}
		base.SetDirty();
	}

	// Token: 0x06008DCF RID: 36303 RVA: 0x00358184 File Offset: 0x00356384
	public override void OnOpen()
	{
		base.OnOpen();
		this.scrollBar.alpha = 0f;
		if (this.thumb != null)
		{
			Color color = this.thumb.Color;
			color.a = 0f;
			this.thumb.Color = color;
		}
		if (this.background != null)
		{
			Color color2 = this.background.Color;
			color2.a = 0f;
			this.background.Color = color2;
		}
		this.onOpenValue = this.scrollBar.value;
		this.openFrame = Time.frameCount;
	}

	// Token: 0x06008DD0 RID: 36304 RVA: 0x0035821B File Offset: 0x0035641B
	public override void OnVisibilityChanged(bool _visibleInScene)
	{
		base.OnVisibilityChanged(_visibleInScene);
		if (_visibleInScene)
		{
			this.onOpenValue = this.scrollBar.value;
			this.openFrame = Time.frameCount;
		}
	}

	// Token: 0x06008DD1 RID: 36305 RVA: 0x00358244 File Offset: 0x00356444
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (Time.frameCount <= this.openFrame + 1)
		{
			float value = this.scrollBar.value;
			if (!Mathf.Approximately(this.onOpenValue, value))
			{
				this.scrollBar.value = this.onOpenValue;
			}
		}
	}

	// Token: 0x06008DD2 RID: 36306 RVA: 0x00358292 File Offset: 0x00356492
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.refreshBoxCollider();
		base.updateData();
	}

	// Token: 0x06008DD3 RID: 36307 RVA: 0x003582A0 File Offset: 0x003564A0
	public void Connect(XUiEvent_OnScrollEventHandler _onScrolled)
	{
		this.scrollBar.foregroundWidget = this.thumb.Sprite;
		this.thumb.Controller.OnScroll += _onScrolled;
		if (this.background != null)
		{
			this.scrollBar.backgroundWidget = this.background.Sprite;
			this.background.Controller.OnScroll += _onScrolled;
		}
	}

	// Token: 0x0400682C RID: 26668
	[PublicizedFrom(EAccessModifier.Private)]
	public UIScrollBar scrollBar;

	// Token: 0x0400682D RID: 26669
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite thumb;

	// Token: 0x0400682E RID: 26670
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x0400682F RID: 26671
	[PublicizedFrom(EAccessModifier.Private)]
	public float onOpenValue;

	// Token: 0x04006830 RID: 26672
	[PublicizedFrom(EAccessModifier.Private)]
	public int openFrame;
}
