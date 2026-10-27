using System;
using System.Globalization;

// Token: 0x0200091B RID: 2331
public class PowerGenerator : PowerSource
{
	// Token: 0x1700073A RID: 1850
	// (get) Token: 0x060042BD RID: 17085 RVA: 0x00080BF2 File Offset: 0x0007EDF2
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Generator;
		}
	}

	// Token: 0x1700073B RID: 1851
	// (get) Token: 0x060042BE RID: 17086 RVA: 0x001A38E9 File Offset: 0x001A1AE9
	public override string OnSound
	{
		get
		{
			return "generator_start";
		}
	}

	// Token: 0x1700073C RID: 1852
	// (get) Token: 0x060042BF RID: 17087 RVA: 0x001A38F0 File Offset: 0x001A1AF0
	public override string OffSound
	{
		get
		{
			return "generator_stop";
		}
	}

	// Token: 0x060042C0 RID: 17088 RVA: 0x001A38F7 File Offset: 0x001A1AF7
	public override void read(PooledBinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.CurrentFuel = _br.ReadUInt16();
	}

	// Token: 0x060042C1 RID: 17089 RVA: 0x001A390D File Offset: 0x001A1B0D
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.CurrentFuel);
	}

	// Token: 0x060042C2 RID: 17090 RVA: 0x001A3922 File Offset: 0x001A1B22
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool ShouldAutoTurnOff()
	{
		return this.CurrentFuel <= 0;
	}

	// Token: 0x060042C3 RID: 17091 RVA: 0x001A3930 File Offset: 0x001A1B30
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void TickPowerGeneration()
	{
		if ((float)(this.MaxPower - this.CurrentPower) >= this.OutputPerFuel && this.CurrentFuel > 0)
		{
			this.CurrentFuel -= 1;
			this.CurrentPower += (ushort)this.OutputPerFuel;
		}
	}

	// Token: 0x060042C4 RID: 17092 RVA: 0x001A3980 File Offset: 0x001A1B80
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

	// Token: 0x0400363C RID: 13884
	public ushort CurrentFuel;

	// Token: 0x0400363D RID: 13885
	public ushort MaxFuel;

	// Token: 0x0400363E RID: 13886
	public float OutputPerFuel;
}
