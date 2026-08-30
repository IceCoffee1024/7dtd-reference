using System;
using Audio;

// Token: 0x02000909 RID: 2313
public class PowerConsumer : PowerItem
{
	// Token: 0x060042F8 RID: 17144 RVA: 0x001A2378 File Offset: 0x001A0578
	public override void HandlePowerUpdate(bool isOn)
	{
		bool flag = this.isPowered && isOn;
		if (this.TileEntity != null)
		{
			this.TileEntity.Activate(flag);
			if (flag && this.lastActivate != flag)
			{
				this.TileEntity.ActivateOnce();
			}
			this.TileEntity.SetModified();
		}
		this.lastActivate = flag;
		if (this.PowerChildren())
		{
			for (int i = 0; i < this.Children.Count; i++)
			{
				this.Children[i].HandlePowerUpdate(isOn);
			}
		}
	}

	// Token: 0x060042F9 RID: 17145 RVA: 0x001A2400 File Offset: 0x001A0600
	public override void SetValuesFromBlock()
	{
		base.SetValuesFromBlock();
		Block block = Block.list[(int)this.BlockID];
		if (block.Properties.Values.ContainsKey("RequiredPower"))
		{
			this.RequiredPower = ushort.Parse(block.Properties.Values["RequiredPower"]);
		}
		if (block.Properties.Values.ContainsKey("StartSound"))
		{
			this.StartSound = block.Properties.Values["StartSound"];
		}
		if (block.Properties.Values.ContainsKey("EndSound"))
		{
			this.EndSound = block.Properties.Values["EndSound"];
		}
	}

	// Token: 0x060042FA RID: 17146 RVA: 0x001A24BB File Offset: 0x001A06BB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void IsPoweredChanged(bool newPowered)
	{
		Manager.BroadcastPlay(this.Position.ToVector3(), newPowered ? this.StartSound : this.EndSound, 0f);
	}

	// Token: 0x04003623 RID: 13859
	[PublicizedFrom(EAccessModifier.Protected)]
	public string StartSound = "";

	// Token: 0x04003624 RID: 13860
	[PublicizedFrom(EAccessModifier.Protected)]
	public string EndSound = "";

	// Token: 0x04003625 RID: 13861
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastActivate;
}
