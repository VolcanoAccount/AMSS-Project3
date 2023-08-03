using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace AMSS
{
	public class RemoveDefaultSplash
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
		static void OnRuntimeMethodLoad()
		{
#if UNITY_WEBGL
			Application.focusChanged += __OnFocusChanged;
#else
			System.Threading.Tasks.Task.Run(AsyncSkip);
#endif
		}

#if UNITY_WEBGL
		private static void __OnFocusChanged(bool b)
		{
			Application.focusChanged -= __OnFocusChanged;
			SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
		}
#else
		private static void AsyncSkip()
		{
			SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
		}
#endif
	}
}