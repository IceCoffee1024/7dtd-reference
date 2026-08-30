using System;
using System.IO;

namespace Platform.XBL.Save.MasterFileTable.V05
{
	// Token: 0x02001C3F RID: 7231
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001A3B RID: 6715
		// (get) Token: 0x0600D5CC RID: 54732 RVA: 0x004D196B File Offset: 0x004CFB6B
		// (set) Token: 0x0600D5CD RID: 54733 RVA: 0x004D1973 File Offset: 0x004CFB73
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 5;

		// Token: 0x17001A3C RID: 6716
		// (get) Token: 0x0600D5CE RID: 54734 RVA: 0x004D197C File Offset: 0x004CFB7C
		// (set) Token: 0x0600D5CF RID: 54735 RVA: 0x004D1984 File Offset: 0x004CFB84
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x17001A3D RID: 6717
		// (get) Token: 0x0600D5D0 RID: 54736 RVA: 0x004D198D File Offset: 0x004CFB8D
		// (set) Token: 0x0600D5D1 RID: 54737 RVA: 0x004D1995 File Offset: 0x004CFB95
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600D5D2 RID: 54738 RVA: 0x004D19A0 File Offset: 0x004CFBA0
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

		// Token: 0x0600D5D3 RID: 54739 RVA: 0x004D1A30 File Offset: 0x004CFC30
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

		// Token: 0x0600D5D4 RID: 54740 RVA: 0x004D1AD9 File Offset: 0x004CFCD9
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A2B3 RID: 41651
		public const ushort VERSION = 5;
	}
}
