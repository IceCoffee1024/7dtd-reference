using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02000F03 RID: 3843
[Preserve]
public class XUiC_DeathBar : XUiController
{
	// Token: 0x17000E13 RID: 3603
	// (get) Token: 0x0600788D RID: 30861 RVA: 0x002D7D3E File Offset: 0x002D5F3E
	// (set) Token: 0x0600788E RID: 30862 RVA: 0x002D7D46 File Offset: 0x002D5F46
	public EntityAlive Target { get; set; }

	// Token: 0x0600788F RID: 30863 RVA: 0x002D7D4F File Offset: 0x002D5F4F
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x06007890 RID: 30864 RVA: 0x002D7D6C File Offset: 0x002D5F6C
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

	// Token: 0x06007891 RID: 30865 RVA: 0x002D7E2C File Offset: 0x002D602C
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

	// Token: 0x04005902 RID: 22786
	[PublicizedFrom(EAccessModifier.Private)]
	public string deathText = "";

	// Token: 0x04005903 RID: 22787
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal LocalPlayer;
}
