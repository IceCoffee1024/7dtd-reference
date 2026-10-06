using System;
using Audio;

// Token: 0x0200091F RID: 2335
public class PowerPressurePlate : PowerTrigger
{
	// Token: 0x17000750 RID: 1872
	// (get) Token: 0x06004323 RID: 17187 RVA: 0x0008148A File Offset: 0x0007F68A
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.PressurePlate;
		}
	}

	// Token: 0x17000751 RID: 1873
	// (get) Token: 0x06004324 RID: 17188 RVA: 0x001A4C66 File Offset: 0x001A2E66
	// (set) Token: 0x06004325 RID: 17189 RVA: 0x001A4C6E File Offset: 0x001A2E6E
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

	// Token: 0x06004326 RID: 17190 RVA: 0x001A4CB0 File Offset: 0x001A2EB0
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

	// Token: 0x06004327 RID: 17191 RVA: 0x001A4D18 File Offset: 0x001A2F18
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleSoundDisable()
	{
		base.HandleSoundDisable();
		this.lastPressed = this.pressed;
		this.pressed = false;
	}

	// Token: 0x04003695 RID: 13973
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool pressed;

	// Token: 0x04003696 RID: 13974
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastPressed;
}
