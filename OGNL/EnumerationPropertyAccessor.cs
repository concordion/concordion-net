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
///Implementation of PropertyAccessor that provides "property" reference to
///"nextElement" (aliases to "next" also) and "hasMoreElements" (also aliased
///to "hasNext").
///</summary>
///@author Luke Blanshard (blanshlu@netscape.net)
///@author Drew Davidson (drew@ognl.org)
///
public class EnumerationPropertyAccessor : ObjectPropertyAccessor , PropertyAccessor 
	// This is here to make javadoc show this class as an implementor
{
	/// <summary>
	/// When you call to "next" or "nextElement", 
	/// a MoveNext () will be call first to avoid Execption.
	/// So, DO NOT CALL THE "hasNext".
	/// </summary>
	/// <param name="context"></param>
	/// <param name="target"></param>
	/// <param name="name"></param>
	/// <returns></returns>
    public override object getProperty( IDictionary context, object target, object name ) // throws OgnlException
    {
        object      result;
        IEnumerator e = (IEnumerator) target;

        if ( name is string ) {
            if (name.Equals("next") || name.Equals("nextElement")) {
				// todo: try to move next first??
				e.MoveNext () ;
                result = e.Current;
            } else {
                if (name.Equals("hasNext") || name.Equals("hasMoreElements")) {
                    result = e.MoveNext() ; // ? Boolean.TRUE : Boolean.FALSE;
                } else {
                    result = base.getProperty( context, target, name );
                }
            }
        } else {
            result = base.getProperty(context, target, name);
        }
        return result;
    }

    public override void setProperty( IDictionary context, object target, object name, object value ) // throws OgnlException
    {
        throw new ArgumentException( "can't set property " + name + " on Enumeration" );
    }
}
}
