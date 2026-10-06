using System;
using Audio;

// Token: 0x0200090E RID: 2318
public class PowerConsumer : PowerItem
{
	// Token: 0x0600428A RID: 17034 RVA: 0x001A247C File Offset: 0x001A067C
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

	// Token: 0x0600428B RID: 17035 RVA: 0x001A2504 File Offset: 0x001A0704
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

	// Token: 0x0600428C RID: 17036 RVA: 0x001A25BF File Offset: 0x001A07BF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void IsPoweredChanged(bool newPowered)
	{
		Manager.BroadcastPlay(this.Position.ToVector3(), newPowered ? this.StartSound : this.EndSound, 0f);
	}

	// Token: 0x04003617 RID: 13847
	[PublicizedFrom(EAccessModifier.Protected)]
	public string StartSound = "";

	// Token: 0x04003618 RID: 13848
	[PublicizedFrom(EAccessModifier.Protected)]
	public string EndSound = "";

	// Token: 0x04003619 RID: 13849
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastActivate;
}
