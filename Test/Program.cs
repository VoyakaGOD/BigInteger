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

			for (int i = 0; i < 15; i++)
			{
				var arr = CreateByteArray(1, 5, rand);
				Console.WriteLine(Convert(arr) + " -> " + BigNumberUtils.ToHex(arr));
			}

			Console.WriteLine("----------------------------------------------------------------------");

			for (int i = 0; i < 15; i++)
			{
				string str = BigNumberUtils.ToHex(CreateByteArray(1, 5, rand));
				Console.WriteLine(str + " -> " + Convert(BigNumberUtils.FromHex(str)));
			}

			Console.WriteLine("----------------------------------------------------------------------");

			for (int i = 0; i < 15; i++)
			{
				var a = CreateByteArray(1, 5, rand);
				var b = CreateByteArray(1, 5, rand);
				byte c = 0;
				Console.WriteLine(BigNumberUtils.ToHex(a) + " + " + BigNumberUtils.ToHex(b) + " = " + BigNumberUtils.ToHex(BigNumberUtils.Add(a, b, ref c)));
			}

			Console.WriteLine("----------------------------------------------------------------------");

			for (int i = 0; i < 15; i++)
			{
				var a = CreateByteArray(1, 5, rand);
				var b = CreateByteArray(1, 5, rand);
				Console.WriteLine(BigNumberUtils.ToHex(a) + "*" + BigNumberUtils.ToHex(b) + "=" + BigNumberUtils.ToHex(BigNumberUtils.Mul(a, b)));
			}

			/*for (int i = 0; i < 15; i++)
			{
				byte a = (byte)rand.Next(256);
				byte b = (byte)rand.Next(256);
				byte c = 0;
				Console.WriteLine(a + "+" + b + "=(" + BigNumberUtils.Add(a, b, ref c) + "," + c + ")");
			}
			Console.WriteLine("----------------------------------------------------------------------");
			for (int i = 0; i < 15; i++)
			{
				byte a = (byte)rand.Next(256);
				byte b = (byte)rand.Next(256);
				byte c = 0;
				Console.WriteLine(a + "*" + b + "=(" + BigNumberUtils.Mul(a, b, ref c) + "," + c + ")");
			}
			Console.WriteLine("----------------------------------------------------------------------");
			for (int i = 0; i < 15; i++)
			{
				int a = rand.Next();
				byte b = (byte)rand.Next(256);
				byte c = 0;
				Console.WriteLine(a + "*" + b + "=(" + BitConverter.ToUInt32(BigNumberUtils.Mul(BitConverter.GetBytes(a), b, ref c), 0) + "," + c + ")");
			}
			Console.WriteLine("----------------------------------------------------------------------");
			for (int i = 0; i < 15; i++)
			{
				int a = rand.Next();
				int b = rand.Next();
				Console.WriteLine(a + "*" + b + "=" + BitConverter.ToUInt64(BigNumberUtils.Mul(BitConverter.GetBytes(a), BitConverter.GetBytes(b)), 0));
			}
			Console.WriteLine(BigNumberUtils.ToHex(BigNumberUtils.Mul(new byte[] { 1, 0 }, new byte[] { 1, 0 })));*/
			Console.ReadLine();
		}
	}
}
