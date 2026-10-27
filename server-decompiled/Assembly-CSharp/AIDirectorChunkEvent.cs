using System;
using UnityEngine.Scripting;

// Token: 0x02000411 RID: 1041
[Preserve]
public class AIDirectorChunkEvent
{
	// Token: 0x0600200C RID: 8204 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorChunkEvent()
	{
	}

	// Token: 0x0600200D RID: 8205 RVA: 0x000C2A74 File Offset: 0x000C0C74
	public AIDirectorChunkEvent(EnumAIDirectorChunkEvent _type, Vector3i _position, float _value, float _duration)
	{
		this.EventType = _type;
		this.Position = _position;
		this.Value = _value;
		this.Duration = _duration;
	}

	// Token: 0x0600200E RID: 8206 RVA: 0x000C2A9C File Offset: 0x000C0C9C
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

	// Token: 0x0600200F RID: 8207 RVA: 0x000C2B08 File Offset: 0x000C0D08
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

	// Token: 0x040015DC RID: 5596
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cVersion = 2;

	// Token: 0x040015DD RID: 5597
	public EnumAIDirectorChunkEvent EventType;

	// Token: 0x040015DE RID: 5598
	public Vector3i Position;

	// Token: 0x040015DF RID: 5599
	public float Value;

	// Token: 0x040015E0 RID: 5600
	public float Duration;
}
