using System;
using System.IO;

// Token: 0x02001259 RID: 4697
public class LiveStats
{
	// Token: 0x06009571 RID: 38257 RVA: 0x00386756 File Offset: 0x00384956
	public LiveStats(int _maxLiveLevel, int _oversaturationLevel)
	{
		this.maxLiveLevel = _maxLiveLevel;
		this.oversaturationLevel = _oversaturationLevel;
		this.Reset();
	}

	// Token: 0x06009572 RID: 38258 RVA: 0x00386774 File Offset: 0x00384974
	public void Copy(LiveStats other)
	{
		this.liveLevel = other.liveLevel;
		this.maxLiveLevel = other.maxLiveLevel;
		this.oversaturationLevel = other.oversaturationLevel;
		this.saturationLevel = other.saturationLevel;
		this.exhaustionLevel = other.exhaustionLevel;
		this.timer = other.timer;
	}

	// Token: 0x06009573 RID: 38259 RVA: 0x003867C9 File Offset: 0x003849C9
	public void Reset()
	{
		this.timer = 0;
		this.liveLevel = this.maxLiveLevel;
		this.saturationLevel = 0f;
		this.exhaustionLevel = 0f;
	}

	// Token: 0x06009574 RID: 38260 RVA: 0x003867F4 File Offset: 0x003849F4
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

	// Token: 0x06009575 RID: 38261 RVA: 0x0038686C File Offset: 0x00384A6C
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

	// Token: 0x06009576 RID: 38262 RVA: 0x003868D9 File Offset: 0x00384AD9
	public void Read(BinaryReader _br)
	{
		this.liveLevel = (int)_br.ReadInt16();
		this.timer = (int)_br.ReadInt16();
		this.saturationLevel = _br.ReadSingle();
		this.exhaustionLevel = _br.ReadSingle();
	}

	// Token: 0x06009577 RID: 38263 RVA: 0x0038690B File Offset: 0x00384B0B
	public void Write(BinaryWriter _bw)
	{
		_bw.Write((ushort)this.liveLevel);
		_bw.Write((ushort)this.timer);
		_bw.Write(this.saturationLevel);
		_bw.Write(this.exhaustionLevel);
	}

	// Token: 0x06009578 RID: 38264 RVA: 0x0038693F File Offset: 0x00384B3F
	public int GetLifeLevel()
	{
		return this.liveLevel;
	}

	// Token: 0x06009579 RID: 38265 RVA: 0x00386947 File Offset: 0x00384B47
	public int GetMaxLifeLevel()
	{
		return this.maxLiveLevel;
	}

	// Token: 0x0600957A RID: 38266 RVA: 0x0038694F File Offset: 0x00384B4F
	public void SetLifeLevel(int _value)
	{
		this.liveLevel = _value;
	}

	// Token: 0x0600957B RID: 38267 RVA: 0x00386958 File Offset: 0x00384B58
	public bool IsFilledUp()
	{
		return this.liveLevel >= this.maxLiveLevel;
	}

	// Token: 0x0600957C RID: 38268 RVA: 0x0038696B File Offset: 0x00384B6B
	public void AddExhaustion(float _v)
	{
		this.exhaustionLevel = Math.Min(this.exhaustionLevel + _v, 40f);
	}

	// Token: 0x0600957D RID: 38269 RVA: 0x00386985 File Offset: 0x00384B85
	public float GetSaturationLevel()
	{
		return this.saturationLevel;
	}

	// Token: 0x0600957E RID: 38270 RVA: 0x0038698D File Offset: 0x00384B8D
	public void SetSaturationLevel(float _level)
	{
		this.saturationLevel = _level;
	}

	// Token: 0x0600957F RID: 38271 RVA: 0x00386996 File Offset: 0x00384B96
	public float GetExhaustionLevel()
	{
		return this.exhaustionLevel;
	}

	// Token: 0x06009580 RID: 38272 RVA: 0x0038699E File Offset: 0x00384B9E
	public void SetExhaustionLevel(float _level)
	{
		this.exhaustionLevel = _level;
	}

	// Token: 0x06009581 RID: 38273 RVA: 0x003869A7 File Offset: 0x00384BA7
	public float GetLifeLevelFraction()
	{
		return (float)this.liveLevel / (float)this.maxLiveLevel;
	}

	// Token: 0x06009582 RID: 38274 RVA: 0x003869B8 File Offset: 0x00384BB8
	public LiveStats Clone()
	{
		LiveStats liveStats = new LiveStats(this.maxLiveLevel, this.oversaturationLevel);
		liveStats.SetSaturationLevel(this.saturationLevel);
		liveStats.SetExhaustionLevel(this.exhaustionLevel);
		liveStats.SetLifeLevel(this.liveLevel);
		return liveStats;
	}

	// Token: 0x04007032 RID: 28722
	[PublicizedFrom(EAccessModifier.Private)]
	public int liveLevel;

	// Token: 0x04007033 RID: 28723
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxLiveLevel;

	// Token: 0x04007034 RID: 28724
	[PublicizedFrom(EAccessModifier.Private)]
	public int oversaturationLevel;

	// Token: 0x04007035 RID: 28725
	[PublicizedFrom(EAccessModifier.Private)]
	public float saturationLevel;

	// Token: 0x04007036 RID: 28726
	[PublicizedFrom(EAccessModifier.Private)]
	public float exhaustionLevel;

	// Token: 0x04007037 RID: 28727
	[PublicizedFrom(EAccessModifier.Private)]
	public int timer;
}
