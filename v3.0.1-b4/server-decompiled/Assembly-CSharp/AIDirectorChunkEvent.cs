using System;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000409 RID: 1033
[Preserve]
public class AIDirectorChunkEvent
{
	// Token: 0x06001FFB RID: 8187 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorChunkEvent()
	{
	}

	// Token: 0x06001FFC RID: 8188 RVA: 0x000C1E78 File Offset: 0x000C0078
	public AIDirectorChunkEvent(EnumAIDirectorChunkEvent _type, Vector3i _position, float _value, float _duration)
	{
		this.EventType = _type;
		this.Position = _position;
		this.Value = _value;
		this.Duration = _duration;
	}

	// Token: 0x06001FFD RID: 8189 RVA: 0x000C1EA0 File Offset: 0x000C00A0
	public void Write(BinaryWriter _stream)
	{
		_stream.Write(2);
		_stream.Write(this.Position.x);
		_stream.Write(this.Position.y);
		_stream.Write(this.Position.z);
		_stream.Write(this.Value);
		_stream.Write((byte)this.EventType);
		_stream.Write(this.Duration);
	}

	// Token: 0x06001FFE RID: 8190 RVA: 0x000C1F0C File Offset: 0x000C010C
	public static AIDirectorChunkEvent Read(BinaryReader _stream)
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

	// Token: 0x040015C0 RID: 5568
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cVersion = 2;

	// Token: 0x040015C1 RID: 5569
	public EnumAIDirectorChunkEvent EventType;

	// Token: 0x040015C2 RID: 5570
	public Vector3i Position;

	// Token: 0x040015C3 RID: 5571
	public float Value;

	// Token: 0x040015C4 RID: 5572
	public float Duration;
}
