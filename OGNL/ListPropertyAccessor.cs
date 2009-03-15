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
	///properties to index into Lists.
	///</summary>
	///@author Luke Blanshard (blanshlu@netscape.net)
	///@author Drew Davidson (drew@ognl.org)
	public class ListPropertyAccessor : ObjectPropertyAccessor, PropertyAccessor
		// This is here to make javadoc show this class as an implementor
	{
		public override object getProperty (IDictionary context, object target, object name) // throws OgnlException
		{
			IList list = (IList) target ;

			if (name is string)
			{
				object result ;

				if (name.Equals ("size"))
				{
					result = list.Count ;
				}
				else if (name.Equals ("iterator"))
				{
					result = list.GetEnumerator () ;
				}
				else if (name.Equals ("isEmpty"))
				{
					result = list.Count == 0 ; // ? Boolean.TRUE : Boolean.FALSE;
				}
				else
				{
					result = base.getProperty (context, target, name) ;
				}

				return result ;
			}

			if (name is ValueType)
				return list [Convert.ToInt32 (name)] ;

			if (name is DynamicSubscript)
			{
				int len = list.Count ;
				switch (((DynamicSubscript) name).getFlag ())
				{
				case DynamicSubscript.FIRST:
					return len > 0 ? list [0] : null ;
				case DynamicSubscript.MID:
					return len > 0 ? list [len / 2] : null ;
				case DynamicSubscript.LAST:
					return len > 0 ? list [len - 1] : null ;
				case DynamicSubscript.ALL:
					return new ArrayList (list) ;
				}
			}

			throw new NoSuchPropertyException (target, name) ;
		}

		public override void setProperty (IDictionary context, object target, object name, object value) // throws OgnlException
		{
			if (name is string)
			{
				base.setProperty (context, target, name, value) ;
				return ;
			}

			IList list = (IList) target ;

			if (name is ValueType)
			{
				list [Convert.ToInt32 (name)] = value ;
				return ;
			}

			if (name is DynamicSubscript)
			{
				int len = list.Count ;
				switch (((DynamicSubscript) name).getFlag ())
				{
				case DynamicSubscript.FIRST:
					if (len > 0) list [0] = value ;
					return ;
				case DynamicSubscript.MID:
					if (len > 0) list [len / 2] = value ;
					return ;
				case DynamicSubscript.LAST:
					if (len > 0) list [len - 1] = value ;
					return ;
				case DynamicSubscript.ALL:
					{
						if (!(value is IEnumerable) &&
							! (value is IEnumerator))
							throw new OgnlException ("Value must be a collection") ;
						list.Clear () ;
						IEnumerator e = value is IEnumerator ?
							(IEnumerator) value :
							((IEnumerable) value).GetEnumerator () ;

						while (e.MoveNext ())
							list.Add (e.Current) ;
						return ;
					}
				}
			}

			throw new NoSuchPropertyException (target, name) ;
		}
	}
}