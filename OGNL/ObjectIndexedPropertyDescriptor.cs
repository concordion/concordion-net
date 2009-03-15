using System.Reflection ;

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
	///<summary>
	///PropertyDescriptor subclass that describes an indexed set of read/write
	///methods to get a property. Unlike IndexedPropertyDescriptor this allows
	///the "key" to be an arbitrary object rather than just an int. 
	///</summary>
	///<remarks>
	///Consequently it does not have a "readMethod" or "writeMethod" because it only expects
	///a pattern like:
	///<code lang="C#">
	///   public void set<i>Property</i>(<i>KeyType</i>, <i>ValueType</i>);
	///   public <i>ValueType</i> get<i>Property</i>(<i>KeyType</i>);
	///</code>
	///<para>and does not require the methods that access it as an array.  OGNL can
	///get away with this without losing functionality because if the object
	///does expose the properties they are most probably in a IDictionary and that case
	///is handled by the normal OGNL property accessors.</para>
	///For example, if an object were to have methods that accessed and "attributes"
	///property it would be natural to index them by string rather than by integer
	///and expose the attributes as a map with a different property name:
	///<code>
	///   public void setAttribute(string name, object value);
	///   public object getAttribute(string name);
	///   public IDictionary getAttributes();
	///</code>
	///Note that the index get/set is called get/set <c>Attribute</c>
	///whereas the collection getter is called <c>Attributes</c>.  This
	///case is handled unambiguously by the OGNL property accessors because the
	///set/get<c>Attribute</c> methods are detected by this object and the
	///"attributes" case is handled by the <c>MapPropertyAccessor</c>.
	///Therefore OGNL expressions calling this code would be handled in the
	///following way:
	///<table>
	/// <tr><th>OGNL Expression</th>
	///     <th>Handling</th>
	/// </tr>
	/// <tr>
	///     <td><code>attribute["name"]</code></td>
	///     <td>Handled by an index getter, like <code>getAttribute(string)</code>.</td>
	/// </tr>
	/// <tr>
	///     <td><code>attribute["name"] = value</code></td>
	///     <td>Handled by an index setter, like <code>setAttribute(string, object)</code>.</td>
	/// </tr>
	/// <tr>
	///     <td><code>attributes["name"]</code></td>
	///     <td>Handled by <code>MapPropertyAccessor</code> via a <code>IDictionary.get()</code>.  This
	///         will <b>not</b> go through the index get accessor.
	///     </td>
	/// </tr>
	/// <tr>
	///     <td><code>attributes["name"] = value</code></td>
	///     <td>Handled by <code>MapPropertyAccessor</code> via a <code>IDictionary.put()</code>.  This
	///         will <b>not</b> go through the index set accessor.
	///     </td>
	/// </tr>
	///</table>
	///</remarks>
	///@author Luke Blanshard (blanshlu@netscape.net)
	///@author Drew Davidson (drew@ognl.org)
	///
	public class ObjectIndexedPropertyDescriptor : IndexedPropertyDescriptor
	{
		public ObjectIndexedPropertyDescriptor (PropertyInfo p)
			: base (p)
		{
		}

	}
}