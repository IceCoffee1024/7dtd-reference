using System;
using System.Collections.Generic;
using DynamicMusic.Factories;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A77 RID: 6775
	public class DayTimeTracker : AbstractDayTimeTracker, INotifiableFilter<MusicActionType, SectionType>, INotifiable<MusicActionType>, IFilter<SectionType>
	{
		// Token: 0x0600CD68 RID: 52584 RVA: 0x004AC878 File Offset: 0x004AAA78
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

		// Token: 0x0600CD69 RID: 52585 RVA: 0x004AC906 File Offset: 0x004AAB06
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

		// Token: 0x0600CD6A RID: 52586 RVA: 0x004AC92E File Offset: 0x004AAB2E
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdateDay()
		{
			this.currentDay = this.GetCurrentDay();
			this.MusicTimeTracker.Notify();
		}

		// Token: 0x0600CD6B RID: 52587 RVA: 0x004AC947 File Offset: 0x004AAB47
		[PublicizedFrom(EAccessModifier.Protected)]
		public override int GetCurrentDay()
		{
			return GameUtils.WorldTimeToDays(this.world.worldTime);
		}

		// Token: 0x0600CD6C RID: 52588 RVA: 0x004AC959 File Offset: 0x004AAB59
		[PublicizedFrom(EAccessModifier.Protected)]
		public override float GetCurrentTime()
		{
			return SkyManager.GetTimeOfDayAsMinutes();
		}

		// Token: 0x0600CD6D RID: 52589 RVA: 0x004AC960 File Offset: 0x004AAB60
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

		// Token: 0x0600CD6E RID: 52590 RVA: 0x004AC9E4 File Offset: 0x004AABE4
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

		// Token: 0x0600CD6F RID: 52591 RVA: 0x004ACA48 File Offset: 0x004AAC48
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

		// Token: 0x0600CD70 RID: 52592 RVA: 0x004ACB48 File Offset: 0x004AAD48
		public void Notify(MusicActionType _state)
		{
			this.MusicTimeTracker.Notify(_state);
		}

		// Token: 0x04009BD1 RID: 39889
		[PublicizedFrom(EAccessModifier.Private)]
		public const float duskDawnWindowRadius = 0.33333334f;

		// Token: 0x04009BD2 RID: 39890
		[PublicizedFrom(EAccessModifier.Private)]
		public World world;

		// Token: 0x04009BD3 RID: 39891
		[PublicizedFrom(EAccessModifier.Private)]
		public Conductor conductor;

		// Token: 0x04009BD4 RID: 39892
		[PublicizedFrom(EAccessModifier.Private)]
		public IMultiNotifiableFilter MusicTimeTracker;
	}
}
