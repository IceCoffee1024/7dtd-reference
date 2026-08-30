using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// Token: 0x0200139E RID: 5022
public class BenchmarkObject
{
	// Token: 0x06009E70 RID: 40560 RVA: 0x003BCAE0 File Offset: 0x003BACE0
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

	// Token: 0x06009E71 RID: 40561 RVA: 0x003BCB40 File Offset: 0x003BAD40
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

	// Token: 0x06009E72 RID: 40562 RVA: 0x003BCBB8 File Offset: 0x003BADB8
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

	// Token: 0x06009E73 RID: 40563 RVA: 0x003BCC34 File Offset: 0x003BAE34
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

	// Token: 0x06009E74 RID: 40564 RVA: 0x003BCCA8 File Offset: 0x003BAEA8
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

	// Token: 0x04007818 RID: 30744
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<object, BenchmarkObject.BenchmarkContainer> benchmarks = new Dictionary<object, BenchmarkObject.BenchmarkContainer>();

	// Token: 0x0200139F RID: 5023
	public class BenchmarkContainer
	{
		// Token: 0x170012C0 RID: 4800
		// (get) Token: 0x06009E77 RID: 40567 RVA: 0x003BCE60 File Offset: 0x003BB060
		// (set) Token: 0x06009E78 RID: 40568 RVA: 0x003BCE68 File Offset: 0x003BB068
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

		// Token: 0x170012C1 RID: 4801
		// (get) Token: 0x06009E79 RID: 40569 RVA: 0x003BCE71 File Offset: 0x003BB071
		public long ticks
		{
			get
			{
				return this.endTick - this.startTick;
			}
		}

		// Token: 0x06009E7A RID: 40570 RVA: 0x003BCE80 File Offset: 0x003BB080
		public BenchmarkContainer(string _name)
		{
			this.pName = _name;
		}

		// Token: 0x04007819 RID: 30745
		[PublicizedFrom(EAccessModifier.Private)]
		public string pName;

		// Token: 0x0400781A RID: 30746
		public long startTick;

		// Token: 0x0400781B RID: 30747
		public long endTick;
	}
}
