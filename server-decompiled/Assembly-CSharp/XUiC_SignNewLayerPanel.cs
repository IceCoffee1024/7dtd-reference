using System;
using UnityEngine.Scripting;

// Token: 0x0200106A RID: 4202
[Preserve]
public class XUiC_SignNewLayerPanel : XUiController
{
	// Token: 0x060084F4 RID: 34036 RVA: 0x0032BE94 File Offset: 0x0032A094
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

	// Token: 0x060084F5 RID: 34037 RVA: 0x0032BF60 File Offset: 0x0032A160
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

	// Token: 0x060084F6 RID: 34038 RVA: 0x0032BF84 File Offset: 0x0032A184
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

	// Token: 0x060084F7 RID: 34039 RVA: 0x0032BFA8 File Offset: 0x0032A1A8
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

	// Token: 0x040062AE RID: 25262
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType layerTypeText;

	// Token: 0x040062AF RID: 25263
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType layerTypePolygon;

	// Token: 0x040062B0 RID: 25264
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType layerTypeNoise;

	// Token: 0x040062B1 RID: 25265
	public Action<SignData.SignLayer> OnLayerTypeSelected;
}
