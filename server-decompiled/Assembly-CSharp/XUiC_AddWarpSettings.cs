using System;
using UnityEngine.Scripting;

// Token: 0x02001060 RID: 4192
[Preserve]
public class XUiC_AddWarpSettings : XUiC_SignLayerSettings
{
	// Token: 0x06008495 RID: 33941 RVA: 0x0032A06C File Offset: 0x0032826C
	public override void Init()
	{
		base.Init();
		this.warpTypeSkew = base.GetChildById("warpTypeSkew").GetChildByType<XUiC_SignLayerType>();
		XUiC_SignLayerType xuiC_SignLayerType = this.warpTypeSkew;
		xuiC_SignLayerType.OnBecameSelected = (Action<XUiC_SignGridEntry>)Delegate.Combine(xuiC_SignLayerType.OnBecameSelected, new Action<XUiC_SignGridEntry>(delegate(XUiC_SignGridEntry _)
		{
			this.AddWarp<SignData.SkewWarp>(new SignData.SkewWarp());
		}));
		this.warpTypeBulge = base.GetChildById("warpTypeBulge").GetChildByType<XUiC_SignLayerType>();
		XUiC_SignLayerType xuiC_SignLayerType2 = this.warpTypeBulge;
		xuiC_SignLayerType2.OnBecameSelected = (Action<XUiC_SignGridEntry>)Delegate.Combine(xuiC_SignLayerType2.OnBecameSelected, new Action<XUiC_SignGridEntry>(delegate(XUiC_SignGridEntry _)
		{
			this.AddWarp<SignData.BulgeWarp>(new SignData.BulgeWarp());
		}));
		this.warpTypeTwirl = base.GetChildById("warpTypeTwirl").GetChildByType<XUiC_SignLayerType>();
		XUiC_SignLayerType xuiC_SignLayerType3 = this.warpTypeTwirl;
		xuiC_SignLayerType3.OnBecameSelected = (Action<XUiC_SignGridEntry>)Delegate.Combine(xuiC_SignLayerType3.OnBecameSelected, new Action<XUiC_SignGridEntry>(delegate(XUiC_SignGridEntry _)
		{
			this.AddWarp<SignData.TwirlWarp>(new SignData.TwirlWarp());
		}));
		this.warpTypeKaleido = base.GetChildById("warpTypeKaleido").GetChildByType<XUiC_SignLayerType>();
		XUiC_SignLayerType xuiC_SignLayerType4 = this.warpTypeKaleido;
		xuiC_SignLayerType4.OnBecameSelected = (Action<XUiC_SignGridEntry>)Delegate.Combine(xuiC_SignLayerType4.OnBecameSelected, new Action<XUiC_SignGridEntry>(delegate(XUiC_SignGridEntry _)
		{
			this.AddWarp<SignData.KaleidoWarp>(new SignData.KaleidoWarp());
		}));
		this.warpTypePerspective = base.GetChildById("warpTypePerspective").GetChildByType<XUiC_SignLayerType>();
		XUiC_SignLayerType xuiC_SignLayerType5 = this.warpTypePerspective;
		xuiC_SignLayerType5.OnBecameSelected = (Action<XUiC_SignGridEntry>)Delegate.Combine(xuiC_SignLayerType5.OnBecameSelected, new Action<XUiC_SignGridEntry>(delegate(XUiC_SignGridEntry _)
		{
			this.AddWarp<SignData.PerspectiveWarp>(new SignData.PerspectiveWarp());
		}));
		this.warpTypeArc = base.GetChildById("warpTypeArc").GetChildByType<XUiC_SignLayerType>();
		XUiC_SignLayerType xuiC_SignLayerType6 = this.warpTypeArc;
		xuiC_SignLayerType6.OnBecameSelected = (Action<XUiC_SignGridEntry>)Delegate.Combine(xuiC_SignLayerType6.OnBecameSelected, new Action<XUiC_SignGridEntry>(delegate(XUiC_SignGridEntry _)
		{
			this.AddWarp<SignData.ArcWarp>(new SignData.ArcWarp());
		}));
		this.warpTypeStretch = base.GetChildById("warpTypeStretch").GetChildByType<XUiC_SignLayerType>();
		XUiC_SignLayerType xuiC_SignLayerType7 = this.warpTypeStretch;
		xuiC_SignLayerType7.OnBecameSelected = (Action<XUiC_SignGridEntry>)Delegate.Combine(xuiC_SignLayerType7.OnBecameSelected, new Action<XUiC_SignGridEntry>(delegate(XUiC_SignGridEntry _)
		{
			this.AddWarp<SignData.StretchWarp>(new SignData.StretchWarp());
		}));
		this.warpTypeGrid = base.GetChildById("warpTypeGrid").GetChildByType<XUiC_SignLayerType>();
		XUiC_SignLayerType xuiC_SignLayerType8 = this.warpTypeGrid;
		xuiC_SignLayerType8.OnBecameSelected = (Action<XUiC_SignGridEntry>)Delegate.Combine(xuiC_SignLayerType8.OnBecameSelected, new Action<XUiC_SignGridEntry>(delegate(XUiC_SignGridEntry _)
		{
			this.AddWarp<SignData.GridWarp>(new SignData.GridWarp());
		}));
	}

	// Token: 0x06008496 RID: 33942 RVA: 0x0032A268 File Offset: 0x00328468
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddWarp<T>(T warp) where T : SignData.SignWarp
	{
		if (this.currentLayer == null)
		{
			Log.Error("Failed to add warp: layer reference is null.");
			return;
		}
		if (this.currentLayer.warps == null)
		{
			Log.Error("Failed to add warp: layer warps list is null.");
			return;
		}
		string name = typeof(T).Name;
		Action<string, bool> onPreLayerSettingsChanged = this.OnPreLayerSettingsChanged;
		if (onPreLayerSettingsChanged != null)
		{
			onPreLayerSettingsChanged("Add " + name, true);
		}
		this.currentLayer.warps.Add(warp);
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged != null)
		{
			onLayerSettingsChanged();
		}
		Action<string> onWarpAdded = this.OnWarpAdded;
		if (onWarpAdded == null)
		{
			return;
		}
		onWarpAdded(name);
	}

	// Token: 0x06008497 RID: 33943 RVA: 0x0032A305 File Offset: 0x00328505
	public override void SetLayer(SignData.SignLayer layer)
	{
		this.currentLayer = layer;
	}

	// Token: 0x04006274 RID: 25204
	public Action<string> OnWarpAdded;

	// Token: 0x04006275 RID: 25205
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SignLayer currentLayer;

	// Token: 0x04006276 RID: 25206
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType warpTypeSkew;

	// Token: 0x04006277 RID: 25207
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType warpTypeBulge;

	// Token: 0x04006278 RID: 25208
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType warpTypeTwirl;

	// Token: 0x04006279 RID: 25209
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType warpTypeKaleido;

	// Token: 0x0400627A RID: 25210
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType warpTypePerspective;

	// Token: 0x0400627B RID: 25211
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType warpTypeArc;

	// Token: 0x0400627C RID: 25212
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType warpTypeStretch;

	// Token: 0x0400627D RID: 25213
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType warpTypeGrid;
}
