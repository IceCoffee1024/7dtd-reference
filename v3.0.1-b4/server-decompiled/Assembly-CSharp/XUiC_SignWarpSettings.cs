using System;
using UnityEngine.Scripting;

// Token: 0x02001029 RID: 4137
[Preserve]
public abstract class XUiC_SignWarpSettings : XUiC_SignLayerSettings
{
	// Token: 0x17000F26 RID: 3878
	// (get) Token: 0x06008334 RID: 33588
	public abstract SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x06008335 RID: 33589 RVA: 0x00320462 File Offset: 0x0031E662
	public override void Init()
	{
		base.Init();
		this.btnRemove = (XUiC_SimpleButton)base.GetChildById("btnRemove");
		this.btnRemove.OnPressed += this.BtnRemove_OnPressed;
	}

	// Token: 0x06008336 RID: 33590 RVA: 0x00320498 File Offset: 0x0031E698
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnRemove_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.currentLayer == null)
		{
			Log.Error("Failed to remove warp. Layer reference is null.");
			return;
		}
		if (this.CurrentWarp == null)
		{
			Log.Error("Failed to remove warp. Warp reference is null.");
			return;
		}
		string name = this.CurrentWarp.GetType().Name;
		if (!this.currentLayer.warps.Contains(this.CurrentWarp))
		{
			Log.Error("Failed to remove " + name + ". Layer warp list does not contain referenced warp.");
			return;
		}
		Action<string, bool> onPreLayerSettingsChanged = this.OnPreLayerSettingsChanged;
		if (onPreLayerSettingsChanged != null)
		{
			onPreLayerSettingsChanged("Remove " + name, true);
		}
		this.currentLayer.warps.Remove(this.CurrentWarp);
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged != null)
		{
			onLayerSettingsChanged();
		}
		Action onWarpRemoved = this.OnWarpRemoved;
		if (onWarpRemoved == null)
		{
			return;
		}
		onWarpRemoved();
	}

	// Token: 0x06008337 RID: 33591
	public abstract void SetWarp(SignData.SignWarp warp);

	// Token: 0x06008338 RID: 33592 RVA: 0x0032055F File Offset: 0x0031E75F
	public override void SetLayer(SignData.SignLayer layer)
	{
		this.currentLayer = layer;
	}

	// Token: 0x06008339 RID: 33593 RVA: 0x0031EB20 File Offset: 0x0031CD20
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SignWarpSettings()
	{
	}

	// Token: 0x0400613E RID: 24894
	public Action OnWarpRemoved;

	// Token: 0x0400613F RID: 24895
	[PublicizedFrom(EAccessModifier.Protected)]
	public SignData.SignLayer currentLayer;

	// Token: 0x04006140 RID: 24896
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SimpleButton btnRemove;
}
