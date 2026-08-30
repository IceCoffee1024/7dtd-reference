using System;
using System.IO;

namespace Platform.XBL.Save.MasterFileTable.Latest
{
	// Token: 0x02001C4E RID: 7246
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001A56 RID: 6742
		// (get) Token: 0x0600D671 RID: 54897 RVA: 0x004D4947 File Offset: 0x004D2B47
		// (set) Token: 0x0600D672 RID: 54898 RVA: 0x004D494F File Offset: 0x004D2B4F
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 6;

		// Token: 0x17001A57 RID: 6743
		// (get) Token: 0x0600D673 RID: 54899 RVA: 0x004D4958 File Offset: 0x004D2B58
		// (set) Token: 0x0600D674 RID: 54900 RVA: 0x004D4960 File Offset: 0x004D2B60
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x17001A58 RID: 6744
		// (get) Token: 0x0600D675 RID: 54901 RVA: 0x004D4969 File Offset: 0x004D2B69
		// (set) Token: 0x0600D676 RID: 54902 RVA: 0x004D4971 File Offset: 0x004D2B71
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600D677 RID: 54903 RVA: 0x004D497C File Offset: 0x004D2B7C
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

		// Token: 0x0600D678 RID: 54904 RVA: 0x004D4A0C File Offset: 0x004D2C0C
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

		// Token: 0x0600D679 RID: 54905 RVA: 0x004D4AB5 File Offset: 0x004D2CB5
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A302 RID: 41730
		public const ushort VERSION = 6;
	}
}
