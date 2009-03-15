using System;
using System.Collections ;

namespace java
{
	/// <summary>
	/// Long 的摘要说明。
	/// </summary>
	public class Util
	{
		private Util()
		{
			
		}

		public static long ParseLong(String s)
		{
			return ParseLong (s , 10) ;
		}
		public static long ParseLong(String s, int radix)
							
		{
			if (s == null) 
			{
				throw new NullReferenceException("null");
			}

			if (radix < 2) 
			{
				throw new FormatException("radix "+ radix +
					" less than 2");
			}
			if (radix > 16) 
			{
				throw new FormatException("radix "+ radix +
					" greater than 16");
			}

			long result = 0;
			bool negative = false;
			int i = 0, max = s.Length ;
			
			int digit;

			if (max > 0) 
			{
				if (s[0] == '-') 
				{
					negative = true;
					
					i++;
				} 
			
				if (i < max) 
				{
					digit = Digit (s[i++],radix);
					if (digit < 0) 
					{
						throw new FormatException (s);
					} 
					else 
					{
						result = -digit;
					}
				}
				while (i < max) 
				{
					// Accumulating negatively avoids surprises near MAX_VALUE
					digit = Digit(s [i++],radix);
					if (digit < 0) 
					{
						throw new FormatException(s);
					}
					
					result *= radix;
					
					result -= digit;
				}
			} 
			else 
			{
				throw new FormatException (s);
			}
			if (negative) 
			{
				if (i > 1) 
				{
					return result;
				} 
				else 
				{	/* Only got "-" */
					throw new FormatException(s);
				}
			} 
			else 
			{
				return -result;
			}
		}

		private static int Digit (char ch, int radix)
		{
			int d = 0 ;
			if (radix <= 10)
				d = ch - '0' ;
			else
			{
				if (ch >= '0' && ch <= '9')
					d = ch - '0' ;
				else
				if (ch >= 'a' && ch <='z')
					d = ch - 'a' + 10 ;
				else
				if (ch >= 'A' && ch <='Z')
					d = ch - 'A' + 10 ;
			}

			if (d >= radix)
				throw new ArgumentOutOfRangeException("digit" , ch , 
					"greate than radix [" + radix + "]") ;

			return d ;
		}

		public static bool IsISOControl(char ch) 
		{
			return (ch <= 0x009F) && ((ch <= 0x001F) || (ch >= 0x007F));
		}
		
		public static IList NCopies (int n , object o)
		{
			return new CopiesList (n , o) ;
		}

		public static bool Eq(Object o1, Object o2) 
		{
			return (o1==null ? o2==null : o1.Equals(o2));
		}

		public static IList asList (Array array)
		{
			ArrayList list = new ArrayList(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				object o = array.GetValue (i) ;
				list.Add (o) ;
			}
			return list;
		}

		public static void putAll (IDictionary source, IDictionary target)
		{
			if (source == null || target == null)
				return ;
			foreach (DictionaryEntry entry in source)
			{
				object key = entry.Key ;
				object value = entry.Value ;
				target [key] = value ;
			}
		}
	}

	internal class CopiesList : IList
	{
		private int n ;
		private object element ;
		public CopiesList (int n, object o)
		{
			if (n < 0)
				throw new ArgumentException("List length = " + n);
			this.n = n ;
			this.element = o ;
		}

		public void CopyTo (Array array, int index)
		{
			throw new NotImplementedException () ;
		}

		public int Count
		{
			get { return n ; }
		}

		public object SyncRoot
		{
			get { return this ; }
		}

		public int Add (object value)
		{
			throw new NotImplementedException () ;
		}

		public bool Contains (object value)
		{
			return n != 0 && Util.Eq(value, element) ;
		}

		public void Clear ()
		{
			throw new NotImplementedException () ;
		}

		public int IndexOf (object value)
		{
			if (Contains (value))
				return 0 ;
			else
				return -1 ;
		}

		public void Insert (int index, object value)
		{
			throw new NotImplementedException () ;
		}

		public void Remove (object value)
		{
			throw new NotImplementedException () ;
		}

		public void RemoveAt (int index)
		{
			throw new NotImplementedException () ;
		}

		public bool IsReadOnly
		{
			get { return true ; }
		}

		public bool IsFixedSize
		{
			get { return true ; }
		}

		public object this [int index]
		{
			get 
			{
				if (index<0 || index>=n)
					 throw new IndexOutOfRangeException("Index: "+index+
						   ", Size: "+n);
				return element; 
			}
			set { throw new NotImplementedException () ; }
		}

		public bool IsSynchronized
		{
			get { return true ; }
		}

		public IEnumerator GetEnumerator ()
		{
			return new CopiesEnumerator(n , element) ;
		}

		class CopiesEnumerator : IEnumerator
		{
			private int n ;
			private object element ;
			private int index = 0 ;
			public CopiesEnumerator (int n, object element)
			{
				this.n = n ;
				this.element = element ;
			}

			public bool MoveNext ()
			{
				return index < n ;
			}

			public void Reset ()
			{
				index = 0 ;
			}

			public object Current
			{
				get
				{
					index ++ ; 
					return element ;
				}
			}
		}
	}

}
