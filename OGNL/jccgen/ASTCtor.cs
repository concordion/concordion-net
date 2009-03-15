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


/**
 * @author Luke Blanshard (blanshlu@netscape.net)
 * @author Drew Davidson (drew@ognl.org)
 */
class ASTCtor : SimpleNode
{
    private string      className;
    private bool     isArray;

    public ASTCtor(int id): base(id) {
       ;
    }

    public ASTCtor(OgnlParser p, int id) :base(p, id){
        ;
    }

      /** Called from parser action. */
    internal void setClassName( string className ) {
        this.className = className;
    }

    internal void setArray(bool value) {
        isArray = value;
    }

    protected override object getValueBody( OgnlContext context, object source ) // throws OgnlException
    {
        object      result,
                    root = context.getRoot();
        int         count = jjtGetNumChildren();
        object[]    args = OgnlRuntime.getObjectArrayPool().create(count);

        try {
            for ( int i=0; i < count; ++i ) {
                args[i] = children[i].getValue(context, root);
            }
            if (isArray) {
                if (args.Length == 1) {
                    try {
                    	Type       componentClass = OgnlRuntime.classForName(context, className);
                        IList        sourceList = null;
                        int         size;

                        if (args[0] is IList) {
                            sourceList = (IList)args[0];
                            size = sourceList.Count;
                        } else {
                            size = (int)OgnlOps.longValue(args[0]);
                        }
                        result = Array.CreateInstance(componentClass, size);
                        if (sourceList != null) {
                            TypeConverter   converter = context.getTypeConverter();

                            for (int i = 0, icount = sourceList.Count; i < icount; i++) {
                                object      o = sourceList [i];

                                if ((o == null) || componentClass.IsInstanceOfType(o)) {
                                    ((Array) result).SetValue (o , i);
                                } else {
									((Array) result).SetValue (converter.convertValue(context, null, null, null, o, componentClass) , i);
                                }
                            }
                        }
                    } catch (TypeLoadException ex) {
                        throw new OgnlException("array component class '" + className + "' not found", ex);
                    }
                } else {
                    throw new OgnlException("only expect array size or fixed initializer list");
                }
            } else {
                result = OgnlRuntime.callConstructor( context, className, args );
            }

            return result;
        } finally {
            OgnlRuntime.getObjectArrayPool().recycle(args);
        }
    }

    public override string ToString()
    {
        string      result = "new " + className;

        if (isArray) {
            if (children[0] is ASTConst) {
                result = result + "[" + children[0] + "]";
            } else {
                result = result + "[] " + children[0];
            }
        } else {
            result = result + "(";
            if ((children != null) && (children.Length > 0)) {
                for (int i = 0; i < children.Length; i++) {
                    if (i > 0) {
                        result = result + ", ";
                    }
                    result = result + children[i];
                }
            }
            result = result + ")";
        }
        return result;
    }
}
}
