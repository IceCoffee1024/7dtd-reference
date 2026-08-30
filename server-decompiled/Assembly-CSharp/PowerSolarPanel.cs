using System;
using System.IO;
using UnityEngine;

// Token: 0x02000912 RID: 2322
public class PowerSolarPanel : PowerSource
{
	// Token: 0x17000720 RID: 1824
	// (get) Token: 0x06004349 RID: 17225 RVA: 0x001A3847 File Offset: 0x001A1A47
	// (set) Token: 0x0600434A RID: 17226 RVA: 0x001A384F File Offset: 0x001A1A4F
	public bool HasLight
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000721 RID: 1825
	// (get) Token: 0x0600434B RID: 17227 RVA: 0x000817FC File Offset: 0x0007F9FC
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.SolarPanel;
		}
	}

	// Token: 0x17000722 RID: 1826
	// (get) Token: 0x0600434C RID: 17228 RVA: 0x001A3858 File Offset: 0x001A1A58
	public override string OnSound
	{
		get
		{
			return "solarpanel_on";
		}
	}

	// Token: 0x17000723 RID: 1827
	// (get) Token: 0x0600434D RID: 17229 RVA: 0x001A385F File Offset: 0x001A1A5F
	public override string OffSound
	{
		get
		{
			return "solarpanel_off";
		}
	}

	// Token: 0x0600434E RID: 17230 RVA: 0x001A3868 File Offset: 0x001A1A68
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

	// Token: 0x0600434F RID: 17231 RVA: 0x001A3912 File Offset: 0x001A1B12
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void TickPowerGeneration()
	{
		if (this.HasLight)
		{
			this.CurrentPower = this.MaxOutput;
		}
	}

	// Token: 0x06004350 RID: 17232 RVA: 0x001A3928 File Offset: 0x001A1B28
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

	// Token: 0x06004351 RID: 17233 RVA: 0x001A3A51 File Offset: 0x001A1C51
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool ShouldClearPower()
	{
		return this.sunLight != 15 || !GameManager.Instance.World.IsDaytime();
	}

	// Token: 0x06004352 RID: 17234 RVA: 0x001A3A71 File Offset: 0x001A1C71
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void RefreshPowerStats()
	{
		base.RefreshPowerStats();
		this.MaxPower = this.MaxOutput;
	}

	// Token: 0x06004353 RID: 17235 RVA: 0x001A3A85 File Offset: 0x001A1C85
	public override void read(BinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		if (PowerManager.Instance.CurrentFileVersion >= 2)
		{
			this.sunLight = _br.ReadByte();
		}
	}

	// Token: 0x06004354 RID: 17236 RVA: 0x001A3AA8 File Offset: 0x001A1CA8
	public override void write(BinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.sunLight);
	}

	// Token: 0x04003658 RID: 13912
	public ushort InputFromSun;

	// Token: 0x04003659 RID: 13913
	[PublicizedFrom(EAccessModifier.Private)]
	public byte sunLight;

	// Token: 0x0400365A RID: 13914
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastHasLight;

	// Token: 0x0400365B RID: 13915
	[PublicizedFrom(EAccessModifier.Private)]
	public string runningSound = "solarpanel_idle";

	// Token: 0x0400365D RID: 13917
	[PublicizedFrom(EAccessModifier.Private)]
	public float lightUpdateTime;
}
