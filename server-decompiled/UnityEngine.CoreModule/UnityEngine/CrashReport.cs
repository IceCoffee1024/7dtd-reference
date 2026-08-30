using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000118 RID: 280
	[NativeHeader("Runtime/Export/CrashReport/CrashReport.bindings.h")]
	public sealed class CrashReport
	{
		// Token: 0x060006C9 RID: 1737 RVA: 0x0000962C File Offset: 0x0000782C
		private static int Compare(CrashReport c1, CrashReport c2)
		{
			long ticks = c1.time.Ticks;
			long ticks2 = c2.time.Ticks;
			bool flag = ticks > ticks2;
			int num;
			if (flag)
			{
				num = 1;
			}
			else
			{
				bool flag2 = ticks < ticks2;
				if (flag2)
				{
					num = -1;
				}
				else
				{
					num = 0;
				}
			}
			return num;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00009674 File Offset: 0x00007874
		private static void PopulateReports()
		{
			object obj = CrashReport.reportsLock;
			lock (obj)
			{
				bool flag2 = CrashReport.internalReports != null;
				if (!flag2)
				{
					string[] reports = CrashReport.GetReports();
					CrashReport.internalReports = new List<CrashReport>(reports.Length);
					foreach (string text in reports)
					{
						double num;
						string reportData = CrashReport.GetReportData(text, out num);
						DateTime dateTime = new DateTime(1970, 1, 1).AddSeconds(num);
						CrashReport.internalReports.Add(new CrashReport(text, dateTime, reportData));
					}
					CrashReport.internalReports.Sort(new Comparison<CrashReport>(CrashReport.Compare));
				}
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00009748 File Offset: 0x00007948
		public static CrashReport[] reports
		{
			get
			{
				CrashReport.PopulateReports();
				object obj = CrashReport.reportsLock;
				CrashReport[] array;
				lock (obj)
				{
					array = CrashReport.internalReports.ToArray();
				}
				return array;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x00009798 File Offset: 0x00007998
		public static CrashReport lastReport
		{
			get
			{
				CrashReport.PopulateReports();
				object obj = CrashReport.reportsLock;
				lock (obj)
				{
					bool flag2 = CrashReport.internalReports.Count > 0;
					if (flag2)
					{
						return CrashReport.internalReports[CrashReport.internalReports.Count - 1];
					}
				}
				return null;
			}
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0000980C File Offset: 0x00007A0C
		public static void RemoveAll()
		{
			foreach (CrashReport crashReport in CrashReport.reports)
			{
				crashReport.Remove();
			}
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0000983A File Offset: 0x00007A3A
		private CrashReport(string id, DateTime time, string text)
		{
			this.id = id;
			this.time = time;
			this.text = text;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0000985C File Offset: 0x00007A5C
		public void Remove()
		{
			bool flag = CrashReport.RemoveReport(this.id);
			if (flag)
			{
				object obj = CrashReport.reportsLock;
				lock (obj)
				{
					CrashReport.internalReports.Remove(this);
				}
			}
		}

		// Token: 0x060006D0 RID: 1744
		[FreeFunction(Name = "CrashReport_Bindings::GetReports", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string[] GetReports();

		// Token: 0x060006D1 RID: 1745
		[FreeFunction(Name = "CrashReport_Bindings::GetReportData", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetReportData(string id, out double secondsSinceUnixEpoch);

		// Token: 0x060006D2 RID: 1746
		[FreeFunction(Name = "CrashReport_Bindings::RemoveReport", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool RemoveReport(string id);

		// Token: 0x0400039C RID: 924
		private static List<CrashReport> internalReports;

		// Token: 0x0400039D RID: 925
		private static object reportsLock = new object();

		// Token: 0x0400039E RID: 926
		private readonly string id;

		// Token: 0x0400039F RID: 927
		public readonly DateTime time;

		// Token: 0x040003A0 RID: 928
		public readonly string text;
	}
}
