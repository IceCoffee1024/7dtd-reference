using System;
using UnityEngine.Scripting;

// Token: 0x02001061 RID: 4193
[Preserve]
public abstract class XUiC_SignWarpSettings : XUiC_SignLayerSettings
{
	// Token: 0x17000F97 RID: 3991
	// (get) Token: 0x060084A1 RID: 33953
	public abstract SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x060084A2 RID: 33954 RVA: 0x0032A376 File Offset: 0x00328576
	public override void Init()
	{
		base.Init();
		this.btnRemove = (XUiC_SimpleButton)base.GetChildById("btnRemove");
		this.btnRemove.OnPressed += this.BtnRemove_OnPressed;
	}

	// Token: 0x060084A3 RID: 33955 RVA: 0x0032A3AC File Offset: 0x003285AC
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

	// Token: 0x060084A4 RID: 33956
	public abstract void SetWarp(SignData.SignWarp warp);

	// Token: 0x060084A5 RID: 33957 RVA: 0x0032A473 File Offset: 0x00328673
	public override void SetLayer(SignData.SignLayer layer)
	{
		this.currentLayer = layer;
	}

	// Token: 0x060084A6 RID: 33958 RVA: 0x00328A34 File Offset: 0x00326C34
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SignWarpSettings()
	{
	}

	// Token: 0x0400627E RID: 25214
	public Action OnWarpRemoved;

	// Token: 0x0400627F RID: 25215
	[PublicizedFrom(EAccessModifier.Protected)]
	public SignData.SignLayer currentLayer;

	// Token: 0x04006280 RID: 25216
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SimpleButton btnRemove;
}
