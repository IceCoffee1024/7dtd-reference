using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Platform.XBL.Save.MasterFileTable.V03
{
	// Token: 0x02001C6E RID: 7278
	public sealed class Node : IDisposable
	{
		// Token: 0x17001ABC RID: 6844
		// (get) Token: 0x0600D7B2 RID: 55218 RVA: 0x004D9641 File Offset: 0x004D7841
		// (set) Token: 0x0600D7B3 RID: 55219 RVA: 0x004D9649 File Offset: 0x004D7849
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001ABD RID: 6845
		// (get) Token: 0x0600D7B4 RID: 55220 RVA: 0x004D9652 File Offset: 0x004D7852
		public IReadOnlyList<ulong> BlobIds
		{
			get
			{
				return this.m_blobIds;
			}
		}

		// Token: 0x17001ABE RID: 6846
		// (get) Token: 0x0600D7B5 RID: 55221 RVA: 0x004D965A File Offset: 0x004D785A
		public IReadOnlyDictionary<string, Node> Children
		{
			get
			{
				return this.m_children;
			}
		}

		// Token: 0x0600D7B6 RID: 55222 RVA: 0x004D9664 File Offset: 0x004D7864
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

		// Token: 0x0600D7B7 RID: 55223 RVA: 0x004D96CC File Offset: 0x004D78CC
		public void SetBlobIds(ulong[] blobIds)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.m_blobIds = blobIds;
			}
		}

		// Token: 0x0600D7B8 RID: 55224 RVA: 0x004D9710 File Offset: 0x004D7910
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

		// Token: 0x0600D7B9 RID: 55225 RVA: 0x004D97EC File Offset: 0x004D79EC
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

		// Token: 0x0600D7BA RID: 55226 RVA: 0x004D98E4 File Offset: 0x004D7AE4
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

		// Token: 0x0600D7BB RID: 55227 RVA: 0x004D99C0 File Offset: 0x004D7BC0
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

		// Token: 0x0600D7BC RID: 55228 RVA: 0x004D9A60 File Offset: 0x004D7C60
		public bool IsDirectory()
		{
			return this.Attributes.HasFlag(NodeAttributes.Directory);
		}

		// Token: 0x0600D7BD RID: 55229 RVA: 0x004D9A78 File Offset: 0x004D7C78
		public bool IsFile()
		{
			return !this.IsDirectory();
		}

		// Token: 0x0600D7BE RID: 55230 RVA: 0x004D9A84 File Offset: 0x004D7C84
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

		// Token: 0x0600D7BF RID: 55231 RVA: 0x004D9AD8 File Offset: 0x004D7CD8
		public Node GetOrCreateChildNode(StringSpan name, bool createDirectory)
		{
			bool flag;
			return this.GetOrCreateChildNode(name, createDirectory, out flag);
		}

		// Token: 0x0600D7C0 RID: 55232 RVA: 0x004D9AF0 File Offset: 0x004D7CF0
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

		// Token: 0x0600D7C1 RID: 55233 RVA: 0x004D9BB0 File Offset: 0x004D7DB0
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

		// Token: 0x0600D7C2 RID: 55234 RVA: 0x004D9C1C File Offset: 0x004D7E1C
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

		// Token: 0x0600D7C3 RID: 55235 RVA: 0x004D9CB8 File Offset: 0x004D7EB8
		public void MoveChild(Node child)
		{
			this.MoveChild(child, child.Name);
		}

		// Token: 0x0600D7C4 RID: 55236 RVA: 0x004D9CCC File Offset: 0x004D7ECC
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

		// Token: 0x0600D7C6 RID: 55238 RVA: 0x004D9D94 File Offset: 0x004D7F94
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

		// Token: 0x0600D7C7 RID: 55239 RVA: 0x004D9E14 File Offset: 0x004D8014
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

		// Token: 0x0400A40E RID: 41998
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly DateTime DefaultLastWriteTime = new DateTime(0L, DateTimeKind.Utc);

		// Token: 0x0400A410 RID: 42000
		public DateTime LastWriteTimeUtc;

		// Token: 0x0400A411 RID: 42001
		public NodeAttributes Attributes;

		// Token: 0x0400A412 RID: 42002
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong[] m_blobIds;

		// Token: 0x0400A413 RID: 42003
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StringSpanDictionary<Node> m_children;

		// Token: 0x0400A414 RID: 42004
		public readonly object m_blobLock;

		// Token: 0x0400A415 RID: 42005
		public readonly object m_lock;

		// Token: 0x0400A416 RID: 42006
		public Node Parent;
	}
}
