using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DC0 RID: 3520
[Preserve]
public class XUiC_OptionsMenuNew : XUiController
{
	// Token: 0x17000BBA RID: 3002
	// (get) Token: 0x06006B20 RID: 27424 RVA: 0x002A0E34 File Offset: 0x0029F034
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

	// Token: 0x17000BBB RID: 3003
	// (get) Token: 0x06006B21 RID: 27425 RVA: 0x002A0E6E File Offset: 0x0029F06E
	[XuiXmlBinding("has_block_list")]
	public bool HasBlockList
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return BlockedPlayerList.Instance != null;
		}
	}

	// Token: 0x06006B22 RID: 27426 RVA: 0x002A0E78 File Offset: 0x0029F078
	public override void Init()
	{
		base.Init();
		XUiC_OptionsMenuNew.ParentSelector = base.GetParentByType<XUiC_WindowSelector>();
		UIOptions.OnOptionsVideoWindowChanged += this.OnVideoOptionsWindowChanged;
	}

	// Token: 0x06006B23 RID: 27427 RVA: 0x002A0E9C File Offset: 0x0029F09C
	public override void Cleanup()
	{
		base.Cleanup();
		XUiC_OptionsMenuNew.ParentSelector = null;
		UIOptions.OnOptionsVideoWindowChanged -= this.OnVideoOptionsWindowChanged;
	}

	// Token: 0x06006B24 RID: 27428 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoOptionsWindowChanged(OptionsVideoWindowMode _mode)
	{
		this.IsDirty = true;
	}

	// Token: 0x06006B25 RID: 27429 RVA: 0x0008A9BD File Offset: 0x00088BBD
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06006B26 RID: 27430 RVA: 0x002A0EBB File Offset: 0x0029F0BB
	public override void OnOpen()
	{
		base.OnOpen();
		GameManager.Instance.Pause(true);
	}

	// Token: 0x06006B27 RID: 27431 RVA: 0x002A0ECE File Offset: 0x0029F0CE
	public override void OnClose()
	{
		base.OnClose();
		GameManager.Instance.Pause(false);
	}

	// Token: 0x040051D2 RID: 20946
	public static XUiC_WindowSelector ParentSelector;
}
