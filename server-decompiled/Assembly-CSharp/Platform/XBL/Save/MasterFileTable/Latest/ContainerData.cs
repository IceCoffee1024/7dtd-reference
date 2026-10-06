using System;
using System.IO;

namespace Platform.XBL.Save.MasterFileTable.Latest
{
	// Token: 0x02001CA7 RID: 7335
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001B6E RID: 7022
		// (get) Token: 0x0600D97A RID: 55674 RVA: 0x004DF8A7 File Offset: 0x004DDAA7
		// (set) Token: 0x0600D97B RID: 55675 RVA: 0x004DF8AF File Offset: 0x004DDAAF
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 6;

		// Token: 0x17001B6F RID: 7023
		// (get) Token: 0x0600D97C RID: 55676 RVA: 0x004DF8B8 File Offset: 0x004DDAB8
		// (set) Token: 0x0600D97D RID: 55677 RVA: 0x004DF8C0 File Offset: 0x004DDAC0
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x17001B70 RID: 7024
		// (get) Token: 0x0600D97E RID: 55678 RVA: 0x004DF8C9 File Offset: 0x004DDAC9
		// (set) Token: 0x0600D97F RID: 55679 RVA: 0x004DF8D1 File Offset: 0x004DDAD1
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600D980 RID: 55680 RVA: 0x004DF8DC File Offset: 0x004DDADC
		public void Write(PooledBinaryWriter writer)
		{
			this.RootNode.Attributes = NodeAttributes.Directory;
			writer.Write(this.Version);
			int num = 0;
			long position = writer.BaseStream.Position;
			writer.Write(num);
			long position2 = writer.BaseStream.Position;
			writer.Write(this.FutureData);
			long position3 = writer.BaseStream.Position;
			num = (int)(position3 - position2);
			writer.BaseStream.Position = position;
			writer.Write(num);
			writer.BaseStream.Position = position3;
			this.RootNode.Write(writer);
		}

		// Token: 0x0600D981 RID: 55681 RVA: 0x004DF96C File Offset: 0x004DDB6C
		public void Read(PooledBinaryReader reader)
		{
			this.Version = reader.ReadUInt16();
			int num = reader.ReadInt32();
			long position = reader.BaseStream.Position;
			long position2 = reader.BaseStream.Position;
			this.FutureData = new byte[(int)((long)num - (position2 - position))];
			int num2;
			if (!reader.TryReadAllBytes(this.FutureData, out num2))
			{
				throw new IOException(string.Format("Expected {0} bytes to be read for future data but only got {1} bytes.", this.FutureData.Length, num2));
			}
			Migrator.ReadMigrate(reader.BaseStream, this.RootNode, "Node", Migrator.s_nodeMigrators);
			this.RootNode.Attributes = NodeAttributes.Directory;
		}

		// Token: 0x0600D982 RID: 55682 RVA: 0x004DFA15 File Offset: 0x004DDC15
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A508 RID: 42248
		public const ushort VERSION = 6;
	}
}
