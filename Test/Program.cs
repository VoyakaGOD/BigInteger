using System;
using System.Text;
using BigInteger;

namespace Test
{
	class Program
	{
		static byte[] CreateByteArray(int minSize, int maxSize, Random rand)
		{
			var result = new byte[rand.Next(minSize, maxSize)];
			for (int i = 0; i < result.Length; i++)
				result[i] = (byte)rand.Next(256);
			return result;
		}

		static string Convert(byte[] arr)
		{
			if (arr.Length == 0)
				return "[empty]";
			var result = new StringBuilder();
			result.Append("[" + arr[0]);
			for(int i = 1; i < arr.Length; i++)
				result.Append(", " + arr[i]);
			result.Append(']');
			return result.ToString();
		}

		static void Main(string[] args)
		{
			Random rand = new Random();
			BigInt[] bigInts = new BigInt[30];

			for (int i = 0; i < bigInts.Length; i++)
			{
				var arr = CreateByteArray(1, 5, rand);
				bigInts[i] = BigInt.FromHex(BigNumberUtils.ToHex(arr));
				Console.WriteLine(Convert(arr) + " -> " + bigInts[i].ToHex());
			}

			Console.WriteLine("----------------------------------------------------------------------");

			for (int i = 0; (i + 1) < bigInts.Length; i += 2)
			{
				var a = bigInts[i];
				var b = bigInts[i + 1];
				var c = a + b;
				Console.WriteLine(a.ToHex() + " + " + b.ToHex() + " = " + c.ToHex());
			}

			Console.WriteLine("----------------------------------------------------------------------");

			for (int i = 0; (i + 1) < bigInts.Length; i += 2)
			{
				var a = bigInts[i];
				var b = bigInts[i + 1];
				var c = a - b;
				Console.WriteLine(a.ToHex() + " - " + b.ToHex() + " = " + c.ToHex());
			}

			Console.WriteLine("----------------------------------------------------------------------");

			for (int i = 0; (i + 1) < bigInts.Length; i += 2)
			{
				var a = bigInts[i];
				var b = bigInts[i + 1];
				var c = a * b;
				Console.WriteLine(a.ToHex() + " * " + b.ToHex() + " = " + c.ToHex());
			}

			Console.WriteLine("----------------------------------------------------------------------");

			for (int i = 0; i < bigInts.Length; i++)
			{
				var a = bigInts[i];
				var c = a * 10;
				Console.WriteLine(a.ToHex() + " * 10 = " + c.ToHex());
			}

			Console.WriteLine("----------------------------------------------------------------------");

			Array.Sort(bigInts);
			for (int i = 0; i < bigInts.Length; i++)
				Console.WriteLine(bigInts[i].ToHex());

			Console.WriteLine("----------------------------------------------------------------------");

			for (int i = 0; i < 15; i++)
			{
				int val = rand.Next();
				Console.WriteLine(val + " - " + (val % 10) + " - " + BigNumberUtils.Mod10(BitConverter.GetBytes(val)));
			}

			Console.WriteLine("----------------------------------------------------------------------");

			for (int i = 0; i < 15; i++)
			{
				int val = rand.Next();
				int r;
				Console.Write(BigInt.FromBytes(false, BitConverter.GetBytes(val / 10)).ToHex() + " - ");
				Console.WriteLine(BigInt.FromBytes(false, BitConverter.GetBytes(val)).UnsignedDiv10(out r).ToHex());
			}

			Console.WriteLine("----------------------------------------------------------------------");

			while (true)
			{
				Console.Write("dec1:");
				var num1 = BigInt.Parse(Console.ReadLine());
				Console.Write("dec2:");
				var num2 = BigInt.Parse(Console.ReadLine());
				Console.WriteLine("+)" + (num1 + num2));
				Console.WriteLine("-)" + (num1 - num2));
				Console.WriteLine("*)" + (num1 * num2));
			}
		}
	}
}
