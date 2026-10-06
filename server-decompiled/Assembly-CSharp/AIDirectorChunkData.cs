using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200040C RID: 1036
[Preserve]
public class AIDirectorChunkData
{
	// Token: 0x170003AF RID: 943
	// (get) Token: 0x06001FF8 RID: 8184 RVA: 0x000C235D File Offset: 0x000C055D
	public float ActivityLevel
	{
		get
		{
			return this.activityLevel;
		}
	}

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x000C2365 File Offset: 0x000C0565
	public bool IsReady
	{
		get
		{
			return this.cooldownDelay <= 0f;
		}
	}

	// Token: 0x06001FFA RID: 8186 RVA: 0x000C2378 File Offset: 0x000C0578
	public void StartNeighborCooldown(bool _isLong)
	{
		float num = (_isLong ? 720f : 180f);
		this.cooldownDelay = Utils.FastMax(this.cooldownDelay, num);
	}

	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x06001FFB RID: 8187 RVA: 0x000C23A7 File Offset: 0x000C05A7
	public int EventCount
	{
		get
		{
			return this.events.Count;
		}
	}

	// Token: 0x06001FFC RID: 8188 RVA: 0x000C23B4 File Offset: 0x000C05B4
	public AIDirectorChunkEvent GetEvent(int _index)
	{
		return this.events[_index];
	}

	// Token: 0x06001FFD RID: 8189 RVA: 0x000C23C4 File Offset: 0x000C05C4
	public void Write(PooledBinaryWriter _stream)
	{
		_stream.Write(2);
		_stream.Write(this.activityLevel);
		_stream.Write(this.events.Count);
		for (int i = 0; i < this.events.Count; i++)
		{
			this.events[i].Write(_stream);
		}
		_stream.Write(this.cooldownDelay);
	}

	// Token: 0x06001FFE RID: 8190 RVA: 0x000C242C File Offset: 0x000C062C
	public void Read(PooledBinaryReader _stream, int outerVersion)
	{
		int num = _stream.ReadInt32();
		this.activityLevel = _stream.ReadSingle();
		this.events.Clear();
		int num2 = _stream.ReadInt32();
		for (int i = 0; i < num2; i++)
		{
			this.events.Add(AIDirectorChunkEvent.Read(_stream));
		}
		if (num >= 2)
		{
			this.cooldownDelay = _stream.ReadSingle();
		}
	}

	// Token: 0x06001FFF RID: 8191 RVA: 0x000C248C File Offset: 0x000C068C
	public void AddEvent(AIDirectorChunkEvent _chunkEvent)
	{
		int num = this.events.FindIndex((AIDirectorChunkEvent e) => e.Position == _chunkEvent.Position && e.EventType == _chunkEvent.EventType);
		if (num < 0)
		{
			this.events.Add(_chunkEvent);
		}
		else
		{
			AIDirectorChunkEvent aidirectorChunkEvent = this.events[num];
			aidirectorChunkEvent.Value += _chunkEvent.Value;
			aidirectorChunkEvent.Duration = _chunkEvent.Duration;
		}
		this.activityLevel += _chunkEvent.Value;
	}

	// Token: 0x06002000 RID: 8192 RVA: 0x000C2521 File Offset: 0x000C0721
	public bool Tick(float _elapsed)
	{
		if (this.cooldownDelay > 0f)
		{
			this.cooldownDelay -= _elapsed;
			return true;
		}
		this.DecayEvents(_elapsed);
		return this.EventCount > 0;
	}

	// Token: 0x06002001 RID: 8193 RVA: 0x000C2554 File Offset: 0x000C0754
	public void DecayEvents(float _elapsed)
	{
		this.activityLevel = 0f;
		int i = 0;
		while (i < this.events.Count)
		{
			AIDirectorChunkEvent aidirectorChunkEvent = this.events[i];
			float num = _elapsed / aidirectorChunkEvent.Duration;
			aidirectorChunkEvent.Value -= aidirectorChunkEvent.Value * num;
			aidirectorChunkEvent.Duration -= _elapsed;
			if (aidirectorChunkEvent.Duration > 0f && aidirectorChunkEvent.Value > 0f)
			{
				this.activityLevel += aidirectorChunkEvent.Value;
				i++;
			}
			else
			{
				this.events.RemoveAt(i);
			}
		}
	}

	// Token: 0x06002002 RID: 8194 RVA: 0x000C25FC File Offset: 0x000C07FC
	public AIDirectorChunkEvent FindBestEventAndReset()
	{
		AIDirectorChunkEvent aidirectorChunkEvent = null;
		if (this.events.Count > 0)
		{
			aidirectorChunkEvent = this.events[0];
			for (int i = 1; i < this.events.Count; i++)
			{
				if (this.events[i].Value > aidirectorChunkEvent.Value)
				{
					aidirectorChunkEvent = this.events[i];
				}
			}
			this.cooldownDelay = 240f;
		}
		this.ClearEvents();
		return aidirectorChunkEvent;
	}

	// Token: 0x06002003 RID: 8195 RVA: 0x000C2674 File Offset: 0x000C0874
	public void SetLongDelay()
	{
		this.cooldownDelay = 1320f;
	}

	// Token: 0x06002004 RID: 8196 RVA: 0x000C2681 File Offset: 0x000C0881
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearEvents()
	{
		this.activityLevel = 0f;
		this.events.Clear();
	}

	// Token: 0x040015BB RID: 5563
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cVersion = 2;

	// Token: 0x040015BC RID: 5564
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownDelay = 240f;

	// Token: 0x040015BD RID: 5565
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownLongDelay = 1320f;

	// Token: 0x040015BE RID: 5566
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownNeighborDelay = 180f;

	// Token: 0x040015BF RID: 5567
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownNeighborLongDelay = 720f;

	// Token: 0x040015C0 RID: 5568
	[PublicizedFrom(EAccessModifier.Private)]
	public float activityLevel;

	// Token: 0x040015C1 RID: 5569
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIDirectorChunkEvent> events = new List<AIDirectorChunkEvent>();

	// Token: 0x040015C2 RID: 5570
	public float cooldownDelay;
}
