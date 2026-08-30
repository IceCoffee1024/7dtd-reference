using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Platform.XBL.Save.MasterFileTable.V02
{
	// Token: 0x02001C4A RID: 7242
	public sealed class Node : IDisposable
	{
		// Token: 0x17001A4F RID: 6735
		// (get) Token: 0x0600D63F RID: 54847 RVA: 0x004D3B06 File Offset: 0x004D1D06
		// (set) Token: 0x0600D640 RID: 54848 RVA: 0x004D3B0E File Offset: 0x004D1D0E
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600D641 RID: 54849 RVA: 0x004D3B18 File Offset: 0x004D1D18
		public Node()
		{
			this.Name = string.Empty;
			this.LastWriteTimeUtc = Node.DefaultLastWriteTime;
			this.BlobIds = new List<ulong>();
			this.Children = new SortedDictionary<string, Node>();
			this.m_lock = new object();
			this.Parent = null;
		}

		// Token: 0x0600D642 RID: 54850 RVA: 0x004D3B6C File Offset: 0x004D1D6C
		public override string ToString()
		{
			object @lock = this.m_lock;
			string text;
			lock (@lock)
			{
				text = string.Format("{0}[{1}=\"{2}\", {3}=[{4}], #{5}={6}]", new object[]
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
			return text;
		}

		// Token: 0x0600D643 RID: 54851 RVA: 0x004D3C18 File Offset: 0x004D1E18
		public void Write(PooledBinaryWriter writer)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				writer.Write(this.Name);
				writer.Write(this.LastWriteTimeUtc.Ticks);
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
		}

		// Token: 0x0600D644 RID: 54852 RVA: 0x004D3D24 File Offset: 0x004D1F24
		public void Read(PooledBinaryReader reader)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.Name = reader.ReadString();
				this.LastWriteTimeUtc = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
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
		}

		// Token: 0x0600D645 RID: 54853 RVA: 0x004D3DE8 File Offset: 0x004D1FE8
		public void Dispose()
		{
			object @lock = this.m_lock;
			lock (@lock)
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
		}

		// Token: 0x0600D646 RID: 54854 RVA: 0x004D3E90 File Offset: 0x004D2090
		public bool IsDirectory()
		{
			object @lock = this.m_lock;
			bool flag2;
			lock (@lock)
			{
				flag2 = this.BlobIds.Count == 0;
			}
			return flag2;
		}

		// Token: 0x0600D647 RID: 54855 RVA: 0x004D3EDC File Offset: 0x004D20DC
		public bool IsFile()
		{
			object @lock = this.m_lock;
			bool flag2;
			lock (@lock)
			{
				flag2 = this.Children.Count == 0;
			}
			return flag2;
		}

		// Token: 0x0600D648 RID: 54856 RVA: 0x004D3F28 File Offset: 0x004D2128
		public Node GetChildNode(string name)
		{
			object @lock = this.m_lock;
			Node node2;
			lock (@lock)
			{
				Node node;
				if (!this.Children.TryGetValue(name, out node))
				{
					node2 = null;
				}
				else
				{
					node2 = node;
				}
			}
			return node2;
		}

		// Token: 0x0600D649 RID: 54857 RVA: 0x004D3F7C File Offset: 0x004D217C
		public Node GetOrCreateChildNode(string name)
		{
			object @lock = this.m_lock;
			Node node2;
			lock (@lock)
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
				node2 = node;
			}
			return node2;
		}

		// Token: 0x0600D64A RID: 54858 RVA: 0x004D3FEC File Offset: 0x004D21EC
		public IEnumerable<Node> Enumerate(bool includeSelf, bool recursive)
		{
			List<Node> list = new List<Node>();
			if (!recursive)
			{
				if (includeSelf)
				{
					list.Add(this);
				}
				object @lock = this.m_lock;
				lock (@lock)
				{
					list.AddRange(this.Children.Values);
					return list;
				}
			}
			Stack<Node> stack = new Stack<Node>();
			if (includeSelf)
			{
				stack.Push(this);
			}
			else
			{
				Node.<Enumerate>g__AddNodeChildrenToStack|19_0(stack, this);
			}
			while (stack.Count > 0)
			{
				Node node = stack.Pop();
				list.Add(node);
				Node.<Enumerate>g__AddNodeChildrenToStack|19_0(stack, node);
			}
			return list;
		}

		// Token: 0x0600D64C RID: 54860 RVA: 0x004D4098 File Offset: 0x004D2298
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <Enumerate>g__AddNodeChildrenToStack|19_0(Stack<Node> stack, Node currentNode)
		{
			object @lock = currentNode.m_lock;
			lock (@lock)
			{
				foreach (Node node in currentNode.Children.Values.Reverse<Node>())
				{
					stack.Push(node);
				}
			}
		}

		// Token: 0x0400A2EB RID: 41707
		public static readonly DateTime DefaultLastWriteTime = new DateTime(0L, DateTimeKind.Utc);

		// Token: 0x0400A2ED RID: 41709
		public DateTime LastWriteTimeUtc;

		// Token: 0x0400A2EE RID: 41710
		public readonly List<ulong> BlobIds;

		// Token: 0x0400A2EF RID: 41711
		public readonly SortedDictionary<string, Node> Children;

		// Token: 0x0400A2F0 RID: 41712
		public readonly object m_lock;

		// Token: 0x0400A2F1 RID: 41713
		public Node Parent;
	}
}
