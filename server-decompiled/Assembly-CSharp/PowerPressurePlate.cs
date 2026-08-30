using System;
using Audio;

// Token: 0x0200091A RID: 2330
public class PowerPressurePlate : PowerTrigger
{
	// Token: 0x17000731 RID: 1841
	// (get) Token: 0x06004391 RID: 17297 RVA: 0x000811D6 File Offset: 0x0007F3D6
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.PressurePlate;
		}
	}

	// Token: 0x17000732 RID: 1842
	// (get) Token: 0x06004392 RID: 17298 RVA: 0x001A4B62 File Offset: 0x001A2D62
	// (set) Token: 0x06004393 RID: 17299 RVA: 0x001A4B6A File Offset: 0x001A2D6A
	public bool Pressed
	{
		get
		{
			return this.pressed;
		}
		set
		{
			this.pressed = value;
			if (this.pressed && !this.lastPressed)
			{
				Manager.BroadcastPlay(this.Position.ToVector3(), "pressureplate_down", 0f);
			}
			this.lastPressed = this.pressed;
		}
	}

	// Token: 0x06004394 RID: 17300 RVA: 0x001A4BAC File Offset: 0x001A2DAC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CheckForActiveChange()
	{
		base.CheckForActiveChange();
		if (!this.pressed && this.lastPressed)
		{
			Manager.BroadcastPlay(this.Position.ToVector3(), "pressureplate_up", 0f);
			if (this.powerTime == 0f)
			{
				this.isActive = false;
				this.HandleDisconnectChildren();
				base.SendHasLocalChangesToRoot();
				this.powerTime = -1f;
			}
		}
	}

	// Token: 0x06004395 RID: 17301 RVA: 0x001A4C14 File Offset: 0x001A2E14
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleSoundDisable()
	{
		base.HandleSoundDisable();
		this.lastPressed = this.pressed;
		this.pressed = false;
	}

	// Token: 0x040036A1 RID: 13985
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool pressed;

	// Token: 0x040036A2 RID: 13986
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastPressed;
}
