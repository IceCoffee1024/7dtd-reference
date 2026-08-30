using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

// Token: 0x02001490 RID: 5264
public sealed class StateHistory<T> : IEnumerable<T>, IEnumerable
{
	// Token: 0x0600A4F0 RID: 42224 RVA: 0x003D6394 File Offset: 0x003D4594
	public StateHistory(int max, bool preventConsecutiveDuplicates = false, bool preventDuplicates = false)
	{
		this.m_states = new T[max];
		this.m_statesSet = (preventDuplicates ? new HashSet<T>() : null);
		this.m_preventConsecutiveDuplicates = preventConsecutiveDuplicates;
		this.m_preventDuplicates = preventDuplicates;
		this.m_index = -1;
		this.m_size = 0;
	}

	// Token: 0x1700134C RID: 4940
	// (get) Token: 0x0600A4F1 RID: 42225 RVA: 0x003D63E0 File Offset: 0x003D45E0
	public T Current
	{
		get
		{
			if (this.m_size <= 0)
			{
				return default(T);
			}
			return this.m_states[this.m_index];
		}
	}

	// Token: 0x1700134D RID: 4941
	// (get) Token: 0x0600A4F2 RID: 42226 RVA: 0x003D6411 File Offset: 0x003D4611
	public int Length
	{
		get
		{
			return this.m_size;
		}
	}

	// Token: 0x0600A4F3 RID: 42227 RVA: 0x003D641C File Offset: 0x003D461C
	public void Add(T state)
	{
		if (this.m_preventDuplicates && this.m_statesSet.Contains(state))
		{
			return;
		}
		if (this.m_preventConsecutiveDuplicates && this.m_size > 0 && EqualityComparer<T>.Default.Equals(this.m_states[this.m_index], state))
		{
			return;
		}
		if (this.m_size < this.m_states.Length)
		{
			this.m_size++;
		}
		this.m_index++;
		if (this.m_index >= this.m_size)
		{
			this.m_index = 0;
		}
		HashSet<T> statesSet = this.m_statesSet;
		if (statesSet != null)
		{
			statesSet.Remove(this.m_states[this.m_index]);
		}
		this.m_states[this.m_index] = state;
		HashSet<T> statesSet2 = this.m_statesSet;
		if (statesSet2 == null)
		{
			return;
		}
		statesSet2.Add(state);
	}

	// Token: 0x0600A4F4 RID: 42228 RVA: 0x003D64F8 File Offset: 0x003D46F8
	public void Clear()
	{
		for (int i = 0; i < this.m_states.Length; i++)
		{
			this.m_states[i] = default(T);
		}
		HashSet<T> statesSet = this.m_statesSet;
		if (statesSet != null)
		{
			statesSet.Clear();
		}
		this.m_index = -1;
		this.m_size = 0;
	}

	// Token: 0x0600A4F5 RID: 42229 RVA: 0x003D654C File Offset: 0x003D474C
	public IEnumerator<T> GetEnumerator()
	{
		int num2;
		for (int i = 0; i < this.m_size; i = num2 + 1)
		{
			int num = this.m_index - i;
			if (num < 0)
			{
				num += this.m_size;
			}
			yield return this.m_states[num];
			num2 = i;
		}
		yield break;
	}

	// Token: 0x0600A4F6 RID: 42230 RVA: 0x003D655B File Offset: 0x003D475B
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x0600A4F7 RID: 42231 RVA: 0x003D6564 File Offset: 0x003D4764
	public override string ToString()
	{
		if (this.m_size <= 0)
		{
			return "No State History";
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (T t in this)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" < ");
			}
			stringBuilder.Append(t);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x04007ABD RID: 31421
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly T[] m_states;

	// Token: 0x04007ABE RID: 31422
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<T> m_statesSet;

	// Token: 0x04007ABF RID: 31423
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool m_preventConsecutiveDuplicates;

	// Token: 0x04007AC0 RID: 31424
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool m_preventDuplicates;

	// Token: 0x04007AC1 RID: 31425
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_index;

	// Token: 0x04007AC2 RID: 31426
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_size;
}
