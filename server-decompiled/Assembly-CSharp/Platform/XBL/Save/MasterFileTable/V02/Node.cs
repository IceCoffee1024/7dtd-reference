using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Platform.XBL.Save.MasterFileTable.V02
{
	// Token: 0x02001CA3 RID: 7331
	public sealed class Node : IDisposable
	{
		// Token: 0x17001B67 RID: 7015
		// (get) Token: 0x0600D948 RID: 55624 RVA: 0x004DEA66 File Offset: 0x004DCC66
		// (set) Token: 0x0600D949 RID: 55625 RVA: 0x004DEA6E File Offset: 0x004DCC6E
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600D94A RID: 55626 RVA: 0x004DEA78 File Offset: 0x004DCC78
		public Node()
		{
			this.Name = string.Empty;
			this.LastWriteTimeUtc = Node.DefaultLastWriteTime;
			this.BlobIds = new List<ulong>();
			this.Children = new SortedDictionary<string, Node>();
			this.m_lock = new object();
			this.Parent = null;
		}

		// Token: 0x0600D94B RID: 55627 RVA: 0x004DEACC File Offset: 0x004DCCCC
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

		// Token: 0x0600D94C RID: 55628 RVA: 0x004DEB78 File Offset: 0x004DCD78
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

		// Token: 0x0600D94D RID: 55629 RVA: 0x004DEC84 File Offset: 0x004DCE84
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

		// Token: 0x0600D94E RID: 55630 RVA: 0x004DED48 File Offset: 0x004DCF48
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

		// Token: 0x0600D94F RID: 55631 RVA: 0x004DEDF0 File Offset: 0x004DCFF0
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

		// Token: 0x0600D950 RID: 55632 RVA: 0x004DEE3C File Offset: 0x004DD03C
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

		// Token: 0x0600D951 RID: 55633 RVA: 0x004DEE88 File Offset: 0x004DD088
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

		// Token: 0x0600D952 RID: 55634 RVA: 0x004DEEDC File Offset: 0x004DD0DC
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

		// Token: 0x0600D953 RID: 55635 RVA: 0x004DEF4C File Offset: 0x004DD14C
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

		// Token: 0x0600D955 RID: 55637 RVA: 0x004DEFF8 File Offset: 0x004DD1F8
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

		// Token: 0x0400A4F1 RID: 42225
		public static readonly DateTime DefaultLastWriteTime = new DateTime(0L, DateTimeKind.Utc);

		// Token: 0x0400A4F3 RID: 42227
		public DateTime LastWriteTimeUtc;

		// Token: 0x0400A4F4 RID: 42228
		public readonly List<ulong> BlobIds;

		// Token: 0x0400A4F5 RID: 42229
		public readonly SortedDictionary<string, Node> Children;

		// Token: 0x0400A4F6 RID: 42230
		public readonly object m_lock;

		// Token: 0x0400A4F7 RID: 42231
		public Node Parent;
	}
}
