﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension {
	public static int SmallestPowerOfTwoGreaterThan (this int n) {
		--n;
		n |= n >> 1;
		n |= n >> 2;
		n |= n >> 4;
		n |= n >> 8;
		n |= n >> 16;
		return ++n;
	}
}
