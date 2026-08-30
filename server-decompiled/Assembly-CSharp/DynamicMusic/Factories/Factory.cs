using System;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic.Factories
{
	// Token: 0x02001A7F RID: 6783
	public static class Factory
	{
		// Token: 0x0600CC43 RID: 52291 RVA: 0x004A6AAE File Offset: 0x004A4CAE
		[Preserve]
		public static Conductor CreateConductor()
		{
			return new Conductor();
		}

		// Token: 0x0600CC44 RID: 52292 RVA: 0x004A6AB5 File Offset: 0x004A4CB5
		[Preserve]
		public static ISectionSelector CreateSectionSelector()
		{
			return new SectionSelector();
		}

		// Token: 0x0600CC45 RID: 52293 RVA: 0x004A6ABC File Offset: 0x004A4CBC
		[Preserve]
		public static IThreatLevel CreateThreatLevel()
		{
			return default(ThreatLevel);
		}

		// Token: 0x0600CC46 RID: 52294 RVA: 0x004A6AD7 File Offset: 0x004A4CD7
		[Preserve]
		public static IFilter<SectionType> CreatePlayerTracker()
		{
			return new PlayerTracker();
		}

		// Token: 0x0600CC47 RID: 52295 RVA: 0x004A6ADE File Offset: 0x004A4CDE
		[Preserve]
		public static DayTimeTracker CreateDayTimeTracker()
		{
			return new DayTimeTracker();
		}

		// Token: 0x0600CC48 RID: 52296 RVA: 0x004A6AE5 File Offset: 0x004A4CE5
		[Preserve]
		public static IMultiNotifiableFilter CreateMusicTimeTracker()
		{
			return new MusicTimeTracker();
		}

		// Token: 0x0600CC49 RID: 52297 RVA: 0x004A6AEC File Offset: 0x004A4CEC
		[Preserve]
		public static ISection CreateSection<T>(SectionType _sectionType) where T : ISection, new()
		{
			T t = new T();
			t.Sect = _sectionType;
			t.Init();
			return t;
		}

		// Token: 0x0600CC4A RID: 52298 RVA: 0x004A6B20 File Offset: 0x004A4D20
		[Preserve]
		public static IConfiguration CreateConfiguration()
		{
			return new Configuration();
		}
	}
}
