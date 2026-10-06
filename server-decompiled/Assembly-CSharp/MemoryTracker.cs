using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UniLinq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

// Token: 0x02001465 RID: 5221
public class MemoryTracker
{
	// Token: 0x17001371 RID: 4977
	// (get) Token: 0x0600A3B5 RID: 41909 RVA: 0x003D3AA8 File Offset: 0x003D1CA8
	public static MemoryTracker Instance
	{
		get
		{
			if (MemoryTracker.m_Instance == null)
			{
				MemoryTracker.m_Instance = new MemoryTracker();
			}
			return MemoryTracker.m_Instance;
		}
	}

	// Token: 0x0600A3B6 RID: 41910 RVA: 0x003D3AC0 File Offset: 0x003D1CC0
	public void New(object _o)
	{
		Dictionary<object, int> dictionary = this.refs;
		lock (dictionary)
		{
			Type type = _o.GetType();
			this.refs[type] = (this.refs.ContainsKey(type) ? (this.refs[type] + 1) : 1);
		}
	}

	// Token: 0x0600A3B7 RID: 41911 RVA: 0x003D3B2C File Offset: 0x003D1D2C
	public void Delete(object _o)
	{
		Dictionary<object, int> dictionary = this.refs;
		lock (dictionary)
		{
			Type type = _o.GetType();
			this.refs[type] = this.refs[type] - 1;
		}
	}

	// Token: 0x0600A3B8 RID: 41912 RVA: 0x003D3B88 File Offset: 0x003D1D88
	public void SetEstimationFunction(object _o, MemoryTracker.EstimateOwnedBytes _func)
	{
		if (_o == null)
		{
			return;
		}
		Dictionary<Type, MemoryTracker.AllocationsForType> dictionary = this.allocTypeDict;
		lock (dictionary)
		{
			Type type = _o.GetType();
			MemoryTracker.AllocationsForType allocationsForType;
			if (!this.allocTypeDict.TryGetValue(type, out allocationsForType))
			{
				allocationsForType = new MemoryTracker.AllocationsForType();
				this.allocTypeDict.Add(type, allocationsForType);
			}
			allocationsForType.allocations.AddLast(new MemoryTracker.Allocation(_o, _func));
		}
	}

	// Token: 0x0600A3B9 RID: 41913 RVA: 0x003D3C04 File Offset: 0x003D1E04
	[PublicizedFrom(EAccessModifier.Private)]
	public int EstimateSelfBytes()
	{
		int num = 0;
		Dictionary<object, int> dictionary = this.refs;
		lock (dictionary)
		{
			num += MemoryTracker.GetUsedSize<object, int>(this.refs);
			num += MemoryTracker.GetUsedSize<object, int>(this.last);
		}
		Dictionary<Type, MemoryTracker.AllocationsForType> dictionary2 = this.allocTypeDict;
		lock (dictionary2)
		{
			num += MemoryTracker.GetUsedSize<Type, MemoryTracker.AllocationsForType>(this.allocTypeDict);
			foreach (MemoryTracker.AllocationsForType allocationsForType in this.allocTypeDict.Values)
			{
				num += allocationsForType.allocations.Count * MemoryTracker.GetSize<MemoryTracker.Allocation>();
			}
		}
		return num;
	}

	// Token: 0x0600A3BA RID: 41914 RVA: 0x003D3CEC File Offset: 0x003D1EEC
	public void Dump()
	{
		Dictionary<string, MemoryTracker.AllocationsForType.Summary> dictionary = new Dictionary<string, MemoryTracker.AllocationsForType.Summary>();
		long num = 0L;
		Dictionary<object, int> dictionary2 = this.refs;
		lock (dictionary2)
		{
			Log.Out("---Classes----------------------------------------");
			foreach (KeyValuePair<object, int> keyValuePair in this.refs)
			{
				Log.Out(string.Concat(new string[]
				{
					keyValuePair.Key.ToString(),
					" = ",
					keyValuePair.Value.ToString(),
					" last = ",
					(this.last.ContainsKey(keyValuePair.Key) ? this.last[keyValuePair.Key] : 0).ToString()
				}));
				this.last[keyValuePair.Key] = keyValuePair.Value;
			}
		}
		long totalMemory = GC.GetTotalMemory(false);
		Dictionary<Type, MemoryTracker.AllocationsForType> dictionary3 = this.allocTypeDict;
		lock (dictionary3)
		{
			foreach (KeyValuePair<Type, MemoryTracker.AllocationsForType> keyValuePair2 in this.allocTypeDict)
			{
				Type key = keyValuePair2.Key;
				MemoryTracker.AllocationsForType value = keyValuePair2.Value;
				MemoryTracker.AllocationsForType.Summary summary = default(MemoryTracker.AllocationsForType.Summary);
				summary.numGC = value.ClearDeadAllocations();
				foreach (MemoryTracker.Allocation allocation in value.allocations)
				{
					summary.totalBytes += (long)allocation.GetOwnedBytes();
					summary.numInstances++;
				}
				dictionary.Add(key.ToString(), summary);
				num += summary.totalBytes;
			}
			MemoryTracker.AllocationsForType allocationsForType;
			if (this.allocTypeDict.TryGetValue(typeof(DynamicProperties), out allocationsForType))
			{
				int num2 = 0;
				MemoryTracker.EstimatedMemoryContext estimatedMemoryContext = new MemoryTracker.EstimatedMemoryContext(0);
				using (LinkedList<MemoryTracker.Allocation>.Enumerator enumerator3 = allocationsForType.allocations.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						object obj;
						if (enumerator3.Current.obj.TryGetTarget(out obj))
						{
							num2++;
							MemoryTracker.EstimateMemoryUsage(obj, ref estimatedMemoryContext);
						}
					}
				}
				double num3 = (double)estimatedMemoryContext.ActualBytes * 9.5367431640625E-07;
				double num4 = (double)estimatedMemoryContext.BytesInterned * 9.5367431640625E-07;
				long num5 = 0L;
				long num6 = 0L;
				foreach (KeyValuePair<string, int> keyValuePair3 in estimatedMemoryContext.InteredStrings)
				{
					num5 += (long)MemoryTracker.GetSize(keyValuePair3.Key);
				}
				foreach (KeyValuePair<string, int> keyValuePair4 in estimatedMemoryContext.NonInteredStrings)
				{
					num6 += (long)(MemoryTracker.GetSize(keyValuePair4.Key) * keyValuePair4.Value);
				}
				double num7 = (double)num5 * 9.5367431640625E-07;
				double num8 = (double)num6 * 9.5367431640625E-07;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("---CSV DynamicProperties---------------------");
				stringBuilder.AppendLine("Type,Count,ActualBytes(MB),BytesInterned(MB),InternedStrings,InternedStrings(MB),NonInternedStrings,NonInternedStrings(MB)");
				stringBuilder.AppendLine(string.Format("DynamicProperties,{0},{1:F2},{2:F2},{3},{4:F2},{5},{6:F2}", new object[]
				{
					num2,
					num3,
					num4,
					estimatedMemoryContext.InteredStrings.Count,
					num7,
					estimatedMemoryContext.NonInteredStrings.Count,
					num8
				}));
				Log.Out(stringBuilder.ToString());
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendLine("---CSV InteredStrings---------------------");
				stringBuilder2.AppendLine("InteredStrings,Count");
				foreach (KeyValuePair<string, int> keyValuePair5 in estimatedMemoryContext.InteredStrings.OrderByDescending<KeyValuePair<string, int>, int>((KeyValuePair<string, int> x) => x.Value))
				{
					stringBuilder2.AppendLine(string.Format("{0},{1}", keyValuePair5.Key, keyValuePair5.Value));
				}
				Log.Out(stringBuilder2.ToString());
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.AppendLine("---CSV NonInteredStrings---------------------");
				stringBuilder3.AppendLine("NonInteredStrings,Count");
				foreach (KeyValuePair<string, int> keyValuePair6 in estimatedMemoryContext.NonInteredStrings.OrderByDescending<KeyValuePair<string, int>, int>((KeyValuePair<string, int> x) => x.Value))
				{
					stringBuilder3.AppendLine(string.Format("{0},{1}", keyValuePair6.Key, keyValuePair6.Value));
				}
				Log.Out(stringBuilder3.ToString());
			}
		}
		int num9 = this.EstimateSelfBytes();
		dictionary.Add(typeof(MemoryTracker).ToString(), new MemoryTracker.AllocationsForType.Summary((long)num9));
		num += (long)num9;
		double num10 = (double)(totalMemory - num) * 9.5367431640625E-07;
		Log.Out("GC.GetTotalMemory (MB): {0:F2}", new object[] { (double)totalMemory * 9.5367431640625E-07 });
		Log.Out("Total Tracked (MB): {0:F2}", new object[] { (double)num * 9.5367431640625E-07 });
		Log.Out("Untracked (MB): {0:F2}", new object[] { num10 });
		if (num > 0L)
		{
			StringBuilder stringBuilder4 = new StringBuilder();
			stringBuilder4.AppendLine("---CSV----------------------------------------");
			stringBuilder4.AppendLine("Type,Bytes(MB),Count,GC Count,");
			stringBuilder4.AppendLine(string.Format("Untracked,{0:F2},0,0", num10));
			Log.Out("---Tracked----------------------------------------");
			foreach (KeyValuePair<string, MemoryTracker.AllocationsForType.Summary> keyValuePair7 in dictionary)
			{
				string key2 = keyValuePair7.Key;
				MemoryTracker.AllocationsForType.Summary value2 = keyValuePair7.Value;
				double num11 = (double)value2.totalBytes * 9.5367431640625E-07;
				stringBuilder4.AppendLine(string.Format("{0},{1:F2},{2},{3}", new object[] { key2, num11, value2.numInstances, value2.numGC }));
			}
			Log.Out(stringBuilder4.ToString());
		}
	}

	// Token: 0x0600A3BB RID: 41915 RVA: 0x003D44B8 File Offset: 0x003D26B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void DrawLabel(float _left, float _top, string _text)
	{
		Rect rect = new Rect(_left, _top, (float)(_text.Length * 20), 30f);
		Rect rect2 = new Rect(rect);
		rect2.x += 1f;
		rect2.y += 1f;
		GUI.color = Color.black;
		GUI.Label(rect2, _text);
		GUI.color = Color.white;
		GUI.Label(rect, _text);
	}

	// Token: 0x0600A3BC RID: 41916 RVA: 0x003D4530 File Offset: 0x003D2730
	public void DebugOnGui()
	{
		this.DrawLabel(800f, 30f, "Type");
		this.DrawLabel(1000f, 30f, "Count");
		int num = 0;
		Dictionary<object, int> dictionary = this.refs;
		lock (dictionary)
		{
			foreach (KeyValuePair<object, int> keyValuePair in this.refs)
			{
				this.DrawLabel(800f, (float)(80 + num * 35), keyValuePair.Key.ToString());
				this.DrawLabel(1000f, (float)(80 + num * 35), keyValuePair.Value.ToString());
				num++;
			}
		}
	}

	// Token: 0x0600A3BD RID: 41917 RVA: 0x003D4618 File Offset: 0x003D2818
	public static int GetSize<T>()
	{
		return MemoryTracker.GetSize(typeof(T));
	}

	// Token: 0x0600A3BE RID: 41918 RVA: 0x003D4629 File Offset: 0x003D2829
	public static int GetSize(Type _type)
	{
		if (_type.IsEnum)
		{
			return Marshal.SizeOf(Enum.GetUnderlyingType(_type));
		}
		if (_type.IsValueType)
		{
			return UnsafeUtility.SizeOf(_type);
		}
		return IntPtr.Size;
	}

	// Token: 0x0600A3BF RID: 41919 RVA: 0x003D4654 File Offset: 0x003D2854
	public static int GetSize<[IsUnmanaged] T>(IBackedArray<T> _array) where T : struct, ValueType
	{
		int num = IntPtr.Size;
		if (_array != null)
		{
			num += MemoryTracker.GetSize<T>() * _array.Length;
		}
		return num;
	}

	// Token: 0x0600A3C0 RID: 41920 RVA: 0x003D467C File Offset: 0x003D287C
	public static int GetSize<T>(T[] _array)
	{
		int num = IntPtr.Size;
		if (_array != null)
		{
			num += MemoryTracker.GetSize<T>() * _array.Length;
		}
		return num;
	}

	// Token: 0x0600A3C1 RID: 41921 RVA: 0x003D46A0 File Offset: 0x003D28A0
	public static int GetSize<T>(T[][] _doubleArray)
	{
		int num = IntPtr.Size;
		if (_doubleArray != null)
		{
			foreach (T[] array in _doubleArray)
			{
				num += MemoryTracker.GetSize<T>(array);
			}
		}
		return num;
	}

	// Token: 0x0600A3C2 RID: 41922 RVA: 0x003D46D4 File Offset: 0x003D28D4
	public static int GetSize<T>(T[,] _array)
	{
		int num = IntPtr.Size;
		if (_array != null)
		{
			num += MemoryTracker.GetSize<T>() * _array.GetLength(0) * _array.GetLength(1);
		}
		return num;
	}

	// Token: 0x0600A3C3 RID: 41923 RVA: 0x003D4704 File Offset: 0x003D2904
	public static int GetSize<T>(List<T> _list)
	{
		int num = IntPtr.Size;
		if (_list != null)
		{
			num += _list.Capacity * MemoryTracker.GetSize<T>();
		}
		return num;
	}

	// Token: 0x0600A3C4 RID: 41924 RVA: 0x003D472C File Offset: 0x003D292C
	public static int GetUsedSize<TKey, TValue>(IDictionary<TKey, TValue> _dictionary)
	{
		int num = IntPtr.Size;
		if (_dictionary != null)
		{
			num += (MemoryTracker.GetSize<TKey>() + MemoryTracker.GetSize<TValue>()) * _dictionary.Count;
		}
		return num;
	}

	// Token: 0x0600A3C5 RID: 41925 RVA: 0x003D4758 File Offset: 0x003D2958
	public static int GetSize(string stringVal)
	{
		if (stringVal == null)
		{
			return IntPtr.Size;
		}
		return stringVal.Length * 2 + IntPtr.Size;
	}

	// Token: 0x0600A3C6 RID: 41926 RVA: 0x003D4774 File Offset: 0x003D2974
	public static int GetSize(Dictionary<string, string> stringDict)
	{
		int num = 0;
		foreach (KeyValuePair<string, string> keyValuePair in stringDict)
		{
			num += MemoryTracker.GetSize(keyValuePair.Key) + MemoryTracker.GetSize(keyValuePair.Value);
		}
		return num;
	}

	// Token: 0x0600A3C7 RID: 41927 RVA: 0x003D47DC File Offset: 0x003D29DC
	public static void EstimateMemoryUsage(object _obj, ref MemoryTracker.EstimatedMemoryContext estimatedMemoryContext)
	{
		if (_obj == null)
		{
			estimatedMemoryContext.ActualBytes += (long)IntPtr.Size;
			return;
		}
		Type type = _obj.GetType();
		if (type.IsEnum)
		{
			estimatedMemoryContext.ActualBytes += (long)Marshal.SizeOf(Enum.GetUnderlyingType(type));
			return;
		}
		if (type.IsValueType)
		{
			estimatedMemoryContext.ActualBytes += (long)UnsafeUtility.SizeOf(type);
			return;
		}
		if (type.IsArray)
		{
			Type elementType = type.GetElementType();
			Array array = _obj as Array;
			int size = IntPtr.Size;
			if (array != null)
			{
				for (int i = 0; i < array.Rank; i++)
				{
					estimatedMemoryContext.ActualBytes += (long)(array.GetLength(i) * MemoryTracker.GetSize(elementType));
				}
			}
			return;
		}
		if (typeof(string).IsAssignableFrom(type))
		{
			string text = (string)_obj;
			estimatedMemoryContext.ActualBytes += (long)IntPtr.Size;
			string text2 = string.IsInterned(text);
			if (text2 != null)
			{
				int num;
				bool flag = estimatedMemoryContext.InteredStrings.TryGetValue(text2, out num);
				estimatedMemoryContext.InteredStrings[text2] = num + 1;
				if (!flag)
				{
					estimatedMemoryContext.ActualBytes += (long)MemoryTracker.GetSize(text);
					estimatedMemoryContext.BytesInterned += (long)MemoryTracker.GetSize(text);
					return;
				}
			}
			else
			{
				int num2;
				bool flag2 = estimatedMemoryContext.NonInteredStrings.TryGetValue(text, out num2);
				estimatedMemoryContext.NonInteredStrings[text] = num2 + 1;
				estimatedMemoryContext.ActualBytes += (long)MemoryTracker.GetSize(text);
				if (!flag2)
				{
					estimatedMemoryContext.BytesInterned += (long)MemoryTracker.GetSize(text);
					return;
				}
			}
		}
		else if (!(_obj is DynamicProperties))
		{
			estimatedMemoryContext.ActualBytes += (long)IntPtr.Size;
			return;
		}
	}

	// Token: 0x0600A3C8 RID: 41928 RVA: 0x003D496C File Offset: 0x003D2B6C
	public static int GetSizeAuto(object _obj)
	{
		if (_obj == null)
		{
			return IntPtr.Size;
		}
		Type type = _obj.GetType();
		if (type.IsEnum)
		{
			return Marshal.SizeOf(Enum.GetUnderlyingType(type));
		}
		if (type.IsValueType)
		{
			return UnsafeUtility.SizeOf(type);
		}
		if (type.IsArray)
		{
			Type elementType = type.GetElementType();
			Array array = _obj as Array;
			int num = IntPtr.Size;
			if (array != null)
			{
				for (int i = 0; i < array.Rank; i++)
				{
					num += array.GetLength(i) * MemoryTracker.GetSize(elementType);
				}
			}
			return num;
		}
		if (typeof(string).IsAssignableFrom(type))
		{
			string text = (string)_obj;
			int num2 = IntPtr.Size;
			if (text != null)
			{
				num2 += MemoryTracker.GetSize(text);
			}
			return num2;
		}
		return IntPtr.Size;
	}

	// Token: 0x04007AAC RID: 31404
	[PublicizedFrom(EAccessModifier.Private)]
	public static MemoryTracker m_Instance;

	// Token: 0x04007AAD RID: 31405
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<object, int> refs = new Dictionary<object, int>();

	// Token: 0x04007AAE RID: 31406
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<object, int> last = new Dictionary<object, int>();

	// Token: 0x04007AAF RID: 31407
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Type, MemoryTracker.AllocationsForType> allocTypeDict = new Dictionary<Type, MemoryTracker.AllocationsForType>();

	// Token: 0x02001466 RID: 5222
	public struct EstimatedMemoryContext
	{
		// Token: 0x0600A3CA RID: 41930 RVA: 0x003D4A56 File Offset: 0x003D2C56
		public EstimatedMemoryContext(int lame)
		{
			this.InteredStrings = new Dictionary<string, int>();
			this.NonInteredStrings = new Dictionary<string, int>();
			this.ActualBytes = 0L;
			this.BytesInterned = 0L;
		}

		// Token: 0x04007AB0 RID: 31408
		public Dictionary<string, int> InteredStrings;

		// Token: 0x04007AB1 RID: 31409
		public Dictionary<string, int> NonInteredStrings;

		// Token: 0x04007AB2 RID: 31410
		public long ActualBytes;

		// Token: 0x04007AB3 RID: 31411
		public long BytesInterned;
	}

	// Token: 0x02001467 RID: 5223
	// (Invoke) Token: 0x0600A3CC RID: 41932
	public delegate int EstimateOwnedBytes(object obj);

	// Token: 0x02001468 RID: 5224
	public struct Allocation
	{
		// Token: 0x0600A3CF RID: 41935 RVA: 0x003D4A7E File Offset: 0x003D2C7E
		public Allocation(object _obj, MemoryTracker.EstimateOwnedBytes _func)
		{
			this.obj = new WeakReference<object>(_obj);
			this.estimateBytesFunc = _func;
		}

		// Token: 0x0600A3D0 RID: 41936 RVA: 0x003D4A94 File Offset: 0x003D2C94
		public int GetOwnedBytes()
		{
			object obj;
			if (this.obj.TryGetTarget(out obj))
			{
				return this.estimateBytesFunc(obj);
			}
			return 0;
		}

		// Token: 0x04007AB4 RID: 31412
		public WeakReference<object> obj;

		// Token: 0x04007AB5 RID: 31413
		public MemoryTracker.EstimateOwnedBytes estimateBytesFunc;
	}

	// Token: 0x02001469 RID: 5225
	public class AllocationsForType
	{
		// Token: 0x0600A3D1 RID: 41937 RVA: 0x003D4AC0 File Offset: 0x003D2CC0
		public int ClearDeadAllocations()
		{
			int num = 0;
			LinkedListNode<MemoryTracker.Allocation> next;
			for (LinkedListNode<MemoryTracker.Allocation> linkedListNode = this.allocations.First; linkedListNode != null; linkedListNode = next)
			{
				next = linkedListNode.Next;
				object obj;
				if (!linkedListNode.Value.obj.TryGetTarget(out obj) || obj == null)
				{
					this.allocations.Remove(linkedListNode);
					num++;
				}
			}
			return num;
		}

		// Token: 0x04007AB6 RID: 31414
		public LinkedList<MemoryTracker.Allocation> allocations = new LinkedList<MemoryTracker.Allocation>();

		// Token: 0x0200146A RID: 5226
		public struct Summary
		{
			// Token: 0x0600A3D3 RID: 41939 RVA: 0x003D4B22 File Offset: 0x003D2D22
			public Summary(long _totalBytes)
			{
				this.totalBytes = _totalBytes;
				this.numInstances = 1;
				this.numGC = 0;
			}

			// Token: 0x04007AB7 RID: 31415
			public long totalBytes;

			// Token: 0x04007AB8 RID: 31416
			public int numInstances;

			// Token: 0x04007AB9 RID: 31417
			public int numGC;
		}
	}
}
