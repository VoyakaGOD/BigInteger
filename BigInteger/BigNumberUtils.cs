using System;
using System.Text;

namespace BigInteger
{
	public static class BigNumberUtils
	{
		public static byte Add(byte left, byte right, ref byte carry)
		{
			int result = left + right + carry;
			carry = (byte)(result >> 8);
			return (byte)result;
		}

		public static void Add(byte[] num, byte[] added, int shift, ref byte carry)
		{
			for (int i = 0; i < added.Length; i++)
				num[i+shift] = Add(num[i + shift], added[i], ref carry);
		}

		public static byte[] Add(byte[] left, byte[] right, byte carry = 0)
		{
			int len = Math.Max(left.Length, right.Length);
			byte[] result = new byte[len + 1];
			int lim = Math.Min(left.Length, right.Length);
			int ptr = 0;
			for (; ptr < lim; ptr++)
				result[ptr] = Add(left[ptr], right[ptr], ref carry);
			byte[] longest = (left.Length > right.Length) ? left : right;
			for (; ptr < len; ptr++)
				result[ptr] = Add(longest[ptr], 0, ref carry);
			result[len] = carry;
			return result;
		}

		public static byte[] GetComplement(byte[] num, int minSize = 0)
		{
			int size = (minSize > num.Length) ? minSize : num.Length;
			byte[] result = new byte[size];
			int ptr = 0;
			for (; ptr < num.Length; ptr++)
				result[ptr] = (byte)(255 - num[ptr]);
			for (; ptr < size; ptr++)
				result[ptr] = 255;
			return result;
		}

		public static byte Mul(byte left, byte right, ref byte carry)
		{
			int result = left * right + carry;
			carry = (byte)(result >> 8);
			return (byte)result;
		}

		public static byte[] Mul(byte[] num, byte b, ref byte carry)
		{
			int len = num.Length;
			byte[] result = new byte[len];
			for (int i = 0; i < len; i++)
				result[i] = Mul(num[i], b, ref carry);
			return result;
		}

		public static byte[] Mul(byte[] left, byte[] right)
		{
			int len = left.Length + right.Length;
			byte[] result = new byte[len];
			for (int i = 0; i < right.Length; i++)
			{
				byte carry = 0;
				Add(result, Mul(left, right[i], ref result[left.Length + i]), i, ref carry);
				result[left.Length + i] += carry;
			}
			return result;
		}

		public static char ToHexChar(int value)
		{
			if (value > 9)
				return (char)('A' - 10 + value);
			return (char)('0' + value);
		}

		public static int FromHexChar(char hex)
		{
			if (hex > '9')
				return hex + 10 - 'A';
			return hex - '0';
		}

		public static string ToHex(byte[] num, int size)
		{
			if (size > num.Length)
				size = num.Length;
			if (size == 0)
				return "";
			var result = new StringBuilder();
			byte hight = num[size - 1];
			if (hight > 15)
				result.Append(ToHexChar(hight / 16));
			result.Append(ToHexChar(hight % 16));
			for (int i = size - 2; i >= 0; i--)
			{
				result.Append(ToHexChar(num[i] / 16));
				result.Append(ToHexChar(num[i] % 16));
			}
			return result.ToString();
		}

		public static string ToHex(byte[] num)
		{
			return ToHex(num, num.Length);
		}

		public static byte GetByteFromHex(char hight, char low)
		{
			int h = FromHexChar(hight);
			if (h < 0 || h > 15)
				throw new ArgumentException();
			int l = FromHexChar(low);
			if (l < 0 || l > 15)
				throw new ArgumentException();
			return (byte)((h << 4) + l);
		}

		public static byte[] FromHex(string hex)
		{
			if (hex.Length == 0)
				return new byte[0];
			int size = hex.Length / 2 + hex.Length % 2;
			byte[] num = new byte[size];
			if (hex.Length % 2 == 1)
				num[size - 1] = GetByteFromHex('0', hex[0]);
			else
				num[size - 1] = GetByteFromHex(hex[0], hex[1]);
			int ptr = 2 - (hex.Length % 2);
			for (int i = size - 2; i >= 0; i--)
			{
				num[i] = GetByteFromHex(hex[ptr], hex[ptr + 1]);
				ptr += 2;
			}
			return num;
		}
	}
}