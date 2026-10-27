using System;
using Audio;

// Token: 0x02000918 RID: 2328
public class PowerConsumer : PowerItem
{
	// Token: 0x060042AD RID: 17069 RVA: 0x001A3524 File Offset: 0x001A1724
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

	// Token: 0x060042AE RID: 17070 RVA: 0x001A35AC File Offset: 0x001A17AC
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

	// Token: 0x060042AF RID: 17071 RVA: 0x001A3667 File Offset: 0x001A1867
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void IsPoweredChanged(bool newPowered)
	{
		Manager.BroadcastPlay(this.Position.ToVector3(), newPowered ? this.StartSound : this.EndSound, 0f);
	}

	// Token: 0x04003637 RID: 13879
	[PublicizedFrom(EAccessModifier.Protected)]
	public string StartSound = "";

	// Token: 0x04003638 RID: 13880
	[PublicizedFrom(EAccessModifier.Protected)]
	public string EndSound = "";

	// Token: 0x04003639 RID: 13881
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastActivate;
}
