using System ;
using System.Reflection ;
using System.Security ;
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
/**
 * @author Luke Blanshard (blanshlu@netscape.net)
 * @author Drew Davidson (drew@ognl.org)
 */
class ASTStaticField : SimpleNode
{
    private string className;
    private string fieldName;

    public ASTStaticField(int id) : base(id){
        
    }

    public ASTStaticField(OgnlParser p, int id) : base(p, id){
        
    }

      /** Called from parser action. */

	internal void init( string className, string fieldName ) {
        this.className = className;
        this.fieldName = fieldName;
    }

    protected override object getValueBody( OgnlContext context, object source ) // throws OgnlException
    {
        return OgnlRuntime.getStaticField( context, className, fieldName );
    }

    public override bool isNodeConstant( OgnlContext context ) // throws OgnlException
    {
        bool     result = false;
    	Exception   reason = null;

        try {
            Type       c = OgnlRuntime.classForName(context, className);

            /*
                Check for virtual static field "class"; this cannot interfere with
                normal static fields because it is a reserved word.  It is considered
                constant.
            */
            if (fieldName.Equals("class")) {
                result = true;
            } else {
            	FieldInfo   f = c.GetField(fieldName);
				if (f == null)
				{
					// try to load Property 
					PropertyInfo p = c.GetProperty (fieldName) ;
					if (p == null)
						throw new MissingFieldException ("Field or Property " + fieldName + " of class " + className + " is not found.") ;
					else
					if (! p.GetAccessors () [0].IsStatic)
						throw new MissingFieldException ("Property " + fieldName + " of class " + className + " is not static.") ;
						// Property can't be constant.
					else
						return false ;
				}
				else
                if (!f.IsStatic) {
                    throw new OgnlException( "Field " + fieldName + " of class " + className + " is not static" );
                }

                result = f.IsLiteral;
            }
        }   catch (TypeLoadException e)    { reason = e; }
            catch (MissingFieldException e)      { reason = e; }
            catch (SecurityException e)         { reason = e; }

        if (reason != null) {
            throw new OgnlException( "Could not get static field " + fieldName + " from class " + className, reason );
        }
        return result;
    }

    public override string ToString()
    {
        return "@" + className + "@" + fieldName;
    }
}
}
