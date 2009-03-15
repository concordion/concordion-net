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
	///<summary>
	///Default class resolution.  Uses Type.GetType() to look up classes by name.
	///It also looks in the "System" package if the class named does not give
	///a package specifier, allowing easier usage of these classes.
	///</summary>
	///<remarks>
	///You can specify Full assamblly class name as parameter. Under constraint of OGNL syntax, 
	///the full name in following form: <c>AssambllyName.namespace.className</c>.
	///</remarks>
	///@author Luke Blanshard (blanshlu@netscape.net)
	///@author Drew Davidson (drew@ognl.org)
	///
	public class DefaultClassResolver : ClassResolver
	{
		IDictionary classes = new Hashtable (101) ;

		static string[] DEFAULT_DLL_NAMES =
			{
				"System", // System.dll
			} ;

		public DefaultClassResolver ()
		{
		}

		public Type classForName (string className, IDictionary context) // throws ClassNotFoundException
		{
			Type result = null ;

			if ((result = (Type) classes [className]) == null)
			{
				result = Type.GetType (className) ;
				if (result == null)
				{
					int index = className.IndexOf ('.') ;
					if (index <= 0)
					{
						// TODO; Use System instead of java.lang
						result = Type.GetType ("System." + className) ;
						classes ["System." + className] = result ;
					}
					else
						// try to resolve with AssemblyQualifiedName 
						if (index < className.Length - 1)
						{
							string assemblyName = className.Substring (0, index) ;
							string clsName = className.Substring (index + 1) ;
							result = Type.GetType (clsName + "," + assemblyName) ;
						}

					// use default DLL Names 
					if (result == null)
					{
						for (int i = 0; i < DEFAULT_DLL_NAMES.Length; i++)
						{
							string assemblyName = DEFAULT_DLL_NAMES [i] ;
							result = Type.GetType (className + "," + assemblyName) ;
							if (result != null)
								break ;
						}

					}

				}
				if (result == null)
					throw new ArgumentException ("This Class [" + className + "] Not Found.", "className") ;
				classes [className] = result ;
			}
			return result ;
		}
	}
}