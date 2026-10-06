using System;
using System.Globalization;

// Token: 0x02000911 RID: 2321
public class PowerGenerator : PowerSource
{
	// Token: 0x17000734 RID: 1844
	// (get) Token: 0x0600429A RID: 17050 RVA: 0x000809CD File Offset: 0x0007EBCD
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Generator;
		}
	}

	// Token: 0x17000735 RID: 1845
	// (get) Token: 0x0600429B RID: 17051 RVA: 0x001A2841 File Offset: 0x001A0A41
	public override string OnSound
	{
		get
		{
			return "generator_start";
		}
	}

	// Token: 0x17000736 RID: 1846
	// (get) Token: 0x0600429C RID: 17052 RVA: 0x001A2848 File Offset: 0x001A0A48
	public override string OffSound
	{
		get
		{
			return "generator_stop";
		}
	}

	// Token: 0x0600429D RID: 17053 RVA: 0x001A284F File Offset: 0x001A0A4F
	public override void read(PooledBinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.CurrentFuel = _br.ReadUInt16();
	}

	// Token: 0x0600429E RID: 17054 RVA: 0x001A2865 File Offset: 0x001A0A65
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.CurrentFuel);
	}

	// Token: 0x0600429F RID: 17055 RVA: 0x001A287A File Offset: 0x001A0A7A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool ShouldAutoTurnOff()
	{
		return this.CurrentFuel <= 0;
	}

	// Token: 0x060042A0 RID: 17056 RVA: 0x001A2888 File Offset: 0x001A0A88
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void TickPowerGeneration()
	{
		if ((float)(this.MaxPower - this.CurrentPower) >= this.OutputPerFuel && this.CurrentFuel > 0)
		{
			this.CurrentFuel -= 1;
			this.CurrentPower += (ushort)this.OutputPerFuel;
		}
	}

	// Token: 0x060042A1 RID: 17057 RVA: 0x001A28D8 File Offset: 0x001A0AD8
	public override void SetValuesFromBlock()
	{
		base.SetValuesFromBlock();
		Block block = Block.list[(int)this.BlockID];
		if (block.Properties.Values.ContainsKey("MaxPower"))
		{
			this.MaxPower = ushort.Parse(block.Properties.Values["MaxPower"]);
		}
		if (block.Properties.Values.ContainsKey("MaxFuel"))
		{
			this.MaxFuel = ushort.Parse(block.Properties.Values["MaxFuel"]);
		}
		else
		{
			this.MaxFuel = 1000;
		}
		if (block.Properties.Values.ContainsKey("OutputPerFuel"))
		{
			this.OutputPerFuel = StringParsers.ParseFloat(block.Properties.Values["OutputPerFuel"], 0, -1, NumberStyles.Any);
			return;
		}
		this.OutputPerFuel = 100f;
	}

	// Token: 0x0400361C RID: 13852
	public ushort CurrentFuel;

	// Token: 0x0400361D RID: 13853
	public ushort MaxFuel;

	// Token: 0x0400361E RID: 13854
	public float OutputPerFuel;
}
