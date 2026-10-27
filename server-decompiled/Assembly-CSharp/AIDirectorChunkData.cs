using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200040E RID: 1038
[Preserve]
public class AIDirectorChunkData
{
	// Token: 0x170003AF RID: 943
	// (get) Token: 0x06001FFD RID: 8189 RVA: 0x000C2709 File Offset: 0x000C0909
	public float ActivityLevel
	{
		get
		{
			return this.activityLevel;
		}
	}

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x06001FFE RID: 8190 RVA: 0x000C2711 File Offset: 0x000C0911
	public bool IsReady
	{
		get
		{
			return this.cooldownDelay <= 0f;
		}
	}

	// Token: 0x06001FFF RID: 8191 RVA: 0x000C2724 File Offset: 0x000C0924
	public void StartNeighborCooldown(bool _isLong)
	{
		float num = (_isLong ? 720f : 180f);
		this.cooldownDelay = Utils.FastMax(this.cooldownDelay, num);
	}

	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x06002000 RID: 8192 RVA: 0x000C2753 File Offset: 0x000C0953
	public int EventCount
	{
		get
		{
			return this.events.Count;
		}
	}

	// Token: 0x06002001 RID: 8193 RVA: 0x000C2760 File Offset: 0x000C0960
	public AIDirectorChunkEvent GetEvent(int _index)
	{
		return this.events[_index];
	}

	// Token: 0x06002002 RID: 8194 RVA: 0x000C2770 File Offset: 0x000C0970
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

	// Token: 0x06002003 RID: 8195 RVA: 0x000C27D8 File Offset: 0x000C09D8
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

	// Token: 0x06002004 RID: 8196 RVA: 0x000C2838 File Offset: 0x000C0A38
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

	// Token: 0x06002005 RID: 8197 RVA: 0x000C28CD File Offset: 0x000C0ACD
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

	// Token: 0x06002006 RID: 8198 RVA: 0x000C2900 File Offset: 0x000C0B00
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

	// Token: 0x06002007 RID: 8199 RVA: 0x000C29A8 File Offset: 0x000C0BA8
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

	// Token: 0x06002008 RID: 8200 RVA: 0x000C2A20 File Offset: 0x000C0C20
	public void SetLongDelay()
	{
		this.cooldownDelay = 1320f;
	}

	// Token: 0x06002009 RID: 8201 RVA: 0x000C2A2D File Offset: 0x000C0C2D
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearEvents()
	{
		this.activityLevel = 0f;
		this.events.Clear();
	}

	// Token: 0x040015CB RID: 5579
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cVersion = 2;

	// Token: 0x040015CC RID: 5580
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownDelay = 240f;

	// Token: 0x040015CD RID: 5581
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownLongDelay = 1320f;

	// Token: 0x040015CE RID: 5582
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownNeighborDelay = 180f;

	// Token: 0x040015CF RID: 5583
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownNeighborLongDelay = 720f;

	// Token: 0x040015D0 RID: 5584
	[PublicizedFrom(EAccessModifier.Private)]
	public float activityLevel;

	// Token: 0x040015D1 RID: 5585
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIDirectorChunkEvent> events = new List<AIDirectorChunkEvent>();

	// Token: 0x040015D2 RID: 5586
	public float cooldownDelay;
}
