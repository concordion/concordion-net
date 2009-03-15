using System ;

namespace ognl
{
	///<summary>
	///Copy from java.beans
	///Thrown when an exception happens during Introspection.
	///</summary>
	///<remarks>
	///Typical causes include not being able to map a string class name
	///to a Class object, not being able to resolve a string method name,
	///or specifying a method name that has the wrong type signature for
	///its intended use.
	///</remarks>
	public class IntrospectionException : Exception
	{
		///<summary>
		///Constructs an <code>IntrospectionException</code> with a 
		///detailed message.
		///</summary>
		///<param name="mess">Descriptive message</param>
		///
		public IntrospectionException (String mess) : base (mess)
		{
		}
	}
}