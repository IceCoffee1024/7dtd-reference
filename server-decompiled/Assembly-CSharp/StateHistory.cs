using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

// Token: 0x020014CB RID: 5323
public sealed class StateHistory<T> : IEnumerable<T>, IEnumerable
{
	// Token: 0x0600A69B RID: 42651 RVA: 0x003E039C File Offset: 0x003DE59C
	public StateHistory(int max, bool preventConsecutiveDuplicates = false, bool preventDuplicates = false)
	{
		this.m_states = new T[max];
		this.m_statesSet = (preventDuplicates ? new HashSet<T>() : null);
		this.m_preventConsecutiveDuplicates = preventConsecutiveDuplicates;
		this.m_preventDuplicates = preventDuplicates;
		this.m_index = -1;
		this.m_size = 0;
	}

	// Token: 0x170013CA RID: 5066
	// (get) Token: 0x0600A69C RID: 42652 RVA: 0x003E03E8 File Offset: 0x003DE5E8
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

	// Token: 0x170013CB RID: 5067
	// (get) Token: 0x0600A69D RID: 42653 RVA: 0x003E0419 File Offset: 0x003DE619
	public int Length
	{
		get
		{
			return this.m_size;
		}
	}

	// Token: 0x0600A69E RID: 42654 RVA: 0x003E0424 File Offset: 0x003DE624
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

	// Token: 0x0600A69F RID: 42655 RVA: 0x003E0500 File Offset: 0x003DE700
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

	// Token: 0x0600A6A0 RID: 42656 RVA: 0x003E0554 File Offset: 0x003DE754
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

	// Token: 0x0600A6A1 RID: 42657 RVA: 0x003E0563 File Offset: 0x003DE763
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x0600A6A2 RID: 42658 RVA: 0x003E056C File Offset: 0x003DE76C
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

	// Token: 0x04007C11 RID: 31761
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly T[] m_states;

	// Token: 0x04007C12 RID: 31762
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<T> m_statesSet;

	// Token: 0x04007C13 RID: 31763
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool m_preventConsecutiveDuplicates;

	// Token: 0x04007C14 RID: 31764
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool m_preventDuplicates;

	// Token: 0x04007C15 RID: 31765
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_index;

	// Token: 0x04007C16 RID: 31766
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_size;
}
