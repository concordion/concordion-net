using System;
using System.Reflection ;

namespace java
{
	/// <summary>
	/// PropertyDescriptor 的摘要说明。
	/// </summary>
	public class PropertyDescriptor
	{
		protected PropertyInfo p ;

		protected PropertyDescriptor () {}
		public PropertyDescriptor (PropertyInfo p)
		{
			if (p == null)
				throw new ArgumentException("PropertyInfo is NULL!") ;
			this.p = p ;
		}

		public virtual MethodInfo getReadMethod ()
		{
			return p.GetGetMethod ();
		}

		public virtual string getName ()
		{
			return p.Name ;
		}

		public virtual MethodInfo getWriteMethod ()
		{
			return p.GetSetMethod ();
		}

		public virtual Type getPropertyType ()
		{
			return p.PropertyType ;
		}
	}
}
