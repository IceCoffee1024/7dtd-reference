using System;
using System.IO;
using UnityEngine;

// Token: 0x02000906 RID: 2310
public class PowerSolarPanel : PowerSource
{
	// Token: 0x17000714 RID: 1812
	// (get) Token: 0x060042E4 RID: 17124 RVA: 0x001A176E File Offset: 0x0019F96E
	// (set) Token: 0x060042E5 RID: 17125 RVA: 0x001A1776 File Offset: 0x0019F976
	public bool HasLight
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000715 RID: 1813
	// (get) Token: 0x060042E6 RID: 17126 RVA: 0x00081148 File Offset: 0x0007F348
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.SolarPanel;
		}
	}

	// Token: 0x17000716 RID: 1814
	// (get) Token: 0x060042E7 RID: 17127 RVA: 0x001A177F File Offset: 0x0019F97F
	public override string OnSound
	{
		get
		{
			return "solarpanel_on";
		}
	}

	// Token: 0x17000717 RID: 1815
	// (get) Token: 0x060042E8 RID: 17128 RVA: 0x001A1786 File Offset: 0x0019F986
	public override string OffSound
	{
		get
		{
			return "solarpanel_off";
		}
	}

	// Token: 0x060042E9 RID: 17129 RVA: 0x001A1790 File Offset: 0x0019F990
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckLightLevel()
	{
		if (this.TileEntity != null)
		{
			Chunk chunk = this.TileEntity.GetChunk();
			Vector3i localChunkPos = this.TileEntity.localChunkPos;
			this.sunLight = chunk.GetLight(localChunkPos.x, localChunkPos.y, localChunkPos.z, Chunk.LIGHT_TYPE.SUN);
		}
		this.lastHasLight = this.HasLight;
		this.HasLight = this.sunLight == 15 && GameManager.Instance.World.IsDaytime();
		if (this.lastHasLight != this.HasLight)
		{
			this.HandleOnOffSound();
			if (!this.HasLight)
			{
				this.CurrentPower = 0;
				this.HandleDisconnect();
				return;
			}
			base.SendHasLocalChangesToRoot();
		}
	}

	// Token: 0x060042EA RID: 17130 RVA: 0x001A183A File Offset: 0x0019FA3A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void TickPowerGeneration()
	{
		if (this.HasLight)
		{
			this.CurrentPower = this.MaxOutput;
		}
	}

	// Token: 0x060042EB RID: 17131 RVA: 0x001A1850 File Offset: 0x0019FA50
	public override void HandleSendPower()
	{
		if (base.IsOn)
		{
			if (Time.time > this.lightUpdateTime)
			{
				this.lightUpdateTime = Time.time + 2f;
				this.CheckLightLevel();
			}
			if (this.HasLight)
			{
				if (this.CurrentPower < this.MaxPower)
				{
					this.TickPowerGeneration();
				}
				else if (this.CurrentPower > this.MaxPower)
				{
					this.CurrentPower = this.MaxPower;
				}
				if (this.ShouldAutoTurnOff())
				{
					this.CurrentPower = 0;
					base.IsOn = false;
				}
				if (this.hasChangesLocal)
				{
					this.LastPowerUsed = 0;
					ushort num = (ushort)Mathf.Min((int)this.MaxOutput, (int)this.CurrentPower);
					ushort num2 = num;
					World world = GameManager.Instance.World;
					for (int i = 0; i < this.Children.Count; i++)
					{
						num = num2;
						this.Children[i].HandlePowerReceived(ref num2);
						this.LastPowerUsed += num - num2;
					}
				}
				if (this.LastPowerUsed >= this.CurrentPower)
				{
					base.SendHasLocalChangesToRoot();
					this.CurrentPower = 0;
					return;
				}
				this.CurrentPower -= this.LastPowerUsed;
			}
		}
	}

	// Token: 0x060042EC RID: 17132 RVA: 0x001A1979 File Offset: 0x0019FB79
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool ShouldClearPower()
	{
		return this.sunLight != 15 || !GameManager.Instance.World.IsDaytime();
	}

	// Token: 0x060042ED RID: 17133 RVA: 0x001A1999 File Offset: 0x0019FB99
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void RefreshPowerStats()
	{
		base.RefreshPowerStats();
		this.MaxPower = this.MaxOutput;
	}

	// Token: 0x060042EE RID: 17134 RVA: 0x001A19AD File Offset: 0x0019FBAD
	public override void read(BinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		if (PowerManager.Instance.CurrentFileVersion >= 2)
		{
			this.sunLight = _br.ReadByte();
		}
	}

	// Token: 0x060042EF RID: 17135 RVA: 0x001A19D0 File Offset: 0x0019FBD0
	public override void write(BinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.sunLight);
	}

	// Token: 0x040035D7 RID: 13783
	public ushort InputFromSun;

	// Token: 0x040035D8 RID: 13784
	[PublicizedFrom(EAccessModifier.Private)]
	public byte sunLight;

	// Token: 0x040035D9 RID: 13785
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastHasLight;

	// Token: 0x040035DA RID: 13786
	[PublicizedFrom(EAccessModifier.Private)]
	public string runningSound = "solarpanel_idle";

	// Token: 0x040035DC RID: 13788
	[PublicizedFrom(EAccessModifier.Private)]
	public float lightUpdateTime;
}
