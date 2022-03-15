using System;
using System.Collections.Generic;
using System.Text;

namespace BigInteger
{
	public sealed class BigInt : IComparable, IComparable<BigInt>, IEquatable<BigInt>
	{
		public static readonly BigInt Zero = new BigInt(false, new byte[] { 0 });
		public static readonly BigInt One = new BigInt(false, new byte[] { 1 });

		private bool _sign;
		private int _size;
		private byte[] _bytes;

		private BigInt(bool sign, byte[] bytes)
		{
			_sign = sign;
			_size = bytes.Length;
			if (bytes.Length > 0 && bytes[bytes.Length - 1] == 0)
				_size--;
			_bytes = bytes;
		}

		public int Size => _size;

		public bool IsNegative => _sign;

		public string ToHex()
		{
			return (_sign ? "-" : "") + BigNumberUtils.ToHex(_bytes, _size);
		}

		public BigInt ShiftByOneByte()
		{
			byte[] result = new byte[_size + 1];
			Array.Copy(_bytes, 0, result, 1, _size);
			return new BigInt(_sign, result);
		}

		public int Mod10()
		{
			return BigNumberUtils.Mod10(_bytes);
		}

		public BigInt UnsignedDiv10(out int remainder)
		{
			BigInt result = Zero;
			remainder = 0;
			for (int i = _size - 1; i >= 0; i--)
			{
				int v = 256 * remainder + _bytes[i];
				int v10 = v / 10;
				remainder = v - 10*v10;
				result = result.ShiftByOneByte() + new BigInt(false, new byte[] { (byte)v10 });
			}
			return result;
		}

		public override string ToString()
		{
			var result = new StringBuilder();
			BigInt num = this;
			int remainder;
			while(num.Size > 0)
			{
				num = num.UnsignedDiv10(out remainder);
				result.Insert(0, remainder);
			}
			if (_sign)
				result.Insert(0, '-');
			return result.ToString();
		}

		public int CompareTo(BigInt other)
		{
			if (other is null)
				return 1;

			if (_sign != other._sign)
				return _sign ? -1 : 1;

			if (_size != other._size)
				return ((_size > other._size) ? 1 : -1) * (_sign ? -1 : 1);

			for (int i = _size - 1; i >= 0; i--)
				if (_bytes[i] != other._bytes[i])
					return (_bytes[i] > other._bytes[i]) ? 1 : -1;

			return 0;
		}

		public int CompareTo(object obj)
		{
			if (obj is null || obj.GetType() != typeof(BigInt))
				throw new ArgumentException("Object must be BigInt.");

			return CompareTo((BigInt)obj);
		}

		public bool Equals(BigInt other)
		{
			if (other is null)
				return false;

			if (_sign != other._sign || _size != other._size)
				return false;
			for (int i = 0; i < _size; i++)
				if (_bytes[i] != other._bytes[i])
					return false;
			return true;
		}

		public override bool Equals(object obj)
		{
			if (obj is null || obj.GetType() != typeof(BigInt))
				return false;

			return Equals((BigInt)obj);
		}

		public override int GetHashCode()
		{
			var hashCode = 1478828392;
			hashCode = hashCode * -1521134295 + _sign.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<byte[]>.Default.GetHashCode(_bytes);
			return hashCode;
		}

		public static BigInt FromHex(string hex)
		{
			if(hex.Length  == 0)
				return Zero;

			bool sign = (hex[0] == '-');
			return new BigInt(sign, BigNumberUtils.FromHex(hex.Substring(sign ? 1 : 0)));
		}

		public static BigInt FromBytes(bool sign, byte[] bytes)
		{
			if (bytes.Length == 0)
				return Zero;

			return new BigInt(sign, (byte[])bytes.Clone());
		}

		public static bool TryParse(string dec, out BigInt result)
		{
			result = Zero;
			if (dec.Length == 0)
				return true;
			bool sign = dec[0] == '-';
			for (int i = (sign ? 1 : 0); i < dec.Length; i++)
			{
				char c = dec[i];
				if (c < '0' || c > '9')
					return false;
				result = result * 10 + (byte)(c - '0') * One;
			}
			if (sign)
				result = -result;
			return true;
		}

		public static BigInt Parse(string dec)
		{
			BigInt result;
			if (TryParse(dec, out result))
				return result;
			throw new ArgumentException("The string has incorrect format.");
		}

		public static bool operator ==(BigInt left, BigInt right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(BigInt left, BigInt right)
		{
			return !left.Equals(right);
		}

		public static BigInt operator -(BigInt obj)
		{
			return new BigInt(!obj._sign, (byte[])obj._bytes.Clone());
		}

		public static BigInt operator +(BigInt left, BigInt right)
		{
			if(left._sign == right._sign)
			{
				return new BigInt(left._sign, BigNumberUtils.Add(left._bytes, right._bytes, Math.Max(left._size, right._size)));
			}
			else
			{
				byte[] bytes = BigNumberUtils.Add(left._bytes, BigNumberUtils.GetComplement(right._bytes, left._size), Math.Max(left._size, right._size), 1);
				bool flag = bytes[bytes.Length - 1] > 0;
				if (flag)
				{
					bytes[bytes.Length - 1] = 0;
					return new BigInt(left._sign, bytes);
				}
				bytes[bytes.Length - 1] = 255;
				bytes[0] -= 1;
				return new BigInt(right._sign, BigNumberUtils.GetComplement(bytes));
			}
		}

		public static BigInt operator -(BigInt left, BigInt right)
		{
			return left + (-right);
		}

		public static BigInt operator *(BigInt left, BigInt right)
		{
			return new BigInt(left._sign != right._sign, BigNumberUtils.Mul(left._bytes, right._bytes));
		}

		public static BigInt operator *(BigInt left, byte right)
		{
			byte[] bytes = new byte[left._size + 1];
			Array.Copy(left._bytes, bytes, left._size);
			byte carry = 0;
			return new BigInt(left._sign, BigNumberUtils.Mul(bytes, right, ref carry));
		}

		public static BigInt operator *(byte left, BigInt right)
		{
			return right * left;
		}
	}
}