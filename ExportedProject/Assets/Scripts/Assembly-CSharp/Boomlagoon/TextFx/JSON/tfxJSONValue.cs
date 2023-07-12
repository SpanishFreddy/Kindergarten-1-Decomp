namespace Boomlagoon.TextFx.JSON
{
	public class tfxJSONValue
	{
		public tfxJSONValueType Type { get; private set; }

		public string Str { get; set; }

		public double Number { get; set; }

		public tfxJSONObject Obj { get; set; }

		public tfxJSONArray Array { get; set; }

		public bool Boolean { get; set; }

		public tfxJSONValue Parent { get; set; }

		public tfxJSONValue(tfxJSONValueType type)
		{
			Type = type;
		}

		public tfxJSONValue(string str)
		{
			Type = tfxJSONValueType.String;
			Str = str;
		}

		public tfxJSONValue(double number)
		{
			Type = tfxJSONValueType.Number;
			Number = number;
		}

		public tfxJSONValue(tfxJSONObject obj)
		{
			if (obj == null)
			{
				Type = tfxJSONValueType.Null;
				return;
			}
			Type = tfxJSONValueType.Object;
			Obj = obj;
		}

		public tfxJSONValue(tfxJSONArray array)
		{
			Type = tfxJSONValueType.Array;
			Array = array;
		}

		public tfxJSONValue(bool boolean)
		{
			Type = tfxJSONValueType.Boolean;
			Boolean = boolean;
		}

		public tfxJSONValue(tfxJSONValue value)
		{
			Type = value.Type;
			switch (Type)
			{
			case tfxJSONValueType.String:
				Str = value.Str;
				break;
			case tfxJSONValueType.Boolean:
				Boolean = value.Boolean;
				break;
			case tfxJSONValueType.Number:
				Number = value.Number;
				break;
			case tfxJSONValueType.Object:
				if (value.Obj != null)
				{
					Obj = new tfxJSONObject(value.Obj);
				}
				break;
			case tfxJSONValueType.Array:
				Array = new tfxJSONArray(value.Array);
				break;
			}
		}

		public static implicit operator tfxJSONValue(string str)
		{
			return new tfxJSONValue(str);
		}

		public static implicit operator tfxJSONValue(double number)
		{
			return new tfxJSONValue(number);
		}

		public static implicit operator tfxJSONValue(tfxJSONObject obj)
		{
			return new tfxJSONValue(obj);
		}

		public static implicit operator tfxJSONValue(tfxJSONArray array)
		{
			return new tfxJSONValue(array);
		}

		public static implicit operator tfxJSONValue(bool boolean)
		{
			return new tfxJSONValue(boolean);
		}

		public override string ToString()
		{
			switch (Type)
			{
			case tfxJSONValueType.Object:
				return Obj.ToString();
			case tfxJSONValueType.Array:
				return Array.ToString();
			case tfxJSONValueType.Boolean:
				return (!Boolean) ? "false" : "true";
			case tfxJSONValueType.Number:
				return Number.ToString();
			case tfxJSONValueType.String:
				return "\"" + Str + "\"";
			case tfxJSONValueType.Null:
				return "null";
			default:
				return "null";
			}
		}
	}
}
