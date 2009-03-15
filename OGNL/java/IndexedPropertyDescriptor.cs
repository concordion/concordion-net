using System;
using System.Reflection ;

namespace java
{
	/// <summary>
	/// IndexedPropertyDescriptor 的摘要说明。
	/// </summary>
	public class IndexedPropertyDescriptor : PropertyDescriptor
	{
	
		public IndexedPropertyDescriptor (PropertyInfo p) : base (p)
		{
			
		}

		public MethodInfo getIndexedReadMethod ()
		{
			return p.GetGetMethod (false);
		}

		public MethodInfo getIndexedWriteMethod ()
		{
			return p.GetSetMethod (false);
		}
	}
}
