using System;
using System.IO;

// Token: 0x0200123D RID: 4669
public class LiveStats
{
	// Token: 0x0600946D RID: 37997 RVA: 0x00381446 File Offset: 0x0037F646
	public LiveStats(int _maxLiveLevel, int _oversaturationLevel)
	{
		this.maxLiveLevel = _maxLiveLevel;
		this.oversaturationLevel = _oversaturationLevel;
		this.Reset();
	}

	// Token: 0x0600946E RID: 37998 RVA: 0x00381464 File Offset: 0x0037F664
	public void Copy(LiveStats other)
	{
		this.liveLevel = other.liveLevel;
		this.maxLiveLevel = other.maxLiveLevel;
		this.oversaturationLevel = other.oversaturationLevel;
		this.saturationLevel = other.saturationLevel;
		this.exhaustionLevel = other.exhaustionLevel;
		this.timer = other.timer;
	}

	// Token: 0x0600946F RID: 37999 RVA: 0x003814B9 File Offset: 0x0037F6B9
	public void Reset()
	{
		this.timer = 0;
		this.liveLevel = this.maxLiveLevel;
		this.saturationLevel = 0f;
		this.exhaustionLevel = 0f;
	}

	// Token: 0x06009470 RID: 38000 RVA: 0x003814E4 File Offset: 0x0037F6E4
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

	// Token: 0x06009471 RID: 38001 RVA: 0x0038155C File Offset: 0x0037F75C
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

	// Token: 0x06009472 RID: 38002 RVA: 0x003815C9 File Offset: 0x0037F7C9
	public void Read(BinaryReader _br)
	{
		this.liveLevel = (int)_br.ReadInt16();
		this.timer = (int)_br.ReadInt16();
		this.saturationLevel = _br.ReadSingle();
		this.exhaustionLevel = _br.ReadSingle();
	}

	// Token: 0x06009473 RID: 38003 RVA: 0x003815FB File Offset: 0x0037F7FB
	public void Write(BinaryWriter _bw)
	{
		_bw.Write((ushort)this.liveLevel);
		_bw.Write((ushort)this.timer);
		_bw.Write(this.saturationLevel);
		_bw.Write(this.exhaustionLevel);
	}

	// Token: 0x06009474 RID: 38004 RVA: 0x0038162F File Offset: 0x0037F82F
	public int GetLifeLevel()
	{
		return this.liveLevel;
	}

	// Token: 0x06009475 RID: 38005 RVA: 0x00381637 File Offset: 0x0037F837
	public int GetMaxLifeLevel()
	{
		return this.maxLiveLevel;
	}

	// Token: 0x06009476 RID: 38006 RVA: 0x0038163F File Offset: 0x0037F83F
	public void SetLifeLevel(int _value)
	{
		this.liveLevel = _value;
	}

	// Token: 0x06009477 RID: 38007 RVA: 0x00381648 File Offset: 0x0037F848
	public bool IsFilledUp()
	{
		return this.liveLevel >= this.maxLiveLevel;
	}

	// Token: 0x06009478 RID: 38008 RVA: 0x0038165B File Offset: 0x0037F85B
	public void AddExhaustion(float _v)
	{
		this.exhaustionLevel = Math.Min(this.exhaustionLevel + _v, 40f);
	}

	// Token: 0x06009479 RID: 38009 RVA: 0x00381675 File Offset: 0x0037F875
	public float GetSaturationLevel()
	{
		return this.saturationLevel;
	}

	// Token: 0x0600947A RID: 38010 RVA: 0x0038167D File Offset: 0x0037F87D
	public void SetSaturationLevel(float _level)
	{
		this.saturationLevel = _level;
	}

	// Token: 0x0600947B RID: 38011 RVA: 0x00381686 File Offset: 0x0037F886
	public float GetExhaustionLevel()
	{
		return this.exhaustionLevel;
	}

	// Token: 0x0600947C RID: 38012 RVA: 0x0038168E File Offset: 0x0037F88E
	public void SetExhaustionLevel(float _level)
	{
		this.exhaustionLevel = _level;
	}

	// Token: 0x0600947D RID: 38013 RVA: 0x00381697 File Offset: 0x0037F897
	public float GetLifeLevelFraction()
	{
		return (float)this.liveLevel / (float)this.maxLiveLevel;
	}

	// Token: 0x0600947E RID: 38014 RVA: 0x003816A8 File Offset: 0x0037F8A8
	public LiveStats Clone()
	{
		LiveStats liveStats = new LiveStats(this.maxLiveLevel, this.oversaturationLevel);
		liveStats.SetSaturationLevel(this.saturationLevel);
		liveStats.SetExhaustionLevel(this.exhaustionLevel);
		liveStats.SetLifeLevel(this.liveLevel);
		return liveStats;
	}

	// Token: 0x04006F59 RID: 28505
	[PublicizedFrom(EAccessModifier.Private)]
	public int liveLevel;

	// Token: 0x04006F5A RID: 28506
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxLiveLevel;

	// Token: 0x04006F5B RID: 28507
	[PublicizedFrom(EAccessModifier.Private)]
	public int oversaturationLevel;

	// Token: 0x04006F5C RID: 28508
	[PublicizedFrom(EAccessModifier.Private)]
	public float saturationLevel;

	// Token: 0x04006F5D RID: 28509
	[PublicizedFrom(EAccessModifier.Private)]
	public float exhaustionLevel;

	// Token: 0x04006F5E RID: 28510
	[PublicizedFrom(EAccessModifier.Private)]
	public int timer;
}
