using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Global {

	public static int SECOND_TO_TICKS = 10000000;

	public static Color changeAlpha( Color col, float a )
	{
		return new Color(col.r,col.g,col.b,a);
	}




}
