using System.Text ;

using java ;
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
class ASTConst : SimpleNode
{
    private object value;

    public ASTConst(int id) : base(id){
        ;
    }

    public ASTConst(OgnlParser p, int id) :  base(p, id){
       ;
    }

      /** Called from parser actions. */
    internal void setValue( object value ) {
        this.value = value;
    }

    public object getValue() {
        return value;
    }

    protected override object getValueBody( OgnlContext context, object source ) {
        return this.value;
    }

    public override bool isNodeConstant( OgnlContext context ) // throws OgnlException
    {
        return true;
    }

    public string getEscapedChar(char ch)
    {
        string          result;

        switch(ch) {
            case '\b':
                result = "\b";
                break;
            case '\t':
                result = "\\t";
                break;
            case '\n':
                result = "\\n";
                break;
            case '\f':
                result = "\\f";
                break;
            case '\r':
                result = "\\r";
                break;
            case '\"':
                result = "\\\"";
                break;
            case '\'':
                result = "\\\'";
                break;
            case '\\':
                result = "\\\\";
                break;
            default:
			// TODO: What's ISO Control. 
                if (Util.IsISOControl(ch) || (ch > 255)) {
                    string      hc = ((int)ch).ToString ("X") ; // Integer.ToString((int)ch, 16);
                    int         hcl = hc.Length;

                    result = "\\u";
                    if (hcl < 4) {
                        if (hcl == 3) {
                            result = result + "0";
                        } else {
                            if (hcl == 2) {
                                result = result + "00";
                            } else {
                                result = result + "000";
                            }
                        }
                    }

                    result = result + hc;
                } else {
					result = new string(new char[] {ch}) ; // new string((char)ch + "");
                }
                break;
        }
        return result;
    }

    public string getEscapedString(string value)
    {
        StringBuilder        result = new StringBuilder();

        for (int i = 0, icount = value.Length; i < icount; i++) {
            result.Append(getEscapedChar(value [i]));
        }
        return result.ToString ();
    }

    public override string ToString()
    {
        string      result;

        if (value == null) {
            result = "null";
        } else {
            if (value is string) {
                result = '\"' + getEscapedString(value.ToString()) + '\"';
            } else {
                if (value is char) {
                    result = '\'' + getEscapedChar((char)value) + '\'';
                } else {
                    result = value.ToString();
                    if (value is long) {
                        result = result + "L";
                    } else {
                        if (value is decimal) {
                            result = result + "B";
                        } else {
							// TODO: BigInteger, Ignore
                            if (false/*value is BigInteger*/) {
                                result = result + "H";
                            } else {
                                if (value is Node) {
                                    result = ":[ " + result + " ]";
                                }
                            }
                        }
                    }
                }
            }
        }
        return result;
    }
}
}
