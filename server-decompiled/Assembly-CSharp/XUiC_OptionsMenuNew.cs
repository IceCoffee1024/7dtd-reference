using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DCB RID: 3531
[Preserve]
public class XUiC_OptionsMenuNew : XUiController
{
	// Token: 0x17000BC0 RID: 3008
	// (get) Token: 0x06006B45 RID: 27461 RVA: 0x002A235C File Offset: 0x002A055C
	[XuiXmlBinding("video_options_simplified")]
	public bool VideoOptionsSimplified
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			OptionsVideoWindowMode optionsVideoWindow = UIOptions.OptionsVideoWindow;
			if (optionsVideoWindow == OptionsVideoWindowMode.Simplified)
			{
				return true;
			}
			if (optionsVideoWindow != OptionsVideoWindowMode.Detailed)
			{
				Log.Error(string.Format("Unknown video options menu {0}", UIOptions.OptionsVideoWindow));
				return false;
			}
			return false;
		}
	}

	// Token: 0x17000BC1 RID: 3009
	// (get) Token: 0x06006B46 RID: 27462 RVA: 0x002A2396 File Offset: 0x002A0596
	[XuiXmlBinding("has_block_list")]
	public bool HasBlockList
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return BlockedPlayerList.Instance != null;
		}
	}

	// Token: 0x06006B47 RID: 27463 RVA: 0x002A23A0 File Offset: 0x002A05A0
	public override void Init()
	{
		base.Init();
		XUiC_OptionsMenuNew.ParentSelector = base.GetParentByType<XUiC_WindowSelector>();
		UIOptions.OnOptionsVideoWindowChanged += this.OnVideoOptionsWindowChanged;
	}

	// Token: 0x06006B48 RID: 27464 RVA: 0x002A23C4 File Offset: 0x002A05C4
	public override void Cleanup()
	{
		base.Cleanup();
		XUiC_OptionsMenuNew.ParentSelector = null;
		UIOptions.OnOptionsVideoWindowChanged -= this.OnVideoOptionsWindowChanged;
	}

	// Token: 0x06006B49 RID: 27465 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoOptionsWindowChanged(OptionsVideoWindowMode _mode)
	{
		this.IsDirty = true;
	}

	// Token: 0x06006B4A RID: 27466 RVA: 0x0008ABF1 File Offset: 0x00088DF1
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06006B4B RID: 27467 RVA: 0x002A23E3 File Offset: 0x002A05E3
	public override void OnOpen()
	{
		base.OnOpen();
		GameManager.Instance.Pause(true);
	}

	// Token: 0x06006B4C RID: 27468 RVA: 0x002A23F6 File Offset: 0x002A05F6
	public override void OnClose()
	{
		base.OnClose();
		GameManager.Instance.Pause(false);
	}

	// Token: 0x040051F7 RID: 20983
	public static XUiC_WindowSelector ParentSelector;
}
