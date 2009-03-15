using System ;
using System.Collections ;

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
	/// <summary>
	///This class defines the execution context for an OGNL expression
	///</summary>
	///@author Luke Blanshard (blanshlu@netscape.net)
	///@author Drew Davidson (drew@ognl.org)
	///
	public class OgnlContext : IDictionary
	{
		public static string CONTEXT_CONTEXT_KEY = "context" ;
		public static string ROOT_CONTEXT_KEY = "root" ;
		public static string THIS_CONTEXT_KEY = "this" ;
		public static string TRACE_EVALUATIONS_CONTEXT_KEY = "_traceEvaluations" ;
		public static string LAST_EVALUATION_CONTEXT_KEY = "_lastEvaluation" ;
		public static string KEEP_LAST_EVALUATION_CONTEXT_KEY = "_keepLastEvaluation" ;
		public static string CLASS_RESOLVER_CONTEXT_KEY = "_classResolver" ;
		public static string TYPE_CONVERTER_CONTEXT_KEY = "_typeConverter" ;
		public static string MEMBER_ACCESS_CONTEXT_KEY = "_memberAccess" ;

		static string PROPERTY_KEY_PREFIX = "ognl" ;
		static bool DEFAULT_TRACE_EVALUATIONS = false ;
		static bool DEFAULT_KEEP_LAST_EVALUATION = false ;

		public static ClassResolver DEFAULT_CLASS_RESOLVER = new DefaultClassResolver () ;
		public static TypeConverter DEFAULT_TYPE_CONVERTER = new DefaultTypeConverter () ;
		public static MemberAccess DEFAULT_MEMBER_ACCESS = new DefaultMemberAccess (false) ;

		static IDictionary RESERVED_KEYS = new Hashtable (11) ;

		object root ;
		object currentObject ;
		Node currentNode ;
		bool traceEvaluations = DEFAULT_TRACE_EVALUATIONS ;
		Evaluation rootEvaluation ;
		Evaluation currentEvaluation ;
		Evaluation lastEvaluation ;
		bool keepLastEvaluation = DEFAULT_KEEP_LAST_EVALUATION ;
		IDictionary values = new Hashtable (23) ;
		ClassResolver classResolver = DEFAULT_CLASS_RESOLVER ;
		TypeConverter typeConverter = DEFAULT_TYPE_CONVERTER ;
		MemberAccess memberAccess = DEFAULT_MEMBER_ACCESS ;

		/* static
    {
        string          s;

        RESERVED_KEYS.put(CONTEXT_CONTEXT_KEY, null);
        RESERVED_KEYS.put(ROOT_CONTEXT_KEY, null);
        RESERVED_KEYS.put(THIS_CONTEXT_KEY, null);
        RESERVED_KEYS.put(TRACE_EVALUATIONS_CONTEXT_KEY, null);
        RESERVED_KEYS.put(LAST_EVALUATION_CONTEXT_KEY, null);
        RESERVED_KEYS.put(KEEP_LAST_EVALUATION_CONTEXT_KEY, null);
        RESERVED_KEYS.put(CLASS_RESOLVER_CONTEXT_KEY, null);
        RESERVED_KEYS.put(TYPE_CONVERTER_CONTEXT_KEY, null);
        RESERVED_KEYS.put(MEMBER_ACCESS_CONTEXT_KEY, null);

        if ((s = System.getProperty(PROPERTY_KEY_PREFIX + ".traceEvaluations")) != null) {
			DEFAULT_TRACE_EVALUATIONS = Boolean.valueOf(s.trim()).booleanValue();
        }
        if ((s = System.getProperty(PROPERTY_KEY_PREFIX + ".keepLastEvaluation")) != null) {
			DEFAULT_KEEP_LAST_EVALUATION = Boolean.valueOf(s.trim()).booleanValue();
        }
    }
*/

		///
		///Constructs a new OgnlContext with the default class resolver, type converter and
		///member access.
		///
		public OgnlContext ()
		{
		}

		///<summary>
		/// Constructs a new OgnlContext with the given class resolver, type converter and
		/// member access.  If any of these parameters is null the default will be used.
		/// </summary>
		///
		public OgnlContext (ClassResolver classResolver, TypeConverter typeConverter, MemberAccess memberAccess)
		{
			if (classResolver != null)
			{
				this.classResolver = classResolver ;
			}
			if (typeConverter != null)
			{
				this.typeConverter = typeConverter ;
			}
			if (memberAccess != null)
			{
				this.memberAccess = memberAccess ;
			}
		}

		public OgnlContext (IDictionary values)
		{
			this.values = values ;
		}

		public OgnlContext (ClassResolver classResolver, TypeConverter typeConverter, MemberAccess memberAccess, IDictionary values)
			: this (classResolver, typeConverter, memberAccess)
		{
			this.values = values ;
		}

		public void setValues (IDictionary value)
		{
			Util.putAll (value, values) ;
		}

		public IDictionary getValues ()
		{
			return values ;
		}

		public void setClassResolver (ClassResolver value)
		{
			if (value == null)
			{
				throw new ArgumentException ("cannot set ClassResolver to null") ;
			}
			classResolver = value ;
		}

		public ClassResolver getClassResolver ()
		{
			return classResolver ;
		}

		public void setTypeConverter (TypeConverter value)
		{
			if (value == null)
			{
				throw new ArgumentException ("cannot set TypeConverter to null") ;
			}
			typeConverter = value ;
		}

		public TypeConverter getTypeConverter ()
		{
			return typeConverter ;
		}

		public void setMemberAccess (MemberAccess value)
		{
			if (value == null)
			{
				throw new ArgumentException ("cannot set MemberAccess to null") ;
			}
			memberAccess = value ;
		}

		public MemberAccess getMemberAccess ()
		{
			return memberAccess ;
		}

		public void setRoot (object value)
		{
			root = value ;
		}

		public object getRoot ()
		{
			return root ;
		}

		public bool getTraceEvaluations ()
		{
			return traceEvaluations ;
		}

		public void setTraceEvaluations (bool value)
		{
			traceEvaluations = value ;
		}

		public Evaluation getLastEvaluation ()
		{
			return lastEvaluation ;
		}

		public void setLastEvaluation (Evaluation value)
		{
			lastEvaluation = value ;
		}

		///<summary>
		/// This method can be called when the last evaluation has been used
		/// and can be returned for reuse in the free pool maintained by the
		/// runtime.  This is not a necessary step, but is useful for keeping
		/// memory usage down.  This will recycle the last evaluation and then
		/// set the last evaluation to null.
		///</summary>
		public void recycleLastEvaluation ()
		{
			OgnlRuntime.getEvaluationPool ().recycleAll (lastEvaluation) ;
			lastEvaluation = null ;
		}

		///<summary>
		/// Returns true if the last evaluation that was done on this
		/// context is retained and available through <code>getLastEvaluation()</code>.
		/// The default is true.
		///</summary>
		public bool getKeepLastEvaluation ()
		{
			return keepLastEvaluation ;
		}

		///<summary>
		/// Sets whether the last evaluation that was done on this
		/// context is retained and available through <code>getLastEvaluation()</code>.
		/// The default is true.
		///</summary>
		public void setKeepLastEvaluation (bool value)
		{
			keepLastEvaluation = value ;
		}

		public void setCurrentObject (object value)
		{
			currentObject = value ;
		}

		public object getCurrentObject ()
		{
			return currentObject ;
		}

		public void setCurrentNode (Node value)
		{
			currentNode = value ;
		}

		public Node getCurrentNode ()
		{
			return currentNode ;
		}

		///
		/// Gets the current Evaluation from the top of the stack.
		/// This is the Evaluation that is in process of evaluating.
		///
		public Evaluation getCurrentEvaluation ()
		{
			return currentEvaluation ;
		}

		public void setCurrentEvaluation (Evaluation value)
		{
			currentEvaluation = value ;
		}

		///<summary>
		/// Gets the root of the evaluation stack.
		/// This Evaluation contains the node representing
		/// the root expression and the source is the root
		/// source object.
		///</summary>
		public Evaluation getRootEvaluation ()
		{
			return rootEvaluation ;
		}

		public void setRootEvaluation (Evaluation value)
		{
			rootEvaluation = value ;
		}

		///<summary>
		/// Returns the Evaluation at the relative index given.  This should be
		/// zero or a negative number as a relative reference back up the evaluation
		/// stack.  Therefore getEvaluation(0) returns the current Evaluation.
		///</summary>
		public Evaluation getEvaluation (int relativeIndex)
		{
			Evaluation result = null ;

			if (relativeIndex <= 0)
			{
				result = currentEvaluation ;
				while ((++relativeIndex < 0) && (result != null))
				{
					result = result.getParent () ;
				}
			}
			return result ;
		}

		///<summary>
		/// Pushes a new Evaluation onto the stack.  This is done
		/// before a node evaluates.  When evaluation is complete
		/// it should be popped from the stack via <code>popEvaluation()</code>.
		///</summary>
		public void pushEvaluation (Evaluation value)
		{
			if (currentEvaluation != null)
			{
				currentEvaluation.addChild (value) ;
			}
			else
			{
				setRootEvaluation (value) ;
			}
			setCurrentEvaluation (value) ;
		}

		///<summary>
		/// Pops the current Evaluation off of the top of the stack.
		/// This is done after a node has completed its evaluation.
		///</summary>
		public Evaluation popEvaluation ()
		{
			Evaluation result ;

			result = currentEvaluation ;
			setCurrentEvaluation (result.getParent ()) ;
			if (currentEvaluation == null)
			{
				setLastEvaluation (getKeepLastEvaluation () ? result : null) ;
				setRootEvaluation (null) ;
				setCurrentNode (null) ;
			}
			return result ;
		}

		/*================= IDictionatry interface =================*/

		public override bool Equals (object o)
		{
			return values.Equals (o) ;
		}

		public override int GetHashCode ()
		{
			return values.GetHashCode () ;
		}

		public void CopyTo (Array array, int index)
		{
			values.CopyTo (array, index) ;
		}

		public int Count
		{
			get { return values.Count ; }
		}

		public object SyncRoot
		{
			get { return values.SyncRoot ; }
		}

		public bool IsSynchronized
		{
			get { return values.IsSynchronized ; }
		}

		public bool Contains (object key)
		{
			return values.Contains (key) ;
		}

		public void Add (object key, object value)
		{
			this [key] = value ;
		}

		public void Clear ()
		{
			values.Clear () ;
			setRoot (null) ;
			setCurrentObject (null) ;
			setRootEvaluation (null) ;
			setCurrentEvaluation (null) ;
			setLastEvaluation (null) ;
			setCurrentNode (null) ;
			setClassResolver (DEFAULT_CLASS_RESOLVER) ;
			setTypeConverter (DEFAULT_TYPE_CONVERTER) ;
			setMemberAccess (DEFAULT_MEMBER_ACCESS) ;
			;
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return values.Values.GetEnumerator () ;
		}

		public IDictionaryEnumerator GetEnumerator ()
		{
			return values.GetEnumerator () ;
		}

		public void Remove (object key)
		{
			object result ;

			if (RESERVED_KEYS.Contains (key))
			{
				if (key.Equals (OgnlContext.THIS_CONTEXT_KEY))
				{
					result = getCurrentObject () ;
					setCurrentObject (null) ;
				}
				else
				{
					if (key.Equals (OgnlContext.ROOT_CONTEXT_KEY))
					{
						result = getRoot () ;
						setRoot (null) ;
					}
					else
					{
						if (key.Equals (OgnlContext.CONTEXT_CONTEXT_KEY))
						{
							throw new ArgumentException ("can't remove " + OgnlContext.CONTEXT_CONTEXT_KEY + " from context") ;
						}
						else
						{
							if (key.Equals (OgnlContext.TRACE_EVALUATIONS_CONTEXT_KEY))
							{
								throw new ArgumentException ("can't remove " + OgnlContext.TRACE_EVALUATIONS_CONTEXT_KEY + " from context") ;
							}
							else
							{
								if (key.Equals (OgnlContext.LAST_EVALUATION_CONTEXT_KEY))
								{
									result = lastEvaluation ;
									setLastEvaluation (null) ;
								}
								else
								{
									if (key.Equals (OgnlContext.KEEP_LAST_EVALUATION_CONTEXT_KEY))
									{
										throw new ArgumentException ("can't remove " + OgnlContext.KEEP_LAST_EVALUATION_CONTEXT_KEY + " from context") ;
									}
									else
									{
										if (key.Equals (OgnlContext.CLASS_RESOLVER_CONTEXT_KEY))
										{
											result = getClassResolver () ;
											setClassResolver (null) ;
										}
										else
										{
											if (key.Equals (OgnlContext.TYPE_CONVERTER_CONTEXT_KEY))
											{
												result = getTypeConverter () ;
												setTypeConverter (null) ;
											}
											else
											{
												if (key.Equals (OgnlContext.MEMBER_ACCESS_CONTEXT_KEY))
												{
													result = getMemberAccess () ;
													setMemberAccess (null) ;
												}
												else
												{
													throw new ArgumentException ("unknown reserved key '" + key + "'") ;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			else
			{
				// result = 
				values.Remove (key) ;
			}
			return ; // result;
		}

		public ICollection Keys
		{
			get { return values.Keys ; }
		}

		public ICollection Values
		{
			get { return values.Values ; }
		}

		public bool IsReadOnly
		{
			get { return values.IsReadOnly ; }
		}

		public bool IsFixedSize
		{
			get { return values.IsFixedSize ; }
		}

		public object this [object key]
		{
			get
			{
				object result ;

                if (RESERVED_KEYS.Contains(key))
				{
					if (key.Equals (OgnlContext.THIS_CONTEXT_KEY))
					{
						result = getCurrentObject () ;
					}
					else
					{
						if (key.Equals (OgnlContext.ROOT_CONTEXT_KEY))
						{
							result = getRoot () ;
						}
						else
						{
							if (key.Equals (OgnlContext.CONTEXT_CONTEXT_KEY))
							{
								result = this ;
							}
							else
							{
								if (key.Equals (OgnlContext.TRACE_EVALUATIONS_CONTEXT_KEY))
								{
									result = getTraceEvaluations () ? true : false ;
								}
								else
								{
									if (key.Equals (OgnlContext.LAST_EVALUATION_CONTEXT_KEY))
									{
										result = getLastEvaluation () ;
									}
									else
									{
										if (key.Equals (OgnlContext.KEEP_LAST_EVALUATION_CONTEXT_KEY))
										{
											result = getKeepLastEvaluation () ? true : false ;
										}
										else
										{
											if (key.Equals (OgnlContext.CLASS_RESOLVER_CONTEXT_KEY))
											{
												result = getClassResolver () ;
											}
											else
											{
												if (key.Equals (OgnlContext.TYPE_CONVERTER_CONTEXT_KEY))
												{
													result = getTypeConverter () ;
												}
												else
												{
													if (key.Equals (OgnlContext.MEMBER_ACCESS_CONTEXT_KEY))
													{
														result = getMemberAccess () ;
													}
													else
													{
														throw new ArgumentException ("unknown reserved key '" + key + "'") ;
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				else
				{
					result = values [key] ;
				}
				return result ;
			}
			set
			{
				object result ;

				if (RESERVED_KEYS.Contains (key))
				{
                    if (key.Equals(OgnlContext.THIS_CONTEXT_KEY))
                    {
                        result = getCurrentObject();
                        setCurrentObject(value);
                    }
                    else
                    {
                        if (key.Equals(OgnlContext.ROOT_CONTEXT_KEY))
                        {
                            result = getRoot();
                            setRoot(value);
                        }
                        else
                        {
                            if (key.Equals(OgnlContext.CONTEXT_CONTEXT_KEY))
                            {
                                throw new ArgumentException("can't change " + OgnlContext.CONTEXT_CONTEXT_KEY + " in context");
                            }
                            else
                            {
                                if (key.Equals(OgnlContext.TRACE_EVALUATIONS_CONTEXT_KEY))
                                {
                                    result = getTraceEvaluations() ? true : false;
                                    setTraceEvaluations(OgnlOps.booleanValue(value));
                                }
                                else
                                {
                                    if (key.Equals(OgnlContext.LAST_EVALUATION_CONTEXT_KEY))
                                    {
                                        result = getLastEvaluation();
                                        lastEvaluation = (Evaluation)value;
                                    }
                                    else
                                    {
                                        if (key.Equals(OgnlContext.KEEP_LAST_EVALUATION_CONTEXT_KEY))
                                        {
                                            result = getKeepLastEvaluation() ? true : false;
                                            setKeepLastEvaluation(OgnlOps.booleanValue(value));
                                        }
                                        else
                                        {
                                            if (key.Equals(OgnlContext.CLASS_RESOLVER_CONTEXT_KEY))
                                            {
                                                result = getClassResolver();
                                                setClassResolver((ClassResolver)value);
                                            }
                                            else
                                            {
                                                if (key.Equals(OgnlContext.TYPE_CONVERTER_CONTEXT_KEY))
                                                {
                                                    result = getTypeConverter();
                                                    setTypeConverter((TypeConverter)value);
                                                }
                                                else
                                                {
                                                    if (key.Equals(OgnlContext.MEMBER_ACCESS_CONTEXT_KEY))
                                                    {
                                                        result = getMemberAccess();
                                                        setMemberAccess((MemberAccess)value);
                                                    }
                                                    else
                                                    {
                                                        throw new ArgumentException("unknown reserved key '" + key + "'");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
				}
				else
				{
                    bool keyFound = false;
                    foreach (object searchKey in values.Keys)
                    {
                        if (searchKey.Equals(key))
                        {
                            keyFound = true;
                            break;
                        }
                    }

                    if (keyFound)
                    {
                        values[key] = value;
                    }
                    else
                    {
                        values.Add(key, value);
                    }
				}
				// return result;

			}
		}
	}
}