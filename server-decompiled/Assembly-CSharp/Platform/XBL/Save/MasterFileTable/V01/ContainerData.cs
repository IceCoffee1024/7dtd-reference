using System;

namespace Platform.XBL.Save.MasterFileTable.V01
{
	// Token: 0x02001CA4 RID: 7332
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001B68 RID: 7016
		// (get) Token: 0x0600D956 RID: 55638 RVA: 0x00020119 File Offset: 0x0001E319
		public ushort Version
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17001B69 RID: 7017
		// (get) Token: 0x0600D957 RID: 55639 RVA: 0x004DF078 File Offset: 0x004DD278
		// (set) Token: 0x0600D958 RID: 55640 RVA: 0x004DF080 File Offset: 0x004DD280
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node();

		// Token: 0x0600D959 RID: 55641 RVA: 0x004DF089 File Offset: 0x004DD289
		public void Write(PooledBinaryWriter writer)
		{
			writer.Write(1);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600D95A RID: 55642 RVA: 0x004DF09E File Offset: 0x004DD29E
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
		}

		// Token: 0x0600D95B RID: 55643 RVA: 0x004DF0B3 File Offset: 0x004DD2B3
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A4F8 RID: 42232
		public const ushort VERSION = 1;
	}
}
