using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// Token: 0x02000536 RID: 1334
public class CameraPerspectives
{
	// Token: 0x06002BF0 RID: 11248 RVA: 0x001154E4 File Offset: 0x001136E4
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetFullFilePath()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return GameIO.GetSaveGameDir() + "/camerapositions.xml";
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			return GameIO.GetSaveGameLocalDir() + "/camerapositions.xml";
		}
		return null;
	}

	// Token: 0x06002BF1 RID: 11249 RVA: 0x0011551F File Offset: 0x0011371F
	public CameraPerspectives(bool _load = true)
	{
		if (_load)
		{
			this.Load();
		}
	}

	// Token: 0x06002BF2 RID: 11250 RVA: 0x00115544 File Offset: 0x00113744
	public bool Load()
	{
		this.Perspectives.Clear();
		string fullFilePath = CameraPerspectives.GetFullFilePath();
		if (!SdFile.Exists(fullFilePath))
		{
			return false;
		}
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.SdLoad(fullFilePath);
		}
		catch (XmlException ex)
		{
			Log.Error("Failed loading camera file: " + ex.Message);
			return false;
		}
		if (xmlDocument.DocumentElement == null)
		{
			Log.Warning("Camera file has no root XML element.");
			return false;
		}
		foreach (object obj in xmlDocument.DocumentElement.ChildNodes)
		{
			XmlNode xmlNode = (XmlNode)obj;
			if (xmlNode.NodeType == XmlNodeType.Element && !(xmlNode.Name != "position"))
			{
				CameraPerspectives.Perspective perspective = CameraPerspectives.Perspective.FromXml((XmlElement)xmlNode);
				if (perspective != null && !this.Perspectives.TryAdd(perspective.Name, perspective))
				{
					Log.Warning("Duplicate camera perspective entry '" + perspective.Name + "'");
				}
			}
		}
		return true;
	}

	// Token: 0x06002BF3 RID: 11251 RVA: 0x00115670 File Offset: 0x00113870
	public void Save()
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.CreateXmlDeclaration();
		XmlElement xmlElement = xmlDocument.AddXmlElement("camerapositions");
		foreach (KeyValuePair<string, CameraPerspectives.Perspective> keyValuePair in this.Perspectives)
		{
			keyValuePair.Value.ToXml(xmlElement);
		}
		xmlDocument.SdSave(CameraPerspectives.GetFullFilePath());
	}

	// Token: 0x0400218F RID: 8591
	public readonly SortedDictionary<string, CameraPerspectives.Perspective> Perspectives = new SortedDictionary<string, CameraPerspectives.Perspective>(StringComparer.OrdinalIgnoreCase);

	// Token: 0x02000537 RID: 1335
	public class Perspective : IEquatable<CameraPerspectives.Perspective>
	{
		// Token: 0x06002BF4 RID: 11252 RVA: 0x001156EC File Offset: 0x001138EC
		public Perspective(string _name, Vector3 _position, Vector3 _direction, string _comment)
		{
			this.Name = _name;
			this.Position = _position;
			this.Direction = _direction;
			this.Comment = _comment;
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x00115714 File Offset: 0x00113914
		public Perspective(string _name, EntityPlayerLocal _player, string _comment = null)
		{
			this.Name = _name;
			if (_player.movementInput.bDetachedCameraMove)
			{
				this.Position = _player.cameraTransform.position - Constants.cDefaultCameraPlayerOffset;
				this.Direction = _player.cameraTransform.localEulerAngles;
				this.Direction.x = -this.Direction.x;
			}
			else
			{
				this.Position = _player.GetPosition();
				this.Direction = _player.rotation;
			}
			this.Comment = _comment;
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x001157A0 File Offset: 0x001139A0
		public void ToPlayer(EntityPlayerLocal _player)
		{
			if (_player.movementInput.bDetachedCameraMove)
			{
				_player.cameraTransform.position = this.Position + Constants.cDefaultCameraPlayerOffset;
				Vector3 direction = this.Direction;
				direction.x = -direction.x;
				_player.cameraTransform.localEulerAngles = direction;
				return;
			}
			_player.TeleportToPosition(this.Position, false, new Vector3?(this.Direction));
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x00115810 File Offset: 0x00113A10
		public bool Equals(CameraPerspectives.Perspective _other)
		{
			return _other != null && (this == _other || (this.Position.Equals(_other.Position) && this.Direction.Equals(_other.Direction)));
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x00115854 File Offset: 0x00113A54
		public override bool Equals(object _obj)
		{
			return _obj != null && (this == _obj || (!(_obj.GetType() != base.GetType()) && this.Equals((CameraPerspectives.Perspective)_obj)));
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x00115884 File Offset: 0x00113A84
		public override int GetHashCode()
		{
			return (this.Position.GetHashCode() * 397) ^ this.Direction.GetHashCode();
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x001158C0 File Offset: 0x00113AC0
		public void ToXml(XmlElement _parent)
		{
			XmlElement xmlElement = _parent.AddXmlElement("position").SetAttrib("name", this.Name).SetAttrib("position", this.Position.ToString())
				.SetAttrib("direction", this.Direction.ToString());
			if (!string.IsNullOrEmpty(this.Comment))
			{
				xmlElement.SetAttrib("comment", this.Comment);
			}
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x00115944 File Offset: 0x00113B44
		public static CameraPerspectives.Perspective FromXml(XmlElement _lineItem)
		{
			if (!_lineItem.HasAttribute("name"))
			{
				Log.Warning("Ignoring camera-entry because of missing 'name' attribute: " + _lineItem.OuterXml);
				return null;
			}
			if (!_lineItem.HasAttribute("position"))
			{
				Log.Warning("Ignoring camera-entry because of missing 'position' attribute: " + _lineItem.OuterXml);
				return null;
			}
			if (!_lineItem.HasAttribute("direction"))
			{
				Log.Warning("Ignoring camera-entry because of missing 'direction' attribute: " + _lineItem.OuterXml);
				return null;
			}
			string attribute = _lineItem.GetAttribute("name");
			Vector3 vector = StringParsers.ParseVector3(_lineItem.GetAttribute("position"), 0, -1);
			Vector3 vector2 = StringParsers.ParseVector3(_lineItem.GetAttribute("direction"), 0, -1);
			string text = null;
			if (_lineItem.HasAttribute("comment"))
			{
				text = _lineItem.GetAttribute("comment");
			}
			return new CameraPerspectives.Perspective(attribute, vector, vector2, text);
		}

		// Token: 0x04002190 RID: 8592
		public readonly string Name;

		// Token: 0x04002191 RID: 8593
		public readonly Vector3 Position;

		// Token: 0x04002192 RID: 8594
		public readonly Vector3 Direction;

		// Token: 0x04002193 RID: 8595
		public readonly string Comment;
	}
}
