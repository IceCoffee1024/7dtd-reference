using System;
using System.IO;

// Token: 0x02001283 RID: 4739
public class LiveStats
{
	// Token: 0x06009655 RID: 38485 RVA: 0x0038D39A File Offset: 0x0038B59A
	public LiveStats(int _maxLiveLevel, int _oversaturationLevel)
	{
		this.maxLiveLevel = _maxLiveLevel;
		this.oversaturationLevel = _oversaturationLevel;
		this.Reset();
	}

	// Token: 0x06009656 RID: 38486 RVA: 0x0038D3B8 File Offset: 0x0038B5B8
	public void Copy(LiveStats other)
	{
		this.liveLevel = other.liveLevel;
		this.maxLiveLevel = other.maxLiveLevel;
		this.oversaturationLevel = other.oversaturationLevel;
		this.saturationLevel = other.saturationLevel;
		this.exhaustionLevel = other.exhaustionLevel;
		this.timer = other.timer;
	}

	// Token: 0x06009657 RID: 38487 RVA: 0x0038D40D File Offset: 0x0038B60D
	public void Reset()
	{
		this.timer = 0;
		this.liveLevel = this.maxLiveLevel;
		this.saturationLevel = 0f;
		this.exhaustionLevel = 0f;
	}

	// Token: 0x06009658 RID: 38488 RVA: 0x0038D438 File Offset: 0x0038B638
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

	// Token: 0x06009659 RID: 38489 RVA: 0x0038D4B0 File Offset: 0x0038B6B0
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

	// Token: 0x0600965A RID: 38490 RVA: 0x0038D51D File Offset: 0x0038B71D
	public void Read(BinaryReader _br)
	{
		this.liveLevel = (int)_br.ReadInt16();
		this.timer = (int)_br.ReadInt16();
		this.saturationLevel = _br.ReadSingle();
		this.exhaustionLevel = _br.ReadSingle();
	}

	// Token: 0x0600965B RID: 38491 RVA: 0x0038D54F File Offset: 0x0038B74F
	public void Write(BinaryWriter _bw)
	{
		_bw.Write((ushort)this.liveLevel);
		_bw.Write((ushort)this.timer);
		_bw.Write(this.saturationLevel);
		_bw.Write(this.exhaustionLevel);
	}

	// Token: 0x0600965C RID: 38492 RVA: 0x0038D583 File Offset: 0x0038B783
	public int GetLifeLevel()
	{
		return this.liveLevel;
	}

	// Token: 0x0600965D RID: 38493 RVA: 0x0038D58B File Offset: 0x0038B78B
	public int GetMaxLifeLevel()
	{
		return this.maxLiveLevel;
	}

	// Token: 0x0600965E RID: 38494 RVA: 0x0038D593 File Offset: 0x0038B793
	public void SetLifeLevel(int _value)
	{
		this.liveLevel = _value;
	}

	// Token: 0x0600965F RID: 38495 RVA: 0x0038D59C File Offset: 0x0038B79C
	public bool IsFilledUp()
	{
		return this.liveLevel >= this.maxLiveLevel;
	}

	// Token: 0x06009660 RID: 38496 RVA: 0x0038D5AF File Offset: 0x0038B7AF
	public void AddExhaustion(float _v)
	{
		this.exhaustionLevel = Math.Min(this.exhaustionLevel + _v, 40f);
	}

	// Token: 0x06009661 RID: 38497 RVA: 0x0038D5C9 File Offset: 0x0038B7C9
	public float GetSaturationLevel()
	{
		return this.saturationLevel;
	}

	// Token: 0x06009662 RID: 38498 RVA: 0x0038D5D1 File Offset: 0x0038B7D1
	public void SetSaturationLevel(float _level)
	{
		this.saturationLevel = _level;
	}

	// Token: 0x06009663 RID: 38499 RVA: 0x0038D5DA File Offset: 0x0038B7DA
	public float GetExhaustionLevel()
	{
		return this.exhaustionLevel;
	}

	// Token: 0x06009664 RID: 38500 RVA: 0x0038D5E2 File Offset: 0x0038B7E2
	public void SetExhaustionLevel(float _level)
	{
		this.exhaustionLevel = _level;
	}

	// Token: 0x06009665 RID: 38501 RVA: 0x0038D5EB File Offset: 0x0038B7EB
	public float GetLifeLevelFraction()
	{
		return (float)this.liveLevel / (float)this.maxLiveLevel;
	}

	// Token: 0x06009666 RID: 38502 RVA: 0x0038D5FC File Offset: 0x0038B7FC
	public LiveStats Clone()
	{
		LiveStats liveStats = new LiveStats(this.maxLiveLevel, this.oversaturationLevel);
		liveStats.SetSaturationLevel(this.saturationLevel);
		liveStats.SetExhaustionLevel(this.exhaustionLevel);
		liveStats.SetLifeLevel(this.liveLevel);
		return liveStats;
	}

	// Token: 0x040070DD RID: 28893
	[PublicizedFrom(EAccessModifier.Private)]
	public int liveLevel;

	// Token: 0x040070DE RID: 28894
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxLiveLevel;

	// Token: 0x040070DF RID: 28895
	[PublicizedFrom(EAccessModifier.Private)]
	public int oversaturationLevel;

	// Token: 0x040070E0 RID: 28896
	[PublicizedFrom(EAccessModifier.Private)]
	public float saturationLevel;

	// Token: 0x040070E1 RID: 28897
	[PublicizedFrom(EAccessModifier.Private)]
	public float exhaustionLevel;

	// Token: 0x040070E2 RID: 28898
	[PublicizedFrom(EAccessModifier.Private)]
	public int timer;
}
