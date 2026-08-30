using System;
using UnityEngine.Scripting;

// Token: 0x02001032 RID: 4146
[Preserve]
public class XUiC_SignNewLayerPanel : XUiController
{
	// Token: 0x06008387 RID: 33671 RVA: 0x00321F80 File Offset: 0x00320180
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

	// Token: 0x06008388 RID: 33672 RVA: 0x0032204C File Offset: 0x0032024C
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

	// Token: 0x06008389 RID: 33673 RVA: 0x00322070 File Offset: 0x00320270
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

	// Token: 0x0600838A RID: 33674 RVA: 0x00322094 File Offset: 0x00320294
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

	// Token: 0x0400616E RID: 24942
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType layerTypeText;

	// Token: 0x0400616F RID: 24943
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType layerTypePolygon;

	// Token: 0x04006170 RID: 24944
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerType layerTypeNoise;

	// Token: 0x04006171 RID: 24945
	public Action<SignData.SignLayer> OnLayerTypeSelected;
}
