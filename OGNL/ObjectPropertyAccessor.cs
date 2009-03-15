using System ;
using System.Collections ;
//--------------------------------------------------------------------------
//	Copyright (c) 1998-2004, Drew Davidson ,  Luke Blanshard and Foxcoming
//  All rights reserved.
//
//	Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are
//  met:
//
//	Redistributions of source code must retain the above copyright notice,
//  this list of conditions and the following disclaimer.
//	Redistributions in binary form must reproduce the above copyright
//  notice, this list of conditions and the following disclaimer in the
//  documentation and/or other materials provided with the distribution.
//	Neither the name of the Drew Davidson nor the names of its contributors
//  may be used to endorse or promote products derived from this software
//  without specific prior written permission.
//
//	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
//  FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
//  COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
//  INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
//  BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS
//  OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED
//  AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
//  OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF
//  THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
//  DAMAGE.
//--------------------------------------------------------------------------

namespace ognl
{
	/// <summary>
	///Implementation of PropertyAccessor that uses reflection on the target object's class to
	///find a field or a pair of set/get methods with the given property name.
	///</summary>
	///@author Luke Blanshard (blanshlu@netscape.net)
	///@author Drew Davidson (drew@ognl.org)
	///
	public class ObjectPropertyAccessor : PropertyAccessor
	{
		///<summary>
		/// Returns OgnlRuntime.NotFound if the property does not exist.
		///</summary>
		public object getPossibleProperty (IDictionary context, object target, string name) // throws OgnlException
		{
			object result ;
			OgnlContext ognlContext = (OgnlContext) context ;

			try
			{
				if ((result = OgnlRuntime.getMethodValue (ognlContext, target, name, true)) == OgnlRuntime.NotFound)
				{
					result = OgnlRuntime.getFieldValue (ognlContext, target, name, true) ;
				}
			}
			catch (OgnlException ex)
			{
				throw ex ;
			}
			catch (Exception ex)
			{
				throw new OgnlException (name, ex) ;
			}
			return result ;
		}

		///<summary>
		/// Returns OgnlRuntime.NotFound if the property does not exist.
		///</summary>
		public object setPossibleProperty (IDictionary context, object target, string name, object value) // throws OgnlException
		{
			object result = null ;
			OgnlContext ognlContext = (OgnlContext) context ;

			try
			{
				if (!OgnlRuntime.setMethodValue (ognlContext, target, name, value, true))
				{
					result = OgnlRuntime.setFieldValue (ognlContext, target, name, value) ? null : OgnlRuntime.NotFound ;
				}
			}
			catch (OgnlException ex)
			{
				throw ex ;
			}
			catch (Exception ex)
			{
				throw new OgnlException (name, ex) ;
			}
			return result ;
		}

		public bool hasGetProperty (OgnlContext context, object target, object oname) // throws OgnlException
		{
			try
			{
				return OgnlRuntime.hasGetProperty (context, target, oname) ;
			}
			catch ( /*Introspection*/Exception ex)
			{
				throw new OgnlException ("checking if " + target + " has gettable property " + oname, ex) ;
			}
		}

		public bool hasGetProperty (IDictionary context, object target, object oname) // throws OgnlException
		{
			return hasGetProperty ((OgnlContext) context, target, oname) ;
		}

		public bool hasSetProperty (OgnlContext context, object target, object oname) // throws OgnlException
		{
			try
			{
				return OgnlRuntime.hasSetProperty (context, target, oname) ;
			}
			catch ( /*Introspection*/Exception ex)
			{
				throw new OgnlException ("checking if " + target + " has settable property " + oname, ex) ;
			}
		}

		public bool hasSetProperty (IDictionary context, object target, object oname) // throws OgnlException
		{
			return hasSetProperty ((OgnlContext) context, target, oname) ;
		}

		public virtual object getProperty (IDictionary context, object target, object oname) // throws OgnlException
		{
			object result = OgnlRuntime.NotFound ;
			Node currentNode = ((OgnlContext) context).getCurrentNode () ;

			bool indexedAccess = false ;

			if (currentNode == null)
			{
				throw new OgnlException ("node is null for '" + oname + "'") ;
			}
			if (!(currentNode is ASTProperty))
			{
				currentNode = currentNode.jjtGetParent () ;
			}
			if (currentNode is ASTProperty)
			{
				indexedAccess = ((ASTProperty) currentNode).isIndexedAccess () ;
			}
			if (((oname is string) && isPropertyName ((string) oname)) &&
				(! indexedAccess ||
					! OgnlRuntime.hasSetIndexer ((OgnlContext) context, target, target.GetType (), 1)))
			{
				result = javaGetProperty (oname, context, target) ;
			}
			else
			{
				// will use index Access property
				/*
			PropertyInfo indexer = IndexerAccessor.getIndexer (target, new object [] {oname}) ;
			if (indexer != null)
				result = indexer.GetValue (target , new object[] {oname}) ;
			else
				// throw new NoSuchPropertyException(target, oname.ToString ()) ;
				// TODO: support old java style property: [propertyName].
				result = javaGetProperty (oname, context, target) ;
			*/
				result = OgnlRuntime.getIndxerValue ((OgnlContext) context, target, oname, new object[] {oname}) ;
			}


			return result ;
		}

		object javaGetProperty (object oname, IDictionary context, object target)
		{
			object result ;
			string name = oname.ToString () ;

			if ((result = getPossibleProperty (context, target, name)) == OgnlRuntime.NotFound)
			{
				throw new NoSuchPropertyException (target, name) ;
			}
			return result ;
		}

		// Check Property Name .
		bool isPropertyName (string name)
		{
			bool result = name != null && name.Length > 1 &&
				((name [0] >= 'a' && name [0] <= 'z') ||
					(name [0] >= 'A' && name [0] <= 'Z') ||
					(name [0] == '_')) ;
			if (! result)
			{
				for (int i = 1; i < name.Length; i ++)
				{
					result = (name [0] >= 'a' && name [0] <= 'z') ||
						(name [0] >= 'A' && name [0] <= 'Z') ||
						(name [0] >= '0' && name [0] <= '9') ||
						name [0] == '_' ;
					if (! result)
						break ;
				}
			}
			return result ;

		}

		public virtual void setProperty (IDictionary context, object target, object oname, object value) // throws OgnlException
		{
			object result = OgnlRuntime.NotFound ;
			string name = oname.ToString () ;
			Node currentNode = ((OgnlContext) context).getCurrentNode () ;

			bool indexedAccess = false ;

			if (currentNode == null)
			{
				throw new OgnlException ("node is null for '" + oname + "'") ;
			}
			if (!(currentNode is ASTProperty))
			{
				currentNode = currentNode.jjtGetParent () ;
			}
			if (currentNode is ASTProperty)
			{
				indexedAccess = ((ASTProperty) currentNode).isIndexedAccess () ;
			}
			if (! indexedAccess || ! OgnlRuntime.hasSetIndexer ((OgnlContext) context, target, target.GetType (), 1))
			{
				if ((result = setPossibleProperty (context, target, name, value)) == OgnlRuntime.NotFound)
				{
					throw new NoSuchPropertyException (target, name) ;
				}
			}
			else
			{
				/*
			PropertyInfo indexer = IndexerAccessor.getIndexer (target, new object[] {oname}) ;
			if (indexer == null)
				throw new NoSuchPropertyException(target, name) ;

			indexer.SetValue (target , value , new object[] {oname}) ;
			*/
				OgnlRuntime.setIndxerValue ((OgnlContext) context, target, oname, value, new object[] {oname}) ;
			}
		}
	}
}