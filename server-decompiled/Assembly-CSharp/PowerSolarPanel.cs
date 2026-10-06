using System;
using UnityEngine;

// Token: 0x02000917 RID: 2327
public class PowerSolarPanel : PowerSource
{
	// Token: 0x1700073F RID: 1855
	// (get) Token: 0x060042DB RID: 17115 RVA: 0x001A394B File Offset: 0x001A1B4B
	// (set) Token: 0x060042DC RID: 17116 RVA: 0x001A3953 File Offset: 0x001A1B53
	public bool HasLight
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000740 RID: 1856
	// (get) Token: 0x060042DD RID: 17117 RVA: 0x00081AB0 File Offset: 0x0007FCB0
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.SolarPanel;
		}
	}

	// Token: 0x17000741 RID: 1857
	// (get) Token: 0x060042DE RID: 17118 RVA: 0x001A395C File Offset: 0x001A1B5C
	public override string OnSound
	{
		get
		{
			return "solarpanel_on";
		}
	}

	// Token: 0x17000742 RID: 1858
	// (get) Token: 0x060042DF RID: 17119 RVA: 0x001A3963 File Offset: 0x001A1B63
	public override string OffSound
	{
		get
		{
			return "solarpanel_off";
		}
	}

	// Token: 0x060042E0 RID: 17120 RVA: 0x001A396C File Offset: 0x001A1B6C
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

	// Token: 0x060042E1 RID: 17121 RVA: 0x001A3A16 File Offset: 0x001A1C16
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void TickPowerGeneration()
	{
		if (this.HasLight)
		{
			this.CurrentPower = this.MaxOutput;
		}
	}

	// Token: 0x060042E2 RID: 17122 RVA: 0x001A3A2C File Offset: 0x001A1C2C
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

	// Token: 0x060042E3 RID: 17123 RVA: 0x001A3B55 File Offset: 0x001A1D55
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool ShouldClearPower()
	{
		return this.sunLight != 15 || !GameManager.Instance.World.IsDaytime();
	}

	// Token: 0x060042E4 RID: 17124 RVA: 0x001A3B75 File Offset: 0x001A1D75
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void RefreshPowerStats()
	{
		base.RefreshPowerStats();
		this.MaxPower = this.MaxOutput;
	}

	// Token: 0x060042E5 RID: 17125 RVA: 0x001A3B89 File Offset: 0x001A1D89
	public override void read(PooledBinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		if (PowerManager.Instance.CurrentFileVersion >= 2)
		{
			this.sunLight = _br.ReadByte();
		}
	}

	// Token: 0x060042E6 RID: 17126 RVA: 0x001A3BAC File Offset: 0x001A1DAC
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.sunLight);
	}

	// Token: 0x0400364C RID: 13900
	public ushort InputFromSun;

	// Token: 0x0400364D RID: 13901
	[PublicizedFrom(EAccessModifier.Private)]
	public byte sunLight;

	// Token: 0x0400364E RID: 13902
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastHasLight;

	// Token: 0x0400364F RID: 13903
	[PublicizedFrom(EAccessModifier.Private)]
	public string runningSound = "solarpanel_idle";

	// Token: 0x04003651 RID: 13905
	[PublicizedFrom(EAccessModifier.Private)]
	public float lightUpdateTime;
}
