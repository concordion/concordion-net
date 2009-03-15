using System;
using System.Reflection ;

namespace java
{
	/// <summary>
	/// BeanPropertyDescriptor 的摘要说明。
	/// </summary>
	public class BeanPropertyDescriptor : PropertyDescriptor
	{
		string name ;
		Type propertyType ;
		MethodInfo reader ;
		MethodInfo writer ;

		public BeanPropertyDescriptor (string name, Type propertyType, MethodInfo reader, MethodInfo writer)
		{
			this.name = name ;
			this.propertyType = propertyType ;
			this.reader = reader ;
			this.writer = writer ;
		}

		public override MethodInfo getReadMethod ()
		{
			return reader;
		}

		public override string getName ()
		{
			return name;
		}

		public override MethodInfo getWriteMethod ()
		{
			return writer ;
		}

		public override Type getPropertyType ()
		{
			return propertyType ;
		}
	}
}
