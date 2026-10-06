using System;
using UnityEngine.Scripting;

// Token: 0x0200040F RID: 1039
[Preserve]
public class AIDirectorChunkEvent
{
	// Token: 0x06002007 RID: 8199 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorChunkEvent()
	{
	}

	// Token: 0x06002008 RID: 8200 RVA: 0x000C26C8 File Offset: 0x000C08C8
	public AIDirectorChunkEvent(EnumAIDirectorChunkEvent _type, Vector3i _position, float _value, float _duration)
	{
		this.EventType = _type;
		this.Position = _position;
		this.Value = _value;
		this.Duration = _duration;
	}

	// Token: 0x06002009 RID: 8201 RVA: 0x000C26F0 File Offset: 0x000C08F0
	public void Write(PooledBinaryWriter _stream)
	{
		_stream.Write(2);
		_stream.Write(this.Position.x);
		_stream.Write(this.Position.y);
		_stream.Write(this.Position.z);
		_stream.Write(this.Value);
		_stream.Write((byte)this.EventType);
		_stream.Write(this.Duration);
	}

	// Token: 0x0600200A RID: 8202 RVA: 0x000C275C File Offset: 0x000C095C
	public static AIDirectorChunkEvent Read(PooledBinaryReader _stream)
	{
		int num = _stream.ReadInt32();
		AIDirectorChunkEvent aidirectorChunkEvent = new AIDirectorChunkEvent();
		aidirectorChunkEvent.Position.x = _stream.ReadInt32();
		aidirectorChunkEvent.Position.y = _stream.ReadInt32();
		aidirectorChunkEvent.Position.z = _stream.ReadInt32();
		aidirectorChunkEvent.Value = _stream.ReadSingle();
		aidirectorChunkEvent.EventType = (EnumAIDirectorChunkEvent)_stream.ReadByte();
		if (num >= 2)
		{
			aidirectorChunkEvent.Duration = _stream.ReadSingle();
		}
		else
		{
			_stream.ReadUInt64();
		}
		return aidirectorChunkEvent;
	}

	// Token: 0x040015CC RID: 5580
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cVersion = 2;

	// Token: 0x040015CD RID: 5581
	public EnumAIDirectorChunkEvent EventType;

	// Token: 0x040015CE RID: 5582
	public Vector3i Position;

	// Token: 0x040015CF RID: 5583
	public float Value;

	// Token: 0x040015D0 RID: 5584
	public float Duration;
}
