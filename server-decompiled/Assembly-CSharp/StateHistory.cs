using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

// Token: 0x020014DD RID: 5341
public sealed class StateHistory<T> : IEnumerable<T>, IEnumerable
{
	// Token: 0x0600A6F3 RID: 42739 RVA: 0x003E27A0 File Offset: 0x003E09A0
	public StateHistory(int max, bool preventConsecutiveDuplicates = false, bool preventDuplicates = false)
	{
		this.m_states = new T[max];
		this.m_statesSet = (preventDuplicates ? new HashSet<T>() : null);
		this.m_preventConsecutiveDuplicates = preventConsecutiveDuplicates;
		this.m_preventDuplicates = preventDuplicates;
		this.m_index = -1;
		this.m_size = 0;
	}

	// Token: 0x170013D6 RID: 5078
	// (get) Token: 0x0600A6F4 RID: 42740 RVA: 0x003E27EC File Offset: 0x003E09EC
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

	// Token: 0x170013D7 RID: 5079
	// (get) Token: 0x0600A6F5 RID: 42741 RVA: 0x003E281D File Offset: 0x003E0A1D
	public int Length
	{
		get
		{
			return this.m_size;
		}
	}

	// Token: 0x0600A6F6 RID: 42742 RVA: 0x003E2828 File Offset: 0x003E0A28
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

	// Token: 0x0600A6F7 RID: 42743 RVA: 0x003E2904 File Offset: 0x003E0B04
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

	// Token: 0x0600A6F8 RID: 42744 RVA: 0x003E2958 File Offset: 0x003E0B58
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

	// Token: 0x0600A6F9 RID: 42745 RVA: 0x003E2967 File Offset: 0x003E0B67
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x0600A6FA RID: 42746 RVA: 0x003E2970 File Offset: 0x003E0B70
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

	// Token: 0x04007C52 RID: 31826
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly T[] m_states;

	// Token: 0x04007C53 RID: 31827
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<T> m_statesSet;

	// Token: 0x04007C54 RID: 31828
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool m_preventConsecutiveDuplicates;

	// Token: 0x04007C55 RID: 31829
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool m_preventDuplicates;

	// Token: 0x04007C56 RID: 31830
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_index;

	// Token: 0x04007C57 RID: 31831
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_size;
}
