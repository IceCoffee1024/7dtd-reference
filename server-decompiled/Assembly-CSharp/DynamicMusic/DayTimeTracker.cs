using System;
using System.Collections.Generic;
using DynamicMusic.Factories;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A4B RID: 6731
	public class DayTimeTracker : AbstractDayTimeTracker, INotifiableFilter<MusicActionType, SectionType>, INotifiable<MusicActionType>, IFilter<SectionType>
	{
		// Token: 0x0600CC10 RID: 52240 RVA: 0x004A83AC File Offset: 0x004A65AC
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

		// Token: 0x0600CC11 RID: 52241 RVA: 0x004A843A File Offset: 0x004A663A
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

		// Token: 0x0600CC12 RID: 52242 RVA: 0x004A8462 File Offset: 0x004A6662
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdateDay()
		{
			this.currentDay = this.GetCurrentDay();
			this.MusicTimeTracker.Notify();
		}

		// Token: 0x0600CC13 RID: 52243 RVA: 0x004A847B File Offset: 0x004A667B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override int GetCurrentDay()
		{
			return GameUtils.WorldTimeToDays(this.world.worldTime);
		}

		// Token: 0x0600CC14 RID: 52244 RVA: 0x004A848D File Offset: 0x004A668D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override float GetCurrentTime()
		{
			return SkyManager.GetTimeOfDayAsMinutes();
		}

		// Token: 0x0600CC15 RID: 52245 RVA: 0x004A8494 File Offset: 0x004A6694
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

		// Token: 0x0600CC16 RID: 52246 RVA: 0x004A8518 File Offset: 0x004A6718
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

		// Token: 0x0600CC17 RID: 52247 RVA: 0x004A857C File Offset: 0x004A677C
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

		// Token: 0x0600CC18 RID: 52248 RVA: 0x004A867C File Offset: 0x004A687C
		public void Notify(MusicActionType _state)
		{
			this.MusicTimeTracker.Notify(_state);
		}

		// Token: 0x04009B0B RID: 39691
		[PublicizedFrom(EAccessModifier.Private)]
		public const float duskDawnWindowRadius = 0.33333334f;

		// Token: 0x04009B0C RID: 39692
		[PublicizedFrom(EAccessModifier.Private)]
		public World world;

		// Token: 0x04009B0D RID: 39693
		[PublicizedFrom(EAccessModifier.Private)]
		public Conductor conductor;

		// Token: 0x04009B0E RID: 39694
		[PublicizedFrom(EAccessModifier.Private)]
		public IMultiNotifiableFilter MusicTimeTracker;
	}
}
