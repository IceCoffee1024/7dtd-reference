using System;
using System.Globalization;
using System.IO;

// Token: 0x02000900 RID: 2304
public class PowerGenerator : PowerSource
{
	// Token: 0x17000709 RID: 1801
	// (get) Token: 0x060042A3 RID: 17059 RVA: 0x00080065 File Offset: 0x0007E265
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Generator;
		}
	}

	// Token: 0x1700070A RID: 1802
	// (get) Token: 0x060042A4 RID: 17060 RVA: 0x001A0665 File Offset: 0x0019E865
	public override string OnSound
	{
		get
		{
			return "generator_start";
		}
	}

	// Token: 0x1700070B RID: 1803
	// (get) Token: 0x060042A5 RID: 17061 RVA: 0x001A066C File Offset: 0x0019E86C
	public override string OffSound
	{
		get
		{
			return "generator_stop";
		}
	}

	// Token: 0x060042A6 RID: 17062 RVA: 0x001A0673 File Offset: 0x0019E873
	public override void read(BinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.CurrentFuel = _br.ReadUInt16();
	}

	// Token: 0x060042A7 RID: 17063 RVA: 0x001A0689 File Offset: 0x0019E889
	public override void write(BinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.CurrentFuel);
	}

	// Token: 0x060042A8 RID: 17064 RVA: 0x001A069E File Offset: 0x0019E89E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool ShouldAutoTurnOff()
	{
		return this.CurrentFuel <= 0;
	}

	// Token: 0x060042A9 RID: 17065 RVA: 0x001A06AC File Offset: 0x0019E8AC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void TickPowerGeneration()
	{
		if ((float)(this.MaxPower - this.CurrentPower) >= this.OutputPerFuel && this.CurrentFuel > 0)
		{
			this.CurrentFuel -= 1;
			this.CurrentPower += (ushort)this.OutputPerFuel;
		}
	}

	// Token: 0x060042AA RID: 17066 RVA: 0x001A06FC File Offset: 0x0019E8FC
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

	// Token: 0x040035A7 RID: 13735
	public ushort CurrentFuel;

	// Token: 0x040035A8 RID: 13736
	public ushort MaxFuel;

	// Token: 0x040035A9 RID: 13737
	public float OutputPerFuel;
}
