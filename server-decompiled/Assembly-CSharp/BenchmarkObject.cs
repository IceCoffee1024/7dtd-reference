using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// Token: 0x020013BE RID: 5054
public class BenchmarkObject
{
	// Token: 0x06009F1B RID: 40731 RVA: 0x003C167C File Offset: 0x003BF87C
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

	// Token: 0x06009F1C RID: 40732 RVA: 0x003C16DC File Offset: 0x003BF8DC
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

	// Token: 0x06009F1D RID: 40733 RVA: 0x003C1754 File Offset: 0x003BF954
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

	// Token: 0x06009F1E RID: 40734 RVA: 0x003C17D0 File Offset: 0x003BF9D0
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

	// Token: 0x06009F1F RID: 40735 RVA: 0x003C1844 File Offset: 0x003BFA44
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

	// Token: 0x04007893 RID: 30867
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<object, BenchmarkObject.BenchmarkContainer> benchmarks = new Dictionary<object, BenchmarkObject.BenchmarkContainer>();

	// Token: 0x020013BF RID: 5055
	public class BenchmarkContainer
	{
		// Token: 0x17001305 RID: 4869
		// (get) Token: 0x06009F22 RID: 40738 RVA: 0x003C19FC File Offset: 0x003BFBFC
		// (set) Token: 0x06009F23 RID: 40739 RVA: 0x003C1A04 File Offset: 0x003BFC04
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

		// Token: 0x17001306 RID: 4870
		// (get) Token: 0x06009F24 RID: 40740 RVA: 0x003C1A0D File Offset: 0x003BFC0D
		public long ticks
		{
			get
			{
				return this.endTick - this.startTick;
			}
		}

		// Token: 0x06009F25 RID: 40741 RVA: 0x003C1A1C File Offset: 0x003BFC1C
		public BenchmarkContainer(string _name)
		{
			this.pName = _name;
		}

		// Token: 0x04007894 RID: 30868
		[PublicizedFrom(EAccessModifier.Private)]
		public string pName;

		// Token: 0x04007895 RID: 30869
		public long startTick;

		// Token: 0x04007896 RID: 30870
		public long endTick;
	}
}
