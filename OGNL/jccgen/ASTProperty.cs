using System.Reflection ;
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
class ASTProperty : SimpleNode
{
    private bool     indexedAccess = false;

    public ASTProperty(int id)
    : base(id){
        
    }

    public ASTProperty(OgnlParser p, int id)
    : base(p, id){
        
    }

    public void setIndexedAccess( bool value )
    {
        indexedAccess = value;
    }

    /**
        Returns true iff this property is itself an index reference.
     */
    public bool isIndexedAccess()
    {
        return indexedAccess;
    }

    /**
        Returns true if this property is described by an IndexedPropertyDescriptor
        and that if followed by an index specifier it will call the index get/set
        methods rather than go through property accessors.
     */
    public int getIndexedPropertyType(OgnlContext context, object source) // throws OgnlException
    {
        if (!isIndexedAccess()) {
            object              property = getProperty(context, source);

            if (property is string) {
                return OgnlRuntime.getIndexedPropertyType(context, (source == null) ? null : source.GetType(), (string)property);
            }
        }
        return OgnlRuntime.INDEXED_PROPERTY_NONE;
    }

	public object getProperty( OgnlContext context, object source ) // throws OgnlException
	{
		return children[0].getValue( context, context.getRoot() );
	}

    protected override object getValueBody( OgnlContext context, object source ) // throws OgnlException
    {
		if (indexedAccess && children [0] is ASTSequence)
		{
			// As property [index1, index2]...
			// Use Indexer.
			object [] indexParameters = ((ASTSequence) children [0]).getValues (context, context.getRoot ()) ;
			
			/* return IndexerAccessor.getIndexerValue (source, indexParameters) ; */
			return OgnlRuntime.getIndxerValue (context, source , "Indexer" , indexParameters) ;
		}
        object      result ;
		
        object      property = getProperty(context, source);
        Node        indexSibling;

        result = OgnlRuntime.getProperty( context, source, property );
        if (result == null) {
            result = OgnlRuntime.getNullHandler(OgnlRuntime.getTargetClass(source)).nullPropertyValue(context, source, property);
        }
        return result;
    }

    protected override void setValueBody( OgnlContext context, object target, object value ) // throws OgnlException
    {
		if (indexedAccess && children [0] is ASTSequence)
		{
			// As property [index1, index2]...
			// Use Indexer.
			object [] indexParameters = ((ASTSequence) children [0]).getValues (context, context.getRoot ()) ;
			
			/*IndexerAccessor.setIndexerValue (target, value ,indexParameters) ;*/
			OgnlRuntime.setIndxerValue (context, target, "Indexer" , value, indexParameters) ;
			return ;
		}

        OgnlRuntime.setProperty( context, target, getProperty( context, target), value );
    }

    public override bool isNodeSimpleProperty( OgnlContext context ) // throws OgnlException
    {
        return (children != null) && (children.Length == 1) && ((SimpleNode)children[0]).isConstant(context);
    }

    public override string ToString()
    {
        string          result;

        if (isIndexedAccess()) {
            result = "[" + children[0] + "]";
        } else {
            result = ((ASTConst)children[0]).getValue().ToString();
        }
        return result;
    }
}
}
