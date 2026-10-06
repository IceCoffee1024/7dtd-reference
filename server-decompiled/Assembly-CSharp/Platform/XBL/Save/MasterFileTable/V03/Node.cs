using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Platform.XBL.Save.MasterFileTable.V03
{
	// Token: 0x02001CA0 RID: 7328
	public sealed class Node : IDisposable
	{
		// Token: 0x17001B62 RID: 7010
		// (get) Token: 0x0600D92B RID: 55595 RVA: 0x004DE17D File Offset: 0x004DC37D
		// (set) Token: 0x0600D92C RID: 55596 RVA: 0x004DE185 File Offset: 0x004DC385
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B63 RID: 7011
		// (get) Token: 0x0600D92D RID: 55597 RVA: 0x004DE18E File Offset: 0x004DC38E
		public IReadOnlyList<ulong> BlobIds
		{
			get
			{
				return this.m_blobIds;
			}
		}

		// Token: 0x17001B64 RID: 7012
		// (get) Token: 0x0600D92E RID: 55598 RVA: 0x004DE196 File Offset: 0x004DC396
		public IReadOnlyDictionary<string, Node> Children
		{
			get
			{
				return this.m_children;
			}
		}

		// Token: 0x0600D92F RID: 55599 RVA: 0x004DE1A0 File Offset: 0x004DC3A0
		public Node()
		{
			this.Name = string.Empty;
			this.LastWriteTimeUtc = Node.DefaultLastWriteTime;
			this.Attributes = NodeAttributes.None;
			this.m_blobIds = Array.Empty<ulong>();
			this.m_blobLock = new object();
			this.m_children = new StringSpanDictionary<Node>(new SortedDictionary<string, Node>());
			this.m_lock = new object();
			this.Parent = null;
		}

		// Token: 0x0600D930 RID: 55600 RVA: 0x004DE208 File Offset: 0x004DC408
		public void SetBlobIds(ulong[] blobIds)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.m_blobIds = blobIds;
			}
		}

		// Token: 0x0600D931 RID: 55601 RVA: 0x004DE24C File Offset: 0x004DC44C
		public override string ToString()
		{
			object @lock = this.m_lock;
			string text;
			lock (@lock)
			{
				text = string.Format("{0}[{1}=\"{2}\", {3}=\"{4}\", {5}=\"{6}\", {7}=[{8}], #{9}={10}]", new object[]
				{
					"Node",
					"Name",
					this.Name,
					"LastWriteTimeUtc",
					this.LastWriteTimeUtc,
					"Attributes",
					this.Attributes,
					"BlobIds",
					string.Join(", ", this.BlobIds.Select<ulong, string>(new Func<ulong, string>(SaveContainer.IdToString))),
					"Children",
					this.Children.Count
				});
			}
			return text;
		}

		// Token: 0x0600D932 RID: 55602 RVA: 0x004DE328 File Offset: 0x004DC528
		public void Write(PooledBinaryWriter writer)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				writer.Write(this.Name);
				writer.Write(this.LastWriteTimeUtc.Ticks);
				writer.Write(Convert.ToUInt32(this.Attributes));
				ulong[] blobIds = this.m_blobIds;
				ushort num = (ushort)blobIds.Length;
				writer.Write(num);
				foreach (ulong num2 in blobIds)
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

		// Token: 0x0600D933 RID: 55603 RVA: 0x004DE420 File Offset: 0x004DC620
		public void Read(PooledBinaryReader reader)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.Name = reader.ReadString();
				this.LastWriteTimeUtc = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
				this.Attributes = (NodeAttributes)reader.ReadUInt32();
				ushort num = reader.ReadUInt16();
				ulong[] array = new ulong[(int)num];
				for (int i = 0; i < (int)num; i++)
				{
					ulong num2 = reader.ReadUInt64();
					array[i] = num2;
				}
				this.m_blobIds = array;
				ushort num3 = reader.ReadUInt16();
				for (int j = 0; j < (int)num3; j++)
				{
					Node node = new Node();
					node.Read(reader);
					this.m_children.Add(node.Name, node);
					node.Parent = this;
				}
			}
		}

		// Token: 0x0600D934 RID: 55604 RVA: 0x004DE4FC File Offset: 0x004DC6FC
		public void Dispose()
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				foreach (Node node in this.Children.Values)
				{
					node.Dispose();
				}
				this.m_children.Clear();
				this.Parent = null;
				this.m_blobIds = Array.Empty<ulong>();
				this.Name = string.Empty;
			}
		}

		// Token: 0x0600D935 RID: 55605 RVA: 0x004DE59C File Offset: 0x004DC79C
		public bool IsDirectory()
		{
			return this.Attributes.HasFlag(NodeAttributes.Directory);
		}

		// Token: 0x0600D936 RID: 55606 RVA: 0x004DE5B4 File Offset: 0x004DC7B4
		public bool IsFile()
		{
			return !this.IsDirectory();
		}

		// Token: 0x0600D937 RID: 55607 RVA: 0x004DE5C0 File Offset: 0x004DC7C0
		public Node GetChildNode(StringSpan name)
		{
			object @lock = this.m_lock;
			Node node2;
			lock (@lock)
			{
				Node node;
				if (!this.m_children.TryGetValue(name, out node))
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

		// Token: 0x0600D938 RID: 55608 RVA: 0x004DE614 File Offset: 0x004DC814
		public Node GetOrCreateChildNode(StringSpan name, bool createDirectory)
		{
			bool flag;
			return this.GetOrCreateChildNode(name, createDirectory, out flag);
		}

		// Token: 0x0600D939 RID: 55609 RVA: 0x004DE62C File Offset: 0x004DC82C
		public Node GetOrCreateChildNode(StringSpan name, bool createDirectory, out bool wasCreated)
		{
			object @lock = this.m_lock;
			Node node2;
			lock (@lock)
			{
				Node node;
				if (this.m_children.TryGetValue(name, out node))
				{
					wasCreated = false;
					node2 = node;
				}
				else
				{
					string text = name.ToString();
					node = new Node
					{
						Name = text
					};
					node.Parent = this;
					this.m_children.Add(text, node);
					this.LastWriteTimeUtc = (node.LastWriteTimeUtc = DateTime.UtcNow);
					this.Attributes |= NodeAttributes.Directory;
					if (createDirectory)
					{
						node.Attributes |= NodeAttributes.Directory;
					}
					wasCreated = true;
					node2 = node;
				}
			}
			return node2;
		}

		// Token: 0x0600D93A RID: 55610 RVA: 0x004DE6EC File Offset: 0x004DC8EC
		public Node DeleteChildNode(StringSpan name)
		{
			object @lock = this.m_lock;
			Node node2;
			lock (@lock)
			{
				Node node;
				if (!this.m_children.TryGetValue(name, out node) || !this.m_children.Remove(name))
				{
					node2 = null;
				}
				else
				{
					this.LastWriteTimeUtc = DateTime.UtcNow;
					node2 = node;
				}
			}
			return node2;
		}

		// Token: 0x0600D93B RID: 55611 RVA: 0x004DE758 File Offset: 0x004DC958
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
				Node.<Enumerate>g__AddNodeChildrenToStack|28_0(stack, this);
			}
			while (stack.Count > 0)
			{
				Node node = stack.Pop();
				list.Add(node);
				Node.<Enumerate>g__AddNodeChildrenToStack|28_0(stack, node);
			}
			return list;
		}

		// Token: 0x0600D93C RID: 55612 RVA: 0x004DE7F4 File Offset: 0x004DC9F4
		public void MoveChild(Node child)
		{
			this.MoveChild(child, child.Name);
		}

		// Token: 0x0600D93D RID: 55613 RVA: 0x004DE808 File Offset: 0x004DCA08
		public void MoveChild(Node child, StringSpan newName)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				object lock2 = child.m_lock;
				lock (lock2)
				{
					Node parent = child.Parent;
					if (parent == null || parent == this)
					{
						Node.<MoveChild>g__MoveChildInternal|30_0(parent, this, child, newName);
					}
					else
					{
						object lock3 = parent.m_lock;
						lock (lock3)
						{
							Node.<MoveChild>g__MoveChildInternal|30_0(parent, this, child, newName);
						}
					}
				}
			}
		}

		// Token: 0x0600D93F RID: 55615 RVA: 0x004DE8D0 File Offset: 0x004DCAD0
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <Enumerate>g__AddNodeChildrenToStack|28_0(Stack<Node> stack, Node currentNode)
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

		// Token: 0x0600D940 RID: 55616 RVA: 0x004DE950 File Offset: 0x004DCB50
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <MoveChild>g__MoveChildInternal|30_0(Node oldParent, Node newParent, Node child, StringSpan newName)
		{
			if (oldParent == newParent && child.Name == newName)
			{
				return;
			}
			DateTime utcNow = DateTime.UtcNow;
			if (oldParent != null)
			{
				oldParent.m_children.Remove(child.Name);
				oldParent.LastWriteTimeUtc = utcNow;
			}
			if (child.Name != newName)
			{
				child.Name = newName.ToString();
			}
			child.Parent = newParent;
			Node node;
			newParent.m_children.Remove(child.Name, out node);
			if (node != null && node != child)
			{
				node.Dispose();
			}
			newParent.m_children.Add(child.Name, child);
			newParent.LastWriteTimeUtc = utcNow;
		}

		// Token: 0x0400A4E3 RID: 42211
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly DateTime DefaultLastWriteTime = new DateTime(0L, DateTimeKind.Utc);

		// Token: 0x0400A4E5 RID: 42213
		public DateTime LastWriteTimeUtc;

		// Token: 0x0400A4E6 RID: 42214
		public NodeAttributes Attributes;

		// Token: 0x0400A4E7 RID: 42215
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong[] m_blobIds;

		// Token: 0x0400A4E8 RID: 42216
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StringSpanDictionary<Node> m_children;

		// Token: 0x0400A4E9 RID: 42217
		public readonly object m_blobLock;

		// Token: 0x0400A4EA RID: 42218
		public readonly object m_lock;

		// Token: 0x0400A4EB RID: 42219
		public Node Parent;
	}
}
