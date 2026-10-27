using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02000F38 RID: 3896
[Preserve]
public class XUiC_DeathBar : XUiController
{
	// Token: 0x17000E54 RID: 3668
	// (get) Token: 0x06007983 RID: 31107 RVA: 0x002E0956 File Offset: 0x002DEB56
	// (set) Token: 0x06007984 RID: 31108 RVA: 0x002E095E File Offset: 0x002DEB5E
	public EntityAlive Target { get; set; }

	// Token: 0x06007985 RID: 31109 RVA: 0x002E0967 File Offset: 0x002DEB67
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x06007986 RID: 31110 RVA: 0x002E0984 File Offset: 0x002DEB84
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

	// Token: 0x06007987 RID: 31111 RVA: 0x002E0A44 File Offset: 0x002DEC44
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

	// Token: 0x04005A1F RID: 23071
	[PublicizedFrom(EAccessModifier.Private)]
	public string deathText = "";

	// Token: 0x04005A20 RID: 23072
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal LocalPlayer;
}
