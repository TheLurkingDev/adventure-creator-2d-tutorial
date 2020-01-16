/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2020
 *	
 *	"MainCameraTrack.cs"
 * 
 *	A TrackAsset used by MainCameraMixer.  This is adapted from CinemachineTrack.cs, published by Unity Technologies, and all credit goes to its respective authors.
 * 
 */

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace AC
{

	[System.Serializable]
	[TrackClipType (typeof (MainCameraShot))]
	[TrackColor (0.73f, 0.1f, 0.1f)]
	/**
	 * A TrackAsset used by MainCameraMixer.  This is adapted from CinemachineTrack.cs, published by Unity Technologies, and all credit goes to its respective authors.
	 */
	public class MainCameraTrack : TrackAsset
	{

		#region PublicFunctions

		public override Playable CreateTrackMixer (PlayableGraph graph, GameObject go, int inputCount)
		{
			ScriptPlayable<MainCameraMixer> mixer = ScriptPlayable<MainCameraMixer>.Create (graph);
			mixer.SetInputCount (inputCount);
			return mixer;
		}

		#endregion

	}

}