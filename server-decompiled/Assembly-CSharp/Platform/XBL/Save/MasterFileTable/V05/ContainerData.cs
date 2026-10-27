using System;
using System.IO;

namespace Platform.XBL.Save.MasterFileTable.V05
{
	// Token: 0x02001CC9 RID: 7369
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001B9B RID: 7067
		// (get) Token: 0x0600D9E9 RID: 55785 RVA: 0x004E041B File Offset: 0x004DE61B
		// (set) Token: 0x0600D9EA RID: 55786 RVA: 0x004E0423 File Offset: 0x004DE623
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 5;

		// Token: 0x17001B9C RID: 7068
		// (get) Token: 0x0600D9EB RID: 55787 RVA: 0x004E042C File Offset: 0x004DE62C
		// (set) Token: 0x0600D9EC RID: 55788 RVA: 0x004E0434 File Offset: 0x004DE634
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x17001B9D RID: 7069
		// (get) Token: 0x0600D9ED RID: 55789 RVA: 0x004E043D File Offset: 0x004DE63D
		// (set) Token: 0x0600D9EE RID: 55790 RVA: 0x004E0445 File Offset: 0x004DE645
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600D9EF RID: 55791 RVA: 0x004E0450 File Offset: 0x004DE650
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

		// Token: 0x0600D9F0 RID: 55792 RVA: 0x004E04E0 File Offset: 0x004DE6E0
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

		// Token: 0x0600D9F1 RID: 55793 RVA: 0x004E0589 File Offset: 0x004DE789
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A5A2 RID: 42402
		public const ushort VERSION = 5;
	}
}
