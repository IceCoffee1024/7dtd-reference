using System;
using System.Globalization;
using System.IO;

// Token: 0x0200090C RID: 2316
public class PowerGenerator : PowerSource
{
	// Token: 0x17000715 RID: 1813
	// (get) Token: 0x06004308 RID: 17160 RVA: 0x00080719 File Offset: 0x0007E919
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Generator;
		}
	}

	// Token: 0x17000716 RID: 1814
	// (get) Token: 0x06004309 RID: 17161 RVA: 0x001A273D File Offset: 0x001A093D
	public override string OnSound
	{
		get
		{
			return "generator_start";
		}
	}

	// Token: 0x17000717 RID: 1815
	// (get) Token: 0x0600430A RID: 17162 RVA: 0x001A2744 File Offset: 0x001A0944
	public override string OffSound
	{
		get
		{
			return "generator_stop";
		}
	}

	// Token: 0x0600430B RID: 17163 RVA: 0x001A274B File Offset: 0x001A094B
	public override void read(BinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.CurrentFuel = _br.ReadUInt16();
	}

	// Token: 0x0600430C RID: 17164 RVA: 0x001A2761 File Offset: 0x001A0961
	public override void write(BinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.CurrentFuel);
	}

	// Token: 0x0600430D RID: 17165 RVA: 0x001A2776 File Offset: 0x001A0976
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool ShouldAutoTurnOff()
	{
		return this.CurrentFuel <= 0;
	}

	// Token: 0x0600430E RID: 17166 RVA: 0x001A2784 File Offset: 0x001A0984
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void TickPowerGeneration()
	{
		if ((float)(this.MaxPower - this.CurrentPower) >= this.OutputPerFuel && this.CurrentFuel > 0)
		{
			this.CurrentFuel -= 1;
			this.CurrentPower += (ushort)this.OutputPerFuel;
		}
	}

	// Token: 0x0600430F RID: 17167 RVA: 0x001A27D4 File Offset: 0x001A09D4
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

	// Token: 0x04003628 RID: 13864
	public ushort CurrentFuel;

	// Token: 0x04003629 RID: 13865
	public ushort MaxFuel;

	// Token: 0x0400362A RID: 13866
	public float OutputPerFuel;
}
