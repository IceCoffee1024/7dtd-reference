using System;
using Audio;

// Token: 0x020008FD RID: 2301
public class PowerConsumer : PowerItem
{
	// Token: 0x06004293 RID: 17043 RVA: 0x001A02A0 File Offset: 0x0019E4A0
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

	// Token: 0x06004294 RID: 17044 RVA: 0x001A0328 File Offset: 0x0019E528
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

	// Token: 0x06004295 RID: 17045 RVA: 0x001A03E3 File Offset: 0x0019E5E3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void IsPoweredChanged(bool newPowered)
	{
		Manager.BroadcastPlay(this.Position.ToVector3(), newPowered ? this.StartSound : this.EndSound, 0f);
	}

	// Token: 0x040035A2 RID: 13730
	[PublicizedFrom(EAccessModifier.Protected)]
	public string StartSound = "";

	// Token: 0x040035A3 RID: 13731
	[PublicizedFrom(EAccessModifier.Protected)]
	public string EndSound = "";

	// Token: 0x040035A4 RID: 13732
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastActivate;
}
