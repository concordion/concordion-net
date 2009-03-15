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
class ASTMap : SimpleNode
{
    private static Type    DEFAULT_MAP_CLASS;
    private string          className;

    static ASTMap () {
        /* Try to get LinkedHashMap; if older JDK than 1.4 use HashMap */
        /* // Just use Hahstable.
		try {
            DEFAULT_MAP_CLASS = Type.forName("java.util.LinkedHashMap");
        } catch (ClassNotFoundException ex) {
            DEFAULT_MAP_CLASS = HashMap.class;
        }*/
		DEFAULT_MAP_CLASS = typeof (Hashtable) ;
    }

    public ASTMap(int id) : base(id){
        
    }

    public ASTMap(OgnlParser p, int id) : base(p, id){
        
    }

    protected internal void setClassName(string value)
    {
        className = value;
    }

    protected override object getValueBody( OgnlContext context, object source ) // throws OgnlException
    {
        IDictionary answer;

        if (className == null) {
            try {
                answer = (IDictionary)DEFAULT_MAP_CLASS.GetConstructor (new Type[0]).Invoke (new object[0]);
            } catch (Exception ex) {
                /* This should never happen */
                throw new OgnlException("Default IDictionary class '" + DEFAULT_MAP_CLASS.Name + "' instantiation error", ex);
            }
        } else {
            try {
                answer = (IDictionary)OgnlRuntime.classForName(context, className).GetConstructor (new Type[0]).Invoke (new object[0]);
            } catch (Exception ex) {
                throw new OgnlException("IDictionary implementor '" + className + "' not found", ex);
            }
        }

        for ( int i=0; i < jjtGetNumChildren(); ++i ) {
            ASTKeyValue     kv = (ASTKeyValue)children[i];
            Node            k = kv.getKey(),
                            v = kv.getValue();

            answer.Add( k.getValue(context, source), (v == null) ? null : v.getValue(context, source) );
        }
        return answer;
    }

    public override string ToString()
    {
        string      result = "#";

        if (className != null) {
            result = result + "@" + className + "@";
        }
        result = result + "{ ";
        for ( int i = 0; i < jjtGetNumChildren(); ++i ) {
            ASTKeyValue     kv = (ASTKeyValue)children[i];

            if (i > 0) {
                result = result + ", ";
            }
            result = result + kv.getKey() + " : " + kv.getValue();
        }
        return result + " }";
    }
}
}
