using System;
using UnityEngine;

// Token: 0x02000921 RID: 2337
public class PowerSolarPanel : PowerSource
{
	// Token: 0x17000745 RID: 1861
	// (get) Token: 0x060042FE RID: 17150 RVA: 0x001A49F3 File Offset: 0x001A2BF3
	// (set) Token: 0x060042FF RID: 17151 RVA: 0x001A49FB File Offset: 0x001A2BFB
	public bool HasLight
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000746 RID: 1862
	// (get) Token: 0x06004300 RID: 17152 RVA: 0x00081CD4 File Offset: 0x0007FED4
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.SolarPanel;
		}
	}

	// Token: 0x17000747 RID: 1863
	// (get) Token: 0x06004301 RID: 17153 RVA: 0x001A4A04 File Offset: 0x001A2C04
	public override string OnSound
	{
		get
		{
			return "solarpanel_on";
		}
	}

	// Token: 0x17000748 RID: 1864
	// (get) Token: 0x06004302 RID: 17154 RVA: 0x001A4A0B File Offset: 0x001A2C0B
	public override string OffSound
	{
		get
		{
			return "solarpanel_off";
		}
	}

	// Token: 0x06004303 RID: 17155 RVA: 0x001A4A14 File Offset: 0x001A2C14
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

	// Token: 0x06004304 RID: 17156 RVA: 0x001A4ABE File Offset: 0x001A2CBE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void TickPowerGeneration()
	{
		if (this.HasLight)
		{
			this.CurrentPower = this.MaxOutput;
		}
	}

	// Token: 0x06004305 RID: 17157 RVA: 0x001A4AD4 File Offset: 0x001A2CD4
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

	// Token: 0x06004306 RID: 17158 RVA: 0x001A4BFD File Offset: 0x001A2DFD
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool ShouldClearPower()
	{
		return this.sunLight != 15 || !GameManager.Instance.World.IsDaytime();
	}

	// Token: 0x06004307 RID: 17159 RVA: 0x001A4C1D File Offset: 0x001A2E1D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void RefreshPowerStats()
	{
		base.RefreshPowerStats();
		this.MaxPower = this.MaxOutput;
	}

	// Token: 0x06004308 RID: 17160 RVA: 0x001A4C31 File Offset: 0x001A2E31
	public override void read(PooledBinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		if (PowerManager.Instance.CurrentFileVersion >= 2)
		{
			this.sunLight = _br.ReadByte();
		}
	}

	// Token: 0x06004309 RID: 17161 RVA: 0x001A4C54 File Offset: 0x001A2E54
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.sunLight);
	}

	// Token: 0x0400366C RID: 13932
	public ushort InputFromSun;

	// Token: 0x0400366D RID: 13933
	[PublicizedFrom(EAccessModifier.Private)]
	public byte sunLight;

	// Token: 0x0400366E RID: 13934
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastHasLight;

	// Token: 0x0400366F RID: 13935
	[PublicizedFrom(EAccessModifier.Private)]
	public string runningSound = "solarpanel_idle";

	// Token: 0x04003671 RID: 13937
	[PublicizedFrom(EAccessModifier.Private)]
	public float lightUpdateTime;
}
