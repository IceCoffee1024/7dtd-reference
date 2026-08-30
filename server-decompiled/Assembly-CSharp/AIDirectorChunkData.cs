using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000406 RID: 1030
[Preserve]
public class AIDirectorChunkData
{
	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x06001FEC RID: 8172 RVA: 0x000C1B0D File Offset: 0x000BFD0D
	public float ActivityLevel
	{
		get
		{
			return this.activityLevel;
		}
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06001FED RID: 8173 RVA: 0x000C1B15 File Offset: 0x000BFD15
	public bool IsReady
	{
		get
		{
			return this.cooldownDelay <= 0f;
		}
	}

	// Token: 0x06001FEE RID: 8174 RVA: 0x000C1B28 File Offset: 0x000BFD28
	public void StartNeighborCooldown(bool _isLong)
	{
		float num = (_isLong ? 720f : 180f);
		this.cooldownDelay = Utils.FastMax(this.cooldownDelay, num);
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x06001FEF RID: 8175 RVA: 0x000C1B57 File Offset: 0x000BFD57
	public int EventCount
	{
		get
		{
			return this.events.Count;
		}
	}

	// Token: 0x06001FF0 RID: 8176 RVA: 0x000C1B64 File Offset: 0x000BFD64
	public AIDirectorChunkEvent GetEvent(int _index)
	{
		return this.events[_index];
	}

	// Token: 0x06001FF1 RID: 8177 RVA: 0x000C1B74 File Offset: 0x000BFD74
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

	// Token: 0x06001FF2 RID: 8178 RVA: 0x000C1BDC File Offset: 0x000BFDDC
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

	// Token: 0x06001FF3 RID: 8179 RVA: 0x000C1C3C File Offset: 0x000BFE3C
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

	// Token: 0x06001FF4 RID: 8180 RVA: 0x000C1CD1 File Offset: 0x000BFED1
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

	// Token: 0x06001FF5 RID: 8181 RVA: 0x000C1D04 File Offset: 0x000BFF04
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

	// Token: 0x06001FF6 RID: 8182 RVA: 0x000C1DAC File Offset: 0x000BFFAC
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

	// Token: 0x06001FF7 RID: 8183 RVA: 0x000C1E24 File Offset: 0x000C0024
	public void SetLongDelay()
	{
		this.cooldownDelay = 1320f;
	}

	// Token: 0x06001FF8 RID: 8184 RVA: 0x000C1E31 File Offset: 0x000C0031
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearEvents()
	{
		this.activityLevel = 0f;
		this.events.Clear();
	}

	// Token: 0x040015AF RID: 5551
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cVersion = 2;

	// Token: 0x040015B0 RID: 5552
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownDelay = 240f;

	// Token: 0x040015B1 RID: 5553
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownLongDelay = 1320f;

	// Token: 0x040015B2 RID: 5554
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownNeighborDelay = 180f;

	// Token: 0x040015B3 RID: 5555
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCooldownNeighborLongDelay = 720f;

	// Token: 0x040015B4 RID: 5556
	[PublicizedFrom(EAccessModifier.Private)]
	public float activityLevel;

	// Token: 0x040015B5 RID: 5557
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIDirectorChunkEvent> events = new List<AIDirectorChunkEvent>();

	// Token: 0x040015B6 RID: 5558
	public float cooldownDelay;
}
