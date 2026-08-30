using System;
using Audio;

// Token: 0x0200090E RID: 2318
public class PowerPressurePlate : PowerTrigger
{
	// Token: 0x17000725 RID: 1829
	// (get) Token: 0x0600432C RID: 17196 RVA: 0x00080B22 File Offset: 0x0007ED22
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.PressurePlate;
		}
	}

	// Token: 0x17000726 RID: 1830
	// (get) Token: 0x0600432D RID: 17197 RVA: 0x001A2A8A File Offset: 0x001A0C8A
	// (set) Token: 0x0600432E RID: 17198 RVA: 0x001A2A92 File Offset: 0x001A0C92
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

	// Token: 0x0600432F RID: 17199 RVA: 0x001A2AD4 File Offset: 0x001A0CD4
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

	// Token: 0x06004330 RID: 17200 RVA: 0x001A2B3C File Offset: 0x001A0D3C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleSoundDisable()
	{
		base.HandleSoundDisable();
		this.lastPressed = this.pressed;
		this.pressed = false;
	}

	// Token: 0x04003620 RID: 13856
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool pressed;

	// Token: 0x04003621 RID: 13857
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastPressed;
}
