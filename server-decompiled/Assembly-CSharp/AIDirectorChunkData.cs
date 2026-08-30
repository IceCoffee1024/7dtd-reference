using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x0200040A RID: 1034
[Preserve]
public class AIDirectorChunkData
{
	// Token: 0x170003BB RID: 955
	// (get) Token: 0x06002009 RID: 8201 RVA: 0x000C22A9 File Offset: 0x000C04A9
	public float ActivityLevel
	{
		get
		{
			return this.activityLevel;
		}
	}

	// Token: 0x170003BC RID: 956
	// (get) Token: 0x0600200A RID: 8202 RVA: 0x000C22B1 File Offset: 0x000C04B1
	public bool IsReady
	{
		get
		{
			return this.cooldownDelay <= 0f;
		}
	}

	// Token: 0x0600200B RID: 8203 RVA: 0x000C22C4 File Offset: 0x000C04C4
	public void StartNeighborCooldown(bool _isLong)
	{
		float num = (_isLong ? 720f : 180f);
		this.cooldownDelay = Utils.FastMax(this.cooldownDelay, num);
	}

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x0600200C RID: 8204 RVA: 0x000C22F3 File Offset: 0x000C04F3
	public int EventCount
	{
		get
		{
			return this.events.Count;
		}
	}

	// Token: 0x0600200D RID: 8205 RVA: 0x000C2300 File Offset: 0x000C0500
	public AIDirectorChunkEvent GetEvent(int _index)
	{
		return this.events[_index];
	}

	// Token: 0x0600200E RID: 8206 RVA: 0x000C2310 File Offset: 0x000C0510
	public void Write(BinaryWriter _stream)
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

	// Token: 0x0600200F RID: 8207 RVA: 0x000C2378 File Offset: 0x000C0578
	public void Read(BinaryReader _stream, int outerVersion)
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

	// Token: 0x06002010 RID: 8208 RVA: 0x000C23D8 File Offset: 0x000C05D8
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

	// Token: 0x06002011 RID: 8209 RVA: 0x000C246D File Offset: 0x000C066D
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

	// Token: 0x06002012 RID: 8210 RVA: 0x000C24A0 File Offset: 0x000C06A0
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

	// Token: 0x06002013 RID: 8211 RVA: 0x000C2548 File Offset: 0x000C0748
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

	// Token: 0x06002014 RID: 8212 RVA: 0x000C25C0 File Offset: 0x000C07C0
	public void SetLongDelay()
	{
		this.cooldownDelay = 1320f;
	}

	// Token: 0x06002015 RID: 8213 RVA: 0x000C25CD File Offset: 0x000C07CD
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearEvents()
	{
		this.activityLevel = 0f;
		this.events.Clear();
	}

	// Token: 0x040015C4 RID: 5572
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cVersion = 2;

	// Token: 0x040015C5 RID: 5573
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownDelay = 240f;

	// Token: 0x040015C6 RID: 5574
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownLongDelay = 1320f;

	// Token: 0x040015C7 RID: 5575
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownNeighborDelay = 180f;

	// Token: 0x040015C8 RID: 5576
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownNeighborLongDelay = 720f;

	// Token: 0x040015C9 RID: 5577
	[PublicizedFrom(EAccessModifier.Private)]
	public float activityLevel;

	// Token: 0x040015CA RID: 5578
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIDirectorChunkEvent> events = new List<AIDirectorChunkEvent>();

	// Token: 0x040015CB RID: 5579
	public float cooldownDelay;
}
