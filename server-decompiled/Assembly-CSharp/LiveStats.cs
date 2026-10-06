using System;
using System.IO;

// Token: 0x02001271 RID: 4721
public class LiveStats
{
	// Token: 0x060095FE RID: 38398 RVA: 0x0038AF9E File Offset: 0x0038919E
	public LiveStats(int _maxLiveLevel, int _oversaturationLevel)
	{
		this.maxLiveLevel = _maxLiveLevel;
		this.oversaturationLevel = _oversaturationLevel;
		this.Reset();
	}

	// Token: 0x060095FF RID: 38399 RVA: 0x0038AFBC File Offset: 0x003891BC
	public void Copy(LiveStats other)
	{
		this.liveLevel = other.liveLevel;
		this.maxLiveLevel = other.maxLiveLevel;
		this.oversaturationLevel = other.oversaturationLevel;
		this.saturationLevel = other.saturationLevel;
		this.exhaustionLevel = other.exhaustionLevel;
		this.timer = other.timer;
	}

	// Token: 0x06009600 RID: 38400 RVA: 0x0038B011 File Offset: 0x00389211
	public void Reset()
	{
		this.timer = 0;
		this.liveLevel = this.maxLiveLevel;
		this.saturationLevel = 0f;
		this.exhaustionLevel = 0f;
	}

	// Token: 0x06009601 RID: 38401 RVA: 0x0038B03C File Offset: 0x0038923C
	public void AddStats(int _addLifeValue)
	{
		this.liveLevel += _addLifeValue;
		if (this.liveLevel > this.maxLiveLevel)
		{
			this.saturationLevel += (float)(this.liveLevel - this.maxLiveLevel);
			this.liveLevel = this.maxLiveLevel;
			this.saturationLevel = Utils.FastMin(this.saturationLevel, (float)this.oversaturationLevel);
		}
		if (this.liveLevel < 0)
		{
			this.liveLevel = 0;
		}
	}

	// Token: 0x06009602 RID: 38402 RVA: 0x0038B0B4 File Offset: 0x003892B4
	public void OnUpdate(EntityPlayer _entityPlayer)
	{
		while (this.exhaustionLevel > 1f)
		{
			this.exhaustionLevel -= 1f;
			if (this.saturationLevel > 0f)
			{
				this.saturationLevel = Math.Max(this.saturationLevel - 1f, 0f);
			}
			else
			{
				this.liveLevel = Math.Max(this.liveLevel - 1, 0);
			}
		}
	}

	// Token: 0x06009603 RID: 38403 RVA: 0x0038B121 File Offset: 0x00389321
	public void Read(BinaryReader _br)
	{
		this.liveLevel = (int)_br.ReadInt16();
		this.timer = (int)_br.ReadInt16();
		this.saturationLevel = _br.ReadSingle();
		this.exhaustionLevel = _br.ReadSingle();
	}

	// Token: 0x06009604 RID: 38404 RVA: 0x0038B153 File Offset: 0x00389353
	public void Write(BinaryWriter _bw)
	{
		_bw.Write((ushort)this.liveLevel);
		_bw.Write((ushort)this.timer);
		_bw.Write(this.saturationLevel);
		_bw.Write(this.exhaustionLevel);
	}

	// Token: 0x06009605 RID: 38405 RVA: 0x0038B187 File Offset: 0x00389387
	public int GetLifeLevel()
	{
		return this.liveLevel;
	}

	// Token: 0x06009606 RID: 38406 RVA: 0x0038B18F File Offset: 0x0038938F
	public int GetMaxLifeLevel()
	{
		return this.maxLiveLevel;
	}

	// Token: 0x06009607 RID: 38407 RVA: 0x0038B197 File Offset: 0x00389397
	public void SetLifeLevel(int _value)
	{
		this.liveLevel = _value;
	}

	// Token: 0x06009608 RID: 38408 RVA: 0x0038B1A0 File Offset: 0x003893A0
	public bool IsFilledUp()
	{
		return this.liveLevel >= this.maxLiveLevel;
	}

	// Token: 0x06009609 RID: 38409 RVA: 0x0038B1B3 File Offset: 0x003893B3
	public void AddExhaustion(float _v)
	{
		this.exhaustionLevel = Math.Min(this.exhaustionLevel + _v, 40f);
	}

	// Token: 0x0600960A RID: 38410 RVA: 0x0038B1CD File Offset: 0x003893CD
	public float GetSaturationLevel()
	{
		return this.saturationLevel;
	}

	// Token: 0x0600960B RID: 38411 RVA: 0x0038B1D5 File Offset: 0x003893D5
	public void SetSaturationLevel(float _level)
	{
		this.saturationLevel = _level;
	}

	// Token: 0x0600960C RID: 38412 RVA: 0x0038B1DE File Offset: 0x003893DE
	public float GetExhaustionLevel()
	{
		return this.exhaustionLevel;
	}

	// Token: 0x0600960D RID: 38413 RVA: 0x0038B1E6 File Offset: 0x003893E6
	public void SetExhaustionLevel(float _level)
	{
		this.exhaustionLevel = _level;
	}

	// Token: 0x0600960E RID: 38414 RVA: 0x0038B1EF File Offset: 0x003893EF
	public float GetLifeLevelFraction()
	{
		return (float)this.liveLevel / (float)this.maxLiveLevel;
	}

	// Token: 0x0600960F RID: 38415 RVA: 0x0038B200 File Offset: 0x00389400
	public LiveStats Clone()
	{
		LiveStats liveStats = new LiveStats(this.maxLiveLevel, this.oversaturationLevel);
		liveStats.SetSaturationLevel(this.saturationLevel);
		liveStats.SetExhaustionLevel(this.exhaustionLevel);
		liveStats.SetLifeLevel(this.liveLevel);
		return liveStats;
	}

	// Token: 0x0400709C RID: 28828
	[PublicizedFrom(EAccessModifier.Private)]
	public int liveLevel;

	// Token: 0x0400709D RID: 28829
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxLiveLevel;

	// Token: 0x0400709E RID: 28830
	[PublicizedFrom(EAccessModifier.Private)]
	public int oversaturationLevel;

	// Token: 0x0400709F RID: 28831
	[PublicizedFrom(EAccessModifier.Private)]
	public float saturationLevel;

	// Token: 0x040070A0 RID: 28832
	[PublicizedFrom(EAccessModifier.Private)]
	public float exhaustionLevel;

	// Token: 0x040070A1 RID: 28833
	[PublicizedFrom(EAccessModifier.Private)]
	public int timer;
}
