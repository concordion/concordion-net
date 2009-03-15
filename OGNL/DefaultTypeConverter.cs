using System ;
using System.Collections ;
using System.Reflection ;
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
	///Default type conversion. Converts among numeric types and also strings.
	///</summary>
	///@author Luke Blanshard (blanshlu@netscape.net)
	///@author Drew Davidson (drew@ognl.org)
	///
	public class DefaultTypeConverter : TypeConverter
	{
		public DefaultTypeConverter ()
		{
		}

		/// <summary>
		/// Use <see cref="OgnlOps.convertValue"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="value"></param>
		/// <param name="toType"></param>
		/// <returns></returns>
		public virtual object convertValue (IDictionary context, object value, Type toType)
		{
			return OgnlOps.convertValue (value, toType) ;
		}

		/// <summary>
		/// A overload version.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="target"></param>
		/// <param name="member"></param>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <param name="toType"></param>
		/// <returns></returns>
		public virtual object convertValue (IDictionary context, object target, MemberInfo member, string propertyName, object value, Type toType)
		{
			return convertValue (context, value, toType) ;
		}
	}
}