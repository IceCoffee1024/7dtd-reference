using System;
using System.IO;

namespace Platform.XBL.Save.MasterFileTable.Latest
{
	// Token: 0x02001CD8 RID: 7384
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001BB6 RID: 7094
		// (get) Token: 0x0600DA8E RID: 55950 RVA: 0x004E33F7 File Offset: 0x004E15F7
		// (set) Token: 0x0600DA8F RID: 55951 RVA: 0x004E33FF File Offset: 0x004E15FF
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 6;

		// Token: 0x17001BB7 RID: 7095
		// (get) Token: 0x0600DA90 RID: 55952 RVA: 0x004E3408 File Offset: 0x004E1608
		// (set) Token: 0x0600DA91 RID: 55953 RVA: 0x004E3410 File Offset: 0x004E1610
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x17001BB8 RID: 7096
		// (get) Token: 0x0600DA92 RID: 55954 RVA: 0x004E3419 File Offset: 0x004E1619
		// (set) Token: 0x0600DA93 RID: 55955 RVA: 0x004E3421 File Offset: 0x004E1621
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600DA94 RID: 55956 RVA: 0x004E342C File Offset: 0x004E162C
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

		// Token: 0x0600DA95 RID: 55957 RVA: 0x004E34BC File Offset: 0x004E16BC
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

		// Token: 0x0600DA96 RID: 55958 RVA: 0x004E3565 File Offset: 0x004E1765
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A5F1 RID: 42481
		public const ushort VERSION = 6;
	}
}
