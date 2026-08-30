using System;

namespace Platform.XBL.Save.MasterFileTable.V01
{
	// Token: 0x02001C72 RID: 7282
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001AC2 RID: 6850
		// (get) Token: 0x0600D7DD RID: 55261 RVA: 0x00020021 File Offset: 0x0001E221
		public ushort Version
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17001AC3 RID: 6851
		// (get) Token: 0x0600D7DE RID: 55262 RVA: 0x004DA53C File Offset: 0x004D873C
		// (set) Token: 0x0600D7DF RID: 55263 RVA: 0x004DA544 File Offset: 0x004D8744
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node();

		// Token: 0x0600D7E0 RID: 55264 RVA: 0x004DA54D File Offset: 0x004D874D
		public void Write(PooledBinaryWriter writer)
		{
			writer.Write(1);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600D7E1 RID: 55265 RVA: 0x004DA562 File Offset: 0x004D8762
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
		}

		// Token: 0x0600D7E2 RID: 55266 RVA: 0x004DA577 File Offset: 0x004D8777
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A423 RID: 42019
		public const ushort VERSION = 1;
	}
}
