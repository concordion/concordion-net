using System ;
using System.Collections ;
using System.IO ;
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
	///This class provides static methods for parsing and interpreting OGNL expressions.
	///</summary>
	///<example>
	///The simplest use of the Ognl class is to get the value of an expression from
	///an object, without extra context or pre-parsing.
	///
	///<code lang="C#">
	///using ognl;
	///
	///  try 
	///  {
	///      result = Ognl.getValue(expression, root);
	///  } catch (OgnlException ex) 
	///  {
	///     // Report error or recover
	///  }
	///</code>
	///
	///This will parse the expression given and evaluate it against the root object
	///given, returning the result.  If there is an error in the expression, such
	///as the property is not found, the exception is encapsulated into an
	///<see href="OgnlException"/>.
	///
	///<para>Other more sophisticated uses of Ognl can pre-parse expressions.  This
	///provides two advantages: in the case of user-supplied expressions it
	///allows you to catch parse errors before evaluation and it allows you to
	///cache parsed expressions into an AST for better speed during repeated use.
	///The pre-parsed expression is always returned as an <c>object</c>
	///to simplify use for programs that just wish to store the value for
	///repeated use and do not care that it is an AST.  If it does care
	///it can always safely cast the value to an <c>AST</c> type.</para>
	///
	///<para>The Ognl class also takes a <I>context map</I> as one of the parameters
	///to the set and get methods.  This allows you to put your own variables
	///into the available namespace for OGNL expressions.  The default context
	///contains only the <c>#root</c> and <c>#context</c> keys,
	///which are required to be present.  The <c>addDefaultContext(object, IDictionary)</c>
	///method will alter an existing <c>IDictionary</c> to put the defaults in.
	///Here is an example that shows how to extract the <c>documentName</c>
	///property out of the root object and append a string with the current user
	///name in parens:</para>
	///
	///<code lang="C#">
	///    private IDictionary	context = new HashMap();
	///
	///    public void setUserName(string value)
	///    {
	///        context.put("userName", value);
	///    }
	///
	///    try 
	///    {
	///       // get value using our own custom context map
	///       result = Ognl.getValue("documentName + \" (\" + ((#userName == null) ? \"&lt;nobody&gt;\" : #userName) + \")\"", context, root);
	///    } catch (OgnlException ex) 
	///    {
	///        // Report error or recover
	///    }
	///
	///</code>
	///</example>
	///
	///@author Luke Blanshard (blanshlu@netscape.net)
	///@author Drew Davidson (drew@ognl.org)
	///@version 27 June 1999
	///
	public abstract class Ognl
	{
		///<summary>
		///Parses the given OGNL expression and returns a tree representation of the
		///expression that can be used by <c>Ognl</c> static methods.
		///</summary>
		///<param name="expression">the OGNL expression to be parsed</param> 
		///<returns>a tree representation of the expression</returns> 
		///<exception cref="ExpressionSyntaxException">if the expression is malformed</exception>
		///<exception cref="OgnlException"> if there is a pathological environmental problem</exception>
		///
		public static object parseExpression (string expression) // throws OgnlException
		{
			try
			{
				OgnlParser parser = new OgnlParser (new StringReader (expression)) ;
				return parser.topLevelExpression () ;
			}
			catch (ParseException e)
			{
				throw new ExpressionSyntaxException (expression, e) ;
			}
			catch (TokenMgrError e)
			{
				throw new ExpressionSyntaxException (expression, e) ;
			}
		}

		///<summary>
		///Creates and returns a new standard naming context for evaluating an OGNL
		///expression.
		///</summary>
		///<param name="root">the root of the object graph</param>
		///<returns>
		///a new IDictionary with the keys <c>root</c> and <c>context</c>
		///set appropriately</returns>
		///
		public static IDictionary createDefaultContext (object root)
		{
			return addDefaultContext (root, null, null, null, new OgnlContext ()) ;
		}

		///<summary>
		///Creates and returns a new standard naming context for evaluating an OGNL
		///expression.
		///</summary>
		///<param name="root">the root of the object graph</param>
		///<returns>
		///a new IDictionary with the keys <c>root</c> and <c>context</c>
		///set appropriately
		///</returns>
		///
		public static IDictionary createDefaultContext (object root, ClassResolver classResolver)
		{
			return addDefaultContext (root, classResolver, null, null, new OgnlContext ()) ;
		}

		///<summary>
		///Creates and returns a new standard naming context for evaluating an OGNL
		///expression.
		///</summary>
		///<param name="root">the root of the object graph</param>
		///<returns>
		///a new IDictionary with the keys <c>root</c> and <c>context</c>
		///set appropriately
		///</returns>
		///
		public static IDictionary createDefaultContext (object root, ClassResolver classResolver, TypeConverter converter)
		{
			return addDefaultContext (root, classResolver, converter, null, new OgnlContext ()) ;
		}

		///<summary>
		///Creates and returns a new standard naming context for evaluating an OGNL
		///expression.
		///</summary>
		///<param name="root">the root of the object graph</param>
		///<returns>
		///a new IDictionary with the keys <c>root</c> and <c>context</c>
		///set appropriately
		///</returns>
		///
		public static IDictionary createDefaultContext (object root, ClassResolver classResolver, TypeConverter converter, MemberAccess memberAccess)
		{
			return addDefaultContext (root, classResolver, converter, memberAccess, new OgnlContext ()) ;
		}

		///<summary>
		///Appends the standard naming context for evaluating an OGNL expression
		///into the context given so that cached maps can be used as a context.
		///</summary>
		///<param name="root"> the root of the object graph</param>
		///<param name="context"> the context to which OGNL context will be added.</param>
		///<returns>
		///a new IDictionary with the keys <c>root</c> and <c>context</c>
		///set appropriately
		///</returns>
		public static IDictionary addDefaultContext (object root, IDictionary context)
		{
			return addDefaultContext (root, null, null, null, context) ;
		}


		///<summary>
		///Appends the standard naming context for evaluating an OGNL expression
		///into the context given so that cached maps can be used as a context.
		///</summary>
		///<param name="root"> the root of the object graph</param>
		///<param name="context"> the context to which OGNL context will be added.</param>
		///<returns>
		///a new IDictionary with the keys <c>root</c> and <c>context</c>
		///set appropriately
		///</returns>
		public static IDictionary addDefaultContext (object root, ClassResolver classResolver, IDictionary context)
		{
			return addDefaultContext (root, classResolver, null, null, context) ;
		}


		///<summary>
		///Appends the standard naming context for evaluating an OGNL expression
		///into the context given so that cached maps can be used as a context.
		///</summary>
		///<param name="root"> the root of the object graph</param>
		///<param name="context"> the context to which OGNL context will be added.</param>
		///<returns>
		///a new IDictionary with the keys <c>root</c> and <c>context</c>
		///set appropriately
		///</returns>
		public static IDictionary addDefaultContext (object root, ClassResolver classResolver, TypeConverter converter, IDictionary context)
		{
			return addDefaultContext (root, classResolver, converter, null, context) ;
		}


		///<summary>
		///Appends the standard naming context for evaluating an OGNL expression
		///into the context given so that cached maps can be used as a context.
		///</summary>
		///<param name="root"> the root of the object graph</param>
		///<param name="context"> the context to which OGNL context will be added.</param>
		///<returns>
		///a new IDictionary with the keys <c>root</c> and <c>context</c>
		///set appropriately
		///</returns>
		public static IDictionary addDefaultContext (object root, ClassResolver classResolver, TypeConverter converter, MemberAccess memberAccess, IDictionary context)
		{
			OgnlContext result ;

			if (!(context is OgnlContext))
			{
				result = new OgnlContext () ;
				result.setValues (context) ;
			}
			else
			{
				result = (OgnlContext) context ;
			}
			if (classResolver != null)
			{
				result.setClassResolver (classResolver) ;
			}
			if (converter != null)
			{
				result.setTypeConverter (converter) ;
			}
			if (memberAccess != null)
			{
				result.setMemberAccess (memberAccess) ;
			}
			result.setRoot (root) ;
			return result ;
		}

		public static void setClassResolver (IDictionary context, ClassResolver classResolver)
		{
			context [OgnlContext.CLASS_RESOLVER_CONTEXT_KEY] = classResolver ;
		}

		public static ClassResolver getClassResolver (IDictionary context)
		{
			return (ClassResolver) context [OgnlContext.CLASS_RESOLVER_CONTEXT_KEY] ;
		}

		public static void setTypeConverter (IDictionary context, TypeConverter converter)
		{
			context [OgnlContext.TYPE_CONVERTER_CONTEXT_KEY] = converter ;
		}

		public static TypeConverter getTypeConverter (IDictionary context)
		{
			return (TypeConverter) context [(OgnlContext.TYPE_CONVERTER_CONTEXT_KEY)] ;
		}

		public static void setMemberAccess (IDictionary context, MemberAccess memberAccess)
		{
			context [OgnlContext.MEMBER_ACCESS_CONTEXT_KEY] = memberAccess ;
		}

		public static MemberAccess getMemberAccess (IDictionary context)
		{
			return (MemberAccess) context [(OgnlContext.MEMBER_ACCESS_CONTEXT_KEY)] ;
		}

		public static void setRoot (IDictionary context, object root)
		{
			context [OgnlContext.ROOT_CONTEXT_KEY] = root ;
		}

		public static object getRoot (IDictionary context)
		{
			return context [(OgnlContext.ROOT_CONTEXT_KEY)] ;
		}

		public static Evaluation getLastEvaluation (IDictionary context)
		{
			return (Evaluation) context [(OgnlContext.LAST_EVALUATION_CONTEXT_KEY)] ;
		}

		///<summary>
		///Evaluates the given OGNL expression tree to extract a value from the given root
		///object. The default context is set for the given context and root via
		///<c>addDefaultContext()</c>.
		///</summary>
		///<param name="tree"> the OGNL expression tree to evaluate, as returned by parseExpression()</param>
		///<param name="context"> the naming context for the evaluation</param>
		///<param name="root"> the root object for the OGNL expression</param>
		///<returns>the result of evaluating the expression</returns>
		///<exception cref="MethodFailedException"> if the expression called a method which failed</exception>
		///<exception cref="NoSuchPropertyException"> if the expression referred to a nonexistent property</exception>
		///<exception cref="InappropriateExpressionException"> if the expression can't be used in this context</exception>
		///<exception cref="OgnlException"> if there is a pathological environmental problem</
		///
		public static object getValue (object tree, IDictionary context, object root) // throws OgnlException
		{
			return getValue (tree, context, root, null) ;
		}

		///<summary>
		///Evaluates the given OGNL expression tree to extract a value from the given root
		///object. The default context is set for the given context and root via
		///<c>addDefaultContext()</c>.
		///</summary>
		///<param name="tree"> the OGNL expression tree to evaluate, as returned by parseExpression()</param>
		///<param name="context"> the naming context for the evaluation</param>
		///<param name="root"> the root object for the OGNL expression</param>
		///<param name="resultType"></param>
		///<returns>the result of evaluating the expression</returns>
		///<exception cref="MethodFailedException"> if the expression called a method which failed</exception>
		///<exception cref="NoSuchPropertyException"> if the expression referred to a nonexistent property</exception>
		///<exception cref="InappropriateExpressionException"> if the expression can't be used in this context</exception>
		///<exception cref="OgnlException"> if there is a pathological environmental problem</
		///
		public static object getValue (object tree, IDictionary context, object root, Type resultType) // throws OgnlException
		{
			object result ;
			OgnlContext ognlContext = (OgnlContext) addDefaultContext (root, context) ;

			result = ((Node) tree).getValue (ognlContext, root) ;
			if (resultType != null)
			{
				result = getTypeConverter (context).convertValue (context, root, null, null, result, resultType) ;
			}
			return result ;
		}

		///<summary>
		///Evaluates the given OGNL expression to extract a value from the given root
		///object in a given context
		///</summary>
		///<param name="expression"> the OGNL expression</param>
		///<param name="context"> the naming context for the evaluation</param>
		///<param name="root"> the root object for the OGNL expression</param>
		///<returns>the result of evaluating the expression</returns>
		///<exception cref="MethodFailedException"> if the expression called a method which failed</exception>
		///<exception cref="NoSuchPropertyException"> if the expression referred to a nonexistent property</exception>
		///<exception cref="InappropriateExpressionException"> if the expression can't be used in this context</exception>
		///<exception cref="OgnlException"> if there is a pathological environmental problem</
		///
		public static object getValue (string expression, IDictionary context, object root) // throws OgnlException
		{
			return getValue (expression, context, root, null) ;
		}

		///<summary>
		///Evaluates the given OGNL expression to extract a value from the given root
		///object in a given context
		///</summary>
		///<param name="expression"> the OGNL expression</param>
		///<param name="context"> the naming context for the evaluation</param>
		///<param name="root"> the root object for the OGNL expression</param>
		///<returns>the result of evaluating the expression</returns>
		///<exception cref="MethodFailedException"> if the expression called a method which failed</exception>
		///<exception cref="NoSuchPropertyException"> if the expression referred to a nonexistent property</exception>
		///<exception cref="InappropriateExpressionException"> if the expression can't be used in this context</exception>
		///<exception cref="OgnlException"> if there is a pathological environmental problem</
		///
		public static object getValue (string expression, IDictionary context, object root, Type resultType) // throws OgnlException
		{
			return getValue (parseExpression (expression), context, root, resultType) ;
		}

		///<summary>
		///Evaluates the given OGNL expression to extract a value from the given root
		///object in a given context
		///</summary>
		///<param name="tree">  the OGNL expression tree to evaluate</param>
		///<param name="root"> the root object for the OGNL expression</param>
		///<returns>the result of evaluating the expression</returns>
		///<exception cref="MethodFailedException"> if the expression called a method which failed</exception>
		///<exception cref="NoSuchPropertyException"> if the expression referred to a nonexistent property</exception>
		///<exception cref="InappropriateExpressionException"> if the expression can't be used in this context</exception>
		///<exception cref="OgnlException"> if there is a pathological environmental problem</
		///
		public static object getValue (object tree, object root) // throws OgnlException
		{
			return getValue (tree, root, null) ;
		}

		///<summary>
		///Evaluates the given OGNL expression to extract a value from the given root
		///object in a given context
		///</summary>
		///<param name="tree">  the OGNL expression tree to evaluate</param>
		///<param name="root"> the root object for the OGNL expression</param>
		///<returns>the result of evaluating the expression</returns>
		///<exception cref="MethodFailedException"> if the expression called a method which failed</exception>
		///<exception cref="NoSuchPropertyException"> if the expression referred to a nonexistent property</exception>
		///<exception cref="InappropriateExpressionException"> if the expression can't be used in this context</exception>
		///<exception cref="OgnlException"> if there is a pathological environmental problem</
		///
		public static object getValue (object tree, object root, Type resultType) // throws OgnlException
		{
			return getValue (tree, createDefaultContext (root), root, resultType) ;
		}

		///
		///Convenience method that combines calls to <code> parseExpression </code> and
		///<code> getValue</code>.
		///
		///@see #parseExpression(string)
		///@see #getValue(object,object)
		///@param expression the OGNL expression to be parsed
		///@param root the root object for the OGNL expression
		///@return the result of evaluating the expression
		///@// throws ExpressionSyntaxException if the expression is malformed
		///@// throws MethodFailedException if the expression called a method which failed
		///@// throws NoSuchPropertyException if the expression referred to a nonexistent property
		///@// throws InappropriateExpressionException if the expression can't be used in this context
		///@// throws OgnlException if there is a pathological environmental problem
		///
		public static object getValue (string expression, object root) // throws OgnlException
		{
			return getValue (expression, root, null) ;
		}

		///
		///Convenience method that combines calls to <code> parseExpression </code> and
		///<code> getValue</code>.
		///
		///@see #parseExpression(string)
		///@see #getValue(object,object)
		///@param expression the OGNL expression to be parsed
		///@param root the root object for the OGNL expression
		///@param resultType the converted type of the resultant object, using the context's type converter
		///@return the result of evaluating the expression
		///@// throws ExpressionSyntaxException if the expression is malformed
		///@// throws MethodFailedException if the expression called a method which failed
		///@// throws NoSuchPropertyException if the expression referred to a nonexistent property
		///@// throws InappropriateExpressionException if the expression can't be used in this context
		///@// throws OgnlException if there is a pathological environmental problem
		///
		public static object getValue (string expression, object root, Type resultType) // throws OgnlException
		{
			return getValue (parseExpression (expression), root, resultType) ;
		}

		///
		///Evaluates the given OGNL expression tree to insert a value into the object graph
		///rooted at the given root object.  The default context is set for the given
		///context and root via <CODE>addDefaultContext()</CODE>.
		///
		///@param tree the OGNL expression tree to evaluate, as returned by parseExpression()
		///@param context the naming context for the evaluation
		///@param root the root object for the OGNL expression
		///@param value the value to insert into the object graph
		///@// throws MethodFailedException if the expression called a method which failed
		///@// throws NoSuchPropertyException if the expression referred to a nonexistent property
		///@// throws InappropriateExpressionException if the expression can't be used in this context
		///@// throws OgnlException if there is a pathological environmental problem
		///
		public static void setValue (object tree, IDictionary context, object root, object value) // throws OgnlException
		{
			OgnlContext ognlContext = (OgnlContext) addDefaultContext (root, context) ;
			Node n = (Node) tree ;

			n.setValue (ognlContext, root, value) ;
		}

		///
		///Evaluates the given OGNL expression to insert a value into the object graph
		///rooted at the given root object given the context.
		///
		///@param expression the OGNL expression to be parsed
		///@param root the root object for the OGNL expression
		///@param context the naming context for the evaluation
		///@param value the value to insert into the object graph
		///@// throws MethodFailedException if the expression called a method which failed
		///@// throws NoSuchPropertyException if the expression referred to a nonexistent property
		///@// throws InappropriateExpressionException if the expression can't be used in this context
		///@// throws OgnlException if there is a pathological environmental problem
		///
		public static void setValue (string expression, IDictionary context, object root, object value) // throws OgnlException
		{
			setValue (parseExpression (expression), context, root, value) ;
		}

		///
		///Evaluates the given OGNL expression tree to insert a value into the object graph
		///rooted at the given root object.
		///
		///@param tree the OGNL expression tree to evaluate, as returned by parseExpression()
		///@param root the root object for the OGNL expression
		///@param value the value to insert into the object graph
		///@// throws MethodFailedException if the expression called a method which failed
		///@// throws NoSuchPropertyException if the expression referred to a nonexistent property
		///@// throws InappropriateExpressionException if the expression can't be used in this context
		///@// throws OgnlException if there is a pathological environmental problem
		///
		public static void setValue (object tree, object root, object value) // throws OgnlException
		{
			setValue (tree, createDefaultContext (root), root, value) ;
		}

		///
		///Convenience method that combines calls to <code> parseExpression </code> and
		///<code> setValue</code>.
		///
		///@see #parseExpression(string)
		///@see #setValue(object,object,object)
		///@param expression the OGNL expression to be parsed
		///@param root the root object for the OGNL expression
		///@param value the value to insert into the object graph
		///@// throws ExpressionSyntaxException if the expression is malformed
		///@// throws MethodFailedException if the expression called a method which failed
		///@// throws NoSuchPropertyException if the expression referred to a nonexistent property
		///@// throws InappropriateExpressionException if the expression can't be used in this context
		///@// throws OgnlException if there is a pathological environmental problem
		///
		public static void setValue (string expression, object root, object value) // throws OgnlException
		{
			setValue (parseExpression (expression), root, value) ;
		}

		public static bool isConstant (object tree, IDictionary context) // throws OgnlException
		{
			return ((SimpleNode) tree).isConstant ((OgnlContext) addDefaultContext (null, context)) ;
		}

		public static bool isConstant (string expression, IDictionary context) // throws OgnlException
		{
			return isConstant (parseExpression (expression), context) ;
		}

		public static bool isConstant (object tree) // throws OgnlException
		{
			return isConstant (tree, createDefaultContext (null)) ;
		}

		public static bool isConstant (string expression) // throws OgnlException
		{
			return isConstant (parseExpression (expression), createDefaultContext (null)) ;
		}

		public static bool isSimpleProperty (object tree, IDictionary context) // throws OgnlException
		{
			return ((SimpleNode) tree).isSimpleProperty ((OgnlContext) addDefaultContext (null, context)) ;
		}

		public static bool isSimpleProperty (string expression, IDictionary context) // throws OgnlException
		{
			return isSimpleProperty (parseExpression (expression), context) ;
		}

		public static bool isSimpleProperty (object tree) // throws OgnlException
		{
			return isSimpleProperty (tree, createDefaultContext (null)) ;
		}

		public static bool isSimpleProperty (string expression) // throws OgnlException
		{
			return isSimpleProperty (parseExpression (expression), createDefaultContext (null)) ;
		}

		public static bool isSimpleNavigationChain (object tree, IDictionary context) // throws OgnlException
		{
			return ((SimpleNode) tree).isSimpleNavigationChain ((OgnlContext) addDefaultContext (null, context)) ;
		}

		public static bool isSimpleNavigationChain (string expression, IDictionary context) // throws OgnlException
		{
			return isSimpleNavigationChain (parseExpression (expression), context) ;
		}

		public static bool isSimpleNavigationChain (object tree) // throws OgnlException
		{
			return isSimpleNavigationChain (tree, createDefaultContext (null)) ;
		}

		public static bool isSimpleNavigationChain (string expression) // throws OgnlException
		{
			return isSimpleNavigationChain (parseExpression (expression), createDefaultContext (null)) ;
		}

		///You can't make one of these. 
		Ognl ()
		{
		}
	}
}