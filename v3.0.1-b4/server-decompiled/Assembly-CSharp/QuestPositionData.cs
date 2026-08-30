using System;
using System.IO;

// Token: 0x020008E5 RID: 2277
public class QuestPositionData
{
	// Token: 0x060041F2 RID: 16882 RVA: 0x0000640C File Offset: 0x0000460C
	public QuestPositionData()
	{
	}

	// Token: 0x060041F3 RID: 16883 RVA: 0x0019BB86 File Offset: 0x00199D86
	public QuestPositionData(int questCode, Quest.PositionDataTypes positionDataType, Vector3i blockPosition)
	{
		this.questCode = questCode;
		this.positionDataType = positionDataType;
		this.blockPosition = blockPosition;
	}

	// Token: 0x060041F4 RID: 16884 RVA: 0x0019BBA3 File Offset: 0x00199DA3
	public static QuestPositionData Read(BinaryReader reader)
	{
		return new QuestPositionData
		{
			questCode = reader.ReadInt32(),
			positionDataType = (Quest.PositionDataTypes)reader.ReadInt32(),
			blockPosition = StreamUtils.ReadVector3i(reader)
		};
	}

	// Token: 0x060041F5 RID: 16885 RVA: 0x0019BBCE File Offset: 0x00199DCE
	public void Write(BinaryWriter writer)
	{
		writer.Write(this.questCode);
		writer.Write((int)this.positionDataType);
		StreamUtils.Write(writer, this.blockPosition);
	}

	// Token: 0x0400353B RID: 13627
	public int questCode;

	// Token: 0x0400353C RID: 13628
	public Quest.PositionDataTypes positionDataType;

	// Token: 0x0400353D RID: 13629
	public Vector3i blockPosition;
}
