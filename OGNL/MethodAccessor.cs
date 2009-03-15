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
	///This interface defines methods for callinig methods in a target object.
	///Methods are broken up into static and instance methods for convenience.
	///indexes into the target object, which must be an array.
	///</summary>
	///@author Luke Blanshard (blanshlu@netscape.net)
	///@author Drew Davidson (drew@ognl.org)
	///
	public interface MethodAccessor
	{
		///<summary>
		/// Calls the static method named with the arguments given on the class given.
		/// </summary>
		/// <param name="context">expression context in which the method should be called</param>
		/// <param name="targetClass">the object in which the method exists</param> 
		/// <param name="methodName">  the name of the method</param>
		/// <param name="args">        the arguments to the method</param>
		/// <returns>result of calling the method</returns>                                             
		/// <exception href="MethodFailedException"> if there is an error calling the method</exception>  
		/// 
		object callStaticMethod (IDictionary context, Type targetClass, string methodName, object[] args) ; // throws MethodFailedException;

		///<summary>
		/// Calls the method named with the arguments given.
		/// </summary> 
		/// <param name="context">expression context in which the method should be called</param>
		/// <param name="target">the object in which the method exists</param>
		/// <param name="methodName">the name of the method</param>
		/// <param name="args">the arguments to the method</param>
		/// <returns>result of calling the method</returns>
		/// <exception href="MethodFailedException"> if there is an error calling the method</exception>
		/// 
		object callMethod (IDictionary context, object target, string methodName, object[] args) ; // throws MethodFailedException;
	}
}