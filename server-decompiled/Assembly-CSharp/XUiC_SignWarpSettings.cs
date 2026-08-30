using System;
using UnityEngine.Scripting;

// Token: 0x02001042 RID: 4162
[Preserve]
public abstract class XUiC_SignWarpSettings : XUiC_SignLayerSettings
{
	// Token: 0x17000F5F RID: 3935
	// (get) Token: 0x06008426 RID: 33830
	public abstract SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x06008427 RID: 33831 RVA: 0x00324C8A File Offset: 0x00322E8A
	public override void Init()
	{
		base.Init();
		this.btnRemove = (XUiC_SimpleButton)base.GetChildById("btnRemove");
		this.btnRemove.OnPressed += this.BtnRemove_OnPressed;
	}

	// Token: 0x06008428 RID: 33832 RVA: 0x00324CC0 File Offset: 0x00322EC0
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

	// Token: 0x06008429 RID: 33833
	public abstract void SetWarp(SignData.SignWarp warp);

	// Token: 0x0600842A RID: 33834 RVA: 0x00324D87 File Offset: 0x00322F87
	public override void SetLayer(SignData.SignLayer layer)
	{
		this.currentLayer = layer;
	}

	// Token: 0x0600842B RID: 33835 RVA: 0x00323348 File Offset: 0x00321548
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SignWarpSettings()
	{
	}

	// Token: 0x04006205 RID: 25093
	public Action OnWarpRemoved;

	// Token: 0x04006206 RID: 25094
	[PublicizedFrom(EAccessModifier.Protected)]
	public SignData.SignLayer currentLayer;

	// Token: 0x04006207 RID: 25095
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SimpleButton btnRemove;
}
