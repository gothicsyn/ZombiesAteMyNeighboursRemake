// /*
// *
// * 	Devils Inc Studios
// * 	How Long
// * 	// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015, 2015
// *	
// *	TODO: Include a description of the file here.
// *
// */

/*
 * References: 
 * http://wiki.unity3d.com/index.php?title=CSharpMessenger
 * http://msdn.microsoft.com/en-us/library/9aackb16(v=vs.110).aspx
 * http://msdn.microsoft.com/en-us/library/ms173175.aspx
 * http://msdn.microsoft.com/en-us/library/aa645739%28VS.71%29.aspx
 * http://wiki.unity3d.com/index.php?title=CSharpMessenger_Extended
 * 
 * Note:
 * We likely need to credit Rod Hyde (badlydrawnrod) over at unityforums
 * We might also need to release this messenger system back as CC 3.0 code even though the edits are minor and it adds custom code not related to it.
 * As stated by http://creativecommons.org/licenses/by-sa/3.0/
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace DI_Events {
	class EventCenter {
		static public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

		static public void addListener(string eventType, CallBack callback) {

			lock (eventTable) {
				// Make sure we aren't adding delegate and null.
				if (!eventTable.ContainsKey(eventType)) {
					eventTable.Add(eventType, null);
				}

				// Add the call back to the list of delgates
				eventTable[eventType] = (CallBack)eventTable[eventType] + callback;
			}
		}

		static public void removeListener(string eventType, CallBack callback) {
			lock (eventTable) {
				if (eventTable.ContainsKey(eventType)) {
					eventTable[eventType] = (CallBack)eventTable[eventType] - callback;

					if (eventTable[eventType] == null) {
						eventTable.Remove(eventType);
					}
				}
			}
		}

		static public void invoke(string eventType) {
			Delegate handler;
			if (eventTable.TryGetValue(eventType, out handler)) {
				CallBack callback = (CallBack)handler;
				if (callback != null) {
					callback();
				}
			}
		}
	}

	class EventCenter<T> {
		static public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
		
		static public void addListener(string eventType, CallBack<T> callback) {
			
			lock (eventTable) {
				// Make sure we aren't adding delegate and null.
				if (!eventTable.ContainsKey(eventType)) {
					eventTable.Add(eventType, null);
				}
				
				// Add the call back to the list of delgates
				eventTable[eventType] = (CallBack<T>)eventTable[eventType] + callback;
			}
		}
		static public void removeListener(string eventType, CallBack<T> callback) {
			lock (eventTable) {
				if (eventTable.ContainsKey(eventType)) {
					eventTable[eventType] = (CallBack<T>)eventTable[eventType] - callback;
					
					if (eventTable[eventType] == null) {
						eventTable.Remove(eventType);
					}
				}
			}
		}
		
		static public void invoke(string eventType, T argv1) {
			Delegate handler;
			if (eventTable.TryGetValue(eventType, out handler)) {
				CallBack<T> callback = (CallBack<T>)handler;
				if (callback != null) {
					callback(argv1);
				}
			}
		}
	}

	class EventCenter<T, U> {
		static public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
		
		static public void addListener(string eventType, CallBack<T, U> callback) {
			
			lock (eventTable) {
				// Make sure we aren't adding delegate and null.
				if (!eventTable.ContainsKey(eventType)) {
					eventTable.Add(eventType, null);
				}
				
				// Add the call back to the list of delgates
				eventTable[eventType] = (CallBack<T, U>)eventTable[eventType] + callback;
			}
		}
		static public void removeListener(string eventType, CallBack<T, U> callback) {
			lock (eventTable) {
				if (eventTable.ContainsKey(eventType)) {
					eventTable[eventType] = (CallBack<T, U>)eventTable[eventType] - callback;
					
					if (eventTable[eventType] == null) {
						eventTable.Remove(eventType);
					}
				}
			}
		}
		
		static public void invoke(string eventType, T argv1, U argv2) {
			Delegate handler;
			if (eventTable.TryGetValue(eventType, out handler)) {
				CallBack<T, U> callback = (CallBack<T, U>)handler;
				if (callback != null) {
					callback(argv1, argv2);
				}
			}
		}
	}

	class EventCenter<T, U, V> {
		static public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
		
		static public void addListener(string eventType, CallBack<T, U, V> callback) {
			
			lock (eventTable) {
				// Make sure we aren't adding delegate and null.
				if (!eventTable.ContainsKey(eventType)) {
					eventTable.Add(eventType, null);
				}

				// Add the call back to the list of delgates
				eventTable[eventType] = (CallBack<T, U, V>)eventTable[eventType] + callback;
			}
		}
		static public void removeListener(string eventType, CallBack<T, U, V> callback) {
			lock (eventTable) {
				if (eventTable.ContainsKey(eventType)) {
					eventTable[eventType] = (CallBack<T, U, V>)eventTable[eventType] - callback;

					if (eventTable[eventType] == null) {
						eventTable.Remove(eventType);
					}
				}
			}
		}
		
		static public void invoke(string eventType, T argv1, U argv2, V argv3) {
			Delegate handler;
			if (eventTable.TryGetValue(eventType, out handler)) {
				CallBack<T, U, V> callback = (CallBack<T, U, V>)handler;
				if (callback != null) {
					callback(argv1, argv2, argv3);
				}
			}
		}
	}

	class EventCenter<T, U, V, W> {
		static public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
		
		static public void addListener(string eventType, CallBack<T, U, V, W> callback) {
			
			lock (eventTable) {
				// Make sure we aren't adding delegate and null.
				if (!eventTable.ContainsKey(eventType)) {
					eventTable.Add(eventType, null);
				}
				
				// Add the call back to the list of delgates
				eventTable[eventType] = (CallBack<T, U, V, W>)eventTable[eventType] + callback;
			}
		}
		static public void removeListener(string eventType, CallBack<T, U, V, W> callback) {
			lock (eventTable) {
				if (eventTable.ContainsKey(eventType)) {
					eventTable[eventType] = (CallBack<T, U, V, W>)eventTable[eventType] - callback;
					
					if (eventTable[eventType] == null) {
						eventTable.Remove(eventType);
					}
				}
			}
		}
		
		static public void invoke(string eventType, T argv1, U argv2, V argv3, W argv4) {
			Delegate handler;
			if (eventTable.TryGetValue(eventType, out handler)) {
				CallBack<T, U, V, W> callback = (CallBack<T, U, V, W>)handler;
				if (callback != null) {
					callback(argv1, argv2, argv3, argv4);
				}
			}
		}
	}
	public delegate void CallBack();
	public delegate void CallBack<T>(T argv1);
	public delegate void CallBack<T, U>(T argv1, U argv2);
	public delegate void CallBack<T, U, V>(T argv1, U argv2, V argv3);
	public delegate void CallBack<T, U, V, W>(T argv1, U argv2, V argv3, W argv4);
}