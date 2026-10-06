using System;
using System.Collections.Generic;
using System.Linq;

namespace Platform.XBL.Save.MasterFileTable.V01
{
	// Token: 0x02001CA5 RID: 7333
	public sealed class Node : IDisposable
	{
		// Token: 0x17001B6A RID: 7018
		// (get) Token: 0x0600D95D RID: 55645 RVA: 0x004DF0E0 File Offset: 0x004DD2E0
		// (set) Token: 0x0600D95E RID: 55646 RVA: 0x004DF0E8 File Offset: 0x004DD2E8
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600D95F RID: 55647 RVA: 0x004DF0F1 File Offset: 0x004DD2F1
		public Node()
		{
			this.Name = string.Empty;
			this.BlobIds = new List<ulong>();
			this.Children = new SortedDictionary<string, Node>();
			this.Parent = null;
		}

		// Token: 0x0600D960 RID: 55648 RVA: 0x004DF124 File Offset: 0x004DD324
		public override string ToString()
		{
			return string.Format("{0}[{1}=\"{2}\", {3}=[{4}], #{5}={6}]", new object[]
			{
				"Node",
				"Name",
				this.Name,
				"BlobIds",
				string.Join(", ", this.BlobIds.Select<ulong, string>(new Func<ulong, string>(SaveContainer.IdToString))),
				"Children",
				this.Children.Count
			});
		}

		// Token: 0x0600D961 RID: 55649 RVA: 0x004DF1A4 File Offset: 0x004DD3A4
		public void Write(PooledBinaryWriter writer)
		{
			writer.Write(this.Name);
			ushort num = (ushort)this.BlobIds.Count;
			writer.Write(num);
			foreach (ulong num2 in this.BlobIds)
			{
				writer.Write(num2);
			}
			ushort num3 = (ushort)this.Children.Count;
			writer.Write(num3);
			foreach (Node node in this.Children.Values)
			{
				node.Write(writer);
			}
		}

		// Token: 0x0600D962 RID: 55650 RVA: 0x004DF274 File Offset: 0x004DD474
		public void Read(PooledBinaryReader reader)
		{
			this.Name = reader.ReadString();
			ushort num = reader.ReadUInt16();
			for (int i = 0; i < (int)num; i++)
			{
				ulong num2 = reader.ReadUInt64();
				this.BlobIds.Add(num2);
			}
			ushort num3 = reader.ReadUInt16();
			for (int j = 0; j < (int)num3; j++)
			{
				Node node = new Node();
				node.Read(reader);
				this.Children.Add(node.Name, node);
				node.Parent = this;
			}
		}

		// Token: 0x0600D963 RID: 55651 RVA: 0x004DF2F8 File Offset: 0x004DD4F8
		public void Dispose()
		{
			foreach (Node node in this.Children.Values)
			{
				node.Dispose();
			}
			this.Children.Clear();
			this.Parent = null;
			this.BlobIds.Clear();
			this.Name = string.Empty;
		}

		// Token: 0x0600D964 RID: 55652 RVA: 0x004DF378 File Offset: 0x004DD578
		public bool IsDirectory()
		{
			return this.BlobIds.Count == 0;
		}

		// Token: 0x0600D965 RID: 55653 RVA: 0x004DF388 File Offset: 0x004DD588
		public bool IsFile()
		{
			return this.Children.Count == 0;
		}

		// Token: 0x0600D966 RID: 55654 RVA: 0x004DF398 File Offset: 0x004DD598
		public Node GetChildNode(string name)
		{
			Node node;
			if (!this.Children.TryGetValue(name, out node))
			{
				return null;
			}
			return node;
		}

		// Token: 0x0600D967 RID: 55655 RVA: 0x004DF3B8 File Offset: 0x004DD5B8
		public Node GetOrCreateChildNode(string name)
		{
			Node node;
			if (!this.Children.TryGetValue(name, out node))
			{
				node = new Node
				{
					Name = name
				};
				node.Parent = this;
				this.Children.Add(name, node);
			}
			return node;
		}

		// Token: 0x0400A4FB RID: 42235
		public readonly List<ulong> BlobIds;

		// Token: 0x0400A4FC RID: 42236
		public readonly SortedDictionary<string, Node> Children;

		// Token: 0x0400A4FD RID: 42237
		public Node Parent;
	}
}
