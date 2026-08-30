using System;

namespace Platform.XBL.Save.MasterFileTable.V03
{
	// Token: 0x02001C46 RID: 7238
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001A48 RID: 6728
		// (get) Token: 0x0600D61B RID: 54811 RVA: 0x00046C7A File Offset: 0x00044E7A
		public ushort Version
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001A49 RID: 6729
		// (get) Token: 0x0600D61C RID: 54812 RVA: 0x004D3196 File Offset: 0x004D1396
		// (set) Token: 0x0600D61D RID: 54813 RVA: 0x004D319E File Offset: 0x004D139E
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600D61E RID: 54814 RVA: 0x004D31A7 File Offset: 0x004D13A7
		public void Write(PooledBinaryWriter writer)
		{
			this.RootNode.Attributes = NodeAttributes.Directory;
			writer.Write(3);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600D61F RID: 54815 RVA: 0x004D31C8 File Offset: 0x004D13C8
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
			this.RootNode.Attributes = NodeAttributes.Directory;
		}

		// Token: 0x0600D620 RID: 54816 RVA: 0x004D31E9 File Offset: 0x004D13E9
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A2DB RID: 41691
		public const ushort VERSION = 3;
	}
}
