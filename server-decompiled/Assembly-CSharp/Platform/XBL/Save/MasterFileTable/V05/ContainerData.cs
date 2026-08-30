using System;
using System.IO;

namespace Platform.XBL.Save.MasterFileTable.V05
{
	// Token: 0x02001C66 RID: 7270
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001AAD RID: 6829
		// (get) Token: 0x0600D75C RID: 55132 RVA: 0x004D7D8F File Offset: 0x004D5F8F
		// (set) Token: 0x0600D75D RID: 55133 RVA: 0x004D7D97 File Offset: 0x004D5F97
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 5;

		// Token: 0x17001AAE RID: 6830
		// (get) Token: 0x0600D75E RID: 55134 RVA: 0x004D7DA0 File Offset: 0x004D5FA0
		// (set) Token: 0x0600D75F RID: 55135 RVA: 0x004D7DA8 File Offset: 0x004D5FA8
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x17001AAF RID: 6831
		// (get) Token: 0x0600D760 RID: 55136 RVA: 0x004D7DB1 File Offset: 0x004D5FB1
		// (set) Token: 0x0600D761 RID: 55137 RVA: 0x004D7DB9 File Offset: 0x004D5FB9
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600D762 RID: 55138 RVA: 0x004D7DC4 File Offset: 0x004D5FC4
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

		// Token: 0x0600D763 RID: 55139 RVA: 0x004D7E54 File Offset: 0x004D6054
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

		// Token: 0x0600D764 RID: 55140 RVA: 0x004D7EFD File Offset: 0x004D60FD
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A3E4 RID: 41956
		public const ushort VERSION = 5;
	}
}
