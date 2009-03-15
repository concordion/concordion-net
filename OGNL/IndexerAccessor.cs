using System;
using System.Reflection ;
using System.Text ;

namespace ognl
{
	/// <summary>
	/// IndexerAccessor
	/// </summary>
	public class IndexerAccessor
	{
		public static object getIndexerValue (object target , object [] parameters)
		{
			PropertyInfo indexer = getIndexer (target, parameters) ;
			if (indexer == null)
			{
				throw new NoSuchPropertyException(target , typesToIndexerName (parameters)) ;
			}
			return indexer.GetValue (target , parameters) ;
		}

		public static PropertyInfo getIndexer (object target, object[] parameters)
		{
			Type targetClass = (target == null) ? null : target.GetType() ;
			Type [] pTypes = new Type [parameters.Length] ;
			for (int i = 0; i < pTypes.Length; i++)
			{
				if (parameters [i] == null)
					throw new ArgumentNullException("parameters [" + i + "]") ;
				pTypes [i] = parameters [i].GetType () ;
				
			}
			return targetClass.GetProperty ("Item" , pTypes) ;
		}

		public static void setIndexerValue (object target , object value , object [] parameters)
		{
			
			PropertyInfo indexer = getIndexer (target, parameters) ;
			if (indexer == null)
				throw new NoSuchPropertyException(target , typesToIndexerName (parameters)) ;

			indexer.SetValue (target , value , parameters) ;
		}

		public static string typesToIndexerName (object [] ts)
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
	}
}
