using System;
using System.IO;

namespace Platform.XBL.Save.MasterFileTable.V05
{
	// Token: 0x02001C98 RID: 7320
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001B53 RID: 6995
		// (get) Token: 0x0600D8D5 RID: 55509 RVA: 0x004DC8CB File Offset: 0x004DAACB
		// (set) Token: 0x0600D8D6 RID: 55510 RVA: 0x004DC8D3 File Offset: 0x004DAAD3
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 5;

		// Token: 0x17001B54 RID: 6996
		// (get) Token: 0x0600D8D7 RID: 55511 RVA: 0x004DC8DC File Offset: 0x004DAADC
		// (set) Token: 0x0600D8D8 RID: 55512 RVA: 0x004DC8E4 File Offset: 0x004DAAE4
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x17001B55 RID: 6997
		// (get) Token: 0x0600D8D9 RID: 55513 RVA: 0x004DC8ED File Offset: 0x004DAAED
		// (set) Token: 0x0600D8DA RID: 55514 RVA: 0x004DC8F5 File Offset: 0x004DAAF5
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600D8DB RID: 55515 RVA: 0x004DC900 File Offset: 0x004DAB00
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

		// Token: 0x0600D8DC RID: 55516 RVA: 0x004DC990 File Offset: 0x004DAB90
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

		// Token: 0x0600D8DD RID: 55517 RVA: 0x004DCA39 File Offset: 0x004DAC39
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A4B9 RID: 42169
		public const ushort VERSION = 5;
	}
}
