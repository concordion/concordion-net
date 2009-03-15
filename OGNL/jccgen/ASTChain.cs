using System ;
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
class ASTChain : SimpleNode
{
    public ASTChain(int id) : base(id) {
        ;
    }

    public ASTChain(OgnlParser p, int id) :base(p, id) {
        ;
    }

    public override void jjtClose() {
        flattenTree();
    }

    protected override object getValueBody( OgnlContext context, object source ) // throws OgnlException
    {
        object      result = source;

        for ( int i = 0, ilast = children.Length - 1; i <= ilast; ++i ) {
            bool         handled = false;

            if (i < ilast) {
                if (children[i] is ASTProperty) {
                    ASTProperty     propertyNode = (ASTProperty)children[i];
                    int             indexType = propertyNode.getIndexedPropertyType(context, result);

                    if ((indexType != OgnlRuntime.INDEXED_PROPERTY_NONE) && (children[i + 1] is ASTProperty)) {
                        ASTProperty     indexNode = (ASTProperty)children[i + 1];

                        if (indexNode.isIndexedAccess()) {
                            object      index = indexNode.getProperty(context, result);

                            if (index is DynamicSubscript) {
                                if (indexType == OgnlRuntime.INDEXED_PROPERTY_INT) {
                                    object      array = propertyNode.getValue(context, result);
                                    int         len = ((Array) array).Length;

                                    switch (((DynamicSubscript)index).getFlag()) {
                                        case DynamicSubscript.ALL:
                                            result = Array.CreateInstance( array.GetType().GetElementType(), len );
                                            Array.Copy( (Array)array, 0, (Array)result, 0, len );
                                            handled = true;
                                            i++;
                                            break;
                                        case DynamicSubscript.FIRST:
                                            index = ((len > 0) ? 0 : -1);
                                            break;
                                        case DynamicSubscript.MID:
                                            index = ((len > 0) ? (len / 2) : -1);
                                            break;
                                        case DynamicSubscript.LAST:
                                            index = ((len > 0) ? (len - 1) : -1);
                                            break;
                                    }
                                } else {
                                    if (indexType == OgnlRuntime.INDEXED_PROPERTY_OBJECT) {
                                        throw new OgnlException("DynamicSubscript '" + indexNode + "' not allowed for object indexed property '" + propertyNode + "'");
                                    }
                                }
                            }
                            if (!handled) {
                                result = OgnlRuntime.getIndexedProperty(context, result, propertyNode.getProperty(context, result).ToString(), index);
                                handled = true;
                                i++;
                            }
                        }
                    }
                }
            }
            if (!handled) {
                result = children[i].getValue( context, result );
            }
        }
        return result;
    }

    protected override void setValueBody( OgnlContext context, object target, object value ) // throws OgnlException
    {
        bool         handled = false;

        for ( int i = 0, ilast = children.Length - 2; i <= ilast; ++i ) {
            if (i == ilast) {
                if (children[i] is ASTProperty) {
                    ASTProperty     propertyNode = (ASTProperty)children[i];
                    int             indexType = propertyNode.getIndexedPropertyType(context, target);

                    if ((indexType != OgnlRuntime.INDEXED_PROPERTY_NONE) && (children[i + 1] is ASTProperty)) {
                        ASTProperty     indexNode = (ASTProperty)children[i + 1];

                        if (indexNode.isIndexedAccess()) {
                            object      index = indexNode.getProperty(context, target);

                            if (index is DynamicSubscript) {
                                if (indexType == OgnlRuntime.INDEXED_PROPERTY_INT) {
                                    object      array = propertyNode.getValue(context, target);
                                    int         len = ((Array) array).Length;

                                    switch (((DynamicSubscript)index).getFlag()) {
                                        case DynamicSubscript.ALL:
                                            Array.Copy((Array)target, 0, (Array)value, 0, len);
                                            handled = true;
                                            i++;
                                            break;
                                        case DynamicSubscript.FIRST:
                                            index = ((len > 0) ? 0 : -1);
                                            break;
                                        case DynamicSubscript.MID:
                                            index = ((len > 0) ? (len / 2) : -1);
                                            break;
                                        case DynamicSubscript.LAST:
                                            index = ((len > 0) ? (len - 1) : -1);
                                            break;
                                    }
                                } else {
                                    if (indexType == OgnlRuntime.INDEXED_PROPERTY_OBJECT) {
                                        throw new OgnlException("DynamicSubscript '" + indexNode + "' not allowed for object indexed property '" + propertyNode + "'");
                                    }
                                }
                            }
                            if (!handled) {
                                OgnlRuntime.setIndexedProperty(context, target, propertyNode.getProperty(context, target).ToString(), index, value);
                                handled = true;
                                i++;
                            }
                        }
                    }
                }
            }
            if (!handled) {
                target = children[i].getValue( context, target );
            }
        }
        if (!handled) {
            children[children.Length - 1].setValue( context, target, value );
        }
    }

    public override bool isSimpleNavigationChain( OgnlContext context ) // throws OgnlException
    {
        bool     result = false;

        if ((children != null) && (children.Length > 0)) {
            result = true;
            for (int i = 0; result && (i < children.Length); i++) {
                if (children[i] is SimpleNode) {
                    result = ((SimpleNode)children[i]).isSimpleProperty(context);
                } else {
                    result = false;
                }
            }
        }
        return result;
    }

    public override string ToString()
    {
        string      result = "";

        if ((children != null) && (children.Length > 0)) {
            for (int i = 0; i < children.Length; i++) {
                if (i > 0) {
                    if (!(children[i] is ASTProperty) || !((ASTProperty)children[i]).isIndexedAccess()) {
                        result = result + ".";
                    }
                }
                result += children[i].ToString();
            }
        }
        return result;
    }
}
}
