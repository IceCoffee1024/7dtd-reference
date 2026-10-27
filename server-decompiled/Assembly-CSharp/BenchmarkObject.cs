using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// Token: 0x020013D0 RID: 5072
public class BenchmarkObject
{
	// Token: 0x06009F73 RID: 40819 RVA: 0x003C3A80 File Offset: 0x003C1C80
	[Conditional("PROFILEx")]
	public static void StartTimer(string _benchmarkName, object _watchObject)
	{
		BenchmarkObject.BenchmarkContainer benchmarkContainer = new BenchmarkObject.BenchmarkContainer(_benchmarkName);
		Dictionary<object, BenchmarkObject.BenchmarkContainer> dictionary = BenchmarkObject.benchmarks;
		lock (dictionary)
		{
			BenchmarkObject.benchmarks[_watchObject] = benchmarkContainer;
		}
		benchmarkContainer.startTick = DateTime.Now.Ticks;
	}

	// Token: 0x06009F74 RID: 40820 RVA: 0x003C3AE0 File Offset: 0x003C1CE0
	[Conditional("PROFILEx")]
	public static void SwitchObject(object _old, object _new)
	{
		Dictionary<object, BenchmarkObject.BenchmarkContainer> dictionary = BenchmarkObject.benchmarks;
		lock (dictionary)
		{
			if (BenchmarkObject.benchmarks.ContainsKey(_old))
			{
				BenchmarkObject.BenchmarkContainer benchmarkContainer = BenchmarkObject.benchmarks[_old];
				BenchmarkObject.benchmarks.Remove(_old);
				BenchmarkObject.benchmarks[_new] = benchmarkContainer;
			}
			else
			{
				Log.Out("SWITCHOBJECT: Object not found");
			}
		}
	}

	// Token: 0x06009F75 RID: 40821 RVA: 0x003C3B58 File Offset: 0x003C1D58
	[Conditional("PROFILEx")]
	public static void UpdateName(object _watchObject, string _nameAppend)
	{
		BenchmarkObject.BenchmarkContainer benchmarkContainer = null;
		Dictionary<object, BenchmarkObject.BenchmarkContainer> dictionary = BenchmarkObject.benchmarks;
		lock (dictionary)
		{
			if (BenchmarkObject.benchmarks.ContainsKey(_watchObject))
			{
				benchmarkContainer = BenchmarkObject.benchmarks[_watchObject];
			}
		}
		if (benchmarkContainer != null)
		{
			BenchmarkObject.BenchmarkContainer benchmarkContainer2 = benchmarkContainer;
			benchmarkContainer2.name += _nameAppend;
			return;
		}
		Log.Out("UPDATENAME: Object not found: " + _nameAppend);
	}

	// Token: 0x06009F76 RID: 40822 RVA: 0x003C3BD4 File Offset: 0x003C1DD4
	[Conditional("PROFILEx")]
	public static void StopTimer(object _watchObject)
	{
		long ticks = DateTime.Now.Ticks;
		Dictionary<object, BenchmarkObject.BenchmarkContainer> dictionary = BenchmarkObject.benchmarks;
		lock (dictionary)
		{
			if (BenchmarkObject.benchmarks.ContainsKey(_watchObject))
			{
				BenchmarkObject.benchmarks[_watchObject].endTick = ticks;
			}
			else
			{
				Log.Out("STOPTIMER: Object not found");
			}
		}
	}

	// Token: 0x06009F77 RID: 40823 RVA: 0x003C3C48 File Offset: 0x003C1E48
	[Conditional("PROFILEx")]
	public static void PrintAll()
	{
		if (BenchmarkObject.benchmarks.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<object, BenchmarkObject.BenchmarkContainer> keyValuePair in BenchmarkObject.benchmarks)
			{
				if (keyValuePair.Value.name.Length > num)
				{
					num = keyValuePair.Value.name.Length;
				}
			}
			string text = "{0} {1} {2} {3}" + Environment.NewLine;
			stringBuilder.Append(string.Format(text, new object[] { "Name", "Start", "End", "Duration" }));
			foreach (BenchmarkObject.BenchmarkContainer benchmarkContainer in from b in BenchmarkObject.benchmarks.Values.ToList<BenchmarkObject.BenchmarkContainer>()
				orderby b.startTick
				select b)
			{
				stringBuilder.Append(string.Format(text, new object[]
				{
					benchmarkContainer.name,
					benchmarkContainer.startTick / 10L,
					benchmarkContainer.endTick / 10L,
					benchmarkContainer.ticks / 10L
				}));
			}
			SdFile.WriteAllText(GameIO.GetGameDir("") + "durations.txt", stringBuilder.ToString());
		}
	}

	// Token: 0x040078D4 RID: 30932
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<object, BenchmarkObject.BenchmarkContainer> benchmarks = new Dictionary<object, BenchmarkObject.BenchmarkContainer>();

	// Token: 0x020013D1 RID: 5073
	public class BenchmarkContainer
	{
		// Token: 0x17001311 RID: 4881
		// (get) Token: 0x06009F7A RID: 40826 RVA: 0x003C3E00 File Offset: 0x003C2000
		// (set) Token: 0x06009F7B RID: 40827 RVA: 0x003C3E08 File Offset: 0x003C2008
		public string name
		{
			get
			{
				return this.pName;
			}
			set
			{
				this.pName = value;
			}
		}

		// Token: 0x17001312 RID: 4882
		// (get) Token: 0x06009F7C RID: 40828 RVA: 0x003C3E11 File Offset: 0x003C2011
		public long ticks
		{
			get
			{
				return this.endTick - this.startTick;
			}
		}

		// Token: 0x06009F7D RID: 40829 RVA: 0x003C3E20 File Offset: 0x003C2020
		public BenchmarkContainer(string _name)
		{
			this.pName = _name;
		}

		// Token: 0x040078D5 RID: 30933
		[PublicizedFrom(EAccessModifier.Private)]
		public string pName;

		// Token: 0x040078D6 RID: 30934
		public long startTick;

		// Token: 0x040078D7 RID: 30935
		public long endTick;
	}
}
