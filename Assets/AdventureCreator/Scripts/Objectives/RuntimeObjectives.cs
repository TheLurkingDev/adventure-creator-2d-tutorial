﻿/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2020
 *	
 *	"RuntimeObjectives.cs"
 * 
 *	This script keeps track of all active Objectives.
 * 
 */

using UnityEngine;
using System.Collections.Generic;

namespace AC
{

	/** This script keeps track of all active Objectives. */
	[HelpURL("https://www.adventurecreator.org/scripting-guide/class_a_c_1_1_runtime_objectives.html")]
	public class RuntimeObjectives : MonoBehaviour
	{

		#region Variables

		protected List<ObjectiveInstance> playerObjectiveInstances = new List<ObjectiveInstance>();
		protected List<ObjectiveInstance> globalObjectiveInstances = new List<ObjectiveInstance>();
		protected ObjectiveInstance selectedObjectiveInstance;

		#endregion


		#region PublicFunctions

		/** 
		 * <summary>Updates the state of an Objective</summary>
		 * <param name = "objectiveID">The ID of the Objective to update</param>
		 * <param name = "newStateID">The ID of the Objective's new state</param>
		 * <param name = "selectAfter">If True, the Objective will be considered 'selected' upon being updated</param>
		 */
		public void SetObjectiveState (int objectiveID, int newStateID, bool selectAfter = false)
		{
			foreach (ObjectiveInstance objectiveInstance in playerObjectiveInstances)
			{
				if (objectiveInstance.Objective.ID == objectiveID)
				{
					objectiveInstance.CurrentStateID = newStateID;
					if (selectAfter)
					{
						SelectedObjective = objectiveInstance;
					}
					return;
				}
			}

			foreach (ObjectiveInstance objectiveInstance in globalObjectiveInstances)
			{
				if (objectiveInstance.Objective.ID == objectiveID)
				{
					objectiveInstance.CurrentStateID = newStateID;
					if (selectAfter)
					{
						SelectedObjective = objectiveInstance;
					}
					return;
				}
			}

			ObjectiveInstance newObjectiveInstance = new ObjectiveInstance (objectiveID, newStateID);
			if (newObjectiveInstance.Objective != null)
			{
				if (newObjectiveInstance.Objective.perPlayer && KickStarter.settingsManager.playerSwitching == PlayerSwitching.Allow)
				{
					playerObjectiveInstances.Add (newObjectiveInstance);
				}
				else
				{
					globalObjectiveInstances.Add (newObjectiveInstance);
				}
				if (selectAfter)
				{
					SelectedObjective = newObjectiveInstance;
				}
				KickStarter.eventManager.Call_OnObjectiveUpdate (newObjectiveInstance);
			}
			else
			{
				ACDebug.LogWarning ("Cannot set the state of objective " + objectiveID + " because that ID does not exist!");
			}
		}


		/**
		 * <summary>Gets the state of an active Objective.<summary>
		 * <param name = "objectiveID">The ID of the Objective</param>
		 * <returns>The Objective's state, if active. If inactive, null is returned</returns>
		 */
		public ObjectiveState GetObjectiveState (int objectiveID)
		{
			foreach (ObjectiveInstance objectiveInstance in playerObjectiveInstances)
			{
				if (objectiveInstance.Objective.ID == objectiveID)
				{
					return objectiveInstance.CurrentState;
				}
			}
			foreach (ObjectiveInstance objectiveInstance in globalObjectiveInstances)
			{
				if (objectiveInstance.Objective.ID == objectiveID)
				{
					return objectiveInstance.CurrentState;
				}
			}
			return null;
		}


		/**
		 * <summary>Marks an Objective as inactive</summary>
		 * <param name = "objectiveID">The ID of the Objective</param>
		 */
		public void CancelObjective (int objectiveID)
		{
			foreach (ObjectiveInstance objectiveInstance in playerObjectiveInstances)
			{
				if (objectiveInstance.Objective.ID == objectiveID)
				{
					playerObjectiveInstances.Remove (objectiveInstance);
					return;
				}
			}

			foreach (ObjectiveInstance objectiveInstance in globalObjectiveInstances)
			{
				if (objectiveInstance.Objective.ID == objectiveID)
				{
					globalObjectiveInstances.Remove (objectiveInstance);
					return;
				}
			}
		}


		/**
		 * <summary>Gets an instance of an active Objective</summary>
		 * <param name = "objectiveID">The ID of the Objective to search for</param>
		 * <returns>The ObjectiveInstance if active, or null if not</returns>
		 */
		public ObjectiveInstance GetObjective (int objectiveID)
		{
			foreach (ObjectiveInstance objectiveInstance in playerObjectiveInstances)
			{
				if (objectiveInstance.Objective.ID == objectiveID)
				{
					return objectiveInstance;
				}
			}
			foreach (ObjectiveInstance objectiveInstance in globalObjectiveInstances)
			{
				if (objectiveInstance.Objective.ID == objectiveID)
				{
					return objectiveInstance;
				}
			}
			return null;
		}


		/**
		 * <summary>Gets all active Objective instances</summary>
		 * <returns>All active Objective instances</returns>
		 */
		public ObjectiveInstance[] GetObjectives ()
		{
			List<ObjectiveInstance> completedObjectives = new List <ObjectiveInstance>();
			foreach (ObjectiveInstance objectiveInstance in playerObjectiveInstances)
			{
				completedObjectives.Add (objectiveInstance);
			}
			foreach (ObjectiveInstance objectiveInstance in globalObjectiveInstances)
			{
				completedObjectives.Add (objectiveInstance);
			}
			return completedObjectives.ToArray ();
		}


		/**
		 * <summary>Gets all active Objective instances currently set to a particular type of state</summary>
		 * <param name = "objectiveStateType">The type of state to search for</param>
		 * <returns>All active Objective instances set to the type of state</returns>
		 */
		public ObjectiveInstance[] GetObjectives (ObjectiveStateType objectiveStateType)
		{
			List<ObjectiveInstance> completedObjectives = new List <ObjectiveInstance>();
			foreach (ObjectiveInstance objectiveInstance in playerObjectiveInstances)
			{
				if (objectiveInstance.CurrentState.stateType == objectiveStateType)
				{
					completedObjectives.Add (objectiveInstance);
				}
			}
			foreach (ObjectiveInstance objectiveInstance in globalObjectiveInstances)
			{
				if (objectiveInstance.CurrentState.stateType == objectiveStateType)
				{
					completedObjectives.Add (objectiveInstance);
				}
			}
			return completedObjectives.ToArray ();
		}


		/**
		 * <summary>Gets all active Objective instances currently set to a particular display type of state</summary>
		 * <param name = "objectiveDisplayType">The type of display state to search for</param>
		 * <returns>All active Objective instances set to the type of display state</returns>
		 */
		public ObjectiveInstance[] GetObjectives (ObjectiveDisplayType objectiveDisplayType)
		{
			List<ObjectiveInstance> completedObjectives = new List <ObjectiveInstance>();
			foreach (ObjectiveInstance objectiveInstance in playerObjectiveInstances)
			{
				if (objectiveInstance.CurrentState.DisplayTypeMatches (objectiveDisplayType))
				{
					completedObjectives.Add (objectiveInstance);
				}
			}
			foreach (ObjectiveInstance objectiveInstance in globalObjectiveInstances)
			{
				if (objectiveInstance.CurrentState.DisplayTypeMatches (objectiveDisplayType))
				{
					completedObjectives.Add (objectiveInstance);
				}
			}
			return completedObjectives.ToArray ();
		}


		/**
		 * <summary>Selects an Objective, so that it can be displayed in a Menu</summary>
		 * <param name = "objectiveID">The ID of the Objective to select</param>
		 */
		public void SelectObjective (int objectiveID)
		{
			SelectedObjective = GetObjective (objectiveID);
		}


		/** De-selects the selected Objective */
		public void DeselectObjective ()
		{
			SelectedObjective = null;
		}


		/** Clears all Objective data */
		public void ClearAll ()
		{
			playerObjectiveInstances.Clear ();
			globalObjectiveInstances.Clear ();
		}


		/** Clears all Objective data that's per-player, and not global */
		public void ClearUniqueToPlayer ()
		{
			playerObjectiveInstances.Clear ();
		}


		/**
		 * <summary>Updates a PlayerData class with Objectives that are unique to the current Player.</summary>
		 * <param name = "playerData">The original PlayerData class</param>
		 * <returns>The updated PlayerData class</returns>
		 */
		public PlayerData SavePlayerObjectives (PlayerData playerData)
		{
			System.Text.StringBuilder playerObjectivesData = new System.Text.StringBuilder ();
			foreach (ObjectiveInstance objectiveInstance in playerObjectiveInstances)
			{
				playerObjectivesData.Append (objectiveInstance.SaveData);
				playerObjectivesData.Append (SaveSystem.pipe);
			}
			if (playerObjectiveInstances.Count > 0)
			{
				playerObjectivesData.Remove (playerObjectivesData.Length-1, 1);
			}
			playerData.playerObjectivesData = playerObjectivesData.ToString ();

			return playerData;
		}


		/**
		 * <summary>Restores saved data from a PlayerData class</summary>
		 * <param name = "playerData">The PlayerData class to load from</param>
		 */
		public void AssignPlayerObjectives (PlayerData playerData)
		{
			playerObjectiveInstances.Clear ();
			SelectedObjective = null;

			if (!string.IsNullOrEmpty (playerData.playerObjectivesData))
			{
				string[] playerObjectivesArray = playerData.playerObjectivesData.Split (SaveSystem.pipe[0]);
				
				foreach (string chunk in playerObjectivesArray)
				{
					ObjectiveInstance objectiveInstance = new ObjectiveInstance (chunk);
					if (objectiveInstance.Objective != null)
					{
						playerObjectiveInstances.Add (objectiveInstance);
					}
				}
			}
		}


		/**
		 * <summary>Updates a MainData class with Objectives that are shared by all Players.</summary>
		 * <param name = "mainData">The original MainData class</param>
		 * <returns>The updated MainData class</returns>
		 */
		public MainData SaveGlobalObjectives (MainData mainData)
		{
			System.Text.StringBuilder globalObjectivesData = new System.Text.StringBuilder ();
			foreach (ObjectiveInstance objectiveInstance in globalObjectiveInstances)
			{
				globalObjectivesData.Append (objectiveInstance.SaveData);
				globalObjectivesData.Append (SaveSystem.pipe);
			}
			if (globalObjectiveInstances.Count > 0)
			{
				globalObjectivesData.Remove (globalObjectivesData.Length-1, 1);
			}
			mainData.globalObjectivesData = globalObjectivesData.ToString ();

			return mainData;
		}


		/**
		 * <summary>Restores saved data from a MainData class</summary>
		 * <param name = "playerData">The MainData class to load from</param>
		 */
		public void AssignGlobalObjectives (MainData mainData)
		{
			globalObjectiveInstances.Clear ();
			SelectedObjective = null;

			if (!string.IsNullOrEmpty (mainData.globalObjectivesData))
			{
				string[] globalObjectivesArray = mainData.globalObjectivesData.Split (SaveSystem.pipe[0]);
				
				foreach (string chunk in globalObjectivesArray)
				{
					ObjectiveInstance objectiveInstance = new ObjectiveInstance (chunk);
					if (objectiveInstance.Objective != null)
					{
						globalObjectiveInstances.Add (objectiveInstance);
					}
				}
			}
		}

		#endregion


		#region GetSet

		/** The instance of the currently-selected Objective */
		public ObjectiveInstance SelectedObjective
		{
			get
			{
				return selectedObjectiveInstance;
			}
			set
			{
				selectedObjectiveInstance = value;

				if (selectedObjectiveInstance != null)
				{
					KickStarter.eventManager.Call_OnObjectiveSelect (selectedObjectiveInstance);
				}
			}
		}

		#endregion

	}

}