using System ;
using System.Collections ;

using System.Reflection ;
using System.Text ;

using java ;

//--------------------------------------------------------------------------
//	Copyright (c) 1998-2004, Drew Davidson ,  Luke Blanshard and Foxcoming
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are
//  met:
//
//  Redistributions of source code must retain the above copyright notice,
//  this list of conditions and the following disclaimer.
//  Redistributions in binary form must reproduce the above copyright
//  notice, this list of conditions and the following disclaimer in the
//  documentation and/or other materials provided with the distribution.
//  Neither the name of the Drew Davidson nor the names of its contributors
//  may be used to endorse or promote products derived from this software
//  without specific prior written permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
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
 * This is an abstract class with static methods that define runtime
 * caching information in OGNL.
 * @author Luke Blanshard (blanshlu@netscape.net)
 * @author Drew Davidson (drew@ognl.org)
 */
public abstract class OgnlRuntime
{
    public static object              NotFound = new object();
    public static IList                NotFoundList = new ArrayList();
    public static IDictionary                 NotFoundMap = new Hashtable ();
    public static object[]            NoArguments = new object[] {};
    public static Type[]             NoArgumentTypes = new Type[] {};

    /** Token returned by TypeConverter for no conversion possible */
    public static object              NoConversionPossible = "ognl.NoConversionPossible";

    /** Not an indexed property */
    public static int                       INDEXED_PROPERTY_NONE = 0;
    /** JavaBeans IndexedProperty */
    public static int                       INDEXED_PROPERTY_INT = 1;
    /** OGNL ObjectIndexedProperty */
    public static int                       INDEXED_PROPERTY_OBJECT = 2;

    public static string              NULL_STRING = "" + null;

    private static string             SET_PREFIX = "Set";
    private static string             SET_PREFIX2 = "set";
    private static string             GET_PREFIX = "Get";
    private static string             GET_PREFIX2 = "get";
    private static string             IS_PREFIX = "Is";

    /**
        Prefix padding for hexadecimal numbers to HEX_LENGTH.
     */
    private static IDictionary			    HEX_PADDING = new Hashtable();

    /**
        Hexadecimal prefix for printing "pointers".
     */
    private static string			    HEX_PREFIX = "0x";

    private static int				HEX_LENGTH = 8;
    /**
        Returned by <CODE>getUniqueDescriptor()</CODE> when the
        object is <CODE>null</CODE>.
     */
    private static string      	    NULL_OBJECT_STRING = "<null>";


    private static ClassCache               methodAccessors = new ClassCache();
    private static ClassCache               propertyAccessors = new ClassCache();
    private static ClassCache               elementsAccessors = new ClassCache();
    private static ClassCache               nullHandlers = new ClassCache();
    private static ClassCache               propertyDescriptorCache = new ClassCache();
    private static ClassCache               constructorCache = new ClassCache();
    private static ClassCache               staticMethodCache = new ClassCache();
    private static ClassCache               instanceMethodCache = new ClassCache();
    private static ClassCache               invokePermissionCache = new ClassCache();
    private static ClassCache               fieldCache = new ClassCache();
    private static IList                     superclasses = new ArrayList(); /* Used by fieldCache lookup */
    private static ClassCache[]             declaredMethods = new ClassCache[] { new ClassCache(), new ClassCache() };   /* set, get */
    private static IDictionary                      primitiveTypes = new Hashtable(101);
    private static ClassCache               primitiveDefaults = new ClassCache();
    private static IDictionary                      methodParameterTypesCache = new Hashtable(101);
    private static IDictionary                      ctorParameterTypesCache = new Hashtable(101);
    // private static SecurityManager          securityManager = System.getSecurityManager();
    private static EvaluationPool           evaluationPool = new EvaluationPool();
    private static ObjectArrayPool          objectArrayPool = new ObjectArrayPool();

    /**
        This is a highly specialized map for storing values keyed by Type objects.
     */
    private class ClassCache : object
    {
        /* this MUST be a power of 2 */
        private static int    TABLE_SIZE = 512;

        /* ...and now you see why.  The table size is used as a mask for generating hashes */
        private static int    TABLE_SIZE_MASK = TABLE_SIZE - 1;

        private Entry[]             table;

        internal class Entry : object
        {
            internal Entry                 next;
            internal Type                 key;
            internal object                value;

            public Entry(Type key, object value)
            {
             
                this.key = key;
                this.value = value;
            }
        }

        public ClassCache()
        {
          
            this.table = new Entry[TABLE_SIZE];
        }

        public object get(Type key)
        {
            object      result = null;
            int         i = key.GetHashCode() & TABLE_SIZE_MASK;

            for (Entry entry = table[i]; entry != null; entry = entry.next) {
                if (entry.key == key) {
                    result = entry.value;
                    break;
                }
            }
            return result;
        }

        public object put(Type key, object value)
        {
            object      result = null;
            int         i = key.GetHashCode() & TABLE_SIZE_MASK;
            Entry       entry = table[i];

            if (entry == null) {
                table[i] = new Entry(key, value);
            } else {
                if (entry.key == key) {
                    result = entry.value;
                    entry.value = value;
                } else {
                    while (true) {
                        if (entry.key == key) {
                            /* replace value */
                            result = entry.value;
                            entry.value = value;
                            break;
                        } else {
                            if (entry.next == null) {
                                /* add value */
                                entry.next = new Entry(key, value);
                                break;
                            }
                        }
                        entry = entry.next;
                    }
                }
            }
            return result;
        }
    }

    static OgnlRuntime ()
    {
        PropertyAccessor p = new ArrayPropertyAccessor();
        setPropertyAccessor( typeof (object), new ObjectPropertyAccessor() );
        setPropertyAccessor( typeof (byte[]), p );
        setPropertyAccessor( typeof (short[]), p );
        setPropertyAccessor( typeof (char[]), p );
        setPropertyAccessor( typeof (int[]), p );
        setPropertyAccessor( typeof (long[]), p );
        setPropertyAccessor( typeof (float[]), p );
        setPropertyAccessor( typeof (double[]), p );
        setPropertyAccessor( typeof (object[]), p );
        setPropertyAccessor( typeof (IList), new ListPropertyAccessor() );
        setPropertyAccessor( typeof (IDictionary), new MapPropertyAccessor() );
        setPropertyAccessor( typeof (ICollection), new SetPropertyAccessor() );
        // TODO: Ignore Iterator
		// setPropertyAccessor( typeof (Iterator), new IteratorPropertyAccessor() );
        setPropertyAccessor( typeof (IEnumerator), new EnumerationPropertyAccessor() );

    	IElementsAccessor e = new ArrayElementsAccessor();
        setElementsAccessor( typeof (object), new ObjectElementsAccessor() );
        setElementsAccessor( typeof (byte[]), e );
        setElementsAccessor( typeof (short[]), e );
        setElementsAccessor( typeof (char[]), e );
        setElementsAccessor( typeof (int[]), e );
        setElementsAccessor( typeof (long[]), e );
        setElementsAccessor( typeof (float[]), e );
        setElementsAccessor( typeof (double[]), e );
        setElementsAccessor( typeof (object[]), e );
        setElementsAccessor( typeof (ICollection), new CollectionElementsAccessor() );
        setElementsAccessor( typeof (IDictionary), new MapElementsAccessor() );
        // TODO: ignore Iterator 
		// setElementsAccessor( typeof (Iterator), new IteratorElementsAccessor() );
        setElementsAccessor( typeof (IEnumerator), new EnumerationElementsAccessor() );
        setElementsAccessor( typeof (ValueType), new NumberElementsAccessor() );

        NullHandler nh = new ObjectNullHandler();
        setNullHandler( typeof (object),  nh);
        setNullHandler( typeof (byte[]), nh );
        setNullHandler( typeof (short[]), nh );
        setNullHandler( typeof (char[]), nh );
        setNullHandler( typeof (int[]), nh );
        setNullHandler( typeof (long[]), nh );
        setNullHandler( typeof (float[]), nh );
        setNullHandler( typeof (double[]), nh );
        setNullHandler( typeof (object[]), nh );

        MethodAccessor  ma = new ObjectMethodAccessor();
        setMethodAccessor( typeof (object), ma );
        setMethodAccessor( typeof (byte[]), ma );
        setMethodAccessor( typeof (short[]), ma );
        setMethodAccessor( typeof (char[]), ma );
        setMethodAccessor( typeof (int[]), ma );
        setMethodAccessor( typeof (long[]), ma );
        setMethodAccessor( typeof (float[]), ma );
        setMethodAccessor( typeof (double[]), ma );
        setMethodAccessor( typeof (object[]), ma );

        primitiveTypes ["bool"] = typeof (bool) ;
        primitiveTypes ["byte"] = typeof (byte) ;
        primitiveTypes ["short"] = typeof (short) ;
        primitiveTypes ["char"] = typeof (char) ;
        primitiveTypes ["int"] = typeof (int) ;
        primitiveTypes ["long"] = typeof (long) ;
        primitiveTypes ["float"] = typeof (float) ;
        primitiveTypes ["double"] = typeof (double) ;
		// Add String as primitive 
		primitiveTypes ["string"] = typeof (string) ;
		// Add object as primitive 
		primitiveTypes ["object"] = typeof (object) ;
		// Add decimal as primitive 
		primitiveTypes ["decimal"] = typeof (decimal) ;
		primitiveTypes ["ulong"] = typeof (ulong) ;
		primitiveTypes ["uint"] = typeof (uint) ;
		primitiveTypes ["ushort"] = typeof (ushort) ;
		
		

        primitiveDefaults.put(typeof (bool), false);
        primitiveDefaults.put(typeof (byte), (byte)0);
        primitiveDefaults.put(typeof (short), (short)0);
        primitiveDefaults.put(typeof (char), (char)0);
        primitiveDefaults.put(typeof (int), 0);
        primitiveDefaults.put(typeof (long), 0L);
        primitiveDefaults.put(typeof (float), 0.0f);
        primitiveDefaults.put(typeof (double), 0.0D);
        primitiveDefaults.put(typeof (decimal), (decimal)0);
		// TODO: match BigInteger.
        // primitiveDefaults.put(typeof (), new BigInteger("0"));
    }

    /**
        Gets the "target" class of an object for looking up accessors that
        are registered on the target.  If the object is a Type object this
        will return the Type itself, else it will return object's GetType()
        result.
     */
    public static Type getTargetClass(object o)
    {
        return (o == null) ? null : ((o is Type) ? (Type)o : o.GetType());
    }

    /**
        Returns the base name (the class name without the
        package name prepended) of the object given.
     */
    public static string getBaseName(object o)
    {
        return (o == null) ? null : getClassBaseName(o.GetType());
    }

    /**
        Returns the base name (the class name without the
        package name prepended) of the class given.
     */
    public static string getClassBaseName(Type c)
    {
        string      s = c.Name;

        return s.Substring(s.LastIndexOf('.') + 1);
    }

    public static string getClassName(object o, bool fullyQualified)
    {
    	if (!(o is Type)) {
    		o = o.GetType();
    	}
    	return getClassName((Type)o, fullyQualified);
    }

    public static string getClassName(Type c, bool fullyQualified)
    {
    	return fullyQualified ? c.Name : getClassBaseName(c);
    }

    /**
        Returns the package name of the object's class.
     */
    public static string getPackageName(object o)
    {
        return (o == null) ? null : getClassPackageName(o.GetType());
    }

    /**
        Returns the package name of the class given.
     */
    public static string getClassPackageName(Type c)
    {
        string      s = c.Name;
        int         i = s.LastIndexOf('.');

        return (i < 0) ? null : s.Substring(0, i);
    }

    /**
        Returns a "pointer" string in the usual format for these
        things - 0x<hex digits>.
     */
    public static string getPointerString(int num)
    {
    	StringBuilder	result = new StringBuilder();
    	string			hex = num.ToString ("X") ; // Integer.toHexString(num),
    	string			pad;
		int			l = hex.Length;

        //result.append(HEX_PREFIX);
        if ((pad = (string)HEX_PADDING [l]) == null) {
	    	StringBuilder	pb = new StringBuilder();

	        for (int i = hex.Length; i < HEX_LENGTH; i++) {
	        	pb.Append('0');
	        }
	        pad = pb.ToString ();
	        HEX_PADDING [l] = pad;
	    }
	    result.Append(pad);
        result.Append(hex);
        return result.ToString ();
    }

    /**
        Returns a "pointer" string in the usual format for these
        things - 0x<hex digits> for the object given.  This will
        always return a unique value for each object.
     */
    public static string getPointerString(object o)
    {
		// TODO:use hashcode instead of identity.
        return getPointerString((o == null) ? 0 : o.GetHashCode ()) ; // System.identityHashCode(o));
    }

    /**
        Returns a unique descriptor string that includes the object's
        class and a unique integer identifier.  If fullyQualified is
        true then the class name will be fully qualified to include
        the package name, else it will be just the class' base name.
     */
    public static string getUniqueDescriptor(object obj, bool fullyQualified)
    {
    	StringBuilder	result = new StringBuilder();

        if (obj != null) {
        	/*
			// Following code deal with Proxy. IGNORED.
			if (obj is Proxy) {
        		Type		interfaceClass = obj.GetType().GetInterfaces()[0];

        		result.Append(getClassName(interfaceClass, fullyQualified));
        		result.Append('^');
        		obj = Proxy.getInvocationHandler(obj);
        	}*/
            result.Append(getClassName(obj, fullyQualified));
            result.Append('@');
            result.Append(getPointerString(obj));
        } else {
            result.Append(NULL_OBJECT_STRING);
        }
        return (result.ToString ());
    }

    /**
        Returns a unique descriptor string that includes the object's
        class' base name and a unique integer identifier.
     */
    public static string getUniqueDescriptor(object obj)
    {
        return getUniqueDescriptor(obj, false);
    }

    /**
        Utility to convert a List into an object[] array.  If the list is zero
        elements this will return a constant array; toArray() on List always
        returns a new object and this is wasteful for our purposes.
     */
    public static object[] toArray(IList list)
    {
        object[]        result;
        int             size = list.Count;

        if (size == 0) {
            result = NoArguments;
        } else {
            result = getObjectArrayPool().create(list.Count);
            for (int i = 0; i < size; i++) {
                result[i] = list[i];
            }
        }
        return result;
    }

    /**
        Returns the parameter types of the given method.
     */
    public static Type[] getParameterTypes(MethodInfo m)
    {
        lock (methodParameterTypesCache) {
            Type[]     result;

            if ((result = (Type[])methodParameterTypesCache[m]) == null) {
            	Type[] pts = getParameterTypes0 (m) ;

            	methodParameterTypesCache [m]  = (result = pts);
            }
            return result;
        }
    }

	static Type[] getParameterTypes0 (MethodInfo m)
	{
		// create Parameter type array
		ParameterInfo[] ps = m.GetParameters () ;
		Type [] pts = new Type[ps.Length];
		for (int i = 0; i < ps.Length; i++)
		{
			ParameterInfo pt = ps [i] ;
			pts [i] = pt.ParameterType ;
		}
		return pts ;
	}

	static Type[] getParameterTypes0 (ConstructorInfo m)
	{
		// create Parameter type array
		ParameterInfo[] ps = m.GetParameters () ;
		Type [] pts = new Type[ps.Length];
		for (int i = 0; i < ps.Length; i++)
		{
			ParameterInfo pt = ps [i] ;
			pts [i] = pt.ParameterType ;
		}
		return pts ;
	}
	/**
        Returns the parameter types of the given method.
     */
    public static Type[] getParameterTypes(ConstructorInfo c)
    {
        lock(ctorParameterTypesCache) 
		{
            Type[]     result;

            if ((result = (Type[])ctorParameterTypesCache[c]) == null) {

                ctorParameterTypesCache [c] = (result = getParameterTypes0(c));
            }
            return result;
        }
    }

    /**
        Permission will be named "invoke.<declaring-class>.<method-name>".
     */
    /* // no permission check needed in C#, IGNORED
	public static Permission getPermission(Method method)
    {
        Permission              result = null;
        Type                   mc = method.getDeclaringClass();

        synchronized(invokePermissionCache) {
            IDictionary                     permissions = (IDictionary)invokePermissionCache.get(mc);

            if (permissions == null) {
                invokePermissionCache.put(mc, permissions = new HashMap(101));
            }
            if ((result = (Permission)permissions.get(method.getName())) == null) {
                result = new OgnlInvokePermission("invoke." + mc.getName() + "." + method.getName());
                permissions.put(method.getName(), result);
            }
        }
        return result;
    }
*/
    public static object invokeMethod( object target, MethodInfo method, object[] argsArray ) // throws InvocationTargetException, IllegalAccessException
    {
        object      result;
        bool     wasAccessible = true;

        /* Accessible, IGNORED.
		if (securityManager != null) {
            try {
                securityManager.checkPermission(getPermission(method));
            } catch (SecurityException ex) {
                throw new IllegalAccessException("Method [" + method + "] cannot be accessed.");
            }
        }
        if (!Modifier.isPublic(method.getModifiers()) || !Modifier.isPublic(method.getDeclaringClass().getModifiers())) {
            if (!(wasAccessible = ((AccessibleObject)method).isAccessible())) {
                ((AccessibleObject)method).setAccessible(true);
            }
        }*/
        result = method.Invoke(target, argsArray );
        /*
		if (!wasAccessible) {
            ((AccessibleObject)method).setAccessible(false);
        }
		*/
        return result;
    }

      /**
       * Gets the class for a method argument that is appropriate for looking up methods
       * by reflection, by looking for the standard primitive wrapper classes and
       * exchanging for them their underlying primitive class objects.  Other classes are
       * passed through unchanged.
       *
       * @param arg an object that is being passed to a method
       * @return the class to use to look up the method
       */
    public static Type getArgClass( object arg )
    {
        if ( arg == null )
            return null;
        Type c = arg.GetType();
       /* No used in C#, Ignored 
		if ( c == Boolean )
            return Boolean.TYPE;
        else if ( c.getSuperclass() == Number ) {
            if ( c == Integer )
                return Integer.TYPE;
            if ( c == Double )
                return Double.TYPE;
            if ( c == Byte )
                return Byte.TYPE;
            if ( c == Long )
                return Long.TYPE;
            if ( c == Float )
                return Float.TYPE;
            if ( c == Short )
                return Short.TYPE;
        }
        else if ( c == Character )
            return Character.TYPE;
		*/
        return c;
    }

      /**
       * Tells whether the given object is compatible with the given class
       * ---that is, whether the given object can be passed as an argument
       * to a method or constructor whose parameter type is the given class.
       * If object is null this will return true because null is compatible
       * with any type.
       */
    public static bool isTypeCompatible( object obj, Type c )
    {
        bool         result = true;

        if ( obj != null ) {
            if ( c.IsPrimitive ) {
                if ( getArgClass(obj) != c ) {
                    result = false;
                }
            } else if ( !c.IsInstanceOfType(obj) ) {
                result = false;
            }
        }
        return result;
    }

      /**
       * Tells whether the given array of objects is compatible with the given array of
       * classes---that is, whether the given array of objects can be passed as arguments
       * to a method or constructor whose parameter types are the given array of classes.
       */
    public static bool areArgsCompatible( object[] args, Type[] classes )
    {
        bool     result = true;

        if ( args.Length != classes.Length ) {
            result = false;
        } else {
            for ( int index=0, count=args.Length; result && (index < count); ++index ) {
              result = isTypeCompatible(args[index], classes[index]);
            }
        }
        return result;
    }

      /**
       * Tells whether the first array of classes is more specific than the second.
       * Assumes that the two arrays are of the same length.
       */
    public static bool isMoreSpecific( Type[] classes1, Type[] classes2 )
    {
        for ( int index=0, count=classes1.Length; index < count; ++index )
          {
            Type c1 = classes1[index], c2 = classes2[index];
            if ( c1 == c2 )
                continue;
            else if ( c1.IsPrimitive)
                return true;
            else if ( c1.IsAssignableFrom(c2) )
                return false;
            else if ( c2.IsAssignableFrom(c1) )
                return true;
          }

          // They are the same!  So the first is not more specific than the second.
        return false;
    }

    /* No Used, Ignored.
	public static string getModifierString(int modifiers)
    {
        string      result;

        if (Modifier.isPublic(modifiers))
            result = "public";
        else
        if (Modifier.isProtected(modifiers))
            result = "protected";
        else
        if (Modifier.isPrivate(modifiers))
            result = "private";
        else
            result = "";
        if (Modifier.isStatic(modifiers))
            result = "static " + result;
        if (Modifier.isFinal(modifiers))
            result = "" + result;
        if (Modifier.isNative(modifiers))
            result = "native " + result;
        if (Modifier.isSynchronized(modifiers))
            result = "synchronized " + result;
        if (Modifier.isTransient(modifiers))
            result = "transient " + result;
        return result;
    }
	*/
    public static Type classForName( OgnlContext context, string className ) // throws ClassNotFoundException
    {
        Type           result = (Type)primitiveTypes[className];

        if (result == null) {
            ClassResolver   resolver;

            if ((context == null) || ((resolver = context.getClassResolver()) == null)) {
                resolver = OgnlContext.DEFAULT_CLASS_RESOLVER;
            }
            result = resolver.classForName (className, context);
        }
        return result;
    }

    public static bool isInstance( OgnlContext context, object value, string className ) // throws OgnlException
    {
        try
          {
            Type c = classForName( context, className);
            return c.IsInstanceOfType( value );
          }
		// TODO: ClassNotFoundException 
        catch (Exception e)
          {
            throw new OgnlException( "No such class: " + className, e );
          }
    }

    public static object getPrimitiveDefaultValue( Type forClass )
    {
        return primitiveDefaults.get(forClass);
    }

    public static object getConvertedType( OgnlContext context, object target, MemberInfo member, string propertyName, object value, Type type)
    {
        return context.getTypeConverter().convertValue(context, target, member, propertyName, value, type);
    }

    public static bool getConvertedTypes( OgnlContext context, object target, MemberInfo member, string propertyName, Type[] parameterTypes, object[] args, object[] newArgs)
    {
        bool         result = false;

        if (parameterTypes.Length == args.Length) {
            result = true;
            for (int i = 0, ilast = parameterTypes.Length - 1; result && (i <= ilast); i++) {
                object      arg = args[i];
                Type       type = parameterTypes[i];

                if (isTypeCompatible(arg, type)) {
                    newArgs[i] = arg;
                } else {
                    object      v = getConvertedType(context, target, member, propertyName, arg, type);

                    if (v == OgnlRuntime.NoConversionPossible) {
                        result = false;
                    } else {
                        newArgs[i] = v;
                    }
                }
            }
        }
        return result;
    }

    public static MethodInfo getConvertedMethodAndArgs( OgnlContext context, object target, string propertyName, IList methods, object[] args, object[] newArgs)
    {
        MethodInfo          result = null;
        TypeConverter   converter = context.getTypeConverter();

        if ((converter != null) && (methods != null)) {
            for (int i = 0, icount = methods.Count; (result == null) && (i < icount); i++) {
                MethodInfo      m = (MethodInfo)methods [i];
                Type[]     parameterTypes = getParameterTypes(m);

                if (getConvertedTypes( context, target, m, propertyName, parameterTypes, args, newArgs )) {
                    result = m;
                }
            }
        }
        return result;
    }

    public static ConstructorInfo getConvertedConstructorAndArgs( OgnlContext context, object target, IList constructors, object[] args, object[] newArgs )
    {
        ConstructorInfo     result = null;
        TypeConverter   converter = context.getTypeConverter();

        if ((converter != null) && (constructors != null)) {
            for (int i = 0, icount = constructors.Count; (result == null) && (i < icount); i++) {
                ConstructorInfo     ctor = (ConstructorInfo)constructors [i];
                Type[]         parameterTypes = getParameterTypes(ctor);

                if (getConvertedTypes( context, target, ctor, null, parameterTypes, args, newArgs )) {
                    result = ctor;
                }
            }
        }
        return result;
    }

    /**
        Gets the appropriate method to be called for the given target, method name and arguments.
        If successful this method will return the Method within the target that can be called
        and the converted arguments in actualArgs.  If unsuccessful this method will return
        null and the actualArgs will be empty.
     */
    public static MethodInfo getAppropriateMethod( OgnlContext context, object source, object target, string methodName, string propertyName, IList methods, object[] args, object[] actualArgs )
    {
        MethodInfo      result = null;
        Type[]     resultParameterTypes = null;

        if (methods != null) {
            for (int i = 0, icount = methods.Count; i < icount; i++) {
                MethodInfo  m = (MethodInfo)methods [i];
                Type[] mParameterTypes = getParameterTypes(m);

                if ( areArgsCompatible(args, mParameterTypes) && ((result == null) || isMoreSpecific(mParameterTypes, resultParameterTypes)) ) {
                    result = m;
                    resultParameterTypes = mParameterTypes;
                    Array.Copy(args, 0, actualArgs, 0, args.Length);
                    for (int j = 0; j < mParameterTypes.Length; j++) {
                        Type       type = mParameterTypes[j];

                        if (type.IsPrimitive && (actualArgs[j] == null)) {
                            actualArgs[j] = getConvertedType(context, source, result, propertyName, null, type);
                        }
                    }
                }
            }
        }
        if ( result == null ) {
            result = getConvertedMethodAndArgs( context, target, propertyName, methods, args, actualArgs );
        }
        return result;
    }

    public static object callAppropriateMethod( OgnlContext context, object source, object target, string methodName, string propertyName, IList methods, object[] args ) // throws MethodFailedException
    {
        Exception   reason = null;
        object[]    actualArgs = objectArrayPool.create(args.Length);

        try {
            MethodInfo method = getAppropriateMethod( context, source, target, methodName, propertyName, methods, args, actualArgs );

            if ( (method == null) || !isMethodAccessible(context, source, method, propertyName) )
            {
                StringBuilder        buffer = new StringBuilder();

                if (args != null) {
                    for (int i = 0, ilast = args.Length - 1; i <= ilast; i++) {
                        object      arg = args[i];

                        buffer.Append((arg == null) ? NULL_STRING : arg.GetType().Name);
                        if (i < ilast) {
                            buffer.Append(", ");
                        }
                    }
                }
                throw new MissingMethodException(methodName + "(" + buffer + ")" );
            }
            return invokeMethod(target, method, actualArgs);
          }
		catch (TargetInvocationException e)
          { reason = e.InnerException;}
        catch (Exception e)
          { reason = e; }
        
        finally {
            objectArrayPool.recycle(actualArgs);
        }
        throw new MethodFailedException( source, methodName, reason );
    }

    public static object callStaticMethod( OgnlContext context, string className, string methodName, object[] args ) // throws OgnlException, MethodFailedException
    {
        try {
            object          result;
            Type           targetClass = classForName(context, className);
            MethodAccessor  ma = getMethodAccessor(targetClass);

            return ma.callStaticMethod(context, targetClass, methodName, args);
        } catch (TypeLoadException ex) {
            throw new MethodFailedException(className, methodName, ex);
        }
    }

    public static object callMethod( OgnlContext context, object target, string methodName, string propertyName, object[] args ) // throws OgnlException, MethodFailedException
    {
        object          result;

        if (target != null) {
            MethodAccessor  ma = getMethodAccessor(target.GetType());

            result = ma.callMethod(context, target, methodName, args);
        } else {
            throw new NullReferenceException("target is null for method " + methodName);
        }
        return result;
    }

    public static object callConstructor( OgnlContext context, string className, object[] args ) // throws OgnlException
    {
        Exception       reason = null;
        object[]        actualArgs = args;

        try
          {
            ConstructorInfo     ctor = null;
            Type[]         ctorParameterTypes = null;
            Type           target = classForName(context, className);
            IList            constructors = getConstructors(target);

            for (int i = 0, icount = constructors.Count; i < icount; i++)
            {
                ConstructorInfo     c = (ConstructorInfo)constructors [i];
                Type[]         cParameterTypes = getParameterTypes(c);

                if ( areArgsCompatible(args, cParameterTypes) && (ctor == null || isMoreSpecific(cParameterTypes, ctorParameterTypes)) ) {
                    ctor = c;
                    ctorParameterTypes = cParameterTypes;
                }
              }
            if ( ctor == null )
            {
                actualArgs = objectArrayPool.create(args.Length);
                if ((ctor = getConvertedConstructorAndArgs( context, target, constructors, args, actualArgs )) == null) {
                    throw new MissingMethodException();
                }
            }
            /* // Ignore Access check. IGNORED.
			if (!context.getMemberAccess().isAccessible(context, target, ctor, null)) {
                throw new IllegalAccessException("access denied to " + target.getName() + "()");
            }
			*/
            return ctor.Invoke( actualArgs );
          }
        catch (TypeLoadException e)
          { reason = e; }
        catch (MissingMethodException e)
          { reason = e; }
        catch (MethodAccessException e)
          { reason = e; }
        catch (TargetInvocationException e)
          { reason = e.InnerException; }
        catch (TypeInitializationException e)
          { reason = e; }
        finally {
            if (actualArgs != args) {
                objectArrayPool.recycle(actualArgs);
            }
        }

        throw new MethodFailedException( className, "new", reason );
    }

    public static object getMethodValue(OgnlContext context, object target, string propertyName) // throws OgnlException, IllegalAccessException, NoSuchMethodException, IntrospectionException
    {
        return getMethodValue(context, target, propertyName, false);
    }

    /**
        If the checkAccessAndExistence flag is true this method will check to see if the
        method exists and if it is accessible according to the context's MemberAccess.
        If neither test passes this will return NotFound.
     */
    public static object getMethodValue(OgnlContext context, object target, string propertyName, bool checkAccessAndExistence) // throws OgnlException, IllegalAccessException, NoSuchMethodException, IntrospectionException
    {
        object              result = null;
        MethodInfo              m = getGetMethod(context, (target == null) ? null : target.GetType(), propertyName);

        // check accessible, IGNORED.
		if (checkAccessAndExistence) {
            if ((m == null) 
				/* || !context.getMemberAccess().isAccessible(context, target, m, propertyName) */) 
			{
                result = NotFound;
            }
        }
		
        if (result == null) {
            if (m != null)
            {
                try
                {
                    result = invokeMethod(target, m, NoArguments);
                }
                catch (TargetInvocationException ex)
                {
                    throw new OgnlException(propertyName, ex.InnerException);
                }
            } else {
                throw new MissingMethodException(propertyName);
            }
        }
        return result;
    }

    public static bool setMethodValue(OgnlContext context, object target, string propertyName, object value) // throws OgnlException, IllegalAccessException, NoSuchMethodException, MethodFailedException, IntrospectionException
    {
        return setMethodValue(context, target, propertyName, value, false);
    }

    public static bool setMethodValue(OgnlContext context, object target, string propertyName, object value, bool checkAccessAndExistence) 
    {
        bool     result = true;
        MethodInfo      m = getSetMethod(context, (target == null) ? null : target.GetType(), propertyName);

        /*
		// Ignored.
		if (checkAccessAndExistence) {
            if ((m == null) || !context.getMemberAccess().isAccessible(context, target, m, propertyName)) {
                result = false;
            }
        }
		*/
        if (result) {
            if (m != null) {
                object[]        args = objectArrayPool.create(value);

                try {
                    callAppropriateMethod(context, target, target, m.Name, propertyName, Util.NCopies(1, m), args);
                } finally {
                    objectArrayPool.recycle(args);
                }
            } else {
                result = false;
            }
        }
        return result;
    }

    public static IList getConstructors(Type targetClass)
    {
        IList        result;

        lock(constructorCache) {
            if ((result = (IList)constructorCache.get(targetClass)) == null) {
				// TODO: Get Constructors.
                constructorCache.put(targetClass, result = new ArrayList(targetClass.GetConstructors()));
            }
        }
        return result;
    }

    public static IDictionary getMethods( Type targetClass, bool staticMethods )
    {
        ClassCache  cache = (staticMethods ? staticMethodCache : instanceMethodCache);
        IDictionary         result;

        lock(cache) {
            if ((result = (IDictionary)cache.get(targetClass)) == null)
            {
                cache.put(targetClass, result = new Hashtable(23));
				Type c = targetClass ;
                /*for (Type c = targetClass; c != null; c = c.BaseType) {*/
					// TODO: getDeclaredMethods, Ignore, Just return full list.
					// Only public method here.
                    MethodInfo[]        ma = c.GetMethods();

                    for (int i = 0, icount = ma.Length; i < icount; i++)
                    {
                        if (ma [i].IsStatic == staticMethods) {
                            IList        ml = (IList)result[ma[i].Name];

                            if (ml == null)
                                result [ma[i].Name] = (ml = new ArrayList());
                            ml.Add(ma[i]);
                        }
                    }
                 /*}*/
            }
        }
        return result;
    }

    public static IList getMethods( Type targetClass, string name, bool staticMethods )
    {
        return (IList)getMethods(targetClass, staticMethods) [name];
    }

    public static IDictionary getFields(Type targetClass)
    {
        IDictionary         result;

        lock(fieldCache) {
            if ((result = (IDictionary)fieldCache.get(targetClass)) == null)
            {
                FieldInfo       []fa;

                result = new Hashtable(23);
				// TODO: getDeclaredFields 
				// Ignore Just Get All Fields
                /* fa = targetClass.getDeclaredFields(); */
				fa = targetClass.GetFields ();
                for (int i = 0; i < fa.Length; i++) {
                    result [fa[i].Name] = fa[i];
                }
                fieldCache.put(targetClass, result);
            }
        }
        return result;
    }

    public static FieldInfo getField(Type inClass, string name)
    {
        FieldInfo       result = null;

        lock(fieldCache)
        {
            object      o = getFields(inClass) [name];
			// No Need to look up base class, Skip it.
            if (false/*o == null*/)
            {
                superclasses.Clear();
                for (Type sc = inClass; (sc != null) && (result == null); sc = sc.BaseType)
                {
                    if ((o = getFields(sc) [name]) == NotFound)
                        break;
                    superclasses.Add(sc);
                    if ((result = (FieldInfo)o) != null)
                        break;
                }
                /*
                    Bubble the found value (either cache miss or actual field)
                    to all supeclasses that we saw for quicker access next time.
                */
                for (int i = 0, icount = superclasses.Count; i < icount; i++)
                {
                    getFields((Type)superclasses [i]) [name] = ((result == null) ? NotFound : result);
                }
            }
            else
            {
                if (o is FieldInfo)
                {
                    result = (FieldInfo)o;
                }
                else
                {
                    if (result == NotFound)
                        result = null;
                }
            }
        }
        return result;
    }

    public static object getFieldValue(OgnlContext context, object target, string propertyName) // throws NoSuchFieldException
    {
        return getFieldValue(context, target, propertyName, false);
    }

    public static object getFieldValue(OgnlContext context, object target, string propertyName, bool checkAccessAndExistence) // throws NoSuchFieldException
    {
        object          result = null;
        FieldInfo           f = getField((target == null) ? null : target.GetType(), propertyName);

		/* Ignored.
        if (checkAccessAndExistence) {
            if ((f == null) || !context.getMemberAccess().isAccessible(context, target, f, propertyName)) {
                result = NotFound;
            }
        }
		*/
        if (result == null) {
            if (f == null) {
                throw new MissingFieldException(propertyName);
            } else {
                try
                {
                    object      state = null;

                    if ((f != null) && ! f.IsStatic)
                    {
                        state = context.getMemberAccess().setup(context, target, f, propertyName);
                        result = f.GetValue(target);
                        context.getMemberAccess().restore(context, target, f, propertyName, state);
                    }
                    else
                        throw new MissingFieldException(propertyName);
                }
                catch (MemberAccessException ex)
                {
                    throw new MissingFieldException(propertyName);
                }
            }
        }
        return result;
    }

    public static bool setFieldValue(OgnlContext context, object target, string propertyName, object value) // throws OgnlException
    {
        bool         result = false;

        try
        {
            FieldInfo       f = getField( (target == null) ? null : target.GetType(), propertyName );
            object      state;

            if ((f != null) && ! f.IsStatic)
            {
                state = context.getMemberAccess().setup(context, target, f, propertyName);
                try
                {
                    if (isTypeCompatible(value, f.FieldType) || ((value = getConvertedType( context, target, f, propertyName, value, f.FieldType)) != null)) {
                        f.SetValue(target, value);
                        result = true;
                    }
                }
                finally
                {
                    context.getMemberAccess().restore(context, target, f, propertyName, state);
                }
            }
        }
        catch (MemberAccessException ex)
        {
            throw new NoSuchPropertyException(target, propertyName, ex);
        }
        return result;
    }

    public static bool isFieldAccessible(OgnlContext context, object target, Type inClass, string propertyName)
    {
        return isFieldAccessible(context, target, getField(inClass, propertyName), propertyName);
    }

    public static bool isFieldAccessible(OgnlContext context, object target, FieldInfo field, string propertyName)
    {
        return context.getMemberAccess().isAccessible(context, target, field, propertyName);
    }

    public static bool hasField(OgnlContext context, object target, Type inClass, string propertyName)
    {
        FieldInfo       f = getField(inClass, propertyName);

        return (f != null) && isFieldAccessible(context, target, f, propertyName);
    }

    public static object getStaticField( OgnlContext context, string className, string fieldName ) // throws OgnlException
    {
        Exception reason = null;
        try
          {
            Type c = classForName(context, className);

            /*
                Check for virtual static field "class"; this cannot interfere with
                normal static fields because it is a reserved word.
             */
            if (fieldName.Equals("class"))
              {
                return c;
              }
            else
              {
                FieldInfo f = c.GetField(fieldName);
				if (f == null)
				{
					// try to load Property 
					PropertyInfo p = c.GetProperty (fieldName) ;
					if (p == null)
						throw new MissingFieldException ("Field or Property " + fieldName + " of class " + className + " is not found.") ;
					else
					if (! p.GetAccessors () [0].IsStatic)
						throw new MissingFieldException ("Property " + fieldName + " of class " + className + " is not static.") ;
					else
					if (! p.CanRead)
						throw new MissingFieldException ("Property " + fieldName + " of class " + className + " is write-only.") ;
					else
					return p.GetValue (null , new object[0]) ;
				}
				else
                if ( !f.IsStatic )
                    throw new OgnlException( "Field " + fieldName + " of class " + className + " is not static" );
                return f.GetValue(null);
              }
          }
        catch (TypeLoadException e)
          { reason = e; }
        catch (MissingFieldException e)
          { reason = e; }
        catch (MemberAccessException e)
          { reason = e; }

        throw new OgnlException( "Could not get static field " + fieldName + " from class " + className, reason );
    }

    public static IList getDeclaredMethods(Type targetClass, string propertyName, bool findSets)
    {
        IList        result = null;
        ClassCache  cache = declaredMethods[findSets ? 0 : 1];

        lock(cache) {
            IDictionary         propertyCache = (IDictionary)cache.get(targetClass);

            if ((propertyCache == null) || ((result = (IList)propertyCache [propertyName]) == null)) {
                string      baseName = propertyName.Substring (0 , 1).ToUpper () + propertyName.Substring(1);
                int         len = baseName.Length;

				Type c = targetClass;
                /*for (Type c = targetClass; c != null; c = c.BaseType) {*/
					// TODO: Get Declared Methods.
                    MethodInfo []        methods = c.GetMethods();

                    for (int i = 0; i < methods.Length; i++) {
                        string      ms = methods[i].Name ;

                        if (ms.EndsWith(baseName)) {
                            bool     isSet = false,
                                        isGet = false,
                                        isIs = false;

                            if ((isSet = ms.StartsWith(SET_PREFIX)) || (isGet = ms.StartsWith(GET_PREFIX)) || (isIs = ms.StartsWith(IS_PREFIX))) {
                                int     prefixLength = (isIs ? 2 : 3);

                                if (isSet == findSets) {
                                    if (baseName.Length == (ms.Length - prefixLength)) {
                                        if (result == null) {
                                            result = new ArrayList();
                                        }
                                        result.Add(methods[i]);
                                    }
                                }
                            }
                        }
                    /*}*/
                }
                if (propertyCache == null) {
                    cache.put(targetClass, propertyCache = new Hashtable(101));
                }
                propertyCache [propertyName] = ((result == null) ? NotFoundList : result);
            }
            return (result == NotFoundList) ? null : result;
        }
    }

    public static MethodInfo getGetMethod(OgnlContext context, Type targetClass, string propertyName) // throws IntrospectionException, OgnlException
    {
        MethodInfo              result = null;
    	PropertyDescriptor  pd = getPropertyDescriptor(targetClass, propertyName);

        if (pd == null) {
            IList        methods = getDeclaredMethods(targetClass, propertyName, false /* find 'get' methods */);

            if (methods != null) {
                for (int i = 0, icount = methods.Count; i < icount; i++) {
                    MethodInfo      m = (MethodInfo)methods [(i)];
                    Type[]     mParameterTypes = getParameterTypes(m);

                    if (mParameterTypes.Length == 0) {
                        result = m;
                        break;
                    }
                }
            }
        } else {
            result = pd.getReadMethod();
        }
        return result;
    }

    public static bool isMethodAccessible(OgnlContext context, object target, MethodInfo method, string propertyName)
    {
        return (method == null) ? false : context.getMemberAccess().isAccessible(context, target, method, propertyName);
    }

    public static bool hasGetMethod(OgnlContext context, object target, Type targetClass, string propertyName) // throws IntrospectionException, OgnlException
    {
        return isMethodAccessible(context, target, getGetMethod(context, targetClass, propertyName), propertyName);
    }

    public static MethodInfo getSetMethod(OgnlContext context, Type targetClass, string propertyName) // throws IntrospectionException, OgnlException
    {
        MethodInfo              result = null;
    	PropertyDescriptor  pd = getPropertyDescriptor(targetClass, propertyName);

        if (pd == null) {
            IList        methods = getDeclaredMethods(targetClass, propertyName, true /* find 'set' methods */);

            if (methods != null) {
                for (int i = 0, icount = methods.Count ; i < icount; i++) {
                    MethodInfo      m = (MethodInfo)methods [i];
                    Type[]     mParameterTypes = getParameterTypes(m);

                    if (mParameterTypes.Length == 1) {
                        result = m;
                        break;
                    }
                }
            }
        } else {
            result = pd.getWriteMethod();
        }
        return result;
    }

    public static bool hasSetMethod(OgnlContext context, object target, Type targetClass, string propertyName) // throws IntrospectionException, OgnlException
    {
        return isMethodAccessible(context, target, getSetMethod(context, targetClass, propertyName), propertyName);
    }

    public static bool hasGetProperty( OgnlContext context, object target, object oname ) // throws IntrospectionException, OgnlException
    {
        Type       targetClass = (target == null) ? null : target.GetType();
        string      name = oname.ToString();

        return hasGetMethod( context, target, targetClass, name ) || hasField( context, target, targetClass, name );
    }

    public static bool hasSetProperty( OgnlContext context, object target, object oname ) // throws IntrospectionException, OgnlException
    {
        Type       targetClass = (target == null) ? null : target.GetType();
        string      name = oname.ToString();

        return hasSetMethod( context, target, targetClass, name ) || hasField( context, target, targetClass, name );
    }

    private static bool indexMethodCheck(IList methods)
    {
        bool         result = false;

        if (methods.Count > 0) {
            MethodInfo          fm = (MethodInfo)methods [(0)];
            Type[]         fmpt = getParameterTypes(fm);
            int             fmpc = fmpt.Length;
			
            Type           lastMethodClass = fm.DeclaringType;

            result = true;
            for (int i = 1; result && (i < methods.Count); i++) {
                MethodInfo      m = (MethodInfo)methods [(i)];
				
                Type       c = m.DeclaringType;

                // Check to see if more than one method implemented per class
                if (lastMethodClass == c) {
                    result = false;
                } else {
                    Type[]     mpt = getParameterTypes(fm);
                    int         mpc = fmpt.Length;

                    if (fmpc != mpc) {
                        result = false;
                    }
                    for (int j = 0; j < fmpc; j++) {
                        if (fmpt[j] != mpt[j]) {
                            result = false;
                            break;
                        }
                    }
                }
                lastMethodClass = c;
            }
        }
        return result;
    }

	/* No used method */
    private static void findObjectIndexedPropertyDescriptors(Type targetClass, IDictionary intoMap) // throws OgnlException
    {
		// TODO: Use Indexed Property.
        IDictionary     allMethods = getMethods(targetClass, false);
        IDictionary     pairs = new Hashtable(101);

        for (IEnumerator it = allMethods.Keys.GetEnumerator (); it.MoveNext(); ) {
            string      methodName = (string)it.Current;
            IList        methods = (IList)allMethods [(methodName)];

            /*
                Only process set/get where there is exactly one implementation
                of the method per class and those implementations are all the
                same
             */
            if (indexMethodCheck(methods)) {
                bool     isGet = false,
                            isSet = false;
                MethodInfo      m = (MethodInfo)methods [0];

                if (((isSet = methodName.StartsWith(SET_PREFIX)) || (isGet = methodName.StartsWith(GET_PREFIX))) && (methodName.Length > 3)) {
                    string      propertyName = /*Introspector.decapitalize*/(methodName.Substring(3));
                    Type[]     parameterTypes = getParameterTypes(m);
                    int         parameterCount = parameterTypes.Length;

                    if (isGet && (parameterCount == 1) && (m.ReturnType != typeof (void))) {
                        IList        pair = (IList)pairs [(propertyName)];

                        if (pair == null) {
                            pairs [propertyName] = (pair = new ArrayList());
                        }
                        pair.Add(m);
                    }
                    if (isSet && (parameterCount == 2) && (m.ReturnType == typeof (void))) {
                        IList        pair = (IList)pairs [(propertyName)];

                        if (pair == null) {
                            pairs [propertyName] = (pair = new ArrayList());
                        }
                        pair.Add(m);
                    }
                }
            }
        }
        for (IEnumerator it = pairs.Keys.GetEnumerator(); it.MoveNext();) {
            string      propertyName = (string)it.Current;
            IList        methods = (IList)pairs [(propertyName)];

            if (methods.Count == 2) {
                MethodInfo      method1 = (MethodInfo)methods [(0)],
                            method2 = (MethodInfo)methods [(1)],
                            setMethod = (method1.GetParameters().Length == 2) ? method1 : method2,
                            getMethod = (setMethod == method1) ? method2 : method1;
                Type        keyType = getMethod.GetParameters()[0].ParameterType;
                Type        propertyType = getMethod.ReturnType;

                if (keyType == setMethod.GetParameters()[0].ParameterType) {
                    if (propertyType == setMethod.GetParameters()[1].ParameterType) {
                        ObjectIndexedPropertyDescriptor     propertyDescriptor;

                        try {
                            propertyDescriptor = null ; // new ObjectIndexedPropertyDescriptor(propertyName, propertyType, getMethod, setMethod);
                        } catch (Exception ex) {
                            throw new OgnlException("creating object indexed property descriptor for '" + propertyName + "' in " + targetClass, ex);
                        }
                        intoMap [propertyName] = (propertyDescriptor);
                    }
                }

            }
        }
    }

    /**
        This method returns the property descriptors for the given class as a IDictionary
     */
    public static IDictionary getPropertyDescriptors(Type targetClass) // throws IntrospectionException, OgnlException
    {
		// TODO: This is the main method about PropertyDescriptor. 
        IDictionary     result;

        lock(propertyDescriptorCache) {
            if ((result = (IDictionary)propertyDescriptorCache.get(targetClass)) == null) {
				// TODO: Introspector
				// No Setter or Getter, Use property.
                PropertyDescriptor[]    pda = Introspector.getPropertyDescriptors(targetClass);

                result = new Hashtable(101);
                for (int i = 0, icount = pda.Length; i < icount; i++) {
                    result [pda[i].getName()] =pda[i];
                }
                // findObjectIndexedPropertyDescriptors(targetClass, result);
                findBeanPropertyDescriptors(targetClass, result);
                propertyDescriptorCache.put(targetClass, result);
            }
        }
        return result;
    }

	private static void findBeanPropertyDescriptors (Type targetClass, IDictionary map)
	{
		IDictionary     allMethods = getMethods(targetClass, false);
        IDictionary     pairs = new Hashtable(101);

        for (IEnumerator it = allMethods.Keys.GetEnumerator (); it.MoveNext(); ) {
            string      methodName = (string)it.Current;
            IList        methods = (IList)allMethods [(methodName)];

            /*
                Only process set/get where there is exactly one implementation
                of the method per class and those implementations are all the
                same
             */
            if (beanMethodCheck(methods)) {
                bool     isGet = false,
                            isSet = false;
                MethodInfo      m = (MethodInfo)methods [0];

                if (((isSet = methodName.StartsWith(SET_PREFIX) || methodName.StartsWith (SET_PREFIX2)) || 
					(isGet = methodName.StartsWith(GET_PREFIX) || methodName.StartsWith (GET_PREFIX2))) && 
					(methodName.Length > 3)) {
                    string      propertyName = /*Introspector.decapitalize*/(methodName.Substring(3));
                    Type[]     parameterTypes = getParameterTypes(m);
                    int         parameterCount = parameterTypes.Length;

					// Ignore property with same name.
					if (map.Contains (propertyName))
						continue ;
                    if (isGet && (parameterCount == 0) && (m.ReturnType != typeof (void))) {
                        IList        pair = (IList)pairs [(propertyName)];

                        if (pair == null) {
                            pairs [propertyName] = (pair = new ArrayList());
                        }
                        pair.Add(m);
                    }
                    if (isSet && (parameterCount == 1) && (m.ReturnType == typeof (void))) {
                        IList        pair = (IList)pairs [(propertyName)];

                        if (pair == null) {
                            pairs [propertyName] = (pair = new ArrayList());
                        }
                        pair.Add(m);
                    }
                }
            }
        }

		for (IEnumerator it = pairs.Keys.GetEnumerator(); it.MoveNext();) 
		{
			string      propertyName = (string)it.Current;
			IList        methods = (IList)pairs [(propertyName)];

			
			// Read/write only property is allowded .
			if (methods.Count == 1)
			{
				MethodInfo      method = (MethodInfo)methods [(0)] ,
				setMethod = (method.GetParameters().Length == 1) ? method : null,
					getMethod = (setMethod == method) ? null : method;

				Type        propertyType = getMethod != null ? getMethod.ReturnType : setMethod.GetParameters () [0].ParameterType;
				PropertyDescriptor propertyDescriptor = new BeanPropertyDescriptor (propertyName, propertyType, getMethod, setMethod) ;
				map [propertyName] = propertyDescriptor;
			}
			if (methods.Count == 2) 
			{

				MethodInfo      method1 = (MethodInfo)methods [(0)],
					method2 = (MethodInfo)methods [(1)],
					setMethod = (method1.GetParameters().Length == 1) ? method1 : method2,
					getMethod = (setMethod == method1) ? method2 : method1;
				// Type        keyType = getMethod.GetParameters()[0].ParameterType;
				Type        propertyType = getMethod.ReturnType;

				if (propertyType == setMethod.GetParameters()[0].ParameterType) 
				{
					PropertyDescriptor propertyDescriptor = new BeanPropertyDescriptor (propertyName, propertyType, getMethod, setMethod) ;
					map [propertyName] = propertyDescriptor;
				}
			}
		}

	}
	private static bool beanMethodCheck(IList methods)
	{
		return true ;
	}

	/**
	 * TODO: About PropertyDescriptor
        This method returns a PropertyDescriptor for the given class and property name using
        a IDictionary lookup (using getPropertyDescriptorsMap()).
     */
    public static PropertyDescriptor getPropertyDescriptor(Type targetClass, string propertyName) // throws IntrospectionException, OgnlException
    {
        return (targetClass == null) ? null : (PropertyDescriptor)getPropertyDescriptors(targetClass) [(propertyName)];
    }

    public static PropertyDescriptor[] getPropertyDescriptorsArray(Type targetClass) // throws IntrospectionException
    {
        PropertyDescriptor[]    result = null;

        if (targetClass != null) {
            lock(propertyDescriptorCache) {
                if ((result = (PropertyDescriptor[])propertyDescriptorCache.get(targetClass)) == null) {
                    propertyDescriptorCache.put(targetClass, result = Introspector.getPropertyDescriptors(targetClass));
                }
            }
        }
        return result;
    }

    /**
        Gets the property descriptor with the given name for the target class given.
        @param targetClass      Type for which property descriptor is desired
        @param name             Name of property
        @return                 PropertyDescriptor of the named property or null if
                                the class has no property with the given name
     */
    public static PropertyDescriptor getPropertyDescriptorFromArray(Type targetClass, string name) // throws IntrospectionException
    {
        PropertyDescriptor      result = null;
        PropertyDescriptor[]    pda = getPropertyDescriptorsArray(targetClass);

        for (int i = 0, icount = pda.Length; (result == null) && (i < icount); i++) {
            if (pda[i].getName().CompareTo(name) == 0) {
                result = pda[i];
            }
        }
        return result;
    }

    public static void setMethodAccessor(Type cls, MethodAccessor accessor)
    {
        lock(methodAccessors) {
            methodAccessors.put( cls, accessor );
        }
    }

    public static MethodAccessor getMethodAccessor( Type cls ) // throws OgnlException
    {
        MethodAccessor answer = (MethodAccessor)getHandler( cls, methodAccessors );
        if ( answer != null )
            return answer;
        throw new OgnlException( "No method accessor for " + cls );
    }

    public static void setPropertyAccessor(Type cls, PropertyAccessor accessor)
    {
        lock(propertyAccessors) {
            propertyAccessors.put( cls, accessor );
        }
    }

    public static PropertyAccessor getPropertyAccessor( Type cls ) // throws OgnlException
    {
        PropertyAccessor answer = (PropertyAccessor)getHandler( cls, propertyAccessors );
        if ( answer != null )
            return answer;

        throw new OgnlException( "No property accessor for class " + cls );
    }

    public static IElementsAccessor getElementsAccessor( Type cls ) // throws OgnlException
    {
    	IElementsAccessor answer = (IElementsAccessor)getHandler( cls, elementsAccessors );
        if ( answer != null )
            return answer;
        throw new OgnlException( "No elements accessor for class " + cls );
    }

    public static void setElementsAccessor( Type cls, IElementsAccessor accessor )
    {
        lock(elementsAccessors) {
            elementsAccessors.put( cls, accessor );
        }
    }

    public static NullHandler getNullHandler( Type cls ) // throws OgnlException
    {
        NullHandler answer = (NullHandler)getHandler( cls, nullHandlers );
        if ( answer != null )
            return answer;
        throw new OgnlException( "No null handler for class " + cls );
    }

    public static void setNullHandler( Type cls, NullHandler handler )
    {
        lock(nullHandlers) {
            nullHandlers.put( cls, handler );
        }
    }

    private static object getHandler( Type forClass, ClassCache handlers )
    {
        object answer = null;

        lock(handlers) {
            if ((answer = handlers.get(forClass)) == null)
            {
                Type   keyFound;

                if (forClass.IsArray)
                {
                    answer = handlers.get(typeof (object[]));
                    keyFound = null;
                }
                else
                {
                    keyFound = forClass;
                    // outer:
                        for ( Type c = forClass; c != null; c = c.BaseType )
                        {
                            answer = handlers.get(c);
                            if ( answer == null )
                            {
                                Type[] interfaces = c.GetInterfaces();
                                for ( int index=0, count=interfaces.Length; index < count; ++index )
                                {
                                    Type   iface = interfaces[index];

                                    answer = handlers.get(iface);
                                    if (answer == null)
                                    {
                                        /* Try base-interfaces */
                                        answer = getHandler(iface, handlers);
                                    }
                                    if ( answer != null )
                                    {
                                        keyFound = iface;
										// TODO: Break to label.
                                        goto outer;
                                    }
                                }
                            }
                            else
                            {
                                keyFound = c;
                                break;
                            }
                        }
					outer: 
						;
                }

                if ( answer != null )
                {
                    if ( keyFound != forClass )
                    {
                        handlers.put( forClass, answer );
                    }
                }
            }
        }
        return answer;
    }

    public static object getProperty( OgnlContext context, object source, object name ) // throws OgnlException
    {
        PropertyAccessor        accessor;

        if (source == null) {
            throw new OgnlException("source is null for getProperty(null, \"" + name + "\")");
        }
        if ((accessor = getPropertyAccessor(getTargetClass(source))) == null) {
            throw new OgnlException("No property accessor for " + getTargetClass(source).Name);
        }
        return accessor.getProperty( context, source, name );
    }

    public static void setProperty( OgnlContext context, object target, object name, object value ) // throws OgnlException
    {
        PropertyAccessor        accessor;

        if (target == null) {
            throw new OgnlException("target is null for setProperty(null, \"" + name + "\", " + value + ")");
        }
        if ((accessor = getPropertyAccessor(getTargetClass(target))) == null) {
            throw new OgnlException("No property accessor for " + getTargetClass(target).Name);
        }
        accessor.setProperty( context, target, name, value );
    }

    /**
        Determines the index property type, if any.  Returns <code>INDEXED_PROPERTY_NONE</code> if the
        property is not index-accessible as determined by OGNL or JavaBeans.  If it is indexable
        then this will return whether it is a JavaBeans indexed property, conforming to the
        indexed property patterns (returns <code>INDEXED_PROPERTY_INT</code>) or if it conforms
        to the OGNL arbitrary object indexable (returns <code>INDEXED_PROPERTY_OBJECT</code>).
     */
    public static int getIndexedPropertyType( OgnlContext context, Type sourceClass, string name) // throws OgnlException
    {
        int     result = INDEXED_PROPERTY_NONE;

        try {
            PropertyDescriptor  pd = getPropertyDescriptor(sourceClass, name);

            if (pd != null) {
                if (pd is IndexedPropertyDescriptor) {
                    result = INDEXED_PROPERTY_INT;
                } else {
                    if (pd is ObjectIndexedPropertyDescriptor) {
                        result = INDEXED_PROPERTY_OBJECT;
                    }
                }
            }
        } catch (Exception ex) {
            throw new OgnlException("problem determining if '" + name + "' is an indexed property", ex);
        }
        return result;
    }

    public static object getIndexedProperty( OgnlContext context, object source, string name, object index ) // throws OgnlException
    {
        Exception       reason = null;
        object[]        args = objectArrayPool.create(index);

        try {
            PropertyDescriptor          pd = getPropertyDescriptor((source == null) ? null : source.GetType(), name);
            MethodInfo                      m;

            if (pd is IndexedPropertyDescriptor) {
                m = ((IndexedPropertyDescriptor)pd).getIndexedReadMethod();
            } else {
                if (pd is ObjectIndexedPropertyDescriptor) {
                    m = ((ObjectIndexedPropertyDescriptor)pd).getIndexedReadMethod();
                } else {
                    throw new OgnlException("property '" + name + "' is not an indexed property");
                }
            }
            return callMethod(context, source, m.Name, name, args);
        } catch (OgnlException ex) {
            throw ex;
        } catch (Exception ex) {
            throw new OgnlException("getting indexed property descriptor for '" + name + "'", ex);
        } finally {
            objectArrayPool.recycle(args);
        }
    }

    public static void setIndexedProperty( OgnlContext context, object source, string name, object index, object value ) 
    {
        Exception       reason = null;
        object[]        args = objectArrayPool.create(index, value);

        try {
            PropertyDescriptor          pd = getPropertyDescriptor((source == null) ? null : source.GetType(), name);
            MethodInfo                      m;

            if (pd is IndexedPropertyDescriptor) {
                m = ((IndexedPropertyDescriptor)pd).getIndexedWriteMethod();
            } else {
                if (pd is ObjectIndexedPropertyDescriptor) {
                    m = ((ObjectIndexedPropertyDescriptor)pd).getIndexedWriteMethod();
                } else {
                    throw new OgnlException("property '" + name + "' is not an indexed property");
                }
            }
            callMethod(context, source, m.Name, name, args);
        } catch (OgnlException ex) {
            throw ex;
        } catch (Exception ex) {
            throw new OgnlException("getting indexed property descriptor for '" + name + "'", ex);
        } finally {
            objectArrayPool.recycle(args);
        }
    }

    public static EvaluationPool getEvaluationPool()
    {
        return evaluationPool;
    }

    public static ObjectArrayPool getObjectArrayPool()
    {
        return objectArrayPool;
    }

	// Use Indexer 
	public static IList getIndexerSetMethods (OgnlContext context , Type source)
	{
		PropertyInfo [] ps = source.GetProperties () ;
		ArrayList ms = new ArrayList(16);
		for (int i = 0; i < ps.Length; i++)
		{
			PropertyInfo p = ps [i] ;
			if (p.CanWrite && p.GetIndexParameters ().Length > 0)
			{
				ms.Add (p.GetSetMethod ()) ;
			}
		}

		return ms ;
	}

	public static IList getIndexerGetMethods (OgnlContext context , Type source)
	{
		PropertyInfo [] ps = source.GetProperties () ;
		ArrayList ms = new ArrayList(16);
		for (int i = 0; i < ps.Length; i++)
		{
			PropertyInfo p = ps [i] ;
			if (p.CanRead && p.GetIndexParameters ().Length > 0)
			{
				ms.Add (p.GetGetMethod ()) ;
			}
		}

		return ms ;
	}

	public static bool setIndxerValue (OgnlContext context, object target, object name, object value , object [] args)
	{
		bool     result = false;
		IList      methods = getIndexerSetMethods(context, (target == null) ? null : target.GetType());

		
		if ((methods == null) || (methods.Count == 0)) 
			return result;
		
		object[]        actualArgs = objectArrayPool.create(args.Length + 1);

		Array.Copy (args , 0 , actualArgs , 0 , args.Length);
		actualArgs [args.Length] = value ;

		try 
		{
			callAppropriateMethod(context, target, target, null , null , methods , actualArgs);
			result = true ;
		} 
		finally 
		{
			objectArrayPool.recycle(args);
		}
		
		return result;
	}

	public static object getIndxerValue (OgnlContext context, object target, object name, object [] args)
	{
		object     result = null;
		IList      methods = getIndexerGetMethods(context, (target == null) ? null : target.GetType());

		
		if ((methods == null) || (methods.Count == 0)) 
			throw new NoSuchPropertyException(target , getIndexerName (args));
		
		result = callAppropriateMethod(context, target, target, getIndexerName (args) , "Indexer" , methods , args);
		
		return result;
	}

	
	public static string getIndexerName (object [] ts)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append ("this [") ;
		for (int i = 0; i < ts.Length; i++)
		{
			if (i > 0)
				sb.Append (", ") ;
			sb.Append (ts [i].GetType ().Name);
		}

		sb.Append ("]") ;
		return sb.ToString () ;
	}

	public static bool hasSetIndexer (OgnlContext context, object target, Type targetClass , int paramCount)
	{
		IList methods = getIndexerSetMethods (context, targetClass);
		for (int i = 0; i < methods.Count; i++)
		{
			MethodInfo method = (MethodInfo) methods [i] ;
			if (method.GetParameters ().Length ==  paramCount + 1)
				return true ;
		}
		return false ;
	}

	public static bool hasGetIndexer (OgnlContext context, object target, Type targetClass , int paramCount)
	{
		IList methods = getIndexerGetMethods (context, targetClass);
		for (int i = 0; i < methods.Count; i++)
		{
			MethodInfo method = (MethodInfo) methods [i] ;
			if (method.GetParameters ().Length ==  paramCount)
				return true ;
		}
		return false ;
	}
}
}

