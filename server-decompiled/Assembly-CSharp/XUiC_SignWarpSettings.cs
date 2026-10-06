using System;
using UnityEngine.Scripting;

// Token: 0x02001051 RID: 4177
[Preserve]
public abstract class XUiC_SignWarpSettings : XUiC_SignLayerSettings
{
	// Token: 0x17000F8C RID: 3980
	// (get) Token: 0x0600844F RID: 33871
	public abstract SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x06008450 RID: 33872 RVA: 0x00328672 File Offset: 0x00326872
	public override void Init()
	{
		base.Init();
		this.btnRemove = (XUiC_SimpleButton)base.GetChildById("btnRemove");
		this.btnRemove.OnPressed += this.BtnRemove_OnPressed;
	}

	// Token: 0x06008451 RID: 33873 RVA: 0x003286A8 File Offset: 0x003268A8
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

	// Token: 0x06008452 RID: 33874
	public abstract void SetWarp(SignData.SignWarp warp);

	// Token: 0x06008453 RID: 33875 RVA: 0x0032876F File Offset: 0x0032696F
	public override void SetLayer(SignData.SignLayer layer)
	{
		this.currentLayer = layer;
	}

	// Token: 0x06008454 RID: 33876 RVA: 0x00326D30 File Offset: 0x00324F30
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SignWarpSettings()
	{
	}

	// Token: 0x04006240 RID: 25152
	public Action OnWarpRemoved;

	// Token: 0x04006241 RID: 25153
	[PublicizedFrom(EAccessModifier.Protected)]
	public SignData.SignLayer currentLayer;

	// Token: 0x04006242 RID: 25154
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SimpleButton btnRemove;
}
