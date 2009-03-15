using System ;
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
	/// An <b>Evaluation</b> is and object that holds a node being evaluated
	/// and the source from which that node will take extract its
	/// value.  
	/// </summary>
	/// <remarks>It refers to child evaluations that occur as
	/// a result of the nodes' evaluation.
	/// </remarks>
	///
	public class Evaluation
	{
		SimpleNode node ;
		object source ;
		bool setOperation ;
		object result ;
		Exception exception ;
		Evaluation parent ;
		Evaluation next ;
		Evaluation previous ;
		Evaluation firstChild ;
		Evaluation lastChild ;

		/// <summary>
		/// Constructs a new "get" <code>Evaluation</code> from the node and source given.
		///</summary> 
		public Evaluation (SimpleNode node, object source)
		{
			this.node = node ;
			this.source = source ;
		}

		///<summary> 
		/// Constructs a new <code>Evaluation</code> from the node and source given.
		/// If <code>setOperation</code> is true this <code>Evaluation</code> represents
		/// a "set" as opposed to a "get".
		/// </summary>
		public Evaluation (SimpleNode node, object source, bool setOperation) : this (node, source)
		{
			this.setOperation = setOperation ;
		}

		///<summary> 
		/// Returns the <code>SimpleNode</code> for this <code>Evaluation</code>
		///</summary>
		public SimpleNode getNode ()
		{
			return node ;
		}

		///<summary> 
		/// Sets the node of the evaluation.  Normally applications do not need to
		/// set this.  Notable exceptions to this rule are custom evaluators that
		/// choose between navigable objects (as in a multi-root evaluator where
		/// the navigable node is chosen at runtime).
		///</summary>
		public void setNode (SimpleNode value)
		{
			node = value ;
		}

		///<summary> 
		/// Returns the source object on which this Evaluation operated.
		///</summary>
		public object getSource ()
		{
			return source ;
		}

		///<summary> 
		/// Sets the source of the evaluation.  Normally applications do not need to
		/// set this.  Notable exceptions to this rule are custom evaluators that
		/// choose between navigable objects (as in a multi-root evaluator where
		/// the navigable node is chosen at runtime).
		///</summary>
		public void setSource (object value)
		{
			source = value ;
		}

		///<summary> 
		/// Returns true if this Evaluation represents a set operation.
		///</summary>
		public bool isSetOperation ()
		{
			return setOperation ;
		}

		///<summary> 
		/// Marks the Evaluation as a set operation if the value is true, else
		/// marks it as a get operation.
		///</summary>
		public void setSetOperation (bool value)
		{
			setOperation = value ;
		}

		///<summary> 
		/// Returns the result of the Evaluation, or null if it was a set operation.
		///</summary>
		public object getResult ()
		{
			return result ;
		}

		///<summary> 
		/// Sets the result of the Evaluation.  This method is normally only used
		/// interally and should not be set without knowledge of what you are doing.
		///</summary>
		public void setResult (object value)
		{
			result = value ;
		}

		///<summary> 
		/// Returns the exception that occurred as a result of evaluating the
		/// Evaluation, or null if no exception occurred.
		///</summary>
		public Exception getException ()
		{
			return exception ;
		}

		///<summary> 
		/// Sets the exception that occurred as a result of evaluating the
		/// Evaluation.  This method is normally only used interally and
		/// should not be set without knowledge of what you are doing.
		///</summary>
		public void setException (Exception value)
		{
			exception = value ;
		}

		///<summary> 
		/// Returns the parent evaluation of this evaluation.  If this returns
		/// null then it is is the root evaluation of a tree.
		///</summary>
		public Evaluation getParent ()
		{
			return parent ;
		}

		///<summary> 
		/// Returns the next sibling of this evaluation.  Returns null if
		/// this is the last in a chain of evaluations.
		///</summary>
		public Evaluation getNext ()
		{
			return next ;
		}

		///<summary> 
		/// Returns the previous sibling of this evaluation.  Returns null if
		/// this is the first in a chain of evaluations.
		///</summary>
		public Evaluation getPrevious ()
		{
			return previous ;
		}

		///<summary> 
		/// Returns the first child of this evaluation.  Returns null if
		/// there are no children.
		///</summary>
		public Evaluation getFirstChild ()
		{
			return firstChild ;
		}

		///<summary> 
		/// Returns the last child of this evaluation.  Returns null if
		/// there are no children.
		///</summary>
		public Evaluation getLastChild ()
		{
			return lastChild ;
		}

		///<summary> 
		/// Gets the first descendent.  In any Evaluation tree this will the
		/// Evaluation that was first executed.
		///</summary>
		public Evaluation getFirstDescendant ()
		{
			if (firstChild != null)
			{
				return firstChild.getFirstDescendant () ;
			}
			return this ;
		}

		///<summary> 
		/// Gets the last descendent.  In any Evaluation tree this will the
		/// Evaluation that was most recently executing.
		///</summary>
		public Evaluation getLastDescendant ()
		{
			if (lastChild != null)
			{
				return lastChild.getLastDescendant () ;
			}
			return this ;
		}

		///<summary> 
		/// Adds a child to the list of children of this evaluation.  The
		/// parent of the child is set to the receiver and the children
		/// references are modified in the receiver to reflect the new child.
		/// The lastChild of the receiver is set to the child, and the
		/// firstChild is set also if child is the first (or only) child.
		///</summary>
		public void addChild (Evaluation child)
		{
			if (firstChild == null)
			{
				firstChild = lastChild = child ;
			}
			else
			{
				if (firstChild == lastChild)
				{
					firstChild.next = child ;
					lastChild = child ;
					lastChild.previous = firstChild ;
				}
				else
				{
					child.previous = lastChild ;
					lastChild.next = child ;
					lastChild = child ;
				}
			}
			child.parent = this ;
		}

		///<summary> 
		/// Reinitializes this Evaluation to the parameters specified.
		///</summary>
		public void init (SimpleNode node, object source, bool setOperation)
		{
			this.node = node ;
			this.source = source ;
			this.setOperation = setOperation ;
			result = null ;
			exception = null ;
			parent = null ;
			next = null ;
			previous = null ;
			firstChild = null ;
			lastChild = null ;
		}

		///<summary> 
		/// Resets this Evaluation to the initial state.
		///</summary>
		public void reset ()
		{
			init (null, null, false) ;
		}

		///<summary> 
		/// Produces a string value for the Evaluation.  If compact is
		/// true then a more compact form of the description only including
		/// the node type and unique identifier is shown, else a full
		/// description including source and result are shown.  If showChildren
		/// is true the child evaluations are printed using the depth string
		/// given as a prefix.
		///</summary>
		public string ToString (bool compact, bool showChildren, string depth)
		{
			string stringResult ;

			if (compact)
			{
				stringResult = depth + "<" + node.GetType ().Name + " " + this.GetHashCode () + ">" ;
			}
			else
			{
				string ss = (source != null) ? source.GetType ().Name : "null",
					rs = (result != null) ? result.GetType ().Name : "null" ;

				stringResult = depth + "<" + node.GetType ().Name + ": [" + (setOperation ? "set" : "get") + "] source = " + ss + ", result = " + result + " [" + rs + "]>" ;
			}
			if (showChildren)
			{
				Evaluation child = firstChild ;

				stringResult += "\n" ;
				while (child != null)
				{
					stringResult += child.ToString (compact, depth + "  ") ;
					child = child.next ;
				}
			}
			return stringResult ;
		}

		///<summary> 
		/// Produces a string value for the Evaluation.  If compact is
		/// true then a more compact form of the description only including
		/// the node type and unique identifier is shown, else a full
		/// description including source and result are shown.  Child
		/// evaluations are printed using the depth string given as a prefix.
		///</summary>
		public string ToString (bool compact, string depth)
		{
			return ToString (compact, true, depth) ;
		}

		///<summary> 
		/// Returns a string description of the Evaluation.
		///</summary>
		public override string ToString ()
		{
			return ToString (false, "") ;
		}
	}
}