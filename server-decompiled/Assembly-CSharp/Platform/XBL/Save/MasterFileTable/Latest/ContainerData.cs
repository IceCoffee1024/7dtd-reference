using System;
using System.IO;

namespace Platform.XBL.Save.MasterFileTable.Latest
{
	// Token: 0x02001C75 RID: 7285
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001AC8 RID: 6856
		// (get) Token: 0x0600D801 RID: 55297 RVA: 0x004DAD6B File Offset: 0x004D8F6B
		// (set) Token: 0x0600D802 RID: 55298 RVA: 0x004DAD73 File Offset: 0x004D8F73
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 6;

		// Token: 0x17001AC9 RID: 6857
		// (get) Token: 0x0600D803 RID: 55299 RVA: 0x004DAD7C File Offset: 0x004D8F7C
		// (set) Token: 0x0600D804 RID: 55300 RVA: 0x004DAD84 File Offset: 0x004D8F84
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x17001ACA RID: 6858
		// (get) Token: 0x0600D805 RID: 55301 RVA: 0x004DAD8D File Offset: 0x004D8F8D
		// (set) Token: 0x0600D806 RID: 55302 RVA: 0x004DAD95 File Offset: 0x004D8F95
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600D807 RID: 55303 RVA: 0x004DADA0 File Offset: 0x004D8FA0
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

		// Token: 0x0600D808 RID: 55304 RVA: 0x004DAE30 File Offset: 0x004D9030
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

		// Token: 0x0600D809 RID: 55305 RVA: 0x004DAED9 File Offset: 0x004D90D9
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A433 RID: 42035
		public const ushort VERSION = 6;
	}
}
