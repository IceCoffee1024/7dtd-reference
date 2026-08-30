using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D9D RID: 3485
[Preserve]
public class XUiC_OptionsMenuNew : XUiController
{
	// Token: 0x17000B87 RID: 2951
	// (get) Token: 0x06006A8D RID: 27277 RVA: 0x0029A944 File Offset: 0x00298B44
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

	// Token: 0x17000B88 RID: 2952
	// (get) Token: 0x06006A8E RID: 27278 RVA: 0x0029A97E File Offset: 0x00298B7E
	[XuiXmlBinding("has_block_list")]
	public bool HasBlockList
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return BlockedPlayerList.Instance != null;
		}
	}

	// Token: 0x06006A8F RID: 27279 RVA: 0x0029A988 File Offset: 0x00298B88
	public override void Init()
	{
		base.Init();
		XUiC_OptionsMenuNew.ParentSelector = base.GetParentByType<XUiC_WindowSelector>();
		UIOptions.OnOptionsVideoWindowChanged += this.OnVideoOptionsWindowChanged;
	}

	// Token: 0x06006A90 RID: 27280 RVA: 0x0029A9AC File Offset: 0x00298BAC
	public override void Cleanup()
	{
		base.Cleanup();
		XUiC_OptionsMenuNew.ParentSelector = null;
		UIOptions.OnOptionsVideoWindowChanged -= this.OnVideoOptionsWindowChanged;
	}

	// Token: 0x06006A91 RID: 27281 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoOptionsWindowChanged(OptionsVideoWindowMode _mode)
	{
		this.IsDirty = true;
	}

	// Token: 0x06006A92 RID: 27282 RVA: 0x0008A091 File Offset: 0x00088291
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06006A93 RID: 27283 RVA: 0x0029A9CB File Offset: 0x00298BCB
	public override void OnOpen()
	{
		base.OnOpen();
		GameManager.Instance.Pause(true);
	}

	// Token: 0x06006A94 RID: 27284 RVA: 0x0029A9DE File Offset: 0x00298BDE
	public override void OnClose()
	{
		base.OnClose();
		GameManager.Instance.Pause(false);
	}

	// Token: 0x040050F9 RID: 20729
	public static XUiC_WindowSelector ParentSelector;
}
