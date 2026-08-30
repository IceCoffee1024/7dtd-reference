using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

// Token: 0x020014AB RID: 5291
public sealed class StateHistory<T> : IEnumerable<T>, IEnumerable
{
	// Token: 0x0600A5ED RID: 42477 RVA: 0x003DB6D4 File Offset: 0x003D98D4
	public StateHistory(int max, bool preventConsecutiveDuplicates = false, bool preventDuplicates = false)
	{
		this.m_states = new T[max];
		this.m_statesSet = (preventDuplicates ? new HashSet<T>() : null);
		this.m_preventConsecutiveDuplicates = preventConsecutiveDuplicates;
		this.m_preventDuplicates = preventDuplicates;
		this.m_index = -1;
		this.m_size = 0;
	}

	// Token: 0x17001385 RID: 4997
	// (get) Token: 0x0600A5EE RID: 42478 RVA: 0x003DB720 File Offset: 0x003D9920
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

	// Token: 0x17001386 RID: 4998
	// (get) Token: 0x0600A5EF RID: 42479 RVA: 0x003DB751 File Offset: 0x003D9951
	public int Length
	{
		get
		{
			return this.m_size;
		}
	}

	// Token: 0x0600A5F0 RID: 42480 RVA: 0x003DB75C File Offset: 0x003D995C
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

	// Token: 0x0600A5F1 RID: 42481 RVA: 0x003DB838 File Offset: 0x003D9A38
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

	// Token: 0x0600A5F2 RID: 42482 RVA: 0x003DB88C File Offset: 0x003D9A8C
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

	// Token: 0x0600A5F3 RID: 42483 RVA: 0x003DB89B File Offset: 0x003D9A9B
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x0600A5F4 RID: 42484 RVA: 0x003DB8A4 File Offset: 0x003D9AA4
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

	// Token: 0x04007B96 RID: 31638
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly T[] m_states;

	// Token: 0x04007B97 RID: 31639
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<T> m_statesSet;

	// Token: 0x04007B98 RID: 31640
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool m_preventConsecutiveDuplicates;

	// Token: 0x04007B99 RID: 31641
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool m_preventDuplicates;

	// Token: 0x04007B9A RID: 31642
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_index;

	// Token: 0x04007B9B RID: 31643
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_size;
}
