using System;
using System.Collections.Generic;
using System.Text;

namespace Platform
{
	// Token: 0x02001BB8 RID: 7096
	public sealed class PlatformMemoryStat<T> : IPlatformMemoryStat<T>, IPlatformMemoryStat
	{
		// Token: 0x0600D423 RID: 54307 RVA: 0x004CB86B File Offset: 0x004C9A6B
		[PublicizedFrom(EAccessModifier.Private)]
		public PlatformMemoryStat(string name)
		{
			this.m_columnValues = new EnumDictionary<MemoryStatColumn, T>();
			this.m_columnLastValues = new EnumDictionary<MemoryStatColumn, T>();
			this.Name = name;
		}

		// Token: 0x17001AB7 RID: 6839
		// (get) Token: 0x0600D424 RID: 54308 RVA: 0x004CB890 File Offset: 0x004C9A90
		public string Name { get; }

		// Token: 0x0600D425 RID: 54309 RVA: 0x004CB898 File Offset: 0x004C9A98
		public void UpdateLast()
		{
			foreach (KeyValuePair<MemoryStatColumn, T> keyValuePair in this.m_columnValues)
			{
				MemoryStatColumn memoryStatColumn;
				T t;
				keyValuePair.Deconstruct(out memoryStatColumn, out t);
				MemoryStatColumn memoryStatColumn2 = memoryStatColumn;
				T t2 = t;
				this.m_columnLastValues[memoryStatColumn2] = t2;
			}
		}

		// Token: 0x0600D426 RID: 54310 RVA: 0x004CB904 File Offset: 0x004C9B04
		public void RenderColumn(StringBuilder builder, MemoryStatColumn column, bool delta)
		{
			T t;
			if (!this.m_columnValues.TryGetValue(column, out t))
			{
				return;
			}
			if (!delta)
			{
				PlatformMemoryRenderValue<T> renderValue = this.RenderValue;
				if (renderValue == null)
				{
					return;
				}
				renderValue(builder, t);
				return;
			}
			else
			{
				T t2 = this.m_columnLastValues[column];
				PlatformMemoryRenderDelta<T> renderDelta = this.RenderDelta;
				if (renderDelta == null)
				{
					return;
				}
				renderDelta(builder, t, t2);
				return;
			}
		}

		// Token: 0x14000123 RID: 291
		// (add) Token: 0x0600D427 RID: 54311 RVA: 0x004CB958 File Offset: 0x004C9B58
		// (remove) Token: 0x0600D428 RID: 54312 RVA: 0x004CB990 File Offset: 0x004C9B90
		public event PlatformMemoryColumnChangedHandler<T> ColumnSetAfter;

		// Token: 0x17001AB8 RID: 6840
		// (get) Token: 0x0600D429 RID: 54313 RVA: 0x004CB9C5 File Offset: 0x004C9BC5
		// (set) Token: 0x0600D42A RID: 54314 RVA: 0x004CB9CD File Offset: 0x004C9BCD
		public PlatformMemoryRenderValue<T> RenderValue { get; set; }

		// Token: 0x17001AB9 RID: 6841
		// (get) Token: 0x0600D42B RID: 54315 RVA: 0x004CB9D6 File Offset: 0x004C9BD6
		// (set) Token: 0x0600D42C RID: 54316 RVA: 0x004CB9DE File Offset: 0x004C9BDE
		public PlatformMemoryRenderDelta<T> RenderDelta { get; set; }

		// Token: 0x0600D42D RID: 54317 RVA: 0x004CB9E7 File Offset: 0x004C9BE7
		public void Set(MemoryStatColumn column, T value)
		{
			this.m_columnValues[column] = value;
			if (!this.m_columnLastValues.ContainsKey(column))
			{
				this.m_columnLastValues[column] = value;
			}
			PlatformMemoryColumnChangedHandler<T> columnSetAfter = this.ColumnSetAfter;
			if (columnSetAfter == null)
			{
				return;
			}
			columnSetAfter(column, value);
		}

		// Token: 0x0600D42E RID: 54318 RVA: 0x004CBA23 File Offset: 0x004C9C23
		public bool TryGet(MemoryStatColumn column, out T value)
		{
			return this.m_columnValues.TryGetValue(column, out value);
		}

		// Token: 0x0600D42F RID: 54319 RVA: 0x004CBA32 File Offset: 0x004C9C32
		public bool TryGetLast(MemoryStatColumn column, out T value)
		{
			return this.m_columnLastValues.TryGetValue(column, out value);
		}

		// Token: 0x0600D430 RID: 54320 RVA: 0x004CBA41 File Offset: 0x004C9C41
		public static IPlatformMemoryStat<T> Create(string name)
		{
			return new PlatformMemoryStat<T>(name);
		}

		// Token: 0x0400A194 RID: 41364
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EnumDictionary<MemoryStatColumn, T> m_columnValues;

		// Token: 0x0400A195 RID: 41365
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EnumDictionary<MemoryStatColumn, T> m_columnLastValues;
	}
}
