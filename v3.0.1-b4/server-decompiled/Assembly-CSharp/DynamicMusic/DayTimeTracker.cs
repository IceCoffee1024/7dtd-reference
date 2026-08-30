using System;
using System.Collections.Generic;
using DynamicMusic.Factories;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A24 RID: 6692
	public class DayTimeTracker : AbstractDayTimeTracker, INotifiableFilter<MusicActionType, SectionType>, INotifiable<MusicActionType>, IFilter<SectionType>
	{
		// Token: 0x0600CA87 RID: 51847 RVA: 0x004A1FB8 File Offset: 0x004A01B8
		public DayTimeTracker()
		{
			this.world = GameManager.Instance.World;
			this.conductor = this.world.dmsConductor;
			ValueTuple<int, int> valueTuple = GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength));
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			int @int = GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
			this.duskTime = (float)item / 24f * (float)@int;
			this.dawnTime = (float)item2 / 24f * (float)@int;
			this.currentDay = this.GetCurrentDay();
			this.MusicTimeTracker = Factory.CreateMusicTimeTracker();
		}

		// Token: 0x0600CA88 RID: 51848 RVA: 0x004A2046 File Offset: 0x004A0246
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			if (this.currentDay != this.GetCurrentDay())
			{
				this.UpdateDay();
			}
			this.currentTime = this.GetCurrentTime();
			this.UpdateDayPeriod();
		}

		// Token: 0x0600CA89 RID: 51849 RVA: 0x004A206E File Offset: 0x004A026E
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdateDay()
		{
			this.currentDay = this.GetCurrentDay();
			this.MusicTimeTracker.Notify();
		}

		// Token: 0x0600CA8A RID: 51850 RVA: 0x004A2087 File Offset: 0x004A0287
		[PublicizedFrom(EAccessModifier.Protected)]
		public override int GetCurrentDay()
		{
			return GameUtils.WorldTimeToDays(this.world.worldTime);
		}

		// Token: 0x0600CA8B RID: 51851 RVA: 0x004A2099 File Offset: 0x004A0299
		[PublicizedFrom(EAccessModifier.Protected)]
		public override float GetCurrentTime()
		{
			return SkyManager.GetTimeOfDayAsMinutes();
		}

		// Token: 0x0600CA8C RID: 51852 RVA: 0x004A20A0 File Offset: 0x004A02A0
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdateDayPeriod()
		{
			if (this.currentTime < this.dawnTime - 0.33333334f)
			{
				this.dayPeriod = AbstractDayTimeTracker.DayPeriodType.Morning;
				return;
			}
			if (this.currentTime <= this.dawnTime + 0.33333334f)
			{
				this.dayPeriod = AbstractDayTimeTracker.DayPeriodType.Dusk;
				return;
			}
			if (this.currentTime < this.duskTime - 0.33333334f)
			{
				this.dayPeriod = AbstractDayTimeTracker.DayPeriodType.Day;
				return;
			}
			if (this.currentTime <= this.duskTime + 0.33333334f)
			{
				this.dayPeriod = AbstractDayTimeTracker.DayPeriodType.Dusk;
				return;
			}
			this.dayPeriod = AbstractDayTimeTracker.DayPeriodType.Night;
		}

		// Token: 0x0600CA8D RID: 51853 RVA: 0x004A2124 File Offset: 0x004A0324
		public override string ToString()
		{
			return string.Format("Current Day: {0}\nCurrent part of the day: {1}\nCurrent Time: {2}\nDawn time: {3}\nDusk Time: {4}\n", new object[]
			{
				this.currentDay,
				this.dayPeriod.ToStringCached<AbstractDayTimeTracker.DayPeriodType>(),
				this.currentTime,
				this.dawnTime,
				this.duskTime
			});
		}

		// Token: 0x0600CA8E RID: 51854 RVA: 0x004A2188 File Offset: 0x004A0388
		public override List<SectionType> Filter(List<SectionType> _sectionTypes)
		{
			this.Update();
			GameStats.GetInt(EnumGameStats.BloodMoonDay);
			GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength));
			if (GameUtils.IsBloodMoonTime(this.world.worldTime, GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength)), GameStats.GetInt(EnumGameStats.BloodMoonDay)))
			{
				_sectionTypes.Clear();
				_sectionTypes.Add(this.conductor.IsBloodmoonMusicEligible ? SectionType.Bloodmoon : SectionType.None);
				return _sectionTypes;
			}
			if (this.dayPeriod.Equals(AbstractDayTimeTracker.DayPeriodType.Dawn) || this.dayPeriod.Equals(AbstractDayTimeTracker.DayPeriodType.Dusk))
			{
				_sectionTypes.Remove(SectionType.Exploration);
				_sectionTypes.Remove(SectionType.HomeDay);
				_sectionTypes.Remove(SectionType.HomeNight);
				_sectionTypes.Remove(SectionType.Suspense);
			}
			else if (!this.dayPeriod.Equals(AbstractDayTimeTracker.DayPeriodType.Day))
			{
				_sectionTypes.Remove(SectionType.Exploration);
				_sectionTypes.Remove(SectionType.HomeDay);
			}
			else
			{
				_sectionTypes.Remove(SectionType.HomeNight);
			}
			return this.MusicTimeTracker.Filter(_sectionTypes);
		}

		// Token: 0x0600CA8F RID: 51855 RVA: 0x004A2288 File Offset: 0x004A0488
		public void Notify(MusicActionType _state)
		{
			this.MusicTimeTracker.Notify(_state);
		}

		// Token: 0x040099DA RID: 39386
		[PublicizedFrom(EAccessModifier.Private)]
		public const float duskDawnWindowRadius = 0.33333334f;

		// Token: 0x040099DB RID: 39387
		[PublicizedFrom(EAccessModifier.Private)]
		public World world;

		// Token: 0x040099DC RID: 39388
		[PublicizedFrom(EAccessModifier.Private)]
		public Conductor conductor;

		// Token: 0x040099DD RID: 39389
		[PublicizedFrom(EAccessModifier.Private)]
		public IMultiNotifiableFilter MusicTimeTracker;
	}
}
