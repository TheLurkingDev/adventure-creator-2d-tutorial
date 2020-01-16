using UnityEngine;

namespace AC
{

	public static class ACScreen
	{

		#if UNITY_EDITOR
		private static int cachedWidth, cachedHeight;
		#endif

		public static int width
		{
			get
			{
				#if UNITY_EDITOR
				if (!Application.isPlaying) return Screen.width;
				if (cachedWidth == 0) UpdateCache ();
				return cachedWidth;
				#else
				return Screen.width;
				#endif
			}
		}


		public static int height
		{
			get
			{
				#if UNITY_EDITOR
				if (!Application.isPlaying) return Screen.height;
				if (cachedHeight == 0) UpdateCache ();
				return cachedHeight;
				#else
				return Screen.height;
				#endif
			}
		}


		public static Rect safeArea
		{
			get
			{
				#if UNITY_EDITOR
				return new Rect (0f, 0f, width, height);
				#else
				return Screen.safeArea;
				#endif
			}
		}


		#if UNITY_EDITOR

		public static void UpdateCache ()
		{
			cachedWidth = Screen.width;
			cachedHeight = Screen.height;			
		}

		#endif

	}

}