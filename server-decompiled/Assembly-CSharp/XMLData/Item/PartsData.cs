using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;

namespace XMLData.Item
{
	// Token: 0x02001668 RID: 5736
	[Preserve]
	public class PartsData : IXMLData
	{
		// Token: 0x170015C6 RID: 5574
		// (get) Token: 0x0600B43C RID: 46140 RVA: 0x0043665D File Offset: 0x0043485D
		// (set) Token: 0x0600B43D RID: 46141 RVA: 0x00436665 File Offset: 0x00434865
		public DataItem<ItemClass> Stock
		{
			get
			{
				return this.pStock;
			}
			set
			{
				this.pStock = value;
			}
		}

		// Token: 0x170015C7 RID: 5575
		// (get) Token: 0x0600B43E RID: 46142 RVA: 0x0043666E File Offset: 0x0043486E
		// (set) Token: 0x0600B43F RID: 46143 RVA: 0x00436676 File Offset: 0x00434876
		public DataItem<ItemClass> Receiver
		{
			get
			{
				return this.pReceiver;
			}
			set
			{
				this.pReceiver = value;
			}
		}

		// Token: 0x170015C8 RID: 5576
		// (get) Token: 0x0600B440 RID: 46144 RVA: 0x0043667F File Offset: 0x0043487F
		// (set) Token: 0x0600B441 RID: 46145 RVA: 0x00436687 File Offset: 0x00434887
		public DataItem<ItemClass> Pump
		{
			get
			{
				return this.pPump;
			}
			set
			{
				this.pPump = value;
			}
		}

		// Token: 0x170015C9 RID: 5577
		// (get) Token: 0x0600B442 RID: 46146 RVA: 0x00436690 File Offset: 0x00434890
		// (set) Token: 0x0600B443 RID: 46147 RVA: 0x00436698 File Offset: 0x00434898
		public DataItem<ItemClass> Barrel
		{
			get
			{
				return this.pBarrel;
			}
			set
			{
				this.pBarrel = value;
			}
		}

		// Token: 0x0600B444 RID: 46148 RVA: 0x0042C337 File Offset: 0x0042A537
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x04008731 RID: 34609
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pStock;

		// Token: 0x04008732 RID: 34610
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pReceiver;

		// Token: 0x04008733 RID: 34611
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pPump;

		// Token: 0x04008734 RID: 34612
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pBarrel;

		// Token: 0x02001669 RID: 5737
		public static class Parser
		{
			// Token: 0x0600B446 RID: 46150 RVA: 0x004366A4 File Offset: 0x004348A4
			public static PartsData Parse(PositionXmlElement _elem, Dictionary<PositionXmlElement, DataItem<ItemClass>> _updateLater)
			{
				string text = (_elem.HasAttribute("class") ? _elem.GetAttribute("class") : "PartsData");
				Type type = Type.GetType(typeof(PartsData.Parser).Namespace + "." + text);
				if (type == null)
				{
					type = Type.GetType(text);
					if (type == null)
					{
						throw new InvalidValueException("Specified class \"" + text + "\" not found", _elem.LineNumber);
					}
				}
				PartsData partsData = (PartsData)Activator.CreateInstance(type);
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (object obj in _elem.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					XmlNodeType nodeType = xmlNode.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.Comment)
						{
							throw new UnexpectedElementException("Unknown node \"" + xmlNode.NodeType.ToString() + "\" found while parsing Parts", ((IXmlLineInfo)xmlNode).LineNumber);
						}
					}
					else
					{
						PositionXmlElement positionXmlElement = (PositionXmlElement)xmlNode;
						if (!PartsData.Parser.knownAttributesMultiplicity.ContainsKey(positionXmlElement.Name))
						{
							throw new UnexpectedElementException("Unknown element \"" + xmlNode.Name + "\" found while parsing Parts", ((IXmlLineInfo)xmlNode).LineNumber);
						}
						string text2 = positionXmlElement.Name;
						if (!(text2 == "Stock"))
						{
							if (!(text2 == "Receiver"))
							{
								if (!(text2 == "Pump"))
								{
									if (text2 == "Barrel")
									{
										ItemClass itemClass;
										try
										{
											itemClass = null;
										}
										catch (Exception ex)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex);
										}
										DataItem<ItemClass> dataItem = new DataItem<ItemClass>("Barrel", itemClass);
										_updateLater.Add(positionXmlElement, dataItem);
										partsData.pBarrel = dataItem;
									}
								}
								else
								{
									ItemClass itemClass2;
									try
									{
										itemClass2 = null;
									}
									catch (Exception ex2)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex2);
									}
									DataItem<ItemClass> dataItem2 = new DataItem<ItemClass>("Pump", itemClass2);
									_updateLater.Add(positionXmlElement, dataItem2);
									partsData.pPump = dataItem2;
								}
							}
							else
							{
								ItemClass itemClass3;
								try
								{
									itemClass3 = null;
								}
								catch (Exception ex3)
								{
									throw new InvalidValueException(string.Concat(new string[]
									{
										"Could not parse attribute \"",
										positionXmlElement.Name,
										"\" value \"",
										ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
										"\""
									}), positionXmlElement.LineNumber, ex3);
								}
								DataItem<ItemClass> dataItem3 = new DataItem<ItemClass>("Receiver", itemClass3);
								_updateLater.Add(positionXmlElement, dataItem3);
								partsData.pReceiver = dataItem3;
							}
						}
						else
						{
							ItemClass itemClass4;
							try
							{
								itemClass4 = null;
							}
							catch (Exception ex4)
							{
								throw new InvalidValueException(string.Concat(new string[]
								{
									"Could not parse attribute \"",
									positionXmlElement.Name,
									"\" value \"",
									ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
									"\""
								}), positionXmlElement.LineNumber, ex4);
							}
							DataItem<ItemClass> dataItem4 = new DataItem<ItemClass>("Stock", itemClass4);
							_updateLater.Add(positionXmlElement, dataItem4);
							partsData.pStock = dataItem4;
						}
						if (!dictionary.ContainsKey(positionXmlElement.Name))
						{
							dictionary[positionXmlElement.Name] = 0;
						}
						Dictionary<string, int> dictionary2 = dictionary;
						text2 = positionXmlElement.Name;
						int num = dictionary2[text2];
						dictionary2[text2] = num + 1;
					}
				}
				foreach (KeyValuePair<string, Range<int>> keyValuePair in PartsData.Parser.knownAttributesMultiplicity)
				{
					int num2 = (dictionary.ContainsKey(keyValuePair.Key) ? dictionary[keyValuePair.Key] : 0);
					if ((keyValuePair.Value.hasMin && num2 < keyValuePair.Value.min) || (keyValuePair.Value.hasMax && num2 > keyValuePair.Value.max))
					{
						throw new IncorrectAttributeOccurrenceException(string.Concat(new string[]
						{
							"Element has incorrect number of \"",
							keyValuePair.Key,
							"\" attribute instances, found ",
							num2.ToString(),
							", expected ",
							keyValuePair.Value.ToString()
						}), _elem.LineNumber);
					}
				}
				return partsData;
			}

			// Token: 0x04008735 RID: 34613
			[PublicizedFrom(EAccessModifier.Private)]
			public static Dictionary<string, Range<int>> knownAttributesMultiplicity = new Dictionary<string, Range<int>>
			{
				{
					"Stock",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Receiver",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Pump",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Barrel",
					new Range<int>(true, 0, true, 1)
				}
			};
		}
	}
}
