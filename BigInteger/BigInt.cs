using System;
using System.Collections.Generic;

namespace BigInteger
{
	public sealed class BigInt : IComparable, IComparable<BigInt>, IEquatable<BigInt>
	{
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

		public override string ToString()
		{
			throw new NotImplementedException();
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
				return new BigInt(false, new byte[0]);

			bool sign = (hex[0] == '-');
			return new BigInt(sign, BigNumberUtils.FromHex(hex.Substring(sign ? 1 : 0)));
		}

		public static bool TryParse(string str, out BigInt result)
		{
			throw new NotImplementedException();
		}

		public static BigInt Parse(string str)
		{
			BigInt result;
			if (TryParse(str, out result))
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
				return new BigInt(left._sign, BigNumberUtils.Add(left._bytes, right._bytes));
			}
			else
			{
				byte[] bytes = BigNumberUtils.Add(left._bytes, BigNumberUtils.GetComplement(right._bytes, left._size), 1);
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