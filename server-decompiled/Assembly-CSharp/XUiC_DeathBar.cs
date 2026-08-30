using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02000F1A RID: 3866
[Preserve]
public class XUiC_DeathBar : XUiController
{
	// Token: 0x17000E2D RID: 3629
	// (get) Token: 0x06007945 RID: 31045 RVA: 0x002DBCCA File Offset: 0x002D9ECA
	// (set) Token: 0x06007946 RID: 31046 RVA: 0x002DBCD2 File Offset: 0x002D9ED2
	public EntityAlive Target { get; set; }

	// Token: 0x06007947 RID: 31047 RVA: 0x002DBCDB File Offset: 0x002D9EDB
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x06007948 RID: 31048 RVA: 0x002DBCF8 File Offset: 0x002D9EF8
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.LocalPlayer == null && this.xui != null && this.xui.playerUI != null && this.xui.playerUI.entityPlayer != null)
		{
			this.LocalPlayer = this.xui.playerUI.entityPlayer;
		}
		if (this.LocalPlayer == null)
		{
			return;
		}
		if (this.LocalPlayer.IsAlive())
		{
			this.viewComponent.IsVisible = false;
			return;
		}
		if (this.deathText != TwitchManager.DeathText)
		{
			this.deathText = TwitchManager.DeathText;
			base.RefreshBindings();
		}
	}

	// Token: 0x06007949 RID: 31049 RVA: 0x002DBDB8 File Offset: 0x002D9FB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (!(_bindingName == "death_text"))
		{
			if (!(_bindingName == "visible"))
			{
				return base.GetBindingValueInternal(ref _value, _bindingName);
			}
			if (this.LocalPlayer == null)
			{
				_value = "false";
				return true;
			}
			if (this.LocalPlayer.IsAlive())
			{
				_value = "false";
				return true;
			}
			if (TwitchManager.DeathText == "")
			{
				_value = "false";
				return true;
			}
			_value = "true";
			return true;
		}
		else
		{
			if (this.LocalPlayer == null)
			{
				_value = "";
				return true;
			}
			_value = this.deathText;
			return true;
		}
	}

	// Token: 0x040059B4 RID: 22964
	[PublicizedFrom(EAccessModifier.Private)]
	public string deathText = "";

	// Token: 0x040059B5 RID: 22965
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal LocalPlayer;
}
