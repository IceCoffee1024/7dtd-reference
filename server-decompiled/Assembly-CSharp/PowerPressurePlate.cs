using System;
using Audio;

// Token: 0x02000929 RID: 2345
public class PowerPressurePlate : PowerTrigger
{
	// Token: 0x17000756 RID: 1878
	// (get) Token: 0x06004346 RID: 17222 RVA: 0x000816AE File Offset: 0x0007F8AE
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.PressurePlate;
		}
	}

	// Token: 0x17000757 RID: 1879
	// (get) Token: 0x06004347 RID: 17223 RVA: 0x001A5D0E File Offset: 0x001A3F0E
	// (set) Token: 0x06004348 RID: 17224 RVA: 0x001A5D16 File Offset: 0x001A3F16
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

	// Token: 0x06004349 RID: 17225 RVA: 0x001A5D58 File Offset: 0x001A3F58
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

	// Token: 0x0600434A RID: 17226 RVA: 0x001A5DC0 File Offset: 0x001A3FC0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleSoundDisable()
	{
		base.HandleSoundDisable();
		this.lastPressed = this.pressed;
		this.pressed = false;
	}

	// Token: 0x040036B5 RID: 14005
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool pressed;

	// Token: 0x040036B6 RID: 14006
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastPressed;
}
