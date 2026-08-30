using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DB1 RID: 3505
[Preserve]
public class XUiC_OptionsMenuNew : XUiController
{
	// Token: 0x17000B9F RID: 2975
	// (get) Token: 0x06006B28 RID: 27432 RVA: 0x0029DAA4 File Offset: 0x0029BCA4
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

	// Token: 0x17000BA0 RID: 2976
	// (get) Token: 0x06006B29 RID: 27433 RVA: 0x0029DADE File Offset: 0x0029BCDE
	[XuiXmlBinding("has_block_list")]
	public bool HasBlockList
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return BlockedPlayerList.Instance != null;
		}
	}

	// Token: 0x06006B2A RID: 27434 RVA: 0x0029DAE8 File Offset: 0x0029BCE8
	public override void Init()
	{
		base.Init();
		XUiC_OptionsMenuNew.ParentSelector = base.GetParentByType<XUiC_WindowSelector>();
		UIOptions.OnOptionsVideoWindowChanged += this.OnVideoOptionsWindowChanged;
	}

	// Token: 0x06006B2B RID: 27435 RVA: 0x0029DB0C File Offset: 0x0029BD0C
	public override void Cleanup()
	{
		base.Cleanup();
		XUiC_OptionsMenuNew.ParentSelector = null;
		UIOptions.OnOptionsVideoWindowChanged -= this.OnVideoOptionsWindowChanged;
	}

	// Token: 0x06006B2C RID: 27436 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoOptionsWindowChanged(OptionsVideoWindowMode _mode)
	{
		this.IsDirty = true;
	}

	// Token: 0x06006B2D RID: 27437 RVA: 0x0008A73D File Offset: 0x0008893D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06006B2E RID: 27438 RVA: 0x0029DB2B File Offset: 0x0029BD2B
	public override void OnOpen()
	{
		base.OnOpen();
		GameManager.Instance.Pause(true);
	}

	// Token: 0x06006B2F RID: 27439 RVA: 0x0029DB3E File Offset: 0x0029BD3E
	public override void OnClose()
	{
		base.OnClose();
		GameManager.Instance.Pause(false);
	}

	// Token: 0x0400519B RID: 20891
	public static XUiC_WindowSelector ParentSelector;
}
