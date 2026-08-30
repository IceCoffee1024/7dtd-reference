using System;

namespace Platform.XBL.Save.MasterFileTable.V01
{
	// Token: 0x02001C4B RID: 7243
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001A50 RID: 6736
		// (get) Token: 0x0600D64D RID: 54861 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public ushort Version
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17001A51 RID: 6737
		// (get) Token: 0x0600D64E RID: 54862 RVA: 0x004D4118 File Offset: 0x004D2318
		// (set) Token: 0x0600D64F RID: 54863 RVA: 0x004D4120 File Offset: 0x004D2320
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node();

		// Token: 0x0600D650 RID: 54864 RVA: 0x004D4129 File Offset: 0x004D2329
		public void Write(PooledBinaryWriter writer)
		{
			writer.Write(1);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600D651 RID: 54865 RVA: 0x004D413E File Offset: 0x004D233E
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
		}

		// Token: 0x0600D652 RID: 54866 RVA: 0x004D4153 File Offset: 0x004D2353
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A2F2 RID: 41714
		public const ushort VERSION = 1;
	}
}
