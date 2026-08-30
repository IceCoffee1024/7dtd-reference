using System;
using System.Reflection;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001624 RID: 5668
	public class PositionXmlDocument : XmlDocument
	{
		// Token: 0x0600B231 RID: 45617 RVA: 0x00427C00 File Offset: 0x00425E00
		public PositionXmlDocument()
		{
			this.setImplementation();
		}

		// Token: 0x0600B232 RID: 45618 RVA: 0x00427C0E File Offset: 0x00425E0E
		[PublicizedFrom(EAccessModifier.ProtectedInternal)]
		public PositionXmlDocument(XmlImplementation _imp)
			: base(_imp)
		{
			this.setImplementation();
		}

		// Token: 0x0600B233 RID: 45619 RVA: 0x00427C1D File Offset: 0x00425E1D
		public PositionXmlDocument(XmlNameTable _nt)
			: base(_nt)
		{
			this.setImplementation();
		}

		// Token: 0x0600B234 RID: 45620 RVA: 0x00427C2C File Offset: 0x00425E2C
		[PublicizedFrom(EAccessModifier.Private)]
		public void setImplementation()
		{
			if (this.implementationFieldInfo == null)
			{
				this.implementationFieldInfo = typeof(XmlDocument).GetField("implementation", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			this.implementationFieldInfo.SetValue(this, new PositionXmlImplementation());
		}

		// Token: 0x0600B235 RID: 45621 RVA: 0x00427C69 File Offset: 0x00425E69
		public override XmlElement CreateElement(string prefix, string localName, string namespaceURI)
		{
			return new PositionXmlElement(prefix, localName, namespaceURI, this, this.lineInfo);
		}

		// Token: 0x0600B236 RID: 45622 RVA: 0x00427C7A File Offset: 0x00425E7A
		public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI)
		{
			return new PositionXmlAttribute(prefix, localName, namespaceURI, this, this.lineInfo);
		}

		// Token: 0x0600B237 RID: 45623 RVA: 0x00427C8B File Offset: 0x00425E8B
		public override XmlCDataSection CreateCDataSection(string data)
		{
			return new PositionXmlCDataSection(data, this, this.lineInfo);
		}

		// Token: 0x0600B238 RID: 45624 RVA: 0x00427C9A File Offset: 0x00425E9A
		public override XmlComment CreateComment(string data)
		{
			return new PositionXmlComment(data, this, this.lineInfo);
		}

		// Token: 0x0600B239 RID: 45625 RVA: 0x00427C7A File Offset: 0x00425E7A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override XmlAttribute CreateDefaultAttribute(string prefix, string localName, string namespaceURI)
		{
			return new PositionXmlAttribute(prefix, localName, namespaceURI, this, this.lineInfo);
		}

		// Token: 0x0600B23A RID: 45626 RVA: 0x00427CA9 File Offset: 0x00425EA9
		public override XmlDocumentFragment CreateDocumentFragment()
		{
			return new PositionXmlDocumentFragment(this, this.lineInfo);
		}

		// Token: 0x0600B23B RID: 45627 RVA: 0x00427CB7 File Offset: 0x00425EB7
		public override XmlDocumentType CreateDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new PositionXmlDocumentType(name, publicId, systemId, internalSubset, this, this.lineInfo);
		}

		// Token: 0x0600B23C RID: 45628 RVA: 0x00427CCA File Offset: 0x00425ECA
		public override XmlEntityReference CreateEntityReference(string name)
		{
			return new PositionXmlEntityReference(name, this, this.lineInfo);
		}

		// Token: 0x0600B23D RID: 45629 RVA: 0x00427CD9 File Offset: 0x00425ED9
		public override XmlNode CreateNode(string nodeTypeString, string name, string namespaceURI)
		{
			Console.WriteLine("CREATING NODE1: " + name);
			return base.CreateNode(nodeTypeString, name, namespaceURI);
		}

		// Token: 0x0600B23E RID: 45630 RVA: 0x00427CF4 File Offset: 0x00425EF4
		public override XmlNode CreateNode(XmlNodeType type, string name, string namespaceURI)
		{
			Console.WriteLine("CREATING NODE2: " + name);
			return base.CreateNode(type, name, namespaceURI);
		}

		// Token: 0x0600B23F RID: 45631 RVA: 0x00427D0F File Offset: 0x00425F0F
		public override XmlNode CreateNode(XmlNodeType type, string prefix, string name, string namespaceURI)
		{
			Console.WriteLine("CREATING NODE3: " + name);
			return base.CreateNode(type, prefix, name, namespaceURI);
		}

		// Token: 0x0600B240 RID: 45632 RVA: 0x00427D2C File Offset: 0x00425F2C
		public override XmlProcessingInstruction CreateProcessingInstruction(string target, string data)
		{
			return new PositionXmlProcessingInstruction(target, data, this, this.lineInfo);
		}

		// Token: 0x0600B241 RID: 45633 RVA: 0x00427D3C File Offset: 0x00425F3C
		public override XmlSignificantWhitespace CreateSignificantWhitespace(string text)
		{
			return new PositionXmlSignificantWhitespace(text, this, this.lineInfo);
		}

		// Token: 0x0600B242 RID: 45634 RVA: 0x00427D4B File Offset: 0x00425F4B
		public override XmlText CreateTextNode(string text)
		{
			return new PositionXmlText(text, this, this.lineInfo);
		}

		// Token: 0x0600B243 RID: 45635 RVA: 0x00427D5A File Offset: 0x00425F5A
		public override XmlWhitespace CreateWhitespace(string text)
		{
			return new PositionXmlWhitespace(text, this, this.lineInfo);
		}

		// Token: 0x0600B244 RID: 45636 RVA: 0x00427D69 File Offset: 0x00425F69
		public override XmlDeclaration CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new PositionXmlDeclaration(version, encoding, standalone, this, this.lineInfo);
		}

		// Token: 0x0600B245 RID: 45637 RVA: 0x00427D7C File Offset: 0x00425F7C
		public override void Load(XmlReader reader)
		{
			this.lineInfo = reader as IXmlLineInfo;
			try
			{
				base.Load(reader);
			}
			finally
			{
				this.lineInfo = null;
			}
		}

		// Token: 0x040085F6 RID: 34294
		[PublicizedFrom(EAccessModifier.Private)]
		public IXmlLineInfo lineInfo;

		// Token: 0x040085F7 RID: 34295
		[PublicizedFrom(EAccessModifier.Private)]
		public FieldInfo implementationFieldInfo;
	}
}
