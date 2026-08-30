using System;
using System.IO;

// Token: 0x020008F1 RID: 2289
public class QuestPositionData
{
	// Token: 0x06004257 RID: 16983 RVA: 0x0000640C File Offset: 0x0000460C
	public QuestPositionData()
	{
	}

	// Token: 0x06004258 RID: 16984 RVA: 0x0019DC5E File Offset: 0x0019BE5E
	public QuestPositionData(int questCode, Quest.PositionDataTypes positionDataType, Vector3i blockPosition)
	{
		this.questCode = questCode;
		this.positionDataType = positionDataType;
		this.blockPosition = blockPosition;
	}

	// Token: 0x06004259 RID: 16985 RVA: 0x0019DC7B File Offset: 0x0019BE7B
	public static QuestPositionData Read(BinaryReader reader)
	{
		return new QuestPositionData
		{
			questCode = reader.ReadInt32(),
			positionDataType = (Quest.PositionDataTypes)reader.ReadInt32(),
			blockPosition = StreamUtils.ReadVector3i(reader)
		};
	}

	// Token: 0x0600425A RID: 16986 RVA: 0x0019DCA6 File Offset: 0x0019BEA6
	public void Write(BinaryWriter writer)
	{
		writer.Write(this.questCode);
		writer.Write((int)this.positionDataType);
		StreamUtils.Write(writer, this.blockPosition);
	}

	// Token: 0x040035BC RID: 13756
	public int questCode;

	// Token: 0x040035BD RID: 13757
	public Quest.PositionDataTypes positionDataType;

	// Token: 0x040035BE RID: 13758
	public Vector3i blockPosition;
}
