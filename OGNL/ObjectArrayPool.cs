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
	public sealed class ObjectArrayPool : object
	{
		Hashtable pools = new Hashtable (23) ;

		public class SizePool : object
		{
			IList arrays = new ArrayList () ;
			int arraySize ;
			int size ;
			int created = 0 ;
			int recovered = 0 ;
			int recycled = 0 ;

			public SizePool (int arraySize) : this (arraySize, 0)
			{
				;
			}

			public SizePool (int arraySize, int initialSize)
			{
				this.arraySize = arraySize ;
				for (int i = 0; i < initialSize; i++)
				{
					arrays.Add (new object[arraySize]) ;
				}
				created = size = initialSize ;
			}

			public int getArraySize ()
			{
				return arraySize ;
			}

			public object[] create ()
			{
				object[] result ;

				if (size > 0)
				{
					result = (object[]) arrays [size - 1] ;
					arrays.Remove (size - 1) ;
					size-- ;
					recovered++ ;
				}
				else
				{
					result = new object[arraySize] ;
					created++ ;
				}
				return result ;
			}

			public void recycle (object[] value)
			{
				lock (this)
				{
					if (value != null)
					{
						if (value.Length != arraySize)
						{
							throw new ArgumentException ("recycled array size " + value.Length + " inappropriate for pool array size " + arraySize) ;
						}
						// Array.Fill(value, null);
						for (int i = 0; i < value.Length; i++)
						{
							value [i] = null ;
						}

						arrays.Add (value) ;
						size++ ;
						recycled++ ;
					}
					else
					{
						throw new ArgumentException ("cannot recycle null object") ;
					}
				}
			}

			/// <summary>
			/// Returns the number of items in the pool
			/// </summary>
			public int getSize ()
			{
				return size ;
			}

			///<summary>
			/// Returns the number of items this pool has created since
			/// it's construction.
			///</summary>
			public int getCreatedCount ()
			{
				return created ;
			}

			///<summary>
			/// Returns the number of items this pool has recovered from
			/// the pool since its construction.
			///</summary>
			public int getRecoveredCount ()
			{
				return recovered ;
			}

			///<summary>
			/// Returns the number of items this pool has recycled since
			/// it's construction.
			///</summary>
			public int getRecycledCount ()
			{
				return recycled ;
			}
		}

		public ObjectArrayPool ()
		{
		}

		public Hashtable getSizePools ()
		{
			return pools ;
		}

		public SizePool getSizePool (int arraySize)
		{
			lock (this)
			{
				SizePool result = (SizePool) pools [arraySize] ;

				if (result == null)
				{
					pools [arraySize] = (result = new SizePool (arraySize)) ;
				}
				return result ;
			}
		}

		public object[] create (int arraySize)
		{
			lock (this)
			{
				return getSizePool (arraySize).create () ;
			}
		}

		public object[] create (object singleton)
		{
			lock (this)
			{
				object[] result = create (1) ;

				result [0] = singleton ;
				return result ;
			}
		}

		public object[] create (object object1, object object2)
		{
			lock (this)
			{
				object[] result = create (2) ;

				result [0] = object1 ;
				result [1] = object2 ;
				return result ;
			}
		}

		public object[] create (object object1, object object2, object object3)
		{
			lock (this)
			{
				object[] result = create (3) ;

				result [0] = object1 ;
				result [1] = object2 ;
				result [2] = object3 ;
				return result ;
			}
		}

		public object[] create (object object1, object object2, object object3, object object4)
		{
			lock (this)
			{
				object[] result = create (4) ;

				result [0] = object1 ;
				result [1] = object2 ;
				result [2] = object3 ;
				result [3] = object4 ;
				return result ;
			}
		}

		public object[] create (object object1, object object2, object object3, object object4, object object5)
		{
			lock (this)
			{
				object[] result = create (5) ;

				result [0] = object1 ;
				result [1] = object2 ;
				result [2] = object3 ;
				result [3] = object4 ;
				result [4] = object5 ;
				return result ;
			}
		}

		public void recycle (object[] value)
		{
			lock (this)
			{
				if (value != null)
				{
					getSizePool (value.Length).recycle (value) ;
				}
			}
		}
	}
}