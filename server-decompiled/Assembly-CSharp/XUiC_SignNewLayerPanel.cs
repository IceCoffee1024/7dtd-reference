using System;
using UnityEngine.Scripting;

// Token: 0x0200105A RID: 4186
[Preserve]
public class XUiC_SignNewLayerPanel : XUiController
{
	// Token: 0x060084A2 RID: 33954 RVA: 0x0032A190 File Offset: 0x00328390
	public override void Init()
	{
		base.Init();
		this.layerTypeText = base.GetChildById("layerTypeText").GetChildByType<XUiC_SignLayerType>();
		this.layerTypePolygon = base.GetChildById("layerTypePolygon").GetChildByType<XUiC_SignLayerType>();
		this.layerTypeNoise = base.GetChildById("layerTypeNoise").GetChildByType<XUiC_SignLayerType>();
		XUiC_SignLayerType xuiC_SignLayerType = this.layerTypeText;
		xuiC_SignLayerType.OnBecameSelected = (Action<XUiC_SignGridEntry>)Delegate.Combine(xuiC_SignLayerType.OnBecameSelected, new Action<XUiC_SignGridEntry>(this.LayerTypeText_OnBecameSelected));
		XUiC_SignLayerType xuiC_SignLayerType2 = this.layerTypePolygon;
		xuiC_SignLayerType2.OnBecameSelected = (Action<XUiC_SignGridEntry>)Delegate.Combine(xuiC_SignLayerType2.OnBecameSelected, new Action<XUiC_SignGridEntry>(this.LayerTypePolygon_OnBecameSelected));
		XUiC_SignLayerType xuiC_SignLayerType3 = this.layerTypeNoise;
		xuiC_SignLayerType3.OnBecameSelected = (Action<XUiC_SignGridEntry>)Delegate.Combine(xuiC_SignLayerType3.OnBecameSelected, new Action<XUiC_SignGridEntry>(this.LayerTypeNoise_OnBecameSelected));
	}

	// Token: 0x060084A3 RID: 33955 RVA: 0x0032A25C File Offset: 0x0032845C
	[PublicizedFrom(EAccessModifier.Private)]
	public void LayerTypeText_OnBecameSelected(XUiC_SignGridEntry gridEntry)
	{
		SignData.SignLayer signLayer = new SignData.TextSignLayer();
		Action<SignData.SignLayer> onLayerTypeSelected = this.OnLayerTypeSelected;
		if (onLayerTypeSelected == null)
		{
			return;
		}
		onLayerTypeSelected(signLayer);
	}

	// Token: 0x060084A4 RID: 33956 RVA: 0x0032A280 File Offset: 0x00328480
	[PublicizedFrom(EAccessModifier.Private)]
	public void LayerTypePolygon_OnBecameSelected(XUiC_SignGridEntry gridEntry)
	{
		SignData.SignLayer signLayer = new SignData.PolygonSignLayer();
		Action<SignData.SignLayer> onLayerTypeSelected = this.OnLayerTypeSelected;
		if (onLayerTypeSelected == null)
		{
			return;
		}
		onLayerTypeSelected(signLayer);
	}

	// Token: 0x060084A5 RID: 33957 RVA: 0x0032A2A4 File Offset: 0x003284A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void LayerTypeNoise_OnBecameSelected(XUiC_SignGridEntry gridEntry)
	{
		SignData.SignLayer signLayer = new SignData.NoiseSignLayer();
		Action<SignData.SignLayer> onLayerTypeSelected = this.OnLayerTypeSelected;
		if (onLayerTypeSelected == null)
		{
			return;
		}
		onLayerTypeSelected(signLayer);
	}

	// Token: 0x04006270 RID: 25200
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType layerTypeText;

	// Token: 0x04006271 RID: 25201
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType layerTypePolygon;

	// Token: 0x04006272 RID: 25202
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType layerTypeNoise;

	// Token: 0x04006273 RID: 25203
	public Action<SignData.SignLayer> OnLayerTypeSelected;
}
