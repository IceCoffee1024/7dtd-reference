using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000409 RID: 1033
[Preserve]
public class AIDirectorBloodMoonComponent : AIDirectorComponent
{
	// Token: 0x06001FD2 RID: 8146 RVA: 0x000C1094 File Offset: 0x000BF294
	public override void InitNewGame()
	{
		base.InitNewGame();
		int num = GameUtils.WorldTimeToDays(this.Director.World.worldTime);
		this.bmDayLast = (num - 1) / 7 * 7;
		this.CalcNextDay(false);
		this.ComputeDawnAndDuskTimes();
	}

	// Token: 0x06001FD3 RID: 8147 RVA: 0x000C10D8 File Offset: 0x000BF2D8
	public override void Tick(double _dt)
	{
		base.Tick(_dt);
		World world = this.Director.World;
		bool flag = this.isBloodMoon;
		this.isBloodMoon = this.IsBloodMoonTime(world.worldTime);
		if (this.isBloodMoon != flag)
		{
			if (this.isBloodMoon)
			{
				this.StartBloodMoon();
			}
			else
			{
				this.EndBloodMoon();
			}
		}
		if (!this.isBloodMoon)
		{
			int @int = GameStats.GetInt(EnumGameStats.BloodMoonDay);
			if (@int != this.bmDay)
			{
				this.bmDay = @int;
				this.bmDayLast = @int - 1;
				Log.Warning("Blood Moon day stat changed {0}", new object[] { @int });
			}
		}
		if (this.isBloodMoon && GameStats.GetBool(EnumGameStats.IsSpawnEnemies))
		{
			this.delay -= (float)_dt;
			for (int i = 0; i < this.players.Count; i++)
			{
				EntityPlayer entityPlayer = this.players[i];
				if (entityPlayer.bloodMoonParty == null && entityPlayer.IsSpawned())
				{
					this.AddPlayerToParty(entityPlayer);
				}
			}
			for (int j = 0; j < this.parties.Count; j++)
			{
				if (this.nextParty >= this.parties.Count)
				{
					this.nextParty = 0;
				}
				AIDirectorBloodMoonParty aidirectorBloodMoonParty = this.parties[j];
				bool flag2 = j == this.nextParty && this.delay <= 0f;
				if (aidirectorBloodMoonParty.IsEmpty)
				{
					aidirectorBloodMoonParty.KillPartyZombies();
					if (flag2)
					{
						this.nextParty++;
					}
				}
				else if (aidirectorBloodMoonParty.Tick(world, _dt, flag2) && flag2)
				{
					this.delay = 1f / (float)this.parties.Count;
					this.nextParty++;
				}
			}
		}
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x06001FD4 RID: 8148 RVA: 0x000C1298 File Offset: 0x000BF498
	public bool BloodMoonActive
	{
		get
		{
			return this.isBloodMoon;
		}
	}

	// Token: 0x06001FD5 RID: 8149 RVA: 0x000C12A0 File Offset: 0x000BF4A0
	public bool SetForToday(bool _keepNextDay)
	{
		int num = GameUtils.WorldTimeToDays(this.Director.World.worldTime);
		if (num == this.bmDay)
		{
			return false;
		}
		if (_keepNextDay)
		{
			this.bmDayNextOverride = this.bmDay;
		}
		this.SetDay(num);
		return true;
	}

	// Token: 0x06001FD6 RID: 8150 RVA: 0x000C12E8 File Offset: 0x000BF4E8
	public override void Read(PooledBinaryReader _stream, int _version)
	{
		base.Read(_stream, _version);
		if (_version >= 8)
		{
			this.bmDayLast = _stream.ReadInt32();
			int num = _stream.ReadInt32();
			int num2 = (int)_stream.ReadInt16();
			int num3 = (int)_stream.ReadInt16();
			int bloodMoonFrequency = AIDirectorBloodMoonComponent.BloodMoonFrequency;
			int bloodMoonRange = AIDirectorBloodMoonComponent.BloodMoonRange;
			if (bloodMoonFrequency != num2 || bloodMoonRange != num3)
			{
				this.CalcNextDay(false);
			}
			else
			{
				this.SetDay(num);
			}
		}
		this.ComputeDawnAndDuskTimes();
	}

	// Token: 0x06001FD7 RID: 8151 RVA: 0x000C134A File Offset: 0x000BF54A
	public override void Write(PooledBinaryWriter _stream)
	{
		base.Write(_stream);
		_stream.Write(this.bmDayLast);
		_stream.Write(this.bmDay);
		_stream.Write((short)AIDirectorBloodMoonComponent.BloodMoonFrequency);
		_stream.Write((short)AIDirectorBloodMoonComponent.BloodMoonRange);
	}

	// Token: 0x06001FD8 RID: 8152 RVA: 0x000C1383 File Offset: 0x000BF583
	public void AddPlayer(EntityPlayer _player)
	{
		this.players.Add(_player);
	}

	// Token: 0x06001FD9 RID: 8153 RVA: 0x000C1394 File Offset: 0x000BF594
	public void RemovePlayer(EntityPlayer _player)
	{
		if (this.players.Remove(_player))
		{
			for (int i = 0; i < this.parties.Count; i++)
			{
				this.parties[i].PlayerLoggedOut(_player);
			}
		}
	}

	// Token: 0x06001FDA RID: 8154 RVA: 0x000C13D8 File Offset: 0x000BF5D8
	public void TimeChanged(bool isSeek = false)
	{
		if (this.isBloodMoon && !this.IsBloodMoonTime(this.Director.World.worldTime))
		{
			this.EndBloodMoon();
		}
		if (this.bmDay != GameUtils.WorldTimeToElements(this.Director.World.worldTime).Item1 && !this.isBloodMoon && !this.IsBloodMoonTime(this.Director.World.worldTime))
		{
			this.CalcNextDay(isSeek);
		}
	}

	// Token: 0x06001FDB RID: 8155 RVA: 0x000C1454 File Offset: 0x000BF654
	[PublicizedFrom(EAccessModifier.Private)]
	public void StartBloodMoon()
	{
		Log.Out("BloodMoon starting for day " + GameUtils.WorldTimeToDays(this.Director.World.worldTime).ToString());
		this.ClearParties();
		for (int i = 0; i < this.players.Count; i++)
		{
			this.players[i].IsBloodMoonDead = false;
		}
		this.delay = 0f;
		DictionaryList<int, Entity> entities = this.Director.World.Entities;
		for (int j = 0; j < entities.Count; j++)
		{
			EntityEnemy entityEnemy = entities.list[j] as EntityEnemy;
			if (entityEnemy != null)
			{
				entityEnemy.IsBloodMoon = true;
				entityEnemy.timeStayAfterDeath /= 3;
			}
		}
	}

	// Token: 0x06001FDC RID: 8156 RVA: 0x000C151C File Offset: 0x000BF71C
	[PublicizedFrom(EAccessModifier.Private)]
	public void EndBloodMoon()
	{
		Log.Out("Blood moon is over!");
		this.isBloodMoon = false;
		if (this.bmDayNextOverride > 0)
		{
			this.bmDay = this.bmDayNextOverride;
			this.bmDayNextOverride = 0;
			this.SetDay(this.bmDay);
		}
		if (GameUtils.WorldTimeToDays(this.Director.World.worldTime) > this.bmDay)
		{
			this.bmDayLast = this.bmDay;
			this.CalcNextDay(false);
		}
		this.ClearParties();
		DictionaryList<int, Entity> entities = this.Director.World.Entities;
		for (int i = 0; i < entities.Count; i++)
		{
			EntityEnemy entityEnemy = entities.list[i] as EntityEnemy;
			if (entityEnemy != null)
			{
				entityEnemy.bIsChunkObserver = false;
				entityEnemy.IsHordeZombie = false;
				entityEnemy.IsBloodMoon = false;
			}
		}
	}

	// Token: 0x06001FDD RID: 8157 RVA: 0x000C15EC File Offset: 0x000BF7EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearParties()
	{
		this.nextParty = 0;
		this.parties.Clear();
		for (int i = 0; i < this.players.Count; i++)
		{
			this.players[i].bloodMoonParty = null;
		}
	}

	// Token: 0x06001FDE RID: 8158 RVA: 0x000C1634 File Offset: 0x000BF834
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddPlayerToParty(EntityPlayer _player)
	{
		for (int i = 0; i < this.parties.Count; i++)
		{
			AIDirectorBloodMoonParty aidirectorBloodMoonParty = this.parties[i];
			if (aidirectorBloodMoonParty.IsMemberOfParty(_player.entityId))
			{
				aidirectorBloodMoonParty.AddPlayer(_player);
				break;
			}
		}
		if (_player.bloodMoonParty == null)
		{
			int num = 0;
			while (num < this.parties.Count && !this.parties[num].TryAddPlayer(_player))
			{
				num++;
			}
		}
		if (_player.bloodMoonParty == null)
		{
			this.CreateNewParty(_player);
		}
	}

	// Token: 0x06001FDF RID: 8159 RVA: 0x000C16BC File Offset: 0x000BF8BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateNewParty(EntityPlayer _player)
	{
		this.parties.Add(new AIDirectorBloodMoonParty(_player, this, AIDirectorBloodMoonComponent.BloodMoonEnemyCount));
	}

	// Token: 0x06001FE0 RID: 8160 RVA: 0x000C16D8 File Offset: 0x000BF8D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComputeDawnAndDuskTimes()
	{
		ValueTuple<int, int> valueTuple = GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength));
		this.duskHour = valueTuple.Item1;
		this.dawnHour = valueTuple.Item2;
	}

	// Token: 0x06001FE1 RID: 8161 RVA: 0x000C170A File Offset: 0x000BF90A
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsBloodMoonTime(ulong worldTime)
	{
		return GameUtils.IsBloodMoonTime(worldTime, new ValueTuple<int, int>(this.duskHour, this.dawnHour), this.bmDay);
	}

	// Token: 0x06001FE2 RID: 8162 RVA: 0x000C172C File Offset: 0x000BF92C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CalcNextDay(bool isSeek = false)
	{
		int bloodMoonFrequency = AIDirectorBloodMoonComponent.BloodMoonFrequency;
		int num;
		if (bloodMoonFrequency <= 0)
		{
			num = 0;
		}
		else
		{
			int bloodMoonRange = AIDirectorBloodMoonComponent.BloodMoonRange;
			int num2 = bloodMoonFrequency + base.Random.RandomRange(0, bloodMoonRange + 1);
			int i = GameUtils.WorldTimeToDays(this.Director.World.worldTime);
			while (i <= this.bmDayLast)
			{
				this.bmDayLast -= num2;
			}
			if (this.bmDayLast < 0)
			{
				this.bmDayLast = 0;
			}
			num = this.bmDayLast;
			do
			{
				num += num2;
			}
			while (num < i);
			this.bmDayLast = num - num2;
			if (isSeek && this.bmDay > this.bmDayLast && this.bmDay <= this.bmDayLast + bloodMoonFrequency + bloodMoonRange)
			{
				num = this.bmDay;
			}
		}
		this.SetDay(num);
	}

	// Token: 0x06001FE3 RID: 8163 RVA: 0x000C17F0 File Offset: 0x000BF9F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetDay(int day)
	{
		if (GameManager.Instance != null && GameManager.Instance.gameStateManager != null)
		{
			GameManager.Instance.gameStateManager.SetBloodMoonDay(day);
		}
		if (this.bmDay != day)
		{
			this.bmDay = day;
			Log.Out("BloodMoon SetDay: day {0}, last day {1}, freq {2}, range {3}", new object[]
			{
				this.bmDay,
				this.bmDayLast,
				AIDirectorBloodMoonComponent.BloodMoonFrequency,
				AIDirectorBloodMoonComponent.BloodMoonRange
			});
		}
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x000C187C File Offset: 0x000BFA7C
	public void LogBM(string format, params object[] args)
	{
		format = string.Format("{0} BM {1}", Time.frameCount, format);
		Log.Warning(format, args);
	}

	// Token: 0x04001590 RID: 5520
	public const int cPartyEnemyMax = 30;

	// Token: 0x04001591 RID: 5521
	public const int cTimeStayAfterDeathScale = 3;

	// Token: 0x04001592 RID: 5522
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSpawnDelay = 1f;

	// Token: 0x04001593 RID: 5523
	[PublicizedFrom(EAccessModifier.Private)]
	public int bmDay;

	// Token: 0x04001594 RID: 5524
	[PublicizedFrom(EAccessModifier.Private)]
	public int bmDayLast;

	// Token: 0x04001595 RID: 5525
	[PublicizedFrom(EAccessModifier.Private)]
	public int bmDayNextOverride;

	// Token: 0x04001596 RID: 5526
	[PublicizedFrom(EAccessModifier.Private)]
	public int dawnHour;

	// Token: 0x04001597 RID: 5527
	[PublicizedFrom(EAccessModifier.Private)]
	public int duskHour;

	// Token: 0x04001598 RID: 5528
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextParty;

	// Token: 0x04001599 RID: 5529
	[PublicizedFrom(EAccessModifier.Private)]
	public float delay;

	// Token: 0x0400159A RID: 5530
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBloodMoon;

	// Token: 0x0400159B RID: 5531
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIDirectorBloodMoonParty> parties = new List<AIDirectorBloodMoonParty>();

	// Token: 0x0400159C RID: 5532
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityPlayer> players = new List<EntityPlayer>();

	// Token: 0x0400159D RID: 5533
	public static int BloodMoonFrequency;

	// Token: 0x0400159E RID: 5534
	public static int BloodMoonRange;

	// Token: 0x0400159F RID: 5535
	public static int BloodMoonEnemyCount;
}
