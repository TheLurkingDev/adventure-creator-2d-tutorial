﻿/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2020
 *	
 *	"MainCameraPlayableBehaviour.cs"
 * 
 *	A PlayableBehaviour used by MainCameraMixer.  This is adapted from CinemachineTrack.cs, published by Unity Technologies, and all credit goes to its respective authors.
 * 
 */

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace AC
{

	/**
	 * A PlayableBehaviour used by MainCameraMixer.  This is adapted from CinemachineTrack.cs, published by Unity Technologies, and all credit goes to its respective authors.
	 */
	internal sealed class MainCameraPlayableBehaviour : PlayableBehaviour
	{

		#region Variables

		public _Camera gameCamera;
		public float shakeIntensity;

		#endregion


		#region GetSet

		public bool IsValid
		{
			get
			{
				return gameCamera != null;
			}
		}

		#endregion

	}

}