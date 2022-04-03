using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
	public static void Shuffle<T>( this List<T> list )
	{
		//This is slow and I don't think it is properly random
		//I believe it biases the beginning of the list
		for ( int i = 0; i < list.Count; ++i )
		{
			int swapIndex = Random.Range( 0, list.Count );

			T temp = list[ swapIndex ];
			list[ swapIndex ] = list[ i ];
			list[ i ] = temp;
		}
	}

	public static void SetupIndexBuffer( this List<int> list, int count )
	{
		list.Clear();
		for ( int i = 0; i < count; ++i )
		{
			list.Add( i );
		}
	}
}
