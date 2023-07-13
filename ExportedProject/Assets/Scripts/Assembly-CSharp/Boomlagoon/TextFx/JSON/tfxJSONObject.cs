using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Boomlagoon.TextFx.JSON
{
	public class tfxJSONObject : IEnumerable<KeyValuePair<string, tfxJSONValue>>, IEnumerable
	{
		private enum JSONParsingState
		{
			Object = 0,
			Array = 1,
			EndObject = 2,
			EndArray = 3,
			Key = 4,
			Value = 5,
			KeyValueSeparator = 6,
			ValueSeparator = 7,
			String = 8,
			Number = 9,
			Boolean = 10,
			Null = 11
		}

		private readonly IDictionary<string, tfxJSONValue> values = new Dictionary<string, tfxJSONValue>();

		public tfxJSONValue this[string key]
		{
			get
			{
				return GetValue(key);
			}
			set
			{
				values[key] = value;
			}
		}

		public tfxJSONObject()
		{
		}

		public tfxJSONObject(tfxJSONObject other)
		{
			values = new Dictionary<string, tfxJSONValue>();
			if (other == null)
			{
				return;
			}
			foreach (KeyValuePair<string, tfxJSONValue> value in other.values)
			{
				values[value.Key] = new tfxJSONValue(value.Value);
			}
		}

		public bool ContainsKey(string key)
		{
			return values.ContainsKey(key);
		}

		public tfxJSONValue GetValue(string key)
		{
			tfxJSONValue value;
			values.TryGetValue(key, out value);
			return value;
		}

		public string GetString(string key)
		{
			tfxJSONValue value = GetValue(key);
			if (value == null)
			{
				JSONLogger.Error(key + "(string) == null");
				return string.Empty;
			}
			return value.Str;
		}

		public double GetNumber(string key)
		{
			tfxJSONValue value = GetValue(key);
			if (value == null)
			{
				JSONLogger.Error(key + " == null");
				return double.NaN;
			}
			return value.Number;
		}

		public tfxJSONObject GetObject(string key)
		{
			tfxJSONValue value = GetValue(key);
			if (value == null)
			{
				JSONLogger.Error(key + " == null");
				return null;
			}
			return value.Obj;
		}

		public bool GetBoolean(string key)
		{
			tfxJSONValue value = GetValue(key);
			if (value == null)
			{
				JSONLogger.Error(key + " == null");
				return false;
			}
			return value.Boolean;
		}

		public tfxJSONArray GetArray(string key)
		{
			tfxJSONValue value = GetValue(key);
			if (value == null)
			{
				JSONLogger.Error(key + " == null");
				return null;
			}
			return value.Array;
		}

		public void Add(string key, tfxJSONValue value)
		{
			values[key] = value;
		}

		public void Add(KeyValuePair<string, tfxJSONValue> pair)
		{
			values[pair.Key] = pair.Value;
		}

		public static tfxJSONObject Parse(string jsonString, bool force_hide_errors = false)
		{
			if (string.IsNullOrEmpty(jsonString))
			{
				return null;
			}
			tfxJSONValue tfxJSONValue2 = null;
			List<string> list = new List<string>();
			JSONParsingState jSONParsingState = JSONParsingState.Object;
			int num;
			for (num = 0; num < jsonString.Length; num++)
			{
				num = SkipWhitespace(jsonString, num);
				switch (jSONParsingState)
				{
				case JSONParsingState.Object:
				{
					if (jsonString[num] != '{')
					{
						return Fail('{', num, force_hide_errors);
					}
					tfxJSONValue tfxJSONValue3 = new tfxJSONObject();
					if (tfxJSONValue2 != null)
					{
						tfxJSONValue3.Parent = tfxJSONValue2;
					}
					tfxJSONValue2 = tfxJSONValue3;
					jSONParsingState = JSONParsingState.Key;
					break;
				}
				case JSONParsingState.EndObject:
					if (jsonString[num] != '}')
					{
						return Fail('}', num, force_hide_errors);
					}
					if (tfxJSONValue2.Parent == null)
					{
						return tfxJSONValue2.Obj;
					}
					switch (tfxJSONValue2.Parent.Type)
					{
					case tfxJSONValueType.Object:
						tfxJSONValue2.Parent.Obj.values[list.Pop()] = new tfxJSONValue(tfxJSONValue2.Obj);
						break;
					case tfxJSONValueType.Array:
						tfxJSONValue2.Parent.Array.Add(new tfxJSONValue(tfxJSONValue2.Obj));
						break;
					default:
						return Fail("valid object", num, force_hide_errors);
					}
					tfxJSONValue2 = tfxJSONValue2.Parent;
					jSONParsingState = JSONParsingState.ValueSeparator;
					break;
				case JSONParsingState.Key:
				{
					if (jsonString[num] == '}')
					{
						num--;
						jSONParsingState = JSONParsingState.EndObject;
						break;
					}
					string text = ParseString(jsonString, ref num, force_hide_errors);
					if (text == null)
					{
						return Fail("key string", num, force_hide_errors);
					}
					list.Add(text);
					jSONParsingState = JSONParsingState.KeyValueSeparator;
					break;
				}
				case JSONParsingState.KeyValueSeparator:
					if (jsonString[num] != ':')
					{
						return Fail(':', num, force_hide_errors);
					}
					jSONParsingState = JSONParsingState.Value;
					break;
				case JSONParsingState.ValueSeparator:
					switch (jsonString[num])
					{
					case ',':
						jSONParsingState = ((tfxJSONValue2.Type != tfxJSONValueType.Object) ? JSONParsingState.Value : JSONParsingState.Key);
						break;
					case '}':
						jSONParsingState = JSONParsingState.EndObject;
						num--;
						break;
					case ']':
						jSONParsingState = JSONParsingState.EndArray;
						num--;
						break;
					default:
						return Fail(", } ]", num, force_hide_errors);
					}
					break;
				case JSONParsingState.Value:
				{
					char c = jsonString[num];
					if (c == '"')
					{
						jSONParsingState = JSONParsingState.String;
					}
					else
					{
						if (!char.IsDigit(c))
						{
							switch (c)
							{
							case '-':
								break;
							case '{':
								goto IL_0294;
							case '[':
								goto IL_029b;
							case ']':
								goto IL_02a2;
							case 'f':
							case 't':
								goto IL_02c7;
							case 'n':
								goto IL_02cf;
							default:
								return Fail("beginning of value", num, force_hide_errors);
							}
						}
						jSONParsingState = JSONParsingState.Number;
					}
					goto IL_02e4;
				}
				case JSONParsingState.String:
				{
					string text2 = ParseString(jsonString, ref num, force_hide_errors);
					if (text2 == null)
					{
						return Fail("string value", num, force_hide_errors);
					}
					switch (tfxJSONValue2.Type)
					{
					case tfxJSONValueType.Object:
						tfxJSONValue2.Obj.values[list.Pop()] = new tfxJSONValue(text2);
						break;
					case tfxJSONValueType.Array:
						tfxJSONValue2.Array.Add(text2);
						break;
					default:
						if (!force_hide_errors)
						{
							JSONLogger.Error("Fatal error, current JSON value not valid");
						}
						return null;
					}
					jSONParsingState = JSONParsingState.ValueSeparator;
					break;
				}
				case JSONParsingState.Number:
				{
					double num2 = ParseNumber(jsonString, ref num);
					if (double.IsNaN(num2))
					{
						return Fail("valid number", num, force_hide_errors);
					}
					switch (tfxJSONValue2.Type)
					{
					case tfxJSONValueType.Object:
						tfxJSONValue2.Obj.values[list.Pop()] = new tfxJSONValue(num2);
						break;
					case tfxJSONValueType.Array:
						tfxJSONValue2.Array.Add(num2);
						break;
					default:
						if (!force_hide_errors)
						{
							JSONLogger.Error("Fatal error, current JSON value not valid");
						}
						return null;
					}
					jSONParsingState = JSONParsingState.ValueSeparator;
					break;
				}
				case JSONParsingState.Boolean:
					if (jsonString[num] == 't')
					{
						if (jsonString.Length < num + 4 || jsonString[num + 1] != 'r' || jsonString[num + 2] != 'u' || jsonString[num + 3] != 'e')
						{
							return Fail("true", num, force_hide_errors);
						}
						switch (tfxJSONValue2.Type)
						{
						case tfxJSONValueType.Object:
							tfxJSONValue2.Obj.values[list.Pop()] = new tfxJSONValue(true);
							break;
						case tfxJSONValueType.Array:
							tfxJSONValue2.Array.Add(new tfxJSONValue(true));
							break;
						default:
							if (!force_hide_errors)
							{
								JSONLogger.Error("Fatal error, current JSON value not valid");
							}
							return null;
						}
						num += 3;
					}
					else
					{
						if (jsonString.Length < num + 5 || jsonString[num + 1] != 'a' || jsonString[num + 2] != 'l' || jsonString[num + 3] != 's' || jsonString[num + 4] != 'e')
						{
							return Fail("false", num, force_hide_errors);
						}
						switch (tfxJSONValue2.Type)
						{
						case tfxJSONValueType.Object:
							tfxJSONValue2.Obj.values[list.Pop()] = new tfxJSONValue(false);
							break;
						case tfxJSONValueType.Array:
							tfxJSONValue2.Array.Add(new tfxJSONValue(false));
							break;
						default:
							if (!force_hide_errors)
							{
								JSONLogger.Error("Fatal error, current JSON value not valid");
							}
							return null;
						}
						num += 4;
					}
					jSONParsingState = JSONParsingState.ValueSeparator;
					break;
				case JSONParsingState.Array:
				{
					if (jsonString[num] != '[')
					{
						return Fail('[', num, force_hide_errors);
					}
					tfxJSONValue tfxJSONValue4 = new tfxJSONArray();
					if (tfxJSONValue2 != null)
					{
						tfxJSONValue4.Parent = tfxJSONValue2;
					}
					tfxJSONValue2 = tfxJSONValue4;
					jSONParsingState = JSONParsingState.Value;
					break;
				}
				case JSONParsingState.EndArray:
					if (jsonString[num] != ']')
					{
						return Fail(']', num, force_hide_errors);
					}
					if (tfxJSONValue2.Parent == null)
					{
						return tfxJSONValue2.Obj;
					}
					switch (tfxJSONValue2.Parent.Type)
					{
					case tfxJSONValueType.Object:
						tfxJSONValue2.Parent.Obj.values[list.Pop()] = new tfxJSONValue(tfxJSONValue2.Array);
						break;
					case tfxJSONValueType.Array:
						tfxJSONValue2.Parent.Array.Add(new tfxJSONValue(tfxJSONValue2.Array));
						break;
					default:
						return Fail("valid object", num, force_hide_errors);
					}
					tfxJSONValue2 = tfxJSONValue2.Parent;
					jSONParsingState = JSONParsingState.ValueSeparator;
					break;
				case JSONParsingState.Null:
					{
						if (jsonString[num] == 'n')
						{
							if (jsonString.Length < num + 4 || jsonString[num + 1] != 'u' || jsonString[num + 2] != 'l' || jsonString[num + 3] != 'l')
							{
								return Fail("null", num, force_hide_errors);
							}
							switch (tfxJSONValue2.Type)
							{
							case tfxJSONValueType.Object:
								tfxJSONValue2.Obj.values[list.Pop()] = new tfxJSONValue(tfxJSONValueType.Null);
								break;
							case tfxJSONValueType.Array:
								tfxJSONValue2.Array.Add(new tfxJSONValue(tfxJSONValueType.Null));
								break;
							default:
								if (!force_hide_errors)
								{
									JSONLogger.Error("Fatal error, current JSON value not valid");
								}
								return null;
							}
							num += 3;
						}
						jSONParsingState = JSONParsingState.ValueSeparator;
						break;
					}
					IL_02cf:
					jSONParsingState = JSONParsingState.Null;
					goto IL_02e4;
					IL_029b:
					jSONParsingState = JSONParsingState.Array;
					goto IL_02e4;
					IL_02c7:
					jSONParsingState = JSONParsingState.Boolean;
					goto IL_02e4;
					IL_0294:
					jSONParsingState = JSONParsingState.Object;
					goto IL_02e4;
					IL_02a2:
					if (tfxJSONValue2.Type == tfxJSONValueType.Array)
					{
						jSONParsingState = JSONParsingState.EndArray;
						goto IL_02e4;
					}
					return Fail("valid array", num, force_hide_errors);
					IL_02e4:
					num--;
					break;
				}
			}
			if (!force_hide_errors)
			{
				JSONLogger.Error("Unexpected end of string");
			}
			return null;
		}

		private static int SkipWhitespace(string str, int pos)
		{
			while (pos < str.Length && char.IsWhiteSpace(str[pos]))
			{
				pos++;
			}
			return pos;
		}

		private static string ParseString(string str, ref int startPosition, bool force_hide_errors = false)
		{
			if (str[startPosition] != '"' || startPosition + 1 >= str.Length)
			{
				Fail('"', startPosition, force_hide_errors);
				return null;
			}
			int num = str.IndexOf('"', startPosition + 1);
			if (num <= startPosition)
			{
				Fail('"', startPosition + 1, force_hide_errors);
				return null;
			}
			while (str[num - 1] == '\\')
			{
				num = str.IndexOf('"', num + 1);
				if (num <= startPosition)
				{
					Fail('"', startPosition + 1, force_hide_errors);
					return null;
				}
			}
			string result = string.Empty;
			if (num > startPosition + 1)
			{
				result = str.Substring(startPosition + 1, num - startPosition - 1);
			}
			startPosition = num;
			return result;
		}

		private static double ParseNumber(string str, ref int startPosition)
		{
			if (startPosition >= str.Length || (!char.IsDigit(str[startPosition]) && str[startPosition] != '-'))
			{
				return double.NaN;
			}
			int i;
			for (i = startPosition + 1; i < str.Length && str[i] != ',' && str[i] != ']' && str[i] != '}'; i++)
			{
			}
			double result;
			if (!double.TryParse(str.Substring(startPosition, i - startPosition), NumberStyles.Float, CultureInfo.InvariantCulture, out result))
			{
				return double.NaN;
			}
			startPosition = i - 1;
			return result;
		}

		private static tfxJSONObject Fail(char expected, int position, bool force_hide_errors = false)
		{
			return Fail(new string(expected, 1), position, force_hide_errors);
		}

		private static tfxJSONObject Fail(string expected, int position, bool force_hide_errors = false)
		{
			if (!force_hide_errors)
			{
				JSONLogger.Error("Invalid json string, expecting " + expected + " at " + position);
			}
			return null;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('{');
			foreach (KeyValuePair<string, tfxJSONValue> value in values)
			{
				stringBuilder.Append("\"" + value.Key + "\"");
				stringBuilder.Append(':');
				stringBuilder.Append(value.Value.ToString());
				stringBuilder.Append(',');
			}
			if (values.Count > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		public IEnumerator<KeyValuePair<string, tfxJSONValue>> GetEnumerator()
		{
			return values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return values.GetEnumerator();
		}

		public void Clear()
		{
			values.Clear();
		}

		public void Remove(string key)
		{
			if (values.ContainsKey(key))
			{
				values.Remove(key);
			}
		}
	}
}
