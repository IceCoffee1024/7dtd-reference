using System;
using UnityEngine.Scripting;

// Token: 0x0200104B RID: 4171
[Preserve]
public class XUiC_SignNewLayerPanel : XUiController
{
	// Token: 0x06008479 RID: 33913 RVA: 0x003267A8 File Offset: 0x003249A8
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

	// Token: 0x0600847A RID: 33914 RVA: 0x00326874 File Offset: 0x00324A74
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

	// Token: 0x0600847B RID: 33915 RVA: 0x00326898 File Offset: 0x00324A98
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

	// Token: 0x0600847C RID: 33916 RVA: 0x003268BC File Offset: 0x00324ABC
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

	// Token: 0x04006235 RID: 25141
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType layerTypeText;

	// Token: 0x04006236 RID: 25142
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType layerTypePolygon;

	// Token: 0x04006237 RID: 25143
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType layerTypeNoise;

	// Token: 0x04006238 RID: 25144
	public Action<SignData.SignLayer> OnLayerTypeSelected;
}
