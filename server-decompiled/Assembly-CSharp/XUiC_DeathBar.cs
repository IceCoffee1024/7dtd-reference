using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02000F29 RID: 3881
[Preserve]
public class XUiC_DeathBar : XUiController
{
	// Token: 0x17000E4A RID: 3658
	// (get) Token: 0x06007940 RID: 31040 RVA: 0x002DF066 File Offset: 0x002DD266
	// (set) Token: 0x06007941 RID: 31041 RVA: 0x002DF06E File Offset: 0x002DD26E
	public EntityAlive Target { get; set; }

	// Token: 0x06007942 RID: 31042 RVA: 0x002DF077 File Offset: 0x002DD277
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x06007943 RID: 31043 RVA: 0x002DF094 File Offset: 0x002DD294
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

	// Token: 0x06007944 RID: 31044 RVA: 0x002DF154 File Offset: 0x002DD354
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

	// Token: 0x040059EC RID: 23020
	[PublicizedFrom(EAccessModifier.Private)]
	public string deathText = "";

	// Token: 0x040059ED RID: 23021
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal LocalPlayer;
}
