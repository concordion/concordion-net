using System ;
using System.Collections ;
//--------------------------------------------------------------------------
//	Copyright (c) 1998-2004, Drew Davidson and Luke Blanshard
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
	///<summary>
	///Implementation of PropertyAccessor that uses numbers and dynamic subscripts as
	///properties to index into Java arrays.
	///</summary>
	///@author Luke Blanshard (blanshlu@netscape.net)
	///@author Drew Davidson (drew@ognl.org)
	///
	public class ArrayPropertyAccessor : ObjectPropertyAccessor
		// This is here to make javadoc show this class as an implementor
	{
		/// <summary>
		/// Specific property is: length.
		/// </summary>
		/// <returns></returns>
		public override object getProperty (IDictionary context, object target, object name) // throws OgnlException
		{
			object result = null ;

			if (name is string)
			{
				if (name.Equals ("length"))
				{
					result = ((Array) target).GetLength (0) ;
				}
				else
				{
					result = base.getProperty (context, target, name) ;
				}
			}
			else
			{
				object index = name ;

				if (index is DynamicSubscript)
				{
					int len = ((Array) target).GetLength (0) ;

					switch (((DynamicSubscript) index).getFlag ())
					{
					case DynamicSubscript.ALL:
						result = Array.CreateInstance (target.GetType ().GetElementType (), len) ;
						Array.Copy ((Array) target, 0, (Array) result, 0, len) ;
						break ;
					case DynamicSubscript.FIRST:
						index = ((len > 0) ? 0 : -1) ;
						break ;
					case DynamicSubscript.MID:
						index = ((len > 0) ? (len / 2) : -1) ;
						break ;
					case DynamicSubscript.LAST:
						index = ((len > 0) ? (len - 1) : -1) ;
						break ;
					}
				}
				if (result == null)
				{
					if (index is ValueType)
					{
						int i = Convert.ToInt32 (index) ;

						result = (i >= 0) ? ((Array) target).GetValue (i) : null ;
					}
					else
					{
						throw new NoSuchPropertyException (target, index) ;
					}
				}
			}
			return result ;
		}

		public override void setProperty (IDictionary context, object target, object name, object value) // throws OgnlException
		{
			object index = name ;
			bool isNumber = (index is ValueType) ;

			if (isNumber || (index is DynamicSubscript))
			{
				TypeConverter converter = ((OgnlContext) context).getTypeConverter () ;
				object convertedValue ;

				convertedValue = converter.convertValue (context, target, null, name.ToString (), value, target.GetType ().GetElementType ()) ;
				if (isNumber)
				{
					int i = Convert.ToInt32 (index) ;

					if (i >= 0)
					{
						((Array) target).SetValue (convertedValue, i) ;
					}
				}
				else
				{
					int len = ((Array) target).GetLength (0) ;

					switch (((DynamicSubscript) index).getFlag ())
					{
					case DynamicSubscript.ALL:
						Array.Copy ((Array) target, 0, (Array) convertedValue, 0, len) ;
						return ;
					case DynamicSubscript.FIRST:
						index = ((len > 0) ? 0 : -1) ;
						break ;
					case DynamicSubscript.MID:
						index = ((len > 0) ? (len / 2) : -1) ;
						break ;
					case DynamicSubscript.LAST:
						index = ((len > 0) ? (len - 1) : -1) ;
						break ;
					}
				}
			}
			else
			{
				if (name is string)
				{
					base.setProperty (context, target, name, value) ;
				}
				else
				{
					throw new NoSuchPropertyException (target, index) ;
				}
			}
		}
	}
}