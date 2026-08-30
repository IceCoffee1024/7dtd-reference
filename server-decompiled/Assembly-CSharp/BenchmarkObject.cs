using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// Token: 0x02001383 RID: 4995
public class BenchmarkObject
{
	// Token: 0x06009D73 RID: 40307 RVA: 0x003B779C File Offset: 0x003B599C
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

	// Token: 0x06009D74 RID: 40308 RVA: 0x003B77FC File Offset: 0x003B59FC
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

	// Token: 0x06009D75 RID: 40309 RVA: 0x003B7874 File Offset: 0x003B5A74
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

	// Token: 0x06009D76 RID: 40310 RVA: 0x003B78F0 File Offset: 0x003B5AF0
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

	// Token: 0x06009D77 RID: 40311 RVA: 0x003B7964 File Offset: 0x003B5B64
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

	// Token: 0x0400773F RID: 30527
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<object, BenchmarkObject.BenchmarkContainer> benchmarks = new Dictionary<object, BenchmarkObject.BenchmarkContainer>();

	// Token: 0x02001384 RID: 4996
	public class BenchmarkContainer
	{
		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x06009D7A RID: 40314 RVA: 0x003B7B1C File Offset: 0x003B5D1C
		// (set) Token: 0x06009D7B RID: 40315 RVA: 0x003B7B24 File Offset: 0x003B5D24
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

		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x06009D7C RID: 40316 RVA: 0x003B7B2D File Offset: 0x003B5D2D
		public long ticks
		{
			get
			{
				return this.endTick - this.startTick;
			}
		}

		// Token: 0x06009D7D RID: 40317 RVA: 0x003B7B3C File Offset: 0x003B5D3C
		public BenchmarkContainer(string _name)
		{
			this.pName = _name;
		}

		// Token: 0x04007740 RID: 30528
		[PublicizedFrom(EAccessModifier.Private)]
		public string pName;

		// Token: 0x04007741 RID: 30529
		public long startTick;

		// Token: 0x04007742 RID: 30530
		public long endTick;
	}
}
