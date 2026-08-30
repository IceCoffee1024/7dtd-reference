using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000403 RID: 1027
[Preserve]
public class AIDirectorBloodMoonComponent : AIDirectorComponent
{
	// Token: 0x06001FC6 RID: 8134 RVA: 0x000C0874 File Offset: 0x000BEA74
	public override void InitNewGame()
	{
		base.InitNewGame();
		int num = GameUtils.WorldTimeToDays(this.Director.World.worldTime);
		this.bmDayLast = (num - 1) / 7 * 7;
		this.CalcNextDay(false);
		this.ComputeDawnAndDuskTimes();
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x000C08B8 File Offset: 0x000BEAB8
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

	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x06001FC8 RID: 8136 RVA: 0x000C0A78 File Offset: 0x000BEC78
	public bool BloodMoonActive
	{
		get
		{
			return this.isBloodMoon;
		}
	}

	// Token: 0x06001FC9 RID: 8137 RVA: 0x000C0A80 File Offset: 0x000BEC80
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

	// Token: 0x06001FCA RID: 8138 RVA: 0x000C0AC8 File Offset: 0x000BECC8
	public override void Read(BinaryReader _stream, int _version)
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

	// Token: 0x06001FCB RID: 8139 RVA: 0x000C0B2A File Offset: 0x000BED2A
	public override void Write(BinaryWriter _stream)
	{
		base.Write(_stream);
		_stream.Write(this.bmDayLast);
		_stream.Write(this.bmDay);
		_stream.Write((short)AIDirectorBloodMoonComponent.BloodMoonFrequency);
		_stream.Write((short)AIDirectorBloodMoonComponent.BloodMoonRange);
	}

	// Token: 0x06001FCC RID: 8140 RVA: 0x000C0B63 File Offset: 0x000BED63
	public void AddPlayer(EntityPlayer _player)
	{
		this.players.Add(_player);
	}

	// Token: 0x06001FCD RID: 8141 RVA: 0x000C0B74 File Offset: 0x000BED74
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

	// Token: 0x06001FCE RID: 8142 RVA: 0x000C0BB8 File Offset: 0x000BEDB8
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

	// Token: 0x06001FCF RID: 8143 RVA: 0x000C0C34 File Offset: 0x000BEE34
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

	// Token: 0x06001FD0 RID: 8144 RVA: 0x000C0CFC File Offset: 0x000BEEFC
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

	// Token: 0x06001FD1 RID: 8145 RVA: 0x000C0DCC File Offset: 0x000BEFCC
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

	// Token: 0x06001FD2 RID: 8146 RVA: 0x000C0E14 File Offset: 0x000BF014
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

	// Token: 0x06001FD3 RID: 8147 RVA: 0x000C0E9C File Offset: 0x000BF09C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateNewParty(EntityPlayer _player)
	{
		this.parties.Add(new AIDirectorBloodMoonParty(_player, this, AIDirectorBloodMoonComponent.BloodMoonEnemyCount));
	}

	// Token: 0x06001FD4 RID: 8148 RVA: 0x000C0EB8 File Offset: 0x000BF0B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComputeDawnAndDuskTimes()
	{
		ValueTuple<int, int> valueTuple = GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength));
		this.duskHour = valueTuple.Item1;
		this.dawnHour = valueTuple.Item2;
	}

	// Token: 0x06001FD5 RID: 8149 RVA: 0x000C0EEA File Offset: 0x000BF0EA
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsBloodMoonTime(ulong worldTime)
	{
		return GameUtils.IsBloodMoonTime(worldTime, new ValueTuple<int, int>(this.duskHour, this.dawnHour), this.bmDay);
	}

	// Token: 0x06001FD6 RID: 8150 RVA: 0x000C0F0C File Offset: 0x000BF10C
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

	// Token: 0x06001FD7 RID: 8151 RVA: 0x000C0FD0 File Offset: 0x000BF1D0
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

	// Token: 0x06001FD8 RID: 8152 RVA: 0x000C105C File Offset: 0x000BF25C
	public void LogBM(string format, params object[] args)
	{
		format = string.Format("{0} BM {1}", Time.frameCount, format);
		Log.Warning(format, args);
	}

	// Token: 0x04001584 RID: 5508
	public const int cPartyEnemyMax = 30;

	// Token: 0x04001585 RID: 5509
	public const int cTimeStayAfterDeathScale = 3;

	// Token: 0x04001586 RID: 5510
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSpawnDelay = 1f;

	// Token: 0x04001587 RID: 5511
	[PublicizedFrom(EAccessModifier.Private)]
	public int bmDay;

	// Token: 0x04001588 RID: 5512
	[PublicizedFrom(EAccessModifier.Private)]
	public int bmDayLast;

	// Token: 0x04001589 RID: 5513
	[PublicizedFrom(EAccessModifier.Private)]
	public int bmDayNextOverride;

	// Token: 0x0400158A RID: 5514
	[PublicizedFrom(EAccessModifier.Private)]
	public int dawnHour;

	// Token: 0x0400158B RID: 5515
	[PublicizedFrom(EAccessModifier.Private)]
	public int duskHour;

	// Token: 0x0400158C RID: 5516
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextParty;

	// Token: 0x0400158D RID: 5517
	[PublicizedFrom(EAccessModifier.Private)]
	public float delay;

	// Token: 0x0400158E RID: 5518
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBloodMoon;

	// Token: 0x0400158F RID: 5519
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIDirectorBloodMoonParty> parties = new List<AIDirectorBloodMoonParty>();

	// Token: 0x04001590 RID: 5520
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityPlayer> players = new List<EntityPlayer>();

	// Token: 0x04001591 RID: 5521
	public static int BloodMoonFrequency;

	// Token: 0x04001592 RID: 5522
	public static int BloodMoonRange;

	// Token: 0x04001593 RID: 5523
	public static int BloodMoonEnemyCount;
}
