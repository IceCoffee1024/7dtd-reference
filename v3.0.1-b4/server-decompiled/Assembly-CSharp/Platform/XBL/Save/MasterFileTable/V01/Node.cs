using System;
using System.Collections.Generic;
using System.Linq;

namespace Platform.XBL.Save.MasterFileTable.V01
{
	// Token: 0x02001C4C RID: 7244
	public sealed class Node : IDisposable
	{
		// Token: 0x17001A52 RID: 6738
		// (get) Token: 0x0600D654 RID: 54868 RVA: 0x004D4180 File Offset: 0x004D2380
		// (set) Token: 0x0600D655 RID: 54869 RVA: 0x004D4188 File Offset: 0x004D2388
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600D656 RID: 54870 RVA: 0x004D4191 File Offset: 0x004D2391
		public Node()
		{
			this.Name = string.Empty;
			this.BlobIds = new List<ulong>();
			this.Children = new SortedDictionary<string, Node>();
			this.Parent = null;
		}

		// Token: 0x0600D657 RID: 54871 RVA: 0x004D41C4 File Offset: 0x004D23C4
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

		// Token: 0x0600D658 RID: 54872 RVA: 0x004D4244 File Offset: 0x004D2444
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

		// Token: 0x0600D659 RID: 54873 RVA: 0x004D4314 File Offset: 0x004D2514
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

		// Token: 0x0600D65A RID: 54874 RVA: 0x004D4398 File Offset: 0x004D2598
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

		// Token: 0x0600D65B RID: 54875 RVA: 0x004D4418 File Offset: 0x004D2618
		public bool IsDirectory()
		{
			return this.BlobIds.Count == 0;
		}

		// Token: 0x0600D65C RID: 54876 RVA: 0x004D4428 File Offset: 0x004D2628
		public bool IsFile()
		{
			return this.Children.Count == 0;
		}

		// Token: 0x0600D65D RID: 54877 RVA: 0x004D4438 File Offset: 0x004D2638
		public Node GetChildNode(string name)
		{
			Node node;
			if (!this.Children.TryGetValue(name, out node))
			{
				return null;
			}
			return node;
		}

		// Token: 0x0600D65E RID: 54878 RVA: 0x004D4458 File Offset: 0x004D2658
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

		// Token: 0x0400A2F5 RID: 41717
		public readonly List<ulong> BlobIds;

		// Token: 0x0400A2F6 RID: 41718
		public readonly SortedDictionary<string, Node> Children;

		// Token: 0x0400A2F7 RID: 41719
		public Node Parent;
	}
}
