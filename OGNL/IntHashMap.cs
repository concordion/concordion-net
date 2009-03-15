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
	///<summary>
	///A IDictionary that uses ints as the keys.
	///</summary>
	///<remarks>
	///<para>Use just like any java.util.IDictionary, except that the keys must be ints.
	///This is much faster than creating a new Integer for each access.</para>
	///<para>For non-IDictionary access (faster) use the put(int, object) method.</para>
	///<para>This class : IDictionary for convenience, but this is not the most
	///efficient usage.</para>
	///</remarks>
	///
	public class IntHashMap : Hashtable
	{
		//    private Entry[]       table;
		//    private int         count;
		//    private int         threshold;
		//    private float       loadFactor;
		//
		//    /*===================================================================
		//        Private static classes
		//      ===================================================================*/
		//    private class IntHashMapIterator : Iterator
		//    {
		//        bool         keys;
		//        int             index;
		//        Entry           []table;
		//        Entry           entry;
		//
		//        IntHashMapIterator(Entry []table, bool keys)
		//    	{
		//    	    base();
		//        	this.table = table;
		//        	this.keys = keys;
		//        	this.index = table.length;
		//    	}
		//
		//        /*===================================================================
		//            Iterator interface
		//          ===================================================================*/
		//        public bool hasNext()
		//        {
		//            if (entry != null) {
		//                return true;
		//            }
		//            while (index-- > 0) {
		//                if ((entry = table[index]) != null) {
		//                    return true;
		//                }
		//            }
		//            return false;
		//        }
		//
		//        public object next()
		//        {
		//            if (entry == null) {
		//                while ((index-- > 0) && ((entry = table[index]) == null)) {
		//                    /* do nothing */
		//                }
		//            }
		//            if (entry != null) {
		//                Entry       e = entry;
		//
		//                entry = e.next;
		//                return keys ? new Integer(e.key) : e.value;
		//            }
		//            throw new NoSuchElementException("IntHashMapIterator");
		//        }
		//
		//        public void remove()
		//        {
		//            throw new UnsupportedOperationException("remove");
		//        }
		//    }
		//
		//    /*===================================================================
		//        Public static classes
		//      ===================================================================*/
		//    public class Entry : object
		//    {
		//        int         hash;
		//        int         key;
		//        object      value;
		//        Entry       next;
		//
		//        public Entry()
		//        {
		//            base();
		//        }
		//    }
		//
		/*===================================================================
        Constructors
      ===================================================================*/

		public IntHashMap (int initialCapacity, float loadFactor)
			: base (initialCapacity, loadFactor)
		{
			/*if (initialCapacity <= 0 || loadFactor <= 0.0) {
            throw new IllegalArgumentException();
        }
        this.loadFactor = loadFactor;
        table = new Entry[initialCapacity];
        threshold = (int)(initialCapacity * loadFactor);*/
		}

		public IntHashMap (int initialCapacity) :
			this (initialCapacity, 0.75f)
		{
		}

		public IntHashMap () : this (101, 0.75f)
		{
			;
		}

		//
		//    /*===================================================================
		//        Protected methods
		//      ===================================================================*/
		//    protected void rehash()
		//    {
		//        int         oldCapacity = table.length;
		//        Entry[]       oldTable = table;
		//        int         newCapacity = oldCapacity * 2 + 1;
		//        Entry[]       newTable = new Entry[newCapacity];
		//
		//        threshold = (int)(newCapacity * loadFactor);
		//        table = newTable;
		//        for (int i = oldCapacity ; i-- > 0 ;) {
		//            for (Entry old = oldTable[i] ; old != null;) {
		//                Entry       e = old;
		//                int         index = ( e.hash & 0x7FFFFFFF ) % newCapacity;
		//
		//                old = old.next;
		//                e.next = newTable[index];
		//                newTable[index] = e;
		//            }
		//        }
		//    }
		//
		//    /*===================================================================
		//        Public methods
		//      ===================================================================*/
		//    public bool containsKey(int key)
		//	{
		//        int         index = (key & 0x7FFFFFFF) % table.length;
		//
		//        for (Entry e = table[index] ; e != null ; e = e.next) {
		//            if ((e.hash == key) && (e.key == key)) {
		//                return true;
		//            }
		//        }
		//        return false;
		//	}
		//
		//    public object get(int key)
		//    {
		//        int         index = (key & 0x7FFFFFFF) % table.length;
		//
		//        for (Entry e = table[index] ; e != null ; e = e.next) {
		//            if ((e.hash == key) && (e.key == key)) {
		//                return e.value;
		//            }
		//        }
		//        return null;
		//    }
		//
		//    public object put(int key, object value)
		//    {
		//        int         index = ( key & 0x7FFFFFFF ) % table.length;
		//
		//        if (value == null) {
		//            throw new IllegalArgumentException();
		//        }
		//        for (Entry e = table[index] ; e != null ; e = e.next) {
		//            if ((e.hash == key) && (e.key == key)) {
		//                object      old = e.value;
		//
		//                e.value = value;
		//                return old;
		//            }
		//        }
		//
		//        if (count >= threshold) {
		//            // Rehash the table if the threshold is exceeded.
		//            rehash();
		//            return put(key, value);
		//        }
		//
		//        Entry       e = new Entry();
		//
		//        e.hash = key;
		//        e.key = key;
		//        e.value = value;
		//        e.next = table[index];
		//        table[index] = e;
		//        ++count;
		//        return null;
		//    }
		//
		//    public object remove(int key)
		//    {
		//        int         index = (key & 0x7FFFFFFF) % table.length;
		//
		//        for (Entry e = table[index], prev = null ; e != null ; prev = e, e = e.next) {
		//            if ((e.hash == key) && (e.key == key)) {
		//                if ( prev != null ) {
		//                    prev.next = e.next;
		//                } else {
		//                    table[index] = e.next;
		//                }
		//                --count;
		//                return e.value;
		//            }
		//        }
		//        return null;
		//    }
		//
		//    /*===================================================================
		//        IDictionary interface
		//      ===================================================================*/
		//    public int size()
		//    {
		//    	return count;
		//    }
		//
		//    public bool isEmpty()
		//    {
		//        return count == 0;
		//    }
		//
		//    public object get(object key)
		//	{
		//        if (!(key is Number)) {
		//            throw new IllegalArgumentException("key is not an Number subclass");
		//        }
		//        return get(((Number)key).intValue());
		//	}
		//
		//    public object put(object key, object value)
		//    {
		//        if (!(key is Number)) {
		//            throw new IllegalArgumentException( "key cannot be null" );
		//        }
		//        return put(((Number)key).intValue(), value );
		//    }
		//
		//    public void putAll(IDictionary otherMap)
		//    {
		//        for (Iterator it = otherMap.keySet().iterator(); it.hasNext();) {
		//            object      k = it.next();
		//
		//            put(k, otherMap.get(k));
		//        }
		//    }
		//
		//    public object remove(object key)
		//	{
		//        if (!(key is Number)) {
		//            throw new IllegalArgumentException("key cannot be null");
		//        }
		//    	return remove(((Number)key).intValue());
		//	}
		//
		//    public void clear()
		//	{
		//        Entry[]     tab = table;
		//
		//        for (int index = tab.length; --index >= 0;) {
		//            tab[index] = null;
		//        }
		//        count = 0;
		//	}
		//
		//    public bool containsKey(object key)
		//	{
		//        if (!(key is Number)) {
		//            throw new InternalError( "key is not an Number subclass" );
		//        }
		//        return containsKey(((Number)key).intValue());
		//	}
		//
		//    public bool containsValue(object value)
		//    {
		//        Entry[]       tab = table;
		//
		//        if (value == null) {
		//            throw new IllegalArgumentException();
		//        }
		//        for (int i = tab.length ; i-- > 0;) {
		//            for (Entry e = tab[i] ; e != null ; e = e.next ) {
		//                if (e.value.Equals(value)) {
		//                    return true;
		//                }
		//            }
		//        }
		//        return false;
		//    }
		//
		//    public Set keySet()
		//    {
		//        Set     result = new HashSet();
		//
		//        for (Iterator it = new IntHashMapIterator(table, true); it.hasNext();) {
		//            result.add(it.next());
		//        }
		//        return result;
		//    }
		//
		//    public Collection values()
		//    {
		//        List        result = new ArrayList();
		//
		//        for (Iterator it = new IntHashMapIterator(table, false); it.hasNext();) {
		//            result.add(it.next());
		//        }
		//        return result;
		//    }
		//
		//    public Set entrySet()
		//    {
		//        throw new UnsupportedOperationException("entrySet");
		//    }
	}
}