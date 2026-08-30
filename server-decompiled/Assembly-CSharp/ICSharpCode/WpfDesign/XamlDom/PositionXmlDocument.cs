using System;
using System.Reflection;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001609 RID: 5641
	public class PositionXmlDocument : XmlDocument
	{
		// Token: 0x0600B134 RID: 45364 RVA: 0x004228C0 File Offset: 0x00420AC0
		public PositionXmlDocument()
		{
			this.setImplementation();
		}

		// Token: 0x0600B135 RID: 45365 RVA: 0x004228CE File Offset: 0x00420ACE
		[PublicizedFrom(EAccessModifier.ProtectedInternal)]
		public PositionXmlDocument(XmlImplementation _imp)
			: base(_imp)
		{
			this.setImplementation();
		}

		// Token: 0x0600B136 RID: 45366 RVA: 0x004228DD File Offset: 0x00420ADD
		public PositionXmlDocument(XmlNameTable _nt)
			: base(_nt)
		{
			this.setImplementation();
		}

		// Token: 0x0600B137 RID: 45367 RVA: 0x004228EC File Offset: 0x00420AEC
		[PublicizedFrom(EAccessModifier.Private)]
		public void setImplementation()
		{
			if (this.implementationFieldInfo == null)
			{
				this.implementationFieldInfo = typeof(XmlDocument).GetField("implementation", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			this.implementationFieldInfo.SetValue(this, new PositionXmlImplementation());
		}

		// Token: 0x0600B138 RID: 45368 RVA: 0x00422929 File Offset: 0x00420B29
		public override XmlElement CreateElement(string prefix, string localName, string namespaceURI)
		{
			return new PositionXmlElement(prefix, localName, namespaceURI, this, this.lineInfo);
		}

		// Token: 0x0600B139 RID: 45369 RVA: 0x0042293A File Offset: 0x00420B3A
		public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI)
		{
			return new PositionXmlAttribute(prefix, localName, namespaceURI, this, this.lineInfo);
		}

		// Token: 0x0600B13A RID: 45370 RVA: 0x0042294B File Offset: 0x00420B4B
		public override XmlCDataSection CreateCDataSection(string data)
		{
			return new PositionXmlCDataSection(data, this, this.lineInfo);
		}

		// Token: 0x0600B13B RID: 45371 RVA: 0x0042295A File Offset: 0x00420B5A
		public override XmlComment CreateComment(string data)
		{
			return new PositionXmlComment(data, this, this.lineInfo);
		}

		// Token: 0x0600B13C RID: 45372 RVA: 0x0042293A File Offset: 0x00420B3A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override XmlAttribute CreateDefaultAttribute(string prefix, string localName, string namespaceURI)
		{
			return new PositionXmlAttribute(prefix, localName, namespaceURI, this, this.lineInfo);
		}

		// Token: 0x0600B13D RID: 45373 RVA: 0x00422969 File Offset: 0x00420B69
		public override XmlDocumentFragment CreateDocumentFragment()
		{
			return new PositionXmlDocumentFragment(this, this.lineInfo);
		}

		// Token: 0x0600B13E RID: 45374 RVA: 0x00422977 File Offset: 0x00420B77
		public override XmlDocumentType CreateDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new PositionXmlDocumentType(name, publicId, systemId, internalSubset, this, this.lineInfo);
		}

		// Token: 0x0600B13F RID: 45375 RVA: 0x0042298A File Offset: 0x00420B8A
		public override XmlEntityReference CreateEntityReference(string name)
		{
			return new PositionXmlEntityReference(name, this, this.lineInfo);
		}

		// Token: 0x0600B140 RID: 45376 RVA: 0x00422999 File Offset: 0x00420B99
		public override XmlNode CreateNode(string nodeTypeString, string name, string namespaceURI)
		{
			Console.WriteLine("CREATING NODE1: " + name);
			return base.CreateNode(nodeTypeString, name, namespaceURI);
		}

		// Token: 0x0600B141 RID: 45377 RVA: 0x004229B4 File Offset: 0x00420BB4
		public override XmlNode CreateNode(XmlNodeType type, string name, string namespaceURI)
		{
			Console.WriteLine("CREATING NODE2: " + name);
			return base.CreateNode(type, name, namespaceURI);
		}

		// Token: 0x0600B142 RID: 45378 RVA: 0x004229CF File Offset: 0x00420BCF
		public override XmlNode CreateNode(XmlNodeType type, string prefix, string name, string namespaceURI)
		{
			Console.WriteLine("CREATING NODE3: " + name);
			return base.CreateNode(type, prefix, name, namespaceURI);
		}

		// Token: 0x0600B143 RID: 45379 RVA: 0x004229EC File Offset: 0x00420BEC
		public override XmlProcessingInstruction CreateProcessingInstruction(string target, string data)
		{
			return new PositionXmlProcessingInstruction(target, data, this, this.lineInfo);
		}

		// Token: 0x0600B144 RID: 45380 RVA: 0x004229FC File Offset: 0x00420BFC
		public override XmlSignificantWhitespace CreateSignificantWhitespace(string text)
		{
			return new PositionXmlSignificantWhitespace(text, this, this.lineInfo);
		}

		// Token: 0x0600B145 RID: 45381 RVA: 0x00422A0B File Offset: 0x00420C0B
		public override XmlText CreateTextNode(string text)
		{
			return new PositionXmlText(text, this, this.lineInfo);
		}

		// Token: 0x0600B146 RID: 45382 RVA: 0x00422A1A File Offset: 0x00420C1A
		public override XmlWhitespace CreateWhitespace(string text)
		{
			return new PositionXmlWhitespace(text, this, this.lineInfo);
		}

		// Token: 0x0600B147 RID: 45383 RVA: 0x00422A29 File Offset: 0x00420C29
		public override XmlDeclaration CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new PositionXmlDeclaration(version, encoding, standalone, this, this.lineInfo);
		}

		// Token: 0x0600B148 RID: 45384 RVA: 0x00422A3C File Offset: 0x00420C3C
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

		// Token: 0x0400851D RID: 34077
		[PublicizedFrom(EAccessModifier.Private)]
		public IXmlLineInfo lineInfo;

		// Token: 0x0400851E RID: 34078
		[PublicizedFrom(EAccessModifier.Private)]
		public FieldInfo implementationFieldInfo;
	}
}
