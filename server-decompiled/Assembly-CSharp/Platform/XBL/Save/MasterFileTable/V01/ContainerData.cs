using System;

namespace Platform.XBL.Save.MasterFileTable.V01
{
	// Token: 0x02001CD5 RID: 7381
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001BB0 RID: 7088
		// (get) Token: 0x0600DA6A RID: 55914 RVA: 0x00020119 File Offset: 0x0001E319
		public ushort Version
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17001BB1 RID: 7089
		// (get) Token: 0x0600DA6B RID: 55915 RVA: 0x004E2BC8 File Offset: 0x004E0DC8
		// (set) Token: 0x0600DA6C RID: 55916 RVA: 0x004E2BD0 File Offset: 0x004E0DD0
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node();

		// Token: 0x0600DA6D RID: 55917 RVA: 0x004E2BD9 File Offset: 0x004E0DD9
		public void Write(PooledBinaryWriter writer)
		{
			writer.Write(1);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600DA6E RID: 55918 RVA: 0x004E2BEE File Offset: 0x004E0DEE
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
		}

		// Token: 0x0600DA6F RID: 55919 RVA: 0x004E2C03 File Offset: 0x004E0E03
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A5E1 RID: 42465
		public const ushort VERSION = 1;
	}
}
